rem Build project manually
tools\nant\nant.exe -l:buildlog.txt -buildfile:build.build %*
if not %errorLevel% == 0 pause