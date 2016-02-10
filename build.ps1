# Run from an isolated shell (ex. powershell .\build)
$binDir = "bin"
$srcDir = "src"
$outFile = "CSBuild.dll"
# Path to System.Management.Automation.dll
$psLibDir = "C:\Program Files (x86)\Reference Assemblies\Microsoft\WindowsPowerShell\3.0"

# We cannot write to the outFile if we have it imported
if (Test-Path $binDir\temp.dll) {
    rm $binDir\temp.dll
}
cp $binDir\$outFile $binDir\temp.dll

# Import the temporary dll
Import-Module .\$binDir\temp.dll -Scope Local -DisableNameChecking

# Create the CSBuild Object
$b = New-CSBuild

# Add build options
$b.Options += "out:$binDir\$outFile"
$b.Options += "target:library"
$b.Options += "nologo"

# Add library directories
$b.LibDirectories += $psLibDir

# Add references
$b.References += "System.Management.Automation.dll"

# Find and add source files
# We could also just do
# $b.Sources += "$srcDir\*.cs"
# but this way serves as en example of the powerful things that can be
# accomplished with Powershell.
Get-ChildItem .\$srcDir -Filter *.cs | % {
    $b.Sources += "$srcDir\$_"
}

# Compile
Compile-CSBuild $b
