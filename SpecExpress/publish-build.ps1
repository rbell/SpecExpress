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
	#Package normal dll
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
	
	# Package MVC
	if (test-path $NuGetPackDir\SpecExpressMVC) {  
		remove-item -force -recurse $NuGetPackDir\SpecExpressMVC -ErrorAction SilentlyContinue | Out-Null
	}
	
	mkdir $NuGetPackDir\SpecExpressMVC
	
    cp "$src_directory\NuGetSpecs\SpecExpress.MVC3.nuspec" "$NuGetPackDir\SpecExpressMVC"

    mkdir "$NuGetPackDir\SpecExpressMVC\lib"
    cp "$release_directory\SpecExpress.MVC.dll" "$NuGetPackDir\SpecExpressMVC\lib"
    
    mkdir "$NuGetPackDir\SpecExpressMVC\content\Scripts"
    cp "$src_directory\SpecExpress.MVC.Example\Scripts\specexpress.unobtrusive.js" "$NuGetPackDir\SpecExpressMVC\content\Scripts"
    cp "$src_directory\SpecExpress.MVC.Example\Scripts\date.js" "$NuGetPackDir\SpecExpressMVC\content\Scripts"
    
	[xml] $Spec = gc "$NuGetPackDir\SpecExpressMVC\SpecExpress.MVC3.nuspec"
    $Spec.package.metadata.version = $version
    $Spec.package.metadata.dependencies.dependency.version = $version
    $Spec.Save("$NuGetPackDir\SpecExpressMVC\SpecExpress.MVC3.nuspec")
	
    exec { nuget pack "$NuGetPackDir\SpecExpressMVC\SpecExpress.MVC3.nuspec" -OutputDirectory "$NuGetPackDir" }

	#Package Silverlight
	if (test-path $NuGetPackDir\SpecExpressSL) {  
		remove-item -force -recurse $NuGetPackDir\SpecExpressSL -ErrorAction SilentlyContinue | Out-Null
	}
	
	mkdir $NuGetPackDir\SpecExpressSL
	
    cp "$src_directory\NuGetSpecs\SpecExpress.Silverlight.nuspec" "$NuGetPackDir\SpecExpressSL"

    mkdir "$NuGetPackDir\SpecExpressSL\lib"
    cp "$release_directory\SpecExpress.Silverlight.dll" "$NuGetPackDir\SpecExpressSL\lib"
    
	[xml] $Spec = gc "$NuGetPackDir\SpecExpressSL\SpecExpress.Silverlight.nuspec"
    $Spec.package.metadata.version = $version
    $Spec.package.metadata.dependencies.dependency.version = $version
    $Spec.Save("$NuGetPackDir\SpecExpressSL\SpecExpress.Silverlight.nuspec")
	
    exec { nuget pack "$NuGetPackDir\SpecExpressSL\SpecExpress.Silverlight.nuspec" -OutputDirectory "$NuGetPackDir" }

}

task Publish -depends PublishCodePlexRelease, PublishNuget

task PublishNuget {
    #We don't care if deleting fails..
    nuget delete SpecExpress $version -NoPrompt
    nuget delete SpecExpress.MVC3 $version -NoPrompt
    nuget delete SpecExpress.Silverlight $version -NoPrompt

    $PackageNames = gci "$NuGetPackDir\*.nupkg"
	foreach ($packageName in $PackageNames)
	{
      exec { nuget push $PackageName }
	}  
}

task PublishCodePlexRelease {
    $codeplexUser = Read-Host "Please enter CodePlex User Name:"
	$codeplexPassword = Read-Host "Please enter CodePlex Password:"
	$realeaseDescription = Read-Host "Please enter description for release:"
	$releaseName = "SpecExpress $version"
    $codeplexPath = resolve-path ".\tools\codeplex-api"
	[Reflection.Assembly]::LoadFrom("$codeplexPath\CodePlex.WebServices.Client.dll")
	[Reflection.Assembly]::LoadFrom("$codeplexPath\ccnet.codeplex.plugin.dll")
	[Reflection.Assembly]::LoadFrom("$codeplexPath\CodePlex.WebServices.Client.XmlSerializers.dll")
	$credentials = New-Object System.Net.NetworkCredential("$codeplexUser","$codeplexPassword")
	$client = New-Object CodePlex.WebServices.Client.ReleaseService
	$client.Credentials = $credentials
	$client.CreateARelease("specexpress","$releaseName","releaseNotes",[system.datetime]::now,[CodePlex.WebServices.Client.ReleaseStatus]::Planned, $false, $false)
	
	$releaseFiles = new-Object "System.Collections.ObjectModel.Collection``1[CodePlex.WebServices.Client.ReleaseFile]"
	$releaseFile = new-Object CodePlex.WebServices.Client.ReleaseFile
	$releaseFile.Name = "SpecExpress $version"
	$releaseFile.FileName = "SpecExpress-$version.zip"
	$releaseFile.FileType = [CodePlex.WebServices.Client.ReleaseFileType]::RuntimeBinary
	$releaseFile.FileDate = File.ReadAllBytes("$archive_directory\SpecExpress-$version.zip")
	$releaseFiles.Add($releaseFile)
	$client.UploadReleaseFiles("specexpress","$releaseName",$releaseFiles)
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


