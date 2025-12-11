<#
.SYNOPSIS
    Setup local development environment tool.

.DESCRIPTION
    An automated tool that sets up a local development environment for building and developing this project.

.PARAMETER BaseGameDirectory
    Sets the directory where the game is installed.
    This option is used to create a symbolic link, and is required if no symbolic link has already been created.
    
    If you want to perform initial setup, you must provide this option.
    
    If a symbolic link already exists, this option will unlink the existing symbolic link and reset it to the specified path.
    
    Alias: -gamedir

.PARAMETER Initialize
    Completely removes the previously set local development environment information and attempts to set it up again from scratch.
    If this option is not specified or set to false, it defaults to 'Update' mode.
    
    Alias: -init
#>
using namespace System

[CmdletBinding()]
param (
    [Alias('gamedir')]
    [Parameter(Mandatory=$false, Position=0)]
    [string]$BaseGameDirectory,
    
    [Alias('init')]
    [Parameter(Mandatory=$false, Position=1)]
    [switch]$Initialize
)

#Requires -Version 7.4
$ErrorActionPreference = 'Stop'

# -- import share module
Import-Module -Name (Join-Path $PSScriptRoot '.shared.psm1') -Force -Scope Local

[string]$script:BaseGameDirectory = $BaseGameDirectory
[switch]$script:Initialize = $Initialize

# -- constants
[string]$script:CompanyName = 'Klei'
[string]$script:ProductName = 'Oxygen Not Included'
[string]$script:NormalizedName = ($script:ProductName).Replace(' ', '')

[string]$script:ReferencesFile = 'References.json'

[string[]]$script:SymbolicLinks = @('.gamedir', '.logsdir', '.modsdir')

# -- external dependencies
[hashtable]$script:ExternalDependencies = @{
    PLib = @{
        BaseURL = "https://github.com/peterhaneve/ONIMods/releases/download/PLib4.19/";
        Link = @(
            'PLib.dll',
            'PLib.xml'
        );
    }
}




# ---------------------------------------------------------
# 
#                   BEGIN FUNCTION
# 
# ---------------------------------------------------------

#region Functions
function Invoke-OnPrepareScript {
    Invoke-PrintAcquiredLocalDevEnvRoot
    
    if ($script:Initialize) {
        Write-Host '  Clean setup starting...'
        
        if (Test-Path -LiteralPath $script:LocalDevEnvRoot -PathType Container) {
            Remove-Item -Path $script:LocalDevEnvRoot -Recurse -Force
        }
        
        foreach ($v in $script:SymbolicLinks) {
            $tempFile = Join-Path $script:LocalDevEnvRoot $v
            if (Test-Path -LiteralPath $tempFile -PathType Leaf) {
                Remove-Item -LiteralPath $tempFile -Force
            }
        }
    }
    
    $__lib = Join-Path $script:LocalDevEnvRoot 'lib'
    $__share = Join-Path $script:LocalDevEnvRoot 'share'
    foreach ($v in @($script:LocalDevEnvRoot, $__lib, $__share)) {
        if (-not (Test-Path -LiteralPath $v -PathType Container)) {
            Write-Host "  Creating directory -> '$v'"
            New-Item -Path $v -ItemType Directory -Force | Out-Null
        }
    }
    
    $__nameNoSpace = $script:NormalizedName
    if ([string]::IsNullOrWhiteSpace($script:BaseGameDirectory)) {
        $__steamapps = [Environment]::GetFolderPath('LocalApplicationData')
        if ($IsWindows) {
            $__steamapps = [Environment]::GetFolderPath('ProgramFilesX86')
        }
        
        $script:BaseGameDirectory = Join-Path $__steamapps 'Steam' 'steamapps' 'common' $__nameNoSpace
        if (-not (Test-Path -LiteralPath $script:BaseGameDirectory -PathType Container)) {
            Write-Warning '  The base game directory path is not specified, and the auto-detected path does not contain an actual directory, so an empty value is used.'
            $script:BaseGameDirectory = ''
        }
    }
    
    Write-Host "  [ OK ] Ready!"
}

function Update-SymbolicLink {
    param (
        [Parameter(Mandatory=$true, Position=0)]
        [string]$FileName,
        
        [Parameter(Mandatory=$false, Position=1)]
        [string]$NewLink,
        
        [Parameter(Mandatory=$false, Position=2)]
        [switch]$Force
    )
    
    if (-not $NewLink -or -not (Test-Path -LiteralPath $NewLink -PathType Container)) {
        if ($script:Initialize) { throw "Path not found. Path: $NewLink" }
        return
    }
    
    $tempFile = Join-Path $script:LocalDevEnvRoot $FileName
    if (Test-Path -LiteralPath $tempFile -PathType Leaf) {
        if (-not $Force) { return }
        Remove-Item -LiteralPath $tempFile -Force | Out-Null
    }
    
    New-Item -Path $tempFile -ItemType SymbolicLink -Value $NewLink -Force | Out-Null
}

function Invoke-CreateSymbolicLinks {
    [string]$defLogs = ''
    [string]$defMods = ''
    if ($IsWindows) {
        [string]$__home = [Environment]::GetFolderPath('UserProfile')
        [string]$__docs = [Environment]::GetFolderPath('MyDocuments')
        
        $defLogs = Join-Path $__home 'AppData' 'LocalLow' $script:CompanyName $script:ProductName
        $defMods = Join-Path $__docs $script:CompanyName $script:NormalizedName
    } else {
        [string]$__appdata = [Environment]::GetFolderPath('ApplicationData')
        
        if ($IsLinux) {
            $__appdata = Join-Path $__appdata 'unity3d' $script:CompanyName $script:ProductName
            
            $defLogs = $__appdata
            $defMods = $__appdata
        } else {
            [string]$__library = Split-Path -Path $__appdata -Parent -Resolve
            
            $defMods = Join-Path $__appdata "unity.$($script:CompanyName).$($script:ProductName)"
            $defLogs = Join-Path $__library 'Logs' $script:CompanyName $script:ProductName
        }
    }
    
    $defMods = Join-Path $defMods 'mods'
    
    if (-not (Test-Path -LiteralPath $defMods -PathType Container)) {
        throw "Local mods directory not exists. Path: $defMods"
    }
    
    if (-not (Test-Path -LiteralPath $defLogs -PathType Container)) {
        throw "Player log directory not exists. Path: $defLogs"
    }
    
    Update-SymbolicLink -FileName $script:SymbolicLinks[0] -NewLink $script:BaseGameDirectory -Force
    Update-SymbolicLink -FileName $script:SymbolicLinks[1] -NewLink $defLogs
    Update-SymbolicLink -FileName $script:SymbolicLinks[2] -NewLink $defMods
    
    # change to real-path
    $__items = New-Object -TypeName System.Collections.Generic.List[string]
    foreach ($v in $script:SymbolicLinks) {
        $__filename = Join-Path $script:LocalDevEnvRoot $v
        if (-not (Test-Path -LiteralPath $__filename -PathType Container)) {
            throw "Symbolic link file not found. Path: $__filename"
        }
        
        Write-Host "  [ OK ] $__filename"
        $__items.Add($__filename)
    }
    
    $script:SymbolicLinks = $__items
}

function Invoke-MakeLibrariesReference {
    $__jsonTarget = Join-Path $PSScriptRoot $script:ReferencesFile
    $__references = (Get-Content -LiteralPath $__jsonTarget -Raw) | ConvertFrom-Json
    
    $__gameNameNoSpace = $script:NormalizedName
    $__gameDataManaged = ''
    if ($IsWindows -or $IsLinux) {
        $__gameDataManaged = Join-Path $script:SymbolicLinks[0] "$($__gameNameNoSpace)_Data" 'Managed'
    } else {
        $__gameDataManaged = Join-Path $script:SymbolicLinks[0] "$($__gameNameNoSpace).app" 'Contents' 'Resources' 'Data' 'Managed'
    }
    
    $__references | Get-Member -MemberType NoteProperty | ForEach-Object {
        $__key = $_.Name
        $__val = $__references.$__key
        
        $__tempDstDir = Join-Path $script:LocalDevEnvRoot $__key
        foreach ($dllname in $__val) {
            $linkSource = Join-Path $__gameDataManaged $dllname
            if (-not (Test-Path -LiteralPath $linkSource -PathType Leaf)) {
                throw "Source DLL not found. Path: $linkSource"
            }
            
            $linkDestination = Join-Path $__tempDstDir $dllname
            if (Test-Path -LiteralPath $linkDestination -PathType Leaf) {
                Remove-Item -LiteralPath $linkDestination -Force | Out-Null
            }
            
            New-Item -Path $linkDestination -ItemType SymbolicLink -Value $linkSource -Force | Out-Null
            Write-Host "  Linked from $linkDestination --> to $linkSource"
        }
    }
}

function Invoke-DownloadExternalDependencies {
    if (-not $script:Initialize) {
        Write-Host "  Skipping because this is not the initial setup..."
        return
    }
    
    Write-Host "  Start downloading..."
    foreach ($val in $script:ExternalDependencies.Values) {
        if ($val -isnot [hashtable]) {
            Write-Warning "  External dependency directive of incorrect type."
            continue
        }
        
        if (-not $val.ContainsKey('BaseURL') -or -not $val.ContainsKey('Link')) {
            Write-Warning "  Missing field";
            continue
        }
        
        foreach ($f in $val.Link) {
            $__uri = $val.BaseURL + $f
            
            $__out = Join-Path $script:LocalDevEnvRoot 'lib' $f
            
            Write-Host "   Downloading $__uri  -->  $__out"
            Invoke-WebRequest -Uri $__uri -OutFile $__out
        }
    }
}
#endregion

#region Script Main
Write-Title 'Prepare Setup' -NewLines 0
Invoke-OnPrepareScript

Write-Title 'Update Symbolic Links'
Invoke-CreateSymbolicLinks

Write-Title 'Update Libraries Reference'
Invoke-MakeLibrariesReference

Write-Title 'Update external dependencies'
Invoke-DownloadExternalDependencies

Write-Title 'Task Complete' -NewLines 2
Write-Host 'done!' -ForegroundColor Green
#endregion