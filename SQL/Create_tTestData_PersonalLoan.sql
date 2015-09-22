--********************************************************
--* Create_tTestData_PersonalLoan.sql
--********************************************************

-- Script to create table for Fossa Mortgage tests

USE [AutomatedTesting]
GO

--DELETE from AutomatedTesting.dbo.tTestData_PersonalLoan
DROP TABLE AutomatedTesting.dbo.tTestData_PersonalLoan

-- Script to create the Test Data Table for Personal Loan
CREATE TABLE tTestData_PersonalLoan (
	[TestCaseID] [int] PRIMARY KEY IDENTITY,
	[TestCaseName] [varchar] (100) NOT NULL,
	[TestEnvironment] [varchar] (10) NOT NULL,
	[BrowserType] [varchar] (20) NULL,
	[Template] [varchar] (20) NULL,
	[Variation] [varchar] (100) NULL,
	[QueryString] [varchar] (200) NULL,
	[LoanPurpose] [varchar] (50) NULL,
	[LoanAmountRequested] [varchar] (20) NULL,
	[EmailAddress] [varchar] (50) NULL,
	[Password] [varchar] (20) NULL,
	[ConfirmPassword] [varchar] (20) NULL,
	[BorrowerFirstName] [varchar] (50) NULL,
	[BorrowerMiddleName] [varchar] (50) NULL,
	[BorrowerLastName] [varchar] (50) NULL,
	[BorrowerSuffix] [varchar] (10) NULL,
	[BorrowerStreetAddress] [varchar] (50) NULL,
	[BorrowerZipCode] [varchar] (12) NULL,
	[BorrowerCity] [varchar] (50) NULL,
	[BorrowerHomePhone1] [varchar] (7) NULL,
	[BorrowerHomePhone2] [varchar] (7) NULL,
	[BorrowerHomePhone3] [varchar] (7) NULL,
	[BorrowerWorkPhone1] [varchar] (7) NULL,
	[BorrowerWorkPhone2] [varchar] (7) NULL,
	[BorrowerWorkPhone3] [varchar] (7) NULL,
	[BorrowerWorkPhoneExt] [varchar] (10) NULL,
	[BorrowerSsn1] [varchar] (7) NULL,			-- [varchar](7) allows for a value of 'default'
	[BorrowerSsn2] [varchar] (7) NULL,	
	[BorrowerSsn3] [varchar] (7) NULL,	
	[DateOfBirthMonth] [varchar] (7) NULL,
	[DateOfBirthDay] [varchar] (7) NULL,
	[DateOfBirthYear] [varchar] (7) NULL,
	[EmploymentStatus] [varchar] (50) NULL,
	[Residence] [varchar] (10) NULL,
	[CreditProfile] [varchar] (20) NULL,
	[BankruptcyYesNo] [char] (1) NULL,
	[BorrowerIncome] [varchar] (50) NULL,
	[CreditPullSuccessYesNo] [char] (1) NULL
	)
GO


