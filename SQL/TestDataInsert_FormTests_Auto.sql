--********************************************************
--* TestDataInsert_FossaTests_Auto.sql
--********************************************************

-- Script to insert values for Form Regression Tests.
-- This data varies as much as possible to try to weed out bugs and issues in Staging and DEV, 
-- where we don't need to worry about generating real referrals to live production lenders

-- SELECT * FROM AutomatedTesting.dbo.tTestData_Auto with(nolock) 
-- WHERE TestCaseName like '%PreProd%'
-- WHERE TestCaseID = 6

-- UPDATE AutomatedTesting.dbo.tTestData_Auto
-- SET BrowserType = 'IE'
-- WHERE TestCaseID IN (2,4)

USE [AutomatedTesting]
GO

-- **************************************************
-- *****    Insert Auto Tests                   *****
-- **************************************************

INSERT [dbo].tTestData_Auto ([TestCaseName], [TestEnvironment], [BrowserType], [Variation], [QueryString], [LoanType], [AutoLoanType], [AutoLoanTerm], [CoborrowerYesNo],
 [PurchaseDownPayment], [RequestedLoanAmount], [VehicleState], [CurrentRate], [CurrentLender], [CurrentPayoffAmount], [CurrentRemainingTerms], [CurrentPayment], 
 [VehicleYear], [VehicleMake], [VehicleModel], [VehicleTrim], [VehicleIdNumber], [VehicleMileage], [MilitaryServiceYesNo],  
 [DateOfBirthMonth], [DateOfBirthDay], [DateOfBirthYear], [USCitizenYesNo], [CreditProfile], [BankruptcyHistory], [BorrowerEmploymentStatus], [BorrowerJobTitle], 
 [BorrowerEmployerName], [BorrowerEmploymentTimeYears], [BorrowerEmploymentTimeMonths], [BorrowerIncome], [BorrowerOtherIncome], [BorrowerAssets], [BorrowerAccountType], [BorrowerFirstName], 
 [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], [BorrowerZipCode], [BorrowerRentOwn], [BorrowerYearsAtAddress], [BorrowerMonthsAtAddress],
 [BorrowerHousingPayment], [BorrowerHomePhone1], [BorrowerHomePhone2], [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerWorkPhoneExt], 
 [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailOptInYesNo], [EmailAddress], [Password], [ConfirmPassword], [MothersMaidenName], 
 [CoborrowerFirstName], [CoborrowerMiddleName], [CoborrowerLastName], [CoborrowerSuffix], [CoDateOfBirthMonth], [CoDateOfBirthDay], [CoDateOfBirthYear], [CoUSCitizenYesNo], 
 [CoBankruptcyHistory], [CoEmploymentStatus], [CoJobTitle], [CoEmployerName], [CoEmploymentTimeYears], [CoEmploymentTimeMonths], [CoborrowerIncome], [CoborrowerOtherIncome], 
 [CoborrowerAssets], [CoborrowerAccountType], [CoSameAddressYesNo], [CoStreetAddress], [CoborrowerZipCode], [CoborrowerRentOwn], [CoYearsAtAddress], [CoMonthsAtAddress], 
 [CoborrowerHomePhone1], [CoborrowerHomePhone2], [CoborrowerHomePhone3], [CoborrowerWorkPhone1], [CoborrowerWorkPhone2], [CoborrowerWorkPhone3], [CoborrowerWorkPhoneExt],  
 [CoborrowerSsn1], [CoborrowerSsn2], [CoborrowerSsn3], [CoEmailAddress])  
VALUES (N'auto_01_NewCarPurchase', N'STAGE', N'random', N'', N'&esourceid=14349', N'AUTO', N'NewCarPurchase', N'60', N'Y', 
N'9000', N'20000', N'Florida', N'', N'', N'', N'', N'', N'2014', N'Honda', N'Accord', N'Accord EX-L', N'', 
N'', N'N', N'10', N'31', N'1975', N'Y', N'Excellent', N'N', N'Full Time', N'V.P. of Sales/Marketing', N'Bristol-Myers Squibb', N'10', 
N'0', N'150000', N'25000', N'50000', N'Checking/Savings', N'Otto', N'B', N'Tester', N'', N'225 Celebration Pl', N'34747', N'Own', 
N'5', N'6', N'2000', N'407', N'939', N'3463', N'407', N'939', N'7675', N'', N'default', N'default', N'default', N'N', N'default', N'default', N'', N'Smith-Jones', 
N'Jane', N'C', N'Tester', N'', N'01', N'02', N'1976', N'N', N'N', N'Part Time', N'Teller', N'Bank of America', N'0', N'6', N'12000', N'0', N'15000', N'Savings', 
N'Y', N'', N'', N'Own', N'5', N'6', N'407', N'939', N'3463', N'407', N'939', N'3463', N'', N'default', N'default', N'default', N'default')
GO

INSERT [dbo].tTestData_Auto ([TestCaseName], [TestEnvironment], [BrowserType], [Variation], [QueryString], [LoanType], [AutoLoanType], [AutoLoanTerm], [CoborrowerYesNo],
 [PurchaseDownPayment], [RequestedLoanAmount], [VehicleState], [CurrentRate], [CurrentLender], [CurrentPayoffAmount], [CurrentRemainingTerms], [CurrentPayment], 
 [VehicleYear], [VehicleMake], [VehicleModel], [VehicleTrim], [VehicleIdNumber], [VehicleMileage], [MilitaryServiceYesNo],  
 [DateOfBirthMonth], [DateOfBirthDay], [DateOfBirthYear], [USCitizenYesNo], [CreditProfile], [BankruptcyHistory], [BorrowerEmploymentStatus], [BorrowerJobTitle], 
 [BorrowerEmployerName], [BorrowerEmploymentTimeYears], [BorrowerEmploymentTimeMonths], [BorrowerIncome], [BorrowerOtherIncome], [BorrowerAssets], [BorrowerAccountType], [BorrowerFirstName], 
 [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], [BorrowerZipCode], [BorrowerRentOwn], [BorrowerYearsAtAddress], [BorrowerMonthsAtAddress],
 [BorrowerHousingPayment], [BorrowerHomePhone1], [BorrowerHomePhone2], [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerWorkPhoneExt], 
 [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailOptInYesNo], [EmailAddress], [Password], [ConfirmPassword], [MothersMaidenName], 
 [CoborrowerFirstName], [CoborrowerMiddleName], [CoborrowerLastName], [CoborrowerSuffix], [CoDateOfBirthMonth], [CoDateOfBirthDay], [CoDateOfBirthYear], [CoUSCitizenYesNo], 
 [CoBankruptcyHistory], [CoEmploymentStatus], [CoJobTitle], [CoEmployerName], [CoEmploymentTimeYears], [CoEmploymentTimeMonths], [CoborrowerIncome], [CoborrowerOtherIncome], 
 [CoborrowerAssets], [CoborrowerAccountType], [CoSameAddressYesNo], [CoStreetAddress], [CoborrowerZipCode], [CoborrowerRentOwn], [CoYearsAtAddress], [CoMonthsAtAddress], 
 [CoborrowerHomePhone1], [CoborrowerHomePhone2], [CoborrowerHomePhone3], [CoborrowerWorkPhone1], [CoborrowerWorkPhone2], [CoborrowerWorkPhone3], [CoborrowerWorkPhoneExt],  
 [CoborrowerSsn1], [CoborrowerSsn2], [CoborrowerSsn3], [CoEmailAddress])  
VALUES (N'auto_02_UsedCarPurchase', N'STAGE', N'random', N'0-0-0-0-0-1-0', N'&esourceid=14349&igen=1&igop=1', N'AUTO', N'UsedCarPurchase', N'48', N'N', 
N'500', N'7500', N'North Carolina', N'', N'', N'', N'', N'', N'2002', N'Toyota', N'Camry', N'Camry LE Sedan 4D', N'', 
N'123500', N'Y', N'02', N'29', N'1988', N'N', N'Good', N'N', N'Student', N'', N'', N'', 
N'', N'12000', N'', N'1000', N'Checking', N'Abdul-Khaaliq', N'', N'El-Sayed', N'', N'11115 Rushmore Dr, Apt #301', N'28277', N'Rent', 
N'0', N'6', N'825', N'704', N'541', N'5351', N'704', N'541', N'5351', N'8320', N'default', N'default', N'default', N'N', N'default', N'default', N'', N'Smith-Jones', 
N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
GO

INSERT [dbo].tTestData_Auto ([TestCaseName], [TestEnvironment], [BrowserType], [Variation], [QueryString], [LoanType], [AutoLoanType], [AutoLoanTerm], [CoborrowerYesNo],
 [PurchaseDownPayment], [RequestedLoanAmount], [VehicleState], [CurrentRate], [CurrentLender], [CurrentPayoffAmount], [CurrentRemainingTerms], [CurrentPayment], 
 [VehicleYear], [VehicleMake], [VehicleModel], [VehicleTrim], [VehicleIdNumber], [VehicleMileage], [MilitaryServiceYesNo],  
 [DateOfBirthMonth], [DateOfBirthDay], [DateOfBirthYear], [USCitizenYesNo], [CreditProfile], [BankruptcyHistory], [BorrowerEmploymentStatus], [BorrowerJobTitle], 
 [BorrowerEmployerName], [BorrowerEmploymentTimeYears], [BorrowerEmploymentTimeMonths], [BorrowerIncome], [BorrowerOtherIncome], [BorrowerAssets], [BorrowerAccountType], [BorrowerFirstName], 
 [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], [BorrowerZipCode], [BorrowerRentOwn], [BorrowerYearsAtAddress], [BorrowerMonthsAtAddress],
 [BorrowerHousingPayment], [BorrowerHomePhone1], [BorrowerHomePhone2], [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerWorkPhoneExt], 
 [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailOptInYesNo], [EmailAddress], [Password], [ConfirmPassword], [MothersMaidenName], 
 [CoborrowerFirstName], [CoborrowerMiddleName], [CoborrowerLastName], [CoborrowerSuffix], [CoDateOfBirthMonth], [CoDateOfBirthDay], [CoDateOfBirthYear], [CoUSCitizenYesNo], 
 [CoBankruptcyHistory], [CoEmploymentStatus], [CoJobTitle], [CoEmployerName], [CoEmploymentTimeYears], [CoEmploymentTimeMonths], [CoborrowerIncome], [CoborrowerOtherIncome], 
 [CoborrowerAssets], [CoborrowerAccountType], [CoSameAddressYesNo], [CoStreetAddress], [CoborrowerZipCode], [CoborrowerRentOwn], [CoYearsAtAddress], [CoMonthsAtAddress], 
 [CoborrowerHomePhone1], [CoborrowerHomePhone2], [CoborrowerHomePhone3], [CoborrowerWorkPhone1], [CoborrowerWorkPhone2], [CoborrowerWorkPhone3], [CoborrowerWorkPhoneExt],  
 [CoborrowerSsn1], [CoborrowerSsn2], [CoborrowerSsn3], [CoEmailAddress])  
VALUES (N'auto_03_Refinance', N'STAGE', N'random', N'0-0-0-0-0-1-0', N'&esourceid=14349&igen=1&igop=1', N'AUTO', N'REFINANCE', N'60', N'Y', 
N'', N'19800', N'California', N'8.375', N'BB&T Corp.', N'19800', N'44', N'492', N'2012', N'BMW', N'5 Series', N'5 Series 528i Sedan 4D', N'VIN45678901234567', 
N'3500', N'N', N'01', N'01', N'1970', N'Y', N'Fair', N'73-84 months ago', N'Homemaker', N'', N'', N'', 
N'', N'0', N'0', N'0', N'None', N'Marie Clair', N'', N'Van der Beek', N'', N'1150 Magic Way', N'92802', N'Own', 
N'2', N'0', N'3100', N'714', N'778', N'6600', N'714', N'956', N'6425', N'', N'default', N'default', N'default', N'N', N'default', N'default', N'', N'O''Reilly', 
N'James', N'C', N'Van der Beek', N'II', N'02', N'28', N'1960', N'N', N'61-72 months ago', N'Full Time', N'V.P. of Sales/Marketing', N'Walt Disney Co.', N'15', N'8', N'175000', N'25000', N'49999', N'Checking/Savings', 
N'Y', N'', N'', N'Own', N'5', N'8', N'714', N'781', N'3463', N'714', N'778', N'6600', N'06', N'default', N'default', N'default', N'default')
GO

INSERT [dbo].tTestData_Auto ([TestCaseName], [TestEnvironment], [BrowserType], [Variation], [QueryString], [LoanType], [AutoLoanType], [AutoLoanTerm], [CoborrowerYesNo],
 [PurchaseDownPayment], [RequestedLoanAmount], [VehicleState], [CurrentRate], [CurrentLender], [CurrentPayoffAmount], [CurrentRemainingTerms], [CurrentPayment], 
 [VehicleYear], [VehicleMake], [VehicleModel], [VehicleTrim], [VehicleIdNumber], [VehicleMileage], [MilitaryServiceYesNo],  
 [DateOfBirthMonth], [DateOfBirthDay], [DateOfBirthYear], [USCitizenYesNo], [CreditProfile], [BankruptcyHistory], [BorrowerEmploymentStatus], [BorrowerJobTitle], 
 [BorrowerEmployerName], [BorrowerEmploymentTimeYears], [BorrowerEmploymentTimeMonths], [BorrowerIncome], [BorrowerOtherIncome], [BorrowerAssets], [BorrowerAccountType], [BorrowerFirstName], 
 [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], [BorrowerZipCode], [BorrowerRentOwn], [BorrowerYearsAtAddress], [BorrowerMonthsAtAddress],
 [BorrowerHousingPayment], [BorrowerHomePhone1], [BorrowerHomePhone2], [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerWorkPhoneExt], 
 [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailOptInYesNo], [EmailAddress], [Password], [ConfirmPassword], [MothersMaidenName], 
 [CoborrowerFirstName], [CoborrowerMiddleName], [CoborrowerLastName], [CoborrowerSuffix], [CoDateOfBirthMonth], [CoDateOfBirthDay], [CoDateOfBirthYear], [CoUSCitizenYesNo], 
 [CoBankruptcyHistory], [CoEmploymentStatus], [CoJobTitle], [CoEmployerName], [CoEmploymentTimeYears], [CoEmploymentTimeMonths], [CoborrowerIncome], [CoborrowerOtherIncome], 
 [CoborrowerAssets], [CoborrowerAccountType], [CoSameAddressYesNo], [CoStreetAddress], [CoborrowerZipCode], [CoborrowerRentOwn], [CoYearsAtAddress], [CoMonthsAtAddress], 
 [CoborrowerHomePhone1], [CoborrowerHomePhone2], [CoborrowerHomePhone3], [CoborrowerWorkPhone1], [CoborrowerWorkPhone2], [CoborrowerWorkPhone3], [CoborrowerWorkPhoneExt],  
 [CoborrowerSsn1], [CoborrowerSsn2], [CoborrowerSsn3], [CoEmailAddress])  
VALUES (N'auto_04_LeaseBuyOut', N'STAGE', N'random', N'0-0-0-0-0-1-0-2-0-0-0-0-2', N'&esourceid=14349&igen=1&igop=1', N'AUTO', N'BUYOUTLEASE', N'36', N'N', 
N'', N'35000', N'Texas', N'5.9', N'Mercedes-Benz Financial Services', N'35500', N'3', N'1065', N'2009', N'Mercedes-Benz', N'E-Class', N'E-Class E550 Sedan 4D', N'VIN12345678901234', 
N'36900', N'N', N'11', N'30', N'1950', N'Y', N'Excellent', N'N', N'Retired', N'', N'', N'', 
N'', N'0', N'24000', N'110000', N'Checking/Savings', N'D''Juan', N'', N'O''Donnell', N'', N'3300 Championship Parkway', N'76177', N'Rent', 
N'0', N'3', N'2400', N'817', N'961', N'0800', N'866', N'348', N'3984', N'', N'default', N'default', N'default', N'N', N'default', N'default', N'', N'O''Reilly', 
N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
GO

INSERT [dbo].tTestData_Auto ([TestCaseName], [TestEnvironment], [BrowserType], [Variation], [QueryString], [LoanType], [AutoLoanType], [AutoLoanTerm], [CoborrowerYesNo],
 [PurchaseDownPayment], [RequestedLoanAmount], [VehicleState], [CurrentRate], [CurrentLender], [CurrentPayoffAmount], [CurrentRemainingTerms], [CurrentPayment], 
 [VehicleYear], [VehicleMake], [VehicleModel], [VehicleTrim], [VehicleIdNumber], [VehicleMileage], [MilitaryServiceYesNo],  
 [DateOfBirthMonth], [DateOfBirthDay], [DateOfBirthYear], [USCitizenYesNo], [CreditProfile], [BankruptcyHistory], [BorrowerEmploymentStatus], [BorrowerJobTitle], 
 [BorrowerEmployerName], [BorrowerEmploymentTimeYears], [BorrowerEmploymentTimeMonths], [BorrowerIncome], [BorrowerOtherIncome], [BorrowerAssets], [BorrowerAccountType], [BorrowerFirstName], 
 [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], [BorrowerZipCode], [BorrowerRentOwn], [BorrowerYearsAtAddress], [BorrowerMonthsAtAddress],
 [BorrowerHousingPayment], [BorrowerHomePhone1], [BorrowerHomePhone2], [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerWorkPhoneExt], 
 [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailOptInYesNo], [EmailAddress], [Password], [ConfirmPassword], [MothersMaidenName], 
 [CoborrowerFirstName], [CoborrowerMiddleName], [CoborrowerLastName], [CoborrowerSuffix], [CoDateOfBirthMonth], [CoDateOfBirthDay], [CoDateOfBirthYear], [CoUSCitizenYesNo], 
 [CoBankruptcyHistory], [CoEmploymentStatus], [CoJobTitle], [CoEmployerName], [CoEmploymentTimeYears], [CoEmploymentTimeMonths], [CoborrowerIncome], [CoborrowerOtherIncome], 
 [CoborrowerAssets], [CoborrowerAccountType], [CoSameAddressYesNo], [CoStreetAddress], [CoborrowerZipCode], [CoborrowerRentOwn], [CoYearsAtAddress], [CoMonthsAtAddress], 
 [CoborrowerHomePhone1], [CoborrowerHomePhone2], [CoborrowerHomePhone3], [CoborrowerWorkPhone1], [CoborrowerWorkPhone2], [CoborrowerWorkPhone3], [CoborrowerWorkPhoneExt],  
 [CoborrowerSsn1], [CoborrowerSsn2], [CoborrowerSsn3], [CoEmailAddress])  
VALUES (N'auto_05_RequiredFieldTest_NoCobo', N'STAGE', N'random', N'0-0-0-0-0-1-0-2-0-0-0-0-2', N'&esourceid=14349&igen=1&igop=1', N'AUTO', N'NewCarPurchase', N'60', N'N', 
N'9000', N'20000', N'Massachusetts', N'', N'', N'', N'', N'', N'2014', N'Honda', N'Accord', N'Accord EX', N'VIN12345678901234', 
N'', N'N', N'11', N'30', N'1985', N'Y', N'Excellent', N'N', N'Full Time', N'V.P. of Sales/Marketing', N'Bristol-Myers Squibb', N'10', 
N'0', N'150000', N'25000', N'50000', N'Checking/Savings', N'Otto', N'B', N'Tester', N'', N'110 Huntington Ave', N'02116', N'Rent', 
N'2', N'6', N'1800', N'617', N'236', N'5800', N'407', N'939', N'3463', N'', N'default', N'default', N'default', N'Y', N'default', N'default', N'', N'O''Riley', 
N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
GO

INSERT [dbo].tTestData_Auto ([TestCaseName], [TestEnvironment], [BrowserType], [Variation], [QueryString], [LoanType], [AutoLoanType], [AutoLoanTerm], [CoborrowerYesNo],
 [PurchaseDownPayment], [RequestedLoanAmount], [VehicleState], [CurrentRate], [CurrentLender], [CurrentPayoffAmount], [CurrentRemainingTerms], [CurrentPayment], 
 [VehicleYear], [VehicleMake], [VehicleModel], [VehicleTrim], [VehicleIdNumber], [VehicleMileage], [MilitaryServiceYesNo],  
 [DateOfBirthMonth], [DateOfBirthDay], [DateOfBirthYear], [USCitizenYesNo], [CreditProfile], [BankruptcyHistory], [BorrowerEmploymentStatus], [BorrowerJobTitle], 
 [BorrowerEmployerName], [BorrowerEmploymentTimeYears], [BorrowerEmploymentTimeMonths], [BorrowerIncome], [BorrowerOtherIncome], [BorrowerAssets], [BorrowerAccountType], [BorrowerFirstName], 
 [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], [BorrowerZipCode], [BorrowerRentOwn], [BorrowerYearsAtAddress], [BorrowerMonthsAtAddress],
 [BorrowerHousingPayment], [BorrowerHomePhone1], [BorrowerHomePhone2], [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerWorkPhoneExt], 
 [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailOptInYesNo], [EmailAddress], [Password], [ConfirmPassword], [MothersMaidenName], 
 [CoborrowerFirstName], [CoborrowerMiddleName], [CoborrowerLastName], [CoborrowerSuffix], [CoDateOfBirthMonth], [CoDateOfBirthDay], [CoDateOfBirthYear], [CoUSCitizenYesNo], 
 [CoBankruptcyHistory], [CoEmploymentStatus], [CoJobTitle], [CoEmployerName], [CoEmploymentTimeYears], [CoEmploymentTimeMonths], [CoborrowerIncome], [CoborrowerOtherIncome], 
 [CoborrowerAssets], [CoborrowerAccountType], [CoSameAddressYesNo], [CoStreetAddress], [CoborrowerZipCode], [CoborrowerRentOwn], [CoYearsAtAddress], [CoMonthsAtAddress], 
 [CoborrowerHomePhone1], [CoborrowerHomePhone2], [CoborrowerHomePhone3], [CoborrowerWorkPhone1], [CoborrowerWorkPhone2], [CoborrowerWorkPhone3], [CoborrowerWorkPhoneExt],  
 [CoborrowerSsn1], [CoborrowerSsn2], [CoborrowerSsn3], [CoEmailAddress])  
VALUES (N'auto_06_RequiredFieldTest_Cobo', N'STAGE', N'random', N'0-0-0-0-0-0-0-2-0-0-0-0-2-0', N'&esourceid=14349&igen=1&igop=1', N'AUTO', N'UsedCarPurchase', N'48', N'Y', 
N'500', N'7500', N'North Carolina', N'', N'', N'', N'', N'', N'2002', N'Toyota', N'Camry', N'Camry LE Sedan 4D', N'', 
N'123500', N'Y', N'02', N'29', N'1988', N'N', N'Good', N'N', N'Student', N'', N'', N'', 
N'', N'12000', N'', N'1000', N'Checking', N'Abdul-Khaaliq', N'', N'El-Sayed', N'', N'11115 Rushmore Dr, Apt #301', N'28277', N'Rent', 
N'0', N'6', N'825', N'704', N'541', N'5351', N'704', N'541', N'5351', N'8320', N'default', N'default', N'default', N'N', N'default', N'default', N'', N'Smith-Jones', 
N'Jane', N'C', N'Tester', N'', N'01', N'02', N'1976', N'N', N'N', N'Part Time', N'Teller', N'Bank of America', N'0', N'6', N'12000', N'0', N'15000', N'Savings', 
N'Y', N'', N'', N'Own', N'5', N'6', N'407', N'939', N'3463', N'407', N'939', N'3463', N'', N'default', N'default', N'default', N'default')
GO

-- **************************************************
-- *****    End of Auto Tests                   *****
-- **************************************************
