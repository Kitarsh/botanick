docker cp botanickContainer:/App/botanick_data.db /home/pi/botanickBackup

:: Change db backup name with timecode.
SET HOUR=%time:~0,2%
SET dtStamp9=%date:~-4%%date:~0,2%%date:~3,2%_0%time:~1,1%%time:~3,2%%time:~6,2%
SET dtStamp24=%date:~-4%%date:~0,2%%date:~3,2%_%time:~0,2%%time:~3,2%%time:~6,2%

if "%HOUR:~0,1%" == " " (SET dtStamp=%dtStamp9%) else (SET dtStamp=%dtStamp24%)

REN "D:\BotANick.BackUp\botanick_data.db" "*-%dtStamp%.db"