@echo off

REM 检查是否安装了 .NET SDK
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
echo 未检测到 .NET SDK，请先安装 .NET 6.0 或更高版本的 SDK。
echo 您可以从以下链接下载：
echo https://dotnet.microsoft.com/download/dotnet/6.0
pause
exit /b 1
)

REM 编译项目
echo 正在编译项目...
dotnet build -c Release
if %errorlevel% neq 0 (
echo 编译失败，请检查错误信息。
pause
exit /b 1
)

REM 运行程序
echo 编译成功，正在启动程序...
start "桌面时钟" /d "bin\Release\net6.0-windows" "DesktopClockWidget.exe"

REM 退出批处理
exit /b 0