@echo off
:: Get current date and time for commit message
for /f "tokens=2 delims==" %%I in ('wmic os get localdatetime /format:list') do set datetime=%%I
set date=%datetime:~0,8%
set time=%datetime:~8,6%

:: Format date as YYYYMMDD
set year=%date:~0,4%
set month=%date:~4,2%
set day=%date:~6,2%

:: Format time as HHMMSS
set hour=%time:~0,2%
set minute=%time:~2,2%
set second=%time:~4,2%

:: Create commit message with formatted date and time
set commit_msg="%year%-%month%-%day%-%hour%-%minute%-%second%"

:: Git commands
git add .
git commit -m %commit_msg%
git push

echo Committed and pushed with message: %commit_msg%
pause