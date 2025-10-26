# Copyright (c) .NET Foundation and Contributors
# See LICENSE file in the project root for full license information.

# This PS checks what binding need to be build in a PR or regular commit and takes care of performing the various checks and build the solution

# setup msbuild
$vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
$msbuild = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath

if ($msbuild) {
    $msbuild = join-path $msbuild 'MSBuild\Current\Bin\amd64\MSBuild.exe'
}

# prepare GitHub API headers using token auth
$headers = @{
    Authorization = "token $env:MY_GITHUBTOKEN"
    'User-Agent'  = 'azure-pipelines'
    Accept        = 'application/vnd.github+json'
}

if($env:System_PullRequest_PullRequestId -ne $null)
{
  "" | Write-Host -ForegroundColor Yellow
  "**********************" | Write-Host -ForegroundColor Yellow
  "* Building from a PR *" | Write-host -ForegroundColor Yellow
  "**********************" | Write-Host -ForegroundColor Yellow
  "" | Write-Host -ForegroundColor Yellow

  # get files changed in PR
  $pageCounter = 1

  do
  {
    "##[debug] INFO: iteration $pageCounter" | Write-Host

    $batch = Invoke-RestMethod `
                -Uri "https://api.github.com/repos/nanoframework/Samples/pulls/$env:System_PullRequest_PullRequestNumber/files?per_page=100&page=$pageCounter" `
                -Headers $headers `
                -Method GET

    if($null -eq $commit)
    {
      $commit = $batch
    }
    else
    {
      $commit += $batch
    }
    
    $pageCounter++
  } until ($batch.Count -eq 0)

  # filter removed files              
  $files = $commit.where{$_.status -ne 'removed'}

}
else
{
    "" | Write-Host -ForegroundColor Yellow
    "**************************" | Write-Host -ForegroundColor Yellow
    "* Building from a commit *" | Write-host -ForegroundColor Yellow
    "**************************" | Write-Host -ForegroundColor Yellow
    "" | Write-Host -ForegroundColor Yellow

    # get files changed in the commit
    $pageCounter = 1

    do
    {
      "##[command] Get API file change page: $pageCounter" | Write-Host

      $batch = Invoke-RestMethod `
                  -Uri "https://api.github.com/repos/nanoframework/Samples/commits/$env:Build_SourceVersion`?per_page=100&page=$pageCounter" `
                  -Headers $headers `
                  -Method GET

      if($null -eq $commit)
      {
        $commit = $batch.files
      }
      else
      {
        $commit += $batch.files
      }
      
      $pageCounter++

    } until ($batch.files.Count -eq 0)

    # filter removed files              
    $files = $commit.where{$_.status -ne 'removed' -and $_.filename -match '(devices)'}
}

# get file names only
$files1 = $files | % {$_.filename} | Where-Object {$_ -match 'samples/*'} 

if($null -eq $files1)
{
  Write-host "No 'samples' found to build"
  exit
}

Write-host "##[group] Files changed:"
$files1 | ForEach-Object { Write-host $_ }
Write-host "##[endgroup]"

# pattern to select samples folder name
$pattern = '(samples\/)(?<folder>[a-zA-Z0-9._]+)(?>\/)'

# filter out the collection
$results = [Regex]::Matches($files1, $pattern)

# get unique folder names
$sampleFolders = $results | Sort-Object | Select-Object | Foreach-Object {$_.Groups["folder"].Value} | Get-Unique 

Write-host "Found $($sampleFolders.count) sample(s) to build"

write-host "##[section] Processing samples..."

foreach ($folder in $sampleFolders)
{
  "" | Write-Host -ForegroundColor Yellow
  "***********************" | Write-Host -ForegroundColor Yellow
  "##[group] Processing sample '$folder'..." | Write-Host -ForegroundColor Yellow
  "***********************" | Write-Host -ForegroundColor Yellow
  "" | Write-Host -ForegroundColor Yellow

  try
  {
    # try to find all solution files
    $solutionFiles = Get-ChildItem -Path "samples\$folder\" -Include "*.sln" -Recurse

    if($null -eq $solutionFiles)
    {
      "" | Write-Host -ForegroundColor Red
      "*****************************************" | Write-Host -ForegroundColor Red
      "##[error] >>>ERROR: Couldn't find any solution files!"  | Write-Host -ForegroundColor Red
      throw "Couldn't find any solution file inside '$folder'..."
    }
    else
    {
      foreach ($slnFile in $solutionFiles)
      {
        "" | Write-Host -ForegroundColor Yellow
        "##[command] INFO: Building solution: '$slnFile'" | Write-Host -ForegroundColor Yellow
        "" | Write-Host -ForegroundColor Yellow
                
        # need to restore NuGets first
        write-host "##[command] Performing nuget restore for '$slnFile'."
        nuget restore $slnFile

        if (-not $?)
        { 
          "" | Write-Host -ForegroundColor Red
          "*****************************************" | Write-Host -ForegroundColor Red
          "##[error] >>>ERROR: Couldn't restore solution: '$slnFile'!"  | Write-Host -ForegroundColor Red
          # set flag
          $buildFailed = $true    
        }

        # build solution
        write-host "##[command] Running msbuild."
        & "$msbuild" "$slnFile" /verbosity:normal /p:Configuration=Release

        if (-not $?) { 
          "" | Write-Host -ForegroundColor Red
          "*****************************************" | Write-Host -ForegroundColor Red
          "##[error] >>>ERROR: Failed building solution: '$slnFile'!"  | Write-Host -ForegroundColor Red
          # set flag
          $buildFailed = $true    
        }
      }
    }
  }
  catch
  {
    "" | Write-Host -ForegroundColor Red
    "*****************************************" | Write-Host -ForegroundColor Red
    "##[error] >>>ERROR: build failed, check output <<<" | Write-Host -ForegroundColor Red
    "*****************************************" | Write-Host -ForegroundColor Red
    "" | Write-Host -ForegroundColor Red

    "" | Write-Host -ForegroundColor Red
    "Exception was: $_" | Write-Host -ForegroundColor Red
    "" | Write-Host -ForegroundColor Red

    # set flag
    $buildFailed = $true
  }

  write-host "##[endgroup]"
}

if($buildFailed)
{
  "********************************" | Write-Host -ForegroundColor Red
  "##[error] Check >>>ERROR<<< messages above" | Write-Host -ForegroundColor Red
  "********************************" | Write-Host -ForegroundColor Red

  exit 1
}
