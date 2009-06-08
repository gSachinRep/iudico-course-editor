@ECHO OFF

if not .%1==. (
SET OUT=%1
)
if .%1==. (
SET OUT=bin
)

ECHO Output directory: %OUT%

IF NOT EXIST "bin\Precompilation" (
	mkdir bin\Precompilation
)
COPY /V /Y Lib\*.dll bin\Precompilation

C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe CourseEditor.sln /t:REBUILD /p:Configuration=Precompilation
if errorlevel 1 (
  ECHO Compilation failured: CourseEditor.sln precompilation mode
  SET RESULT=Compilation failured: CourseEditor.sln precompilation mode
)


bin\Precompilation\CourseEditor.exe
if errorlevel 1 (
  ECHO PRECOMPILATION FAILURED!!!
  SET RESULT=ECHO PRECOMPILATION FAILURED!!!
)

COPY /V /Y Lib\*.dll %OUT%
COPY /V /Y bin\Precompilation\regex.dll %OUT%
COPY /V /Y bin\Precompilation\CourseEditor.xmls.dll %OUT% 

if not .%2==.np (
pause
)