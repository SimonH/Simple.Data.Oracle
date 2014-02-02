@echo off
md "%~dp0Simple.Data.Oracle\lib\net40"
md "%~dp0Simple.Data.Oracle.Devart\lib\net40"
md "%~dp0Simple.Data.Oracle.ManagedDataAccess\lib\net40"
md "%~dp0Simple.Data.Oracle\App_Readme"
md "%~dp0Simple.Data.Oracle.Devart\App_Readme"
md "%~dp0Simple.Data.Oracle.ManagedDataAccess\App_Readme"

copy /y "%~dp0..\Simple.Data.Oracle\bin\Release\Simple.Data.Oracle.dll" "%~dp0Simple.Data.Oracle\lib\net40"
copy /y "%~dp0..\Simple.Data.Oracle\bin\DevartRelease\Simple.Data.Oracle.Devart.dll" "%~dp0Simple.Data.Oracle.Devart\lib\net40"
copy /y "%~dp0..\Simple.Data.Oracle\bin\ManagedOdpRelease\Simple.Data.Oracle.ManagedDataAccess.dll" "%~dp0Simple.Data.Oracle.ManagedDataAccess\lib\net40"
copy /y "%~dp0..\readme.md" "%~dp0Simple.Data.Oracle\App_Readme\Simple.Data.Oracle.readme.md"
copy /y "%~dp0..\readme.md" "%~dp0Simple.Data.Oracle.Devart\App_Readme\Simple.Data.Oracle.Devart.readme.md"
copy /y "%~dp0..\readme.md" "%~dp0Simple.Data.Oracle.ManagedDataAccess\App_Readme\Simple.Data.Oracle.ManagedDataAccess.readme.md"
