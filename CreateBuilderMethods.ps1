param
(
	[string]$filter
)

Function Pause($M="Press any key to continue . . . "){If($psISE){$S=New-Object -ComObject "WScript.Shell";$B=$S.Popup("Click OK to continue.",0,"Script Paused",0);Return};Write-Host -NoNewline $M -foregroundcolor green;$I=16,17,18,20,91,92,93,144,145,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183;While($K.VirtualKeyCode -Eq $Null -Or $I -Contains $K.VirtualKeyCode){$K=$Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")};Write-Host}

$debugpreference = "silentlycontinue"
#$debugpreference = "continue"

$models = gci .\SharpZendeskApi.Core\Models\I* -exclude *pagedresponse* | ?{-not $_.PSIsContainer}
foreach($model in $models)
{
	$newlines = @()
	$content = gc $model.fullname
	$modelName = $model.basename.SubString(1)
	
	if($modelName -notlike "*$filter*")
	{
		continue;
	}
	
	write-host ""
	write-host $modelname -foregroundcolor yellow
	write-host ""
	
	$newlines += `
@"
namespace SharpZendeskApi.Core.Handlers
{
using System;
using System.Collections.Generic;
using SharpZendeskApi.Core.Models;
"@
	
	$newlines += [string]::format("public class {0}Builder : ModelBuilderBase<{1}>",$modelName,$model.basename)
	$newlines += "{"
	
	$newlines += [string]::format("public override {0} Build()`n{{`nreturn ({0})this.BuildObject();`n}}`n", $model.basename)
	
	$prevReadOnly = $false;
	$props = 0;
	
	foreach($line in $content)
	{		
		write-debug $line		
		$matches = [regex]::Matches($line,"^\s+([^\s/]+) ([^\s]+)\s+[{get;}\s]+\s*$")		
						
		if($matches.count -gt 0 -and -not $prevReadOnly)
		{
			write-debug ":::::::::::prop found"
			$props++;
			$propType = $matches[0].Groups[1].Value
			$propName = $matches[0].Groups[2].Value
			#$propNameCamel = $propName.Substring(0,1).ToLower() + $propName.Substring(1)
		
			$newlines += [string]::format("public {2}Builder With{1}({0} value)`n{{`nthis.ThrowIfBuilt();`n(({2})this.objectToBuild).{1} = value;`nreturn this;`n}}",$propType,$propName,$modelName)
		}
		
		$prevReadOnly = [regex]::IsMatch($line,"^\s+\[ReadOnly\]$")
		if($prevReadOnly)
		{
			write-debug ":::::::::::readonly found"
		}
	}
		
	$newlines += "}"
	$newlines += "}"
	
	if($props -gt 0)
	{
		$filename = [string]::format(".\SharpZendeskApi.Core\Handlers\{0}Builder.cs",$modelName)
	
		write-host ""
		write-host "writing file $filename" -foregroundcolor green
		
		$newlines | Out-File $filename -encoding UTF8
	}
	else
	{
		write-warning "not writing builder for $modelName - all properties are readonly!"
	}
		
	write-host ""
	Pause
}