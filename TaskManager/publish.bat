@echo off

rem Define the path to the .NET project
set "projectPath=C:\Users\User\source\repos\Real-Estate-Management-System-Ghaderi-Freema-\BackEnd\Real Estate Ghaderi Freema\Real Estate Ghaderi Freema\RealEstateMgmt.WebApi.csproj"
set "pscpPath=C:\Program Files\PuTTY\pscp.exe"
set "plinkPath=C:\Program Files\PuTTY\plink.exe"
echo projectPath is set to: %projectPath%

rem Define the output directory for the published files
set "outputPath=D:\Ghaderi\publish"

rem Define the configuration (Debug/Release)
set "configuration=Release"

rem Define the target framework (e.g., net6.0, net7.0)
set "framework=net7.0"

rem Define the remote server details
set "remoteUser=Silicon-Soft"
set "remoteHost=192.168.1.178"
set "remotePath=C:\nvhost\GhaderiBE"
set "password=Silicon321"  rem Replace with your actual password

rem Define the IIS site name
set "siteName=GhaderiBE"

rem Run the publish command
dotnet publish "%projectPath%" -c %configuration% -f %framework% -o "%outputPath%"

rem Check if the publish command was successful
if %ERRORLEVEL%==0 (
    echo Project published successfully to %outputPath%
    
    rem Stop the specific IIS site on the remote server
    "%plinkPath%" -pw %password% -batch %remoteUser%@%remoteHost% "appcmd stop site /site.name:%siteName%"

    rem Delete existing files on the remote server
    "%plinkPath%" -pw %password% -batch %remoteUser%@%remoteHost% "del /q %remotePath%\*.*"
    
    rem Check if the pscpPath exists
    if exist "%pscpPath%" (
        rem Copy the new files to the remote server
        "%pscpPath%" -r -pw %password% -batch "%outputPath%\*" %remoteUser%@%remoteHost%:%remotePath%
        
        rem Check if the pscp command was successful
        if %ERRORLEVEL%==0 (
            echo Files successfully copied to %remoteHost%:%remotePath%
            
            rem Restart the specific IIS site on the remote server
            "%plinkPath%" -pw %password% -batch %remoteUser%@%remoteHost% "appcmd start site /site.name:%siteName%"
        ) else (
            echo Failed to copy files to the remote server
        )
    ) else (
        echo pscp.exe not found at "%pscpPath%"
    )
) else (
    echo Failed to publish the project
)

pause
