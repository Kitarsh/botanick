param(
    [string]$repositoryRootPath
)

### Target paths ###
$binPath = $repositoryRootPath + "\BotANick.MainConsole\bin\Release\netcoreapp3.1\publish"
$corePath = $binPath + "\BotANick.Core"

### Source paths ###
$configSourcePath = "D:\actions-runner\Config"

### Copy config files and DB folder###
Copy-Item -Path ($configSourcePath + "\*") -Destination $binPath -Recurse

### Copy DbBackUp ###
&docker cp nervous_shockley:/App/BotANick.Core/botanick_data.db $corePath
