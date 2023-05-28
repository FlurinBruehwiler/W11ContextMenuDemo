cd "C:\Program Files (x86)\Windows Kits\10\bin\10.0.22000.0\x64\"
.\makeappx.exe pack /d "C:\Programming\Github\W11ContextMenuDemo\ContextMenuRegistrationPkg" /p "C:\Programming\Github\W11ContextMenuDemo\ContextMenuConsoleApp\mypackage.msix" /nv
cd "C:\Program Files (x86)\Windows Kits\10\bin\10.0.22000.0\x64"
.\signtool.exe sign /fd SHA256 /a /f "C:\Programming\Github\W11ContextMenuDemo\MyPFX.pfx" /p 123 "C:\Programming\Github\W11ContextMenuDemo\ContextMenuConsoleApp\mypackage.msix"
Get-AppxPackage "ContextMenuRegistration" -AllUsers | Remove-AppxPackage