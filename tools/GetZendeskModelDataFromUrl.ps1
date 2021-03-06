param
(
	[string]$url
)

# Example
# GetZendeskModelDataFromUrl http://developer.zendesk.com/documentation/rest_api/ticket_forms.html | ft -autosize
function GetZendeskModelDataFromUrl
{
	param([string]$url)
	
	function Coalesce($a, $b) { if ($a -ne $null -and -not [string]::IsNullOrEmpty($a)) { $a } else { $b } }	
	
	function CoalesceAndTrim([string]$value, [string]$alt = [string]::Empty)
	{
		return (Coalesce $value $alt).Trim()
	}
	
	function CreateCustomObject
	{
		param($values)
		$obj = new-object System.Management.Automation.PSObject
		
		$obj | add-member -membertype noteproperty -Name Name -Value (CoalesceAndTrim $values[0])	
		
		if($values.Count -eq 4)
		{
			$values += $values[3]
			$values[3] = "no"
		}
		
		$secondColumnName = "Type"
		$thirdColumnName = "ReadOnly"
		
		switch($values.Count)
		{
			2 {	$secondColumnName = "Description" }
			3 { $thirdColumnName = "Description" }
		}				
		$obj | add-member -membertype noteproperty -Name $secondColumnName -Value (CoalesceAndTrim $values[1])
		if($values.Count -gt 2)
		{
			$obj | add-member -membertype noteproperty -Name $thirdColumnName -Value (CoalesceAndTrim $values[2] "no")
			if($values.Count -gt 3)
			{
				$obj | add-member -membertype noteproperty -Name Mandatory -Value (CoalesceAndTrim $values[3] "no")
				$obj | add-member -membertype noteproperty -Name Comment -Value (CoalesceAndTrim $values[4])
			}		
		}		
		
		return $obj
	}
	
	$r = Invoke-WebRequest $url
	
	$definitionTables = @($r.ParsedHtml.getElementsByTagName("table"))
	
	foreach($definitionTable in $definitionTables)
	{
		$collection = new-object System.Collections.ArrayList;
	
		[string]$desc = $definitionTable.previousSibling.innerText	
		Write-Host $desc -ForegroundColor Cyan
		
		$tableBodyRows = $definitionTable
		
		foreach($row in @($definitionTable.rows | Select -Skip 1))
		{
			$rowvalues = @()
			foreach($cell in $row.cells)
			{
				$rowvalues += (Coalesce $([string]$cell.InnerText).ToString() $[string]::Empty)
			}
			$collection.Add((CreateCustomObject $rowvalues)) | Out-Null
		}		
		
		$collection | Sort-Object Name | ft -AutoSize -Wrap | Out-Host
	}	
}

#if($myinvocation.invocationname -ne ".")
#{
	Write-Output (GetZendeskModelDataFromUrl $url)
#}