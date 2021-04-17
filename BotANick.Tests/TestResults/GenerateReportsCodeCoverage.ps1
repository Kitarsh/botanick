Get-ChildItem -Recurse -Filter "*.coverage" | % {
    $outfile = "$([System.IO.Path]::GetFileNameWithoutExtension($_.FullName)).coveragexml"
    $output = [System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($_.FullName), $outfile)
    "Analyse '$($_.Name)' with output '$outfile'..."
    . ".\Dynamic Code Coverage Tools\CodeCoverage.exe" analyze /output:$output $_.FullName
}