
IF "%tests_to_run%"=="" GOTO TestsError

@echo off


set nunit_path="C:\Program Files (x86)\NUnit 2.5.10\bin\net-2.0\nunit-console"

set timestamp=%date:~10,4%_%date:~7,2%_%date:~4,2%__%time:~0,2%_%time:~3,2%_%time:~6,2%
set xml_output=C:\TestResults\%log_label%_%timestamp%.xml

REM set assembly=C:\Users\gaweir\Documents\Visual Studio 2010\Projects\TestAutomation\TestAutomation\bin\Debug\TestAutomation.dll
set assembly=C:\LendingTree\TestAutomation\TestAutomation.dll

set log_file=C:\TestResults\%log_label%_%timestamp%.log

REM In 2.6.x, the below command changes from /run to /test.
@echo on

%nunit_path% /nologo /run:%tests_to_run% /xml:"%xml_output%" "%assembly%" > "%log_file%"

@echo off
GOTO End

:TestsError

@echo on

ECHO ERROR: tests_to_run variable was not set! Do not run this batch file directly.
SET ERRORLEV=1
EXIT

:End