[CmdletBinding()]
Param(
  [Parameter(Mandatory=$true, Position=0)]
  [string]$Action
)

$Image="notimeforhero/authserver"
$Volume="${PWD}/Backend/settings:/app/settings:ro"
$RunName="authserver"

switch ($Action.ToLower()) {
	build {
		& docker build -f Dockerfile -t $Image .
	}
	start {
		& docker run --rm -p 3000:80 --name $RunName -v $Volume $Image
	}
	stop {
	    & docker stop $RunName
	}
	list {
		& docker run --rm -it --entrypoint bash --name $RunName -v $Volume $Image
	}
	default {
		Write-Host "Unknown action: $Action!"
	}
}