param(
    [string]$repositoryRootPath
)

### Target paths ###
$binPath = $repositoryRootPath + "\BotANick.MainConsole\bin\Release\netcoreapp3.1\publish"
$corePath = $repositoryRootPath + "\BotANick.Core"

### Source paths ###
$configSourcePath = "D:\actions-runner\Config"
$twitchConfig = $configSourcePath + "\twitch-config-prod.yml"
$discordConfig = $configSourcePath + "\discord-config-prod.yml"

### Copy config files ###
Copy-Item -Path $twitchConfig -Destination $binPath -Recurse
Copy-Item -Path $discordConfig -Destination $binPath -Recurse

### Copy DbBackUp ###
&docker cp nervous_shockley:/App/botanick_data.db $corePath
