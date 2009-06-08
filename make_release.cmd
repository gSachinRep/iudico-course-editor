@echo off

ECHO Cleaning up RELEASE directory
DEL RELEASE\*.* /s /f /q

SET RESULT=.


IF not ".%1"==".-s" (
if not exist Bin\RELEASE mkdir Bin\RELEASE 
COPY Lib\*.dll Bin\RELEASE
C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe CourseEditor.sln /t:REBUILD /p:Configuration=RELEASE
  if errorlevel 1 (
    ECHO Compilation failured
    SET RESULT=Compilation failured: CourseEditor.sln Precompilation mode
    GOTO :ERROR
  )
)
ECHO Build successed. Copying binaries to RELEASE directory

if not exist RELEASE mkdir RELEASE

COPY Bin\Release\CourseEditor.exe RELEASE\CourseEditor.exe
COPY Bin\Release\CourseEditor.exe.config RELEASE\CourseEditor.exe.config
COPY Lib\*.dll RELEASE\*.dll

CALL PRECOMPILE RELEASE np

:PACK
ECHO Creating self-extracting package  RELEASE\FireFlyCourseEditor.exe
CALL rar a -ep1 -s -m5 -ac -sfx RELEASE\FireFlyCourseEditor.rar RELEASE\*.*

:EXIT
:ERROR
ECHO %RESULT%
ECHO Done.

PAUSE