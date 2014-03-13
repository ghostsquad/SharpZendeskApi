$debugpreference = "silentlycontinue"
#$debugpreference = "continue"

$models = gci .\SharpZendeskApi.Core\Models\I* -exclude *pagedresponse* | ?{-not $_.PSIsContainer}
foreach($model in $models)
{
	$newlines = @()
	$content = gc $model.fullname
	$modelName = $model.basename.SubString(1)
	
	write-output ([string]::format("this.TinyIoCContainer.Register<{0}, {1}>();", $model.basename, $modelName))	
}