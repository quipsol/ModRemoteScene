param([switch]$Debug)
$ErrorActionPreference = "Stop"


# ----- SETUP -----

$ProjectRoot = (Get-Item $PSScriptRoot).Parent.Parent.FullName
Write-Host "Project Root: $ProjectRoot"
Set-Location $ProjectRoot


# Parse AssetSourcePath from local.props
$localProps = Join-Path $ProjectRoot "local.props"
if (-not (Test-Path $localProps)) {
    Write-Host "ERROR: local.props not found. Copy local.props.example to local.props first." -ForegroundColor Red
    exit 1
}
$xml = [xml](Get-Content $localProps)
$assetSourcePath = $xml.Project.PropertyGroup.AssetSourcePath
if (-not $assetSourcePath -or -not (Test-Path $assetSourcePath)) {
    Write-Host "ERROR: AssetSourcePath '$assetSourcePath' not found or does not exist. Check local.props." -ForegroundColor Red
    exit 1
}
Write-Host "Asset source: $assetSourcePath"

# Parse .godot/imported for spatlas and spskel files
$projectGodotImported = Join-Path $ProjectRoot ".godot/imported"
$assetsGodotImported = Join-Path $assetSourcePath ".godot/imported"
if (-not (Test-Path $projectGodotImported)) {
    Write-Host "ERROR: $projectGodotImported not found. Generate the .godot folder first." -ForegroundColor Red
    exit 1
}
if (-not (Test-Path $assetsGodotImported)) {
    Write-Host "ERROR: $assetsGodotImported not found. Check your decomp folder." -ForegroundColor Red
    exit 1
}


# ----- COPYING FILES -----

# Copy "default_bus_layout.tres" from root
$file = "default_bus_layout.tres"
Write-Host "Copying $file" -ForegroundColor Yellow
$source = Join-Path $assetSourcePath $file
$destination = Join-Path $ProjectRoot $file
Copy-Item $source $destination

# Copy files that in root that start with "--" and end in ".cs" or ".cs.uid"
Write-Host "Copying Compiler generated scripts" -ForegroundColor Yellow
$files = Get-ChildItem -Path $assetSourcePath -File |
        Where-Object { $_.Name -like "--*.cs" -or $_.Name -like "--*.cs.uid" }
foreach ($file in $files) {
    $source = $file.FullName
    $destination = Join-Path $ProjectRoot $file.Name
    if($Debug)  {
        Write-Host "Copying $source"
        Write-Host "     -> $destination"
    }
    Copy-Item -Path $source -Destination $destination
}

# Copy "spatlas" and "spskel" files from ".godot/imported"
Write-Host "Copying spatlas and spskel files from .godot/imported" -ForegroundColor Yellow
$files = Get-ChildItem -Path $assetsGodotImported -File |
        Where-Object { $_.Name -like "*.spatlas" -or $_.Name -like "*.spskel" }
foreach ($file in $files) {
    $source = $file.FullName
    $destination = Join-Path $projectGodotImported $file.Name
    if($Debug)  {
        Write-Host "Copying $source"
        Write-Host "     -> $destination"
    }
    Copy-Item -Path $source -Destination $destination
}



# Copy "project.godot" from root and modify it
$file = "project.godot"
Write-Host "Copying $file" -ForegroundColor Yellow
$source = Join-Path $assetSourcePath $file
$destination = Join-Path $ProjectRoot $file
Copy-Item $source $destination

# ----- MODIFYING PROJECT.GODOT -----

Write-Host "Modifying $file" -ForegroundColor Yellow
# Project name = root folder name, with spaces inserted before capital letters
$assembyName = (Get-Item $ProjectRoot).Name
$projectName = (Get-Item $ProjectRoot).Name -creplace '(?<!^)([A-Z])', ' $1'
if($Debug)
{
    Write-Host "Assembly Name: $assembyName"
    Write-Host "Project  Name: $projectName"
}

$content = Get-Content $destination
$result = [System.Collections.Generic.List[string]]::new()
$currentSection = ""

foreach ($line in $content) {
    # Get current section name
    if ($line -match '^\[(.+)\]$')
    {
        $currentSection = $Matches[1]
    }

    # Adding new things at the beginning
    if ($line -eq "[autoload]") {
        $result.Add($line)
        $result.Add("")
        $result.Add('RuntimeBootstrap="*res://editor/GameRegistrationBootstrap/RuntimeBootstrap.cs"')
        continue
    }
    if ($line -eq "[display]") {
        $result.Add($line)
        $result.Add("")
        $result.Add('window/size/window_width_override=1280')
        $result.Add('window/size/window_height_override=720')
        continue
    }
    
    # Modifying things in context
    if($currentSection -eq "application")
    {
        if($line -match '^\s*config/name\s*='){
            $result.Add('config/name="' + $projectName + '"')
            continue
        }
    }
    
    if($currentSection -eq "autoload")
    {
        if ($line -match '^\s*SentryBootstrap\s*=') {
            continue
        }
    }
    if($currentSection -eq "display")
    {
        if ($line -match '^\s*window/size/window_width_override\s*=') {
            continue
        }
        if ($line -match '^\s*window/size/window_height_override\s*=') {
            continue
        }
    }
    if($currentSection -eq "dotnet")
    {
        if ($line -match '^\s*project/assembly_name\s*=') {
            $result.Add('project/assembly_name="' + $assembyName + '"')
            continue
        }
        if ($line -match '^\s*project/solution_directory\s*=') {
            continue
        }
    }

    if($currentSection -eq "sentry")
    {
        if ($line -match '^\s*config/dsn\s*=') {
            continue
        }
        if ($line -match '^\s*config/disabled_in_editor\s*=') {
            $result.Add('config/disabled_in_editor=false')
            continue
        }
    }

    if($currentSection -eq "editor_plugins")
    {
        if ($line -match '^\s*enabled\s*=') {
            # Add a new entry as the first plugin to run
            $line = $line -replace 'PackedStringArray\(', 'PackedStringArray("res://editor/GameRegistrationBootstrap/plugin.cfg", '
            $result.Add($line)
            continue
        }
    }

    $result.Add($line)
}
Set-Content -Path $destination -Value $result


# ----- FIN -----


Write-Host "`Files imported and godot.project modified" -ForegroundColor Green