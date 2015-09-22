--********************************************************
--* TestDataInsert_ProdTests_Forms_Auto.sql
--********************************************************
-- Script to insert values for Form tests in Production - Auto.
-- This data is designed so that it does not receive referrals in Production
--		100k Loan on a Maserati
--		Current Bankruptcy, Poor Credit
--		Student / Part Time
--		Low Income, No Assets

USE [AutomatedTesting]
GO
-- ******************************************************
-- *****    Insert Auto Tests (tid=auto)            *****
-- ******************************************************
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
VALUES (N'Prod_Auto_NewCarPurchase', N'PROD', N'firefox', N'', N'&esourceid=14349&mpid=8550&devicemonitor=selenium', N'AUTO', N'NewCarPurchase', N'72', N'Y', 
N'0', N'100000', N'Alaska', N'', N'', N'', N'', N'', N'2014', N'Maserati', N'GranTurismo', N'GranTurismo Sport', N'', 
N'', N'N', N'01', N'01', N'1990', N'Y', N'Poor', N'Not yet removed', N'Student', N'', N'', N'', 
N'', N'12000', N'0', N'0', N'None', N'Otto', N'', N'Tester', N'', N'11115 Rushmore Dr', N'28277', N'Rent', 
N'0', N'6', N'1500', N'704', N'541', N'5351', N'407', N'939', N'3463', N'', N'default', N'default', N'default', N'N', N'default', N'default', N'', N'Smith-Jones', 
N'Jane', N'', N'Tester', N'', N'02', N'02', N'1991', N'N', N'1-12 months ago', N'Part Time', N'Tester', N'Just Testing', N'0', N'6', N'13000', N'0', N'0', N'None', 
N'Y', N'', N'', N'Rent', N'0', N'6', N'704', N'541', N'5351', N'407', N'939', N'3463', N'', N'default', N'default', N'default', N'default')

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
VALUES (N'Prod_Auto_UsedCarPurchase', N'PROD', N'IE', N'', N'&esourceid=14349&mpid=8550&devicemonitor=selenium', N'AUTO', N'UsedCarPurchase', N'48', N'N', 
N'0', N'100000', N'Alaska', N'', N'', N'', N'', N'', N'2010', N'Maserati', N'GranTurismo', N'GranTurismo S Coupe 2D', N'', 
N'7200', N'N', N'01', N'01', N'1990', N'Y', N'Poor', N'Not yet removed', N'Student', N'', N'', N'', 
N'', N'12000', N'0', N'0', N'None', N'Otto', N'', N'Tester', N'', N'11115 Rushmore Dr', N'28277', N'Rent', 
N'0', N'6', N'1500', N'704', N'541', N'5351', N'407', N'939', N'3463', N'', N'default', N'default', N'default', N'N', N'default', N'default', N'', N'Smith-Jones', 
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
VALUES (N'Prod_Auto_Refinance', N'PROD', N'firefox', N'', N'&esourceid=14349&mpid=8550&devicemonitor=selenium', N'AUTO', N'REFINANCE', N'72', N'N', 
N'', N'100000', N'Alaska', N'8.375', N'Test Lender', N'100000', N'44', N'2412', N'2012', N'BMW', N'7 Series', N'7 Series 760Li Sedan 4D', N'TESTVIN8901234567', 
N'4500', N'N', N'01', N'01', N'1990', N'Y', N'Poor', N'Not yet removed', N'Student', N'', N'', N'', 
N'', N'0', N'0', N'0', N'None', N'Otto', N'', N'Tester', N'', N'11115 Rushmore Dr', N'28277', N'Rent', 
N'0', N'6', N'1500', N'704', N'541', N'5351', N'407', N'939', N'3463', N'', N'default', N'default', N'default', N'N', N'default', N'default', N'', N'Smith-Jones', 
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
VALUES (N'Prod_Auto_LeaseBuyOut', N'PROD', N'IE', N'', N'&esourceid=14349&mpid=8550&devicemonitor=selenium', N'AUTO', N'BUYOUTLEASE', N'72', N'N', 
N'', N'100000', N'Alaska', N'8.375', N'Test Lender', N'100000', N'3', N'2412', N'2010', N'Maserati', N'GranTurismo', N'GranTurismo S Coupe 2D', N'TESTVIN8901234567', 
N'4500', N'N', N'12', N'31', N'1983', N'Y', N'Poor', N'Not yet removed', N'Student', N'', N'', N'', 
N'', N'0', N'0', N'0', N'None', N'Otto', N'', N'Tester', N'', N'11115 Rushmore Dr', N'28277', N'Rent', 
N'0', N'6', N'1500', N'704', N'541', N'5351', N'407', N'939', N'3463', N'', N'default', N'default', N'default', N'N', N'default', N'default', N'', N'Smith-Jones', 
N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
GO
-- ******************************************************
-- *****    End of Auto Tests (tid=auto)            *****
-- ******************************************************
