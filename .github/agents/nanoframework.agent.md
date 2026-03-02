---
name: .NET nanoFramework Agent
description: Agent instructions for generating, reviewing, and modifying C# code for .NET nanoFramework samples on constrained embedded devices.
tools:
  - search/codebase
  - search
  - web/fetch
  - web/githubRepo
  - read/readFile
  - search/fileSearch
  - search/textSearch
  - search/listDirectory
  - edit/createDirectory
  - edit/createFile
  - edit/editFiles
  - execute/runInTerminal
---

# Copilot Instructions for .NET nanoFramework

This repository targets **.NET nanoFramework**, which runs C# on constrained embedded devices (for example ESP32, STM32, and others).

When generating, reviewing, or modifying code in this repo, follow these rules:

## 1 - Validate API availability first

- Do **not** assume an API exists just because it is available in desktop/main .NET.
- Always verify availability in the nanoFramework API browser:
  - https://docs.nanoframework.net/api/index.html
- If an API is not available, propose a nanoFramework-compatible alternative.

## 2 - Prefer patterns already used in this repository

- Use existing samples in this repo as the primary source for implementation patterns, coding style, and project structure.
- Before introducing a new approach, check if an equivalent pattern already exists in:
  - https://github.com/nanoframework/samples
- Keep solutions simple and aligned with embedded constraints (memory, CPU, storage, networking reliability).

## 3 - Account for nanoFramework differences from main .NET

- Assume APIs are simplified even when naming aligns with main .NET.
- Avoid desktop-only features, heavy abstractions, reflection-heavy code, or dependencies that are unlikely to run on embedded targets.
- Prefer deterministic, lightweight implementations suitable for microcontrollers.
- Always consider the constraints of the target hardware when proposing solutions.
- Generics, async patterns, and certain language features are not yet supported on nanoFramework.

## 4 - Use device bindings when applicable

- nanoFramework supports many IoT device bindings.
- For hardware-specific code, check existing bindings and guidance first:
  - https://docs.nanoframework.net/devicesdetails/README.html
- Reuse existing bindings and samples rather than creating custom low-level implementations when a supported binding exists.

## 5 - Practical coding expectations

- Keep allocations and background work minimal.
- Be explicit about timeouts, retries, and error handling for I/O and networking.
- Favor clear, maintainable code that can run reliably on constrained devices.
- When in doubt, choose the simpler implementation.

## 6 - If uncertain, state assumptions

- Explicitly mention when a proposal depends on API availability or board-specific capabilities.
- Add concise notes about what should be verified on target hardware.

## 7 - Build and solution validation workflow

- Always include a build step when changes affect code, project files, or package references.
- Follow the nanoFramework VS Code extension build model: use `nuget restore` + `msbuild`.
- Build scope selection:
  - If a matching `*.sln` exists for the changed sample, build the solution (preferred, same as extension workflow).
  - If no solution exists, build the target `*.nfproj` directly.
  - When targeting a project, use the project path and project system path defined/imported by the `.nfproj`.
- Preferred command sequence (PowerShell):
  - `nuget restore <path-to-solution-or-project>`
  - `msbuild <path-to-solution-or-project> /p:platform="Any CPU" /p:Configuration=Release /verbosity:minimal`
- If `msbuild` is not on `PATH`, resolve it first (for example with `vswhere` on Windows), then run the same restore/build steps.
- Windows example to resolve `MSBuild.exe` with `vswhere` and build:
  - `$msbuild = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -products * -latest -prerelease -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\amd64\MSBuild.exe" | Select-Object -First 1`
  - `& $msbuild <path-to-solution-or-project> /p:platform="Any CPU" /p:Configuration=Release /verbosity:minimal`
- If build fails:
  - Report the exact error output.
  - Propose a minimal fix.
  - Re-run the same `nuget restore` + `msbuild` sequence after the fix.
- In final results, summarize:
  - Which solution/project was built.
  - Whether `nuget restore` + `msbuild` succeeded.
  - Any remaining manual hardware validation steps (deployment/flash/runtime checks).

## 8 - Testing workflow for nanoFramework

- Do not default to `dotnet test` for nanoFramework projects.
- For unit tests, follow the nanoFramework Test Platform workflow (`nanoFramework.TestFramework`, `nanoFramework.UnitTestLauncher`, `nanoFramework.TestAdapter`).
- Identify unit test projects by checking `.nfproj` metadata such as:
  - `<IsTestProject>true</IsTestProject>`
  - `<TestProjectType>UnitTest</TestProjectType>`
- Ensure a `.runsettings` file exists (often `nano.runsettings`) and includes nanoFramework adapter settings.
- Test discovery/execution flow:
  - Build first (`nuget restore` + `msbuild`) so tests are discovered.
  - Run tests through Visual Studio / Test Explorer (or equivalent VS test host integration).
  - For hardware execution, set `<IsRealHardware>True</IsRealHardware>` in `.runsettings`.
- Pipeline/automation flow:
  - Use `vstest.Console.exe` with `nanoFramework.TestAdapter.dll`.
  - Keep test adapter and test framework package versions aligned.
- When reporting test outcomes, include:
  - Which test project and runsettings file were used.
  - Whether tests ran on emulator/simulated mode or real hardware.
  - Pass/fail/skip summary and any device-specific constraints.
