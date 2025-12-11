<#
.SYNOPSIS
    Common script supports module

.DESCRIPTION
    A lightweight and simple module for gathering useful functions.
#>
[string]$script:LocalDevEnvRoot = ''
[string]$script:SolutionDirectory = ''

function Get-LocalDevEnvRoot {
    [OutputType([string])]
    param ()
    
    if ([string]::IsNullOrEmpty($script:LocalDevEnvRoot)) {
        $script:SolutionDirectory = Split-Path -Path $PSScriptRoot -Parent
        $script:LocalDevEnvRoot = Join-Path -Path $script:SolutionDirectory -ChildPath '.ldenv'
    }
    
    return $script:LocalDevEnvRoot
}

function Invoke-PrintAcquiredLocalDevEnvRoot {
    Write-Host "  Local development enviroment root directory: $(Get-LocalDevEnvRoot)"
}

function Write-Title {
    param (
        [Parameter(Mandatory=$true, Position=0)]
        [string]$Text,
        
        [Parameter(Mandatory=$false, Position=2)]
        [int]$NewLines=1
    )
    
    for ($i = 0; $i -lt $NewLines; $i++) { 
        Write-Host '' 
    }
    
    Write-Host "=== $Text ==="
}

# prepare local dev env root path
Get-LocalDevEnvRoot

# check platform
if (-not ($IsWindows -or $IsLinux -or $IsMacOS)) {
    throw 'Platform not supported.'
}

# re-base location
Set-Location -LiteralPath $script:SolutionDirectory

Export-ModuleMember -Function * -Variable *