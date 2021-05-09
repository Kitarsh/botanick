$Table = docker ps -a

$containers = New-Object -TypeName System.Collections.ArrayList
$Table | 
Select-Object -Skip 1 | # skip the header row
ForEach-Object {
    # Split on space(s)
    $Values = $_ -split '\s+' 
    # Put together object and store in $Objects array
    $containers.Add([PSCustomObject]@{
            ContainerID = $Values[0]
            Image       = $Values[1]
            Command     = $Values[2]
            Created     = $Values[3]
            Status      = $Values[4]
            Ports       = $Values[5]
            Names       = $Values[6]
        })
}

if ($containers.Count -gt 0) {
    Foreach ($container in $containers) {
        if ($container.Image = "botanick") {
            docker stop $container.ContainerID
            docker rm $container.ContainerID
        }
    }
}

### Delete old images ###
docker rmi $(docker images -f “dangling=true” -q)

docker run -d --name nervous_shockley -v botanick_data.db:/App botanick