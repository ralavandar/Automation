--********************************************************
--* Create_tTestData_Auto.sql
--********************************************************
-- Script to create table for Fossa Mortgage Auto tests

USE [AutomatedTesting]
GO
-- DELETE FROM AutomatedTesting.dbo.tTestData_Auto
-- DROP TABLE AutomatedTesting.dbo.tTestData_Auto

CREATE TABLE tTestData_Auto (
	[TestCaseID] [int] PRIMARY KEY IDENTITY,
	[TestCaseName] [varchar] (100) NOT NULL,
	[TestEnvironment] [varchar] (10) NOT NULL,
	[BrowserType] [varchar] (20) NULL,
	[Variation] [varchar] (50) NULL,
	[QueryString] [varchar] (200) NULL,
	[LoanType] [varchar] (50) NOT NULL,
	-- Step 1
	[AutoLoanType] [varchar] (50) NOT NULL,
	[AutoLoanTerm] [varchar] (50) NULL,
	[CoborrowerYesNo] [char] (1) NULL,
	-- Step 2
	[PurchaseDownPayment] [varchar] (50) NULL,
	[RequestedLoanAmount] [varchar] (50) NULL,
	[VehicleState] [varchar] (50) NULL,
	[CurrentRate] [varchar] (50) NULL,
	[CurrentLender] [varchar] (50) NULL,
	[CurrentPayoffAmount] [varchar] (50) NULL,
	[CurrentRemainingTerms] [varchar] (50) NULL,
	[CurrentPayment] [varchar] (50) NULL,
	-- Step 3
	[VehicleYear] [varchar] (50) NULL,
	[VehicleMake] [varchar] (50) NULL,
	[VehicleModel] [varchar] (50) NULL,
	[VehicleTrim] [varchar] (50) NULL,
	[VehicleIdNumber] [varchar] (50) NULL,
	[VehicleMileage] [varchar] (50) NULL,
	-- Step 4
	[MilitaryServiceYesNo] [char] (1) NULL,
	[DateOfBirthMonth] [varchar] (7) NULL,
	[DateOfBirthDay] [varchar] (7) NULL,
	[DateOfBirthYear] [varchar] (7) NULL,
	[USCitizenYesNo] [char] (1) NULL,
	[CreditProfile] [varchar] (20) NULL,
	[BankruptcyHistory] [varchar] (50) NULL,
	-- Step 5
	[BorrowerEmploymentStatus] [varchar] (50) NULL,
	[BorrowerJobTitle] [varchar] (50) NULL,
	[BorrowerEmployerName] [varchar] (50) NULL,
	[BorrowerEmploymentTimeYears] [varchar] (10) NULL,
	[BorrowerEmploymentTimeMonths] [varchar] (10) NULL,
	-- Step 6
	[BorrowerIncome] [varchar] (50) NULL,
	[BorrowerOtherIncome] [varchar] (50) NULL,
	[BorrowerAssets] [varchar] (50) NULL,
	[BorrowerAccountType] [varchar] (50) NULL,
	-- Step 7
	[BorrowerFirstName] [varchar] (50) NULL,
	[BorrowerMiddleName] [varchar] (50) NULL,
	[BorrowerLastName] [varchar] (50) NULL,
	[BorrowerSuffix] [varchar] (10) NULL,
	-- Step 8
	[BorrowerStreetAddress] [varchar] (50) NULL,
	[BorrowerZipCode] [varchar] (12) NULL,
	[BorrowerRentOwn] [varchar] (50) NULL,
	[BorrowerYearsAtAddress] [varchar] (7) NULL,
	[BorrowerMonthsAtAddress] [varchar] (7) NULL,
	[BorrowerHousingPayment] [varchar] (50) NULL,
	-- Step 9
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
	-- Step 10
	[EmailOptInYesNo] [char] (1) NULL,
	[EmailAddress] [varchar] (50) NULL,
	[Password] [varchar] (20) NULL,
	[ConfirmPassword] [varchar] (20) NULL,		-- not currently in Auto QF
	[MothersMaidenName] [varchar] (50) NULL,
	-- Step 11
	[CoborrowerFirstName] [varchar] (50) NULL,
	[CoborrowerMiddleName] [varchar] (50) NULL,
	[CoborrowerLastName] [varchar] (50) NULL,
	[CoborrowerSuffix] [varchar] (10) NULL,
	-- Step 12
	[CoDateOfBirthMonth] [varchar] (7) NULL,
	[CoDateOfBirthDay] [varchar] (7) NULL,
	[CoDateOfBirthYear] [varchar] (7) NULL,
	[CoUSCitizenYesNo] [char] (1) NULL,
	[CoBankruptcyHistory] [varchar] (50) NULL,
	-- Step 13
	[CoEmploymentStatus] [varchar] (50) NULL,
	[CoJobTitle] [varchar] (50) NULL,
	[CoEmployerName] [varchar] (50) NULL,
	[CoEmploymentTimeYears] [varchar] (10) NULL,
	[CoEmploymentTimeMonths] [varchar] (10) NULL,
	-- Step 14
	[CoborrowerIncome] [varchar] (50) NULL,
	[CoborrowerOtherIncome] [varchar] (50) NULL,
	[CoborrowerAssets] [varchar] (50) NULL,
	[CoborrowerAccountType] [varchar] (50) NULL,
	-- Step 15
	[CoSameAddressYesNo] [char] (1) NULL,
	[CoStreetAddress] [varchar] (50) NULL,
	[CoborrowerZipCode] [varchar] (12) NULL,
	[CoborrowerRentOwn] [varchar] (50) NULL,
	[CoYearsAtAddress] [varchar] (7) NULL,
	[CoMonthsAtAddress] [varchar] (7) NULL,
	-- Step 16
	[CoborrowerHomePhone1] [varchar] (7) NULL,
	[CoborrowerHomePhone2] [varchar] (7) NULL,
	[CoborrowerHomePhone3] [varchar] (7) NULL,
	[CoborrowerWorkPhone1] [varchar] (7) NULL,
	[CoborrowerWorkPhone2] [varchar] (7) NULL,
	[CoborrowerWorkPhone3] [varchar] (7) NULL,
	[CoborrowerWorkPhoneExt] [varchar] (10) NULL,
	[CoborrowerSsn1] [varchar] (7) NULL,			-- [varchar](7) allows for a value of 'default'
	[CoborrowerSsn2] [varchar] (7) NULL,	
	[CoborrowerSsn3] [varchar] (7) NULL,	
	[CoEmailAddress] [varchar] (50) NULL
	)
GO

