param(
    [string]$repositoryRootPath
)

#############################################################
### Check if entity framework tool for dotnet is installed.
### Install it if necessary
#############################################################

$globalPkgString = (dotnet tool list --global)

$globalPkg = New-Object -TypeName System.Collections.ArrayList
$globalPkgString | 
Select-Object -Skip 2 | # skip the header row
ForEach-Object {
    # Split on space(s)
    $Values = $_ -split '\s+' 
    # Put together object and store in $Objects array
    $globalPkg.Add([PSCustomObject]@{
            PackageID = $Values[0]
            Version   = $Values[1]
            Commands  = $Values[2]
        })
}

$isEfNotInstalled = $true
if ($globalPkg.Count -gt 0) {
    foreach ($pkg in $globalPkg) {
        if ($pkg.PackageID -eq "dotnet-ef") {
            $isEfNotInstalled = $false
        }
    }
}

if ($isEfNotInstalled) {
    dotnet tool install --global dotnet-ef
}


#############################################################
### Get the last Db Snapshot and put it to BotANick.Core
#############################################################

### Source paths ###
$configSourcePath = "D:\actions-runner\Config"
$coreFiles = $configSourcePath + "\BotANick.Core"

### Target paths ###
$botanickCoreTargetFolder = $repositoryRootPath + "\BotANick.Core"

### Copy config files and DB to BotANick.Core folder ###
Copy-Item -Path ($coreFiles + "\*") -Destination $botanickCoreTargetFolder -Recurse
## db is already in BotANick.Core folder. It was done in the "BotANick.Deploy\GitHubRunner.Helpers\CopyDbAndConfig.ps1" command.

#############################################################
### Generate a migration script and update the database
#############################################################

dotnet ef migrations add "MigForDeploy" --project "BotANick.Core" --context "SqlLiteContext"

dotnet ef database update "MigForDeploy" --project "BotANick.Core" --context "SqlLiteContext"

#############################################################
### Save the new db snapshot ###
#############################################################

### Source paths ###
$botanickCoreSourceSnapshotFile = $repositoryRootPath + "\BotANick.Core\Migrations\SqlLiteContextModelSnapshot.cs"

### Target paths ###
$configSourcePath = "D:\actions-runner\Config"
$snapshotFile = $configSourcePath + "\BotANick.Core\Migrations\SqlLiteContextModelSnapshot.cs"

Copy-Item -Path $botanickCoreSourceSnapshotFile -Destination $snapshotFile -Recurse

#############################################################
### Replace the database in the publish folder for docker image
#############################################################

### Source paths ###
$dbSourcePath = $repositoryRootPath + "\BotANick.Core\botanick_data.db"

### Target paths ###
$binPath = $repositoryRootPath + "\BotANick.MainConsole\bin\Release\netcoreapp3.1\publish"
$dbTargetPath = $binPath

### Copy Db file ###
Copy-Item -Path $dbSourcePath -Destination $dbTargetPath -Recurse
