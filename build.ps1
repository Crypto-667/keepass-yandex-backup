# Simple build script
$KeePassPath = "C:\Program Files\KeePass Password Safe 2"
$ProjectDir = $PSScriptRoot
$OutputDir = "$ProjectDir\src\bin\Debug"

Write-Host "Building Yandex.Disk Backup Plugin..." -ForegroundColor Green

# Clean
if (Test-Path $OutputDir) {
    Remove-Item "$OutputDir\*" -Recurse -Force
}

# Build
dotnet build src/YandexDiskBackup.csproj --configuration Debug

if ($LASTEXITCODE -eq 0) {
    $dll = "$OutputDir\YandexDiskBackup.dll"
    
    if (Test-Path $dll) {
        # Install
        $pluginsDir = "$KeePassPath\Plugins"
        if (!(Test-Path $pluginsDir)) {
            New-Item -ItemType Directory -Path $pluginsDir -Force | Out-Null
        }
        
        Copy-Item $dll $pluginsDir -Force
        
        Write-Host "SUCCESS! Plugin installed." -ForegroundColor Green
        Write-Host "Location: $pluginsDir" -ForegroundColor White
        Write-Host ""
        Write-Host "Restart KeePass to load the plugin." -ForegroundColor Yellow
    } else {
        Write-Host "ERROR: DLL not found after build" -ForegroundColor Red
    }
} else {
    Write-Host "BUILD FAILED!" -ForegroundColor Red
}
