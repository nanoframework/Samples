#
# Copyright(c) 2018 The nanoFramework project contributors
# See LICENSE file in the project root for full license information.
#


#################################################################################################
# make sure that a valid path is set bellow                                                     #
# if this is for a class library it's OK to leave it as it is                                   #
# if this is an Interop module the path has to be set where the build can find the source files #
#################################################################################################

# native code directory
set(BASE_PATH_FOR_THIS_MODULE "${BASE_PATH_FOR_CLASS_LIBRARIES_MODULES}/NF.AwesomeLib")


# set include directories
list(APPEND NF.AwesomeLib_INCLUDE_DIRS "${PROJECT_SOURCE_DIR}/src/CLR/Core")
list(APPEND NF.AwesomeLib_INCLUDE_DIRS "${PROJECT_SOURCE_DIR}/src/CLR/Include")
list(APPEND NF.AwesomeLib_INCLUDE_DIRS "${PROJECT_SOURCE_DIR}/src/HAL/Include")
list(APPEND NF.AwesomeLib_INCLUDE_DIRS "${PROJECT_SOURCE_DIR}/src/PAL/Include")
list(APPEND NF.AwesomeLib_INCLUDE_DIRS "${BASE_PATH_FOR_THIS_MODULE}")

# source files
set(NF.AwesomeLib_SRCS

    NF_AwesomeLib.cpp
    NF_AwesomeLib_NF_AwesomeLib_Math_mshl.cpp
    NF_AwesomeLib_NF_AwesomeLib_Math.cpp
    NF_AwesomeLib_NF_AwesomeLib_Utilities_mshl.cpp
    NF_AwesomeLib_NF_AwesomeLib_Utilities.cpp

)

foreach(SRC_FILE ${NF.AwesomeLib_SRCS})
    set(NF.AwesomeLib_SRC_FILE SRC_FILE-NOTFOUND)
    find_file(NF.AwesomeLib_SRC_FILE ${SRC_FILE}
	    PATHS
	        "${BASE_PATH_FOR_THIS_MODULE}"
	        "${TARGET_BASE_LOCATION}"

	    CMAKE_FIND_ROOT_PATH_BOTH
    )
# message("${SRC_FILE} >> ${NF.AwesomeLib_SRC_FILE}") # debug helper
list(APPEND NF.AwesomeLib_SOURCES ${NF.AwesomeLib_SRC_FILE})
endforeach()

include(FindPackageHandleStandardArgs)

FIND_PACKAGE_HANDLE_STANDARD_ARGS(NF.AwesomeLib DEFAULT_MSG NF.AwesomeLib_INCLUDE_DIRS NF.AwesomeLib_SOURCES)

