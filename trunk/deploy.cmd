@echo off
ECHO Copying files to "ROUSE"
COPY RELEASE\*.* "\\rose\d$\Projects\FireFly Course Editor"
ECHO Complete

ECHO Copying files to "APPLE"
COPY RELEASE\*.* "\\apple\d$\Projects\FireFly Course Editor"
ECHO Complete
ECHO Done.
PAUSE