@echo off

cd /d %~dp0
echo Current directory: %cd%

REM 设置项目路径
set "PROJECT_PATH=..\Lemon.ModuleNavigation.Sample.DesktopHosting\Lemon.ModuleNavigation.Sample.DesktopHosting.csproj"
echo Project directory: %PROJECT_PATH%

REM 设置发布配置
set "CONFIGURATION=Release"

REM 设置目标框架
set "FRAMEWORK=net8.0"

REM 设置目标运行时 aot发布暂不支持x86
set "RUNTIME=linux-musl-arm64"

REM 设置输出目录
set "OUTPUT_DIR=%RUNTIME%"

REM 是否自包含
set "SELF_CONTAINED=true"

REM 设置版本号
set "VERSION=1.0.1"
set "ASSEMBLY_VERSION=1.0.1.0"
set "FILE_VERSION=1.0.1.0"
set "INFORMATIONAL_VERSION=1.0.1"

REM 执行发布命令
dotnet publish "%PROJECT_PATH%" -c %CONFIGURATION% -f %FRAMEWORK% -r %RUNTIME% --self-contained %SELF_CONTAINED% -o %OUTPUT_DIR% -p:PublishReadyToRun=true -p:Version=%VERSION% -p:AssemblyVersion=%ASSEMBLY_VERSION% -p:FileVersion=%FILE_VERSION% -p:InformationalVersion=%INFORMATIONAL_VERSION%


echo Publish completed New Bee!
pause
