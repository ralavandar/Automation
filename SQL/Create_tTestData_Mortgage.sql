--********************************************************
--* Create_tTestData_Mortgage.sql
--********************************************************

-- Script to create table for Fossa Mortgage tests

USE [AutomatedTesting]
GO

--DELETE from AutomatedTesting.dbo.tTestData_Mortgage
--DROP TABLE AutomatedTesting.dbo.tTestData_Mortgage

-- Script to create the Test Data Table for Mortgage
CREATE TABLE tTestData_Mortgage (
	[TestCaseID] [int] PRIMARY KEY IDENTITY,
	[TestCaseName] [varchar] (100) NOT NULL,
	[TestEnvironment] [varchar] (10) NOT NULL,
	[BrowserType] [varchar] (20) NULL,
	[Template] [varchar] (20) NULL,
	[Variation] [varchar] (100) NULL,
	[QueryString] [varchar] (200) NULL,
	[LoanType] [varchar] (50) NOT NULL,
	[HomeDescription] [varchar] (50) NULL,
	[PropertyUseType] [varchar] (20) NULL,
	[PropertyState] [varchar] (50) NULL,
	[PropertyCity] [varchar] (50) NULL,
	[PropertyStreet] [varchar] (50) NULL,
	[PropertyZipCode] [varchar] (12) NULL,
	[FoundNewHomeYesNo] [char] (1) NULL,
	[FirstTimeBuyerYesNo] [char] (1) NULL,
	[PurchasePrice] [varchar] (50) NULL,
	[PurchaseDownPayment] [varchar] (50) NULL,
	[RefiPropertyValue] [varchar] (50) NULL,
	[FirstMortgageBalance] [varchar] (50) NULL,
	[FirstMortgagePayment] [varchar] (50) NULL,
	[FirstMortgageRate] [varchar] (50) NULL,
	[SecondMortgageYesNo] [char] (1) NULL,
	[SecondMortgageBalance] [varchar] (50) NULL,
	[SecondMortgagePayment] [varchar] (50) NULL,
	[SecondMortgageRate] [varchar] (50) NULL,
	[RefiCashoutAmount] [varchar] (50) NULL,
	[EmailAddress] [varchar] (50) NULL,
	[Password] [varchar] (20) NULL,
	[ConfirmPassword] [varchar] (20) NULL,
	[BorrowerFirstName] [varchar] (50) NULL,
	[BorrowerMiddleName] [varchar] (50) NULL,
	[BorrowerLastName] [varchar] (50) NULL,
	[BorrowerSuffix] [varchar] (10) NULL,
	[BorrowerStreetAddress] [varchar] (50) NULL,
	[BorrowerZipCode] [varchar] (12) NULL,
	[BorrowerHomePhone1] [varchar] (7) NULL,
	[BorrowerHomePhone2] [varchar] (7) NULL,
	[BorrowerHomePhone3] [varchar] (7) NULL,
	[BorrowerWorkPhone1] [varchar] (7) NULL,
	[BorrowerWorkPhone2] [varchar] (7) NULL,
	[BorrowerWorkPhone3] [varchar] (7) NULL,
	[BorrowerWorkPhoneExt] [varchar] (10) NULL,
	[WorkPhoneSameAsHomeYesNo] [char] (1) NULL,	-- for "PhoneFields" variation
	[BorrowerSsn1] [varchar] (7) NULL,			-- [varchar](7) allows for a value of 'default'
	[BorrowerSsn2] [varchar] (7) NULL,	
	[BorrowerSsn3] [varchar] (7) NULL,	
	[DateOfBirthMonth] [varchar] (7) NULL,
	[DateOfBirthDay] [varchar] (7) NULL,
	[DateOfBirthYear] [varchar] (7) NULL,
	[CreditProfile] [varchar] (20) NULL,
	[MilitaryServiceYesNo] [char] (1) NULL,
	[CurrentLoanVAYesNo] [char] (1) NULL,		-- Refi only
	[ForeclosureHistory] [varchar] (50) NULL,
	[BankruptcyYesNo] [char] (1) NULL,
	[BankruptcyHistory] [varchar] (50) NULL,
	[CreditCardDebtAmount] [varchar] (50) NULL,
	[RealtorConsultYesNo] [char] (1) NULL,		-- Purchase only
	[DebtConsultYesNo] [char] (1) NULL,			-- Purchase and Refi; if credit card debt > 10,000
	[MortgageInsuranceYesNo] [char] (1) NULL,	-- mortInsurance variation 
	[EmailOptInYesNo] [char] (1) NULL,			-- emailOptin variation
	[HomeServicesOptInYesNo] [char] (1) NULL,	-- Home Services OptIn variation
	[CurrentREAgentYesNo] [char] (1) NULL,		-- Purchase only
	[CreditScoreProductYesNo] [char] (1) NULL,	-- cc-qf only
	[EduOptInYesNo] [char] (1) NULL,			-- EDU opt-in
	[CcMessageToBorrower] [varchar] (1000) NULL,-- cc-qf only
	[SpouseDateOfBirthMonth] [varchar] (7) NULL,-- reverse-mortgage only
	[SpouseDateOfBirthDay] [varchar] (7) NULL,  -- "
	[SpouseDateOfBirthYear] [varchar] (7) NULL, -- "
	)
GO


