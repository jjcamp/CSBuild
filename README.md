# About
CSBuild leverages the dynamism of Powershell as a way to create build files for
C# projects.

CSBuild was not created because there is a lack of build tools available (quite
the opposite), but because I work in an environment with no access to any
programming tools except for the CSC compiler.

# Compiling
For the first compile, The project must be built with csc.exe.

Example:
`C:\Windows\Microsoft.NET\Framework\[version]\csc.exe /target:library
/out:build\CSBuild.dll /reference:"[Path to Powershell
References]\Windows.Management.Automation" .\src\*.cs`
Where `[version]` is the .Net Framework version of choice and`[Path to
Powershell References]` is where your Powershell .Net libraries reside.

For subsequent compiles, CSBuild itself can be used.  However, since the build script is compiling a module that it also uses, you will want to run the buildscript in its own powershell session.

`powershell .\build`

The included build script creates a temporary copy of the dll, which it then imports into the shell. If the build script is not run in its own shell, then subsequent runs of the script in the same shell will produce an error (cannot delete temp.dll because it is in use).

# Use
Once the module is compiled, you can install it to a powershell module directory
or call it directly from its local path.

Example `build.ps1`:
```posh
Import-Module CSBuild

$build = New-CSBuild
$build.Sources += "*.cs"
$build.Options += "out:example.exe"

Compile-CSBuild $build
```

For another example, check out the project's `build.ps1`.  Also, you can run
`New-CSBuild` in Powershell to see the CSBuild object.
