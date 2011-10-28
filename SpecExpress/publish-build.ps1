properties { 
  
	#Paths
	$base_directory		=	resolve-path .
	$src_directory 		=   "$base_directory\src"	
	$build_directory 	=	"$base_directory\build"
	$release_directory 	=	"$base_directory\deploy"
	$tools_directory 	=	"$base_directory\tools"
	$archive_directory 	=	"$base_directory\archive"
	$NuGetPackDir		=   "$base_directory\nuget"

  # ****************  CONFIGURE ****************     	
    #Version
	$revision = Get-RevisionFromGit $src_directory	
	$version = "1.6.1.$revision"
}

$framework = '4.0'
task default -depends PackNuget

task PackNuget {
	if (test-path $NuGetPackDir\SpecExpress) {  
		remove-item -force -recurse $NuGetPackDir\SpecExpress -ErrorAction SilentlyContinue | Out-Null
	}
	
	mkdir $NuGetPackDir\SpecExpress
	
    cp "$src_directory\NuGetSpecs\SpecExpress.nuspec" "$NuGetPackDir\SpecExpress"

    mkdir "$NuGetPackDir\SpecExpress\lib"
    cp "$release_directory\SpecExpress.dll" "$NuGetPackDir\SpecExpress\lib"
    
	[xml] $Spec = gc "$NuGetPackDir\SpecExpress\SpecExpress.nuspec"
    $Spec.package.metadata.version = $version
    $Spec.Save("$NuGetPackDir\SpecExpress\SpecExpress.nuspec")
	
    exec { nuget pack "$NuGetPackDir\SpecExpress\SpecExpress.nuspec" -OutputDirectory "$NuGetPackDir" }
}

task PublishNuget -depends PackNuget {
    $PackageName = gci *.nupkg
    #We don't care if deleting fails..
    nuget delete $NuGetPackageName $version -NoPrompt
    exec { nuget push $PackageName }
}

function Get-RevisionFromGit([string]$path) {
	Write-Host "Fetchin revision from Git"
	# git describe will return a string in the format of <last tag>-<number of commits since tag>-<short guid>
	$ver = Invoke-Expression 'git describe'
	# Get revision by matching the <number of cimmits since last tag> portion of the description.
	$isMatch = $ver -match "(?<=-)\d*"
	if ($isMatch )
	{
		return $matches[0]
	}
	else
	{
		return ""
	}
}


