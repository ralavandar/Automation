--********************************************************
-- TestDataInsert_ProdTests_Forms_PersonalLoan.sql
--********************************************************

-- Script to insert values for Form tests in QA Personal Loan
-- This data is designed so that it does not receive referrals in Production
--		Million Dollar Mobile Home in Montana
--		Secondary/Investment Property
--		Current Bankruptcy
--		Current Foreclosure
-- SELECT * FROM dbo.tTestData_PersonalLoan with(nolock) WHERE TestCaseName like 'QA_%'

-- ************************************
-- ***** Insert PersonalLoan Tests    *****
-- ************************************

INSERT [dbo].[tTestData_PersonalLoan] 
(
--insert Columns Below this line
[TestCaseName]
,[TestEnvironment]
,[BrowserType]
,[Template]
,[Variation]
,[QueryString]
,[LoanAmountRequested]
,[HomeDescription]
,[EmailAddress]
,[Password]
,[ConfirmPassword]
,[BorrowerFirstName]
,[BorrowerMiddleName]
,[BorrowerLastName]
,[BorrowerSuffix]
,[BorrowerStreetAddress]
,[BorrowerZipCode]
,[BorrowerCity]
,[BorrowerHomePhone1]
,[BorrowerHomePhone2]
,[BorrowerHomePhone3]
,[BorrowerWorkPhone1]
,[BorrowerWorkPhone2]
,[BorrowerWorkPhone3]
,[BorrowerWorkPhoneExt]
,[BorrowerSsn1]
,[BorrowerSsn2]
,[BorrowerSsn3]
,[DateOfBirthMonth]
,[DateOfBirthDay]
,[DateOfBirthYear]
,[EmploymentStatus]
,[CreditProfile]
,[BankruptcyYesNo]
,[BorrowerIncome]

) 
VALUES 
(
--insert Values Below this line
N'personalLoan_01_LowestLoan',
N'DEV',
N'random',
N'pla',
N'',
N'',
N'1000',
N'',
N'qa@lendingtree.com', 
N'default',
N'',
N'Otto',
N'',
N'Tester',
N'',
N'11115 Rushmore Dr',
N'28277',
N'Charlotte',
N'704',
N'939',
N'3462',
N'',
N'',
N'',
N'',
N'default',
N'default',
N'default',
N'10',
N'12',
N'1976',
N'Full-time',
N'EXCELLENT',
N'N',
N'100000'
)
