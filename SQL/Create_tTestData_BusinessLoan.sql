--********************************************************
--* Create_tTestData_BusinessLoan.sql
--********************************************************

-- Script to create table for Fossa BizLoan tests
USE [AutomatedTesting]
GO

--DELETE from AutomatedTesting.dbo.tTestData_PersonalLoan
--DROP TABLE AutomatedTesting.dbo.tTestData_BusinessLoan

-- Script to create the test data table
CREATE TABLE AutomatedTesting.dbo.tTestData_BusinessLoan (
	[TestCaseID] [int] PRIMARY KEY IDENTITY,
	[TestCaseName] [varchar] (100) NOT NULL,
	[TestEnvironment] [varchar] (10) NOT NULL,
	[BrowserType] [varchar] (20) NULL,
	[Template] [varchar] (20) NULL,
	[Variation] [varchar] (100) NULL,
	[QueryString] [varchar] (200) NULL,
	[BusinessType] [varchar] (50) NULL,
	[LoanAmountRequested] [varchar] (20) NULL,
	[InceptionMonth] [varchar] (7) NULL,
	[InceptionYear] [varchar] (7) NULL,
	[AnnualRevenue] [varchar] (20) NULL,
	[B2BYesNo] [char] (1) NULL,
	[BankruptcyYesNo] [char] (1) NULL,
	[ProfitableYesNo] [char] (1) NULL,
	[BusinessName] [varchar] (50) NULL,
	[BusinessZipCode] [varchar] (12) NULL,
	[BusinessCity] [varchar] (50) NULL,
	[BusinessState] [varchar] (50) NULL,
	[BorrowerFirstName] [varchar] (50) NULL,
	[BorrowerLastName] [varchar] (50) NULL,
	[CreditProfile] [varchar] (20) NULL,
	[BorrowerHomePhone1] [varchar] (7) NULL,
	[BorrowerHomePhone2] [varchar] (7) NULL,
	[BorrowerHomePhone3] [varchar] (7) NULL,
	[EmailAddress] [varchar] (50) NULL,
	[Password] [varchar] (20) NULL,
	[ConfirmPassword] [varchar] (20) NULL,
	[TargusPassYesNo] [char] (1) NULL,
	[CreditPullSuccessYesNo] [char] (1) NULL
	)
GO


