@echo off
setlocal enabledelayedexpansion

echo ==========================================
echo Generating Unity .meta files for .cs files
echo ==========================================

for /r %%f in (*.cs) do (
    if not exist "%%f.meta" (
        
        for /f %%g in ('powershell -command "[guid]::NewGuid().ToString(\"N\")"') do set GUID=%%g
        
        (
        echo fileFormatVersion: 2
        echo guid: !GUID!
        echo MonoImporter:
        echo   externalObjects: {}
        echo   serializedVersion: 2
        echo   defaultReferences: []
        echo   executionOrder: 0
        echo   icon: {instanceID: 0}
        echo   userData:
        echo   assetBundleName:
        echo   assetBundleVariant:
        ) > "%%f.meta"

        echo Created meta for: %%f
    )
)

echo ==========================================
echo Done
echo ==========================================

pause