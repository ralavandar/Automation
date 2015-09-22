--********************************************************
--* Create_tTestData_HomeEquity.sql
--********************************************************

-- Script to create table for Fossa Home Equity test cases

USE [AutomatedTesting]
GO

--DELETE from AutomatedTesting.dbo.tTestData_HomeEquity
--DROP TABLE AutomatedTesting.dbo.tTestData_HomeEquity

CREATE TABLE tTestData_HomeEquity (
	[TestCaseID] [int] PRIMARY KEY IDENTITY,
	[TestCaseName] [varchar] (100) NOT NULL,
	[TestEnvironment] [varchar] (10) NOT NULL,
	[BrowserType] [varchar] (20) NULL,
	[Template] [varchar] (20) NULL,
	[Variation] [varchar] (50) NULL,
	[QueryString] [varchar] (200) NULL,
	[LoanType] [varchar] (50) NOT NULL,
	[PropertyType] [varchar] (50) NULL,
	[HELoanType] [varchar] (50) NULL,
	[HELoanTerm] [varchar] (50) NULL,
	[LoanPurposeDebtYesNo] [char] (1) NULL,
	[LoanPurposeBoatYesNo] [char] (1) NULL,
	[LoanPurposeRvYesNo] [char] (1) NULL,
	[LoanPurposeMotorcycleYesNo] [char] (1) NULL,
	[LoanPurposeImprovementYesNo] [char] (1) NULL,
	[LoanPurposeAutoYesNo] [char] (1) NULL,
	[LoanPurposeOtherYesNo] [char] (1) NULL,
	[LoanPurposeOtherReason] [varchar] (50) NULL,
	[PropertyZipCode] [varchar] (12) NULL,
	[PropertyState] [varchar] (50) NULL,
	[PropertyCity] [varchar] (50) NULL,
	[PropertyUse] [varchar] (20) NULL,
	[PropertyValue] [varchar] (50) NULL,
	[PurchasePrice] [varchar] (50) NULL,
	[PurchaseYear] [varchar] (7) NULL,
	[CurrentMortgages] [varchar] (50) NULL,
	[FirstMortgageBalance] [varchar] (50) NULL,
	[FirstMortgagePayment] [varchar] (50) NULL,
	[SecondMortgageBalance] [varchar] (50) NULL,
	[SecondMortgagePayment] [varchar] (50) NULL,
	[RequestedLoanAmount] [varchar] (50) NULL,
	[CreditProfile] [varchar] (50) NULL,
	[DateOfBirthMonth] [varchar] (7) NULL,
	[DateOfBirthDay] [varchar] (7) NULL,
	[DateOfBirthYear] [varchar] (7) NULL,
	[ForeclosureHistory] [varchar] (50) NULL,
	[BankruptcyHistory] [varchar] (50) NULL,
	[BorrowerFirstName] [varchar] (50) NULL,
	[BorrowerMiddleName] [varchar] (50) NULL,
	[BorrowerLastName] [varchar] (50) NULL,
	[BorrowerSuffix] [varchar] (10) NULL,
	[BorrowerStreetAddress] [varchar] (50) NULL,
	[BorrowerZipCode] [varchar] (12) NULL,
	[BorrowerState] [varchar] (50) NULL,
	[BorrowerCity] [varchar] (12) NULL,
	[YearsAtAddress] [varchar] (7) NULL,
	[MonthsAtAddress] [varchar] (7) NULL,
	[PreviousStreetAddress] [varchar] (50) NULL,
	[PreviousZipCode] [varchar] (12) NULL,
	[BorrowerHomePhone1] [varchar] (7) NULL,
	[BorrowerHomePhone2] [varchar] (7) NULL,
	[BorrowerHomePhone3] [varchar] (7) NULL,
	[BorrowerWorkPhone1] [varchar] (7) NULL,
	[BorrowerWorkPhone2] [varchar] (7) NULL,
	[BorrowerWorkPhone3] [varchar] (7) NULL,
	[BorrowerSsn1] [varchar] (7) NULL,			-- [varchar](7) allows for a value of 'default'
	[BorrowerSsn2] [varchar] (7) NULL,	
	[BorrowerSsn3] [varchar] (7) NULL,	
	[EmailAddress] [varchar] (50) NULL,
	[Password] [varchar] (20) NULL,
	[ConfirmPassword] [varchar] (20) NULL
	)
GO

