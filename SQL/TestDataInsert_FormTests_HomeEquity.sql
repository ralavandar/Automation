--********************************************************
--* TestDataInsert_FormTests_HomeEquity.sql
--********************************************************

-- Script to insert values for Form Regression Tests.
-- This data varies as much as possible to try to weed out bugs and issues in Staging and DEV, 
-- where we don't need to worry about generating real referrals to live production lenders

-- UPDATE AutomatedTesting.dbo.tTestData_HomeEquity
-- SET TestEnvironment = 'STAGE' --'Never Foreclosed'
-- WHERE TestCaseID IN (49)

-- DELETE from AutomatedTesting.dbo.tTestData_HomeEquity
-- SELECT * FROM AutomatedTesting.dbo.tTestData_HomeEquity
-- WHERE TestCaseID in (24,25)

USE [AutomatedTesting]
GO

-- ************************************
-- ***** Insert HEA Tests         *****
-- ************************************

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'hea_01_PrimaryFirstAndSecond', N'STAGE', N'random', N'hea', N'', N'&esourceid=14349', N'HOMEEQUITY', N'Single-Family Detached', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'34747', N'Florida', N'Kissimmee', N'Primary Home', N'$350,001 - $375,000', 
N'$300,001 - $325,000', N'2004', N'First & second mortgages', N'$140,001 - $150,000', N'', N'$20,001 - $30,000', N'', N'$45,001 - $50,000', N'stated-credit-history-excellent', N'01', N'02', N'1983', 
N'Never Foreclosed', N'Never/Not in the last 7 years', N'Otto', N'', N'Tester', N'', N'225 Celebration Pl', N'34747', N'Florida', N'Kissimmee', N'', N'', N'', N'', 
N'407', N'939', N'3463', N'', N'', N'', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'hea_02_SecondaryFirstOnly', N'STAGE', N'random', N'hea', N'', N'&igen=1&esourceid=14349', N'HOMEEQUITY', N'Town House', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'28710', N'North Carolina', N'Bat Cave', N'Secondary home', N'$300,001 - $325,000', 
N'$300,001 - $325,000', N'2010', N'First mortgage only', N'$190,001 - $200,000', N'', N'', N'', N'$20,001 - $25,000', N'stated-credit-history-good', N'12', N'31', N'1970', 
N'Over 84 months ago', N'73-84 months ago', N'Marie Clair', N'', N'Van Der Beek', N'', N'11115 Rushmore Dr', N'28277', N'North Carolina', N'Charlotte', N'', N'', N'', N'',
N'704', N'943', N'8320', N'', N'', N'', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'hea_03_InvestmentPropNoCurrentMortgage', N'STAGE', N'random', N'hea', N'', N'&igen=1&esourceid=14349', N'HOMEEQUITY', N'Condominium w/5 or more stories', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'33139', N'Florida', N'Miami Beach', N'Investment Property', N'Over $1,000,000', 
N'Over $1,000,000', N'1991', N'No current mortgage', N'', N'', N'', N'', N'Over $400,000', N'stated-credit-history-fair', N'02', N'29', N'1984', 
N'61-72 months ago', N'73-84 months ago', N'Abdul-Khaaliq', N'', N'El-Sayed', N'', N'11115-01 Rushmore Dr', N'28277', N'North Carolina', N'Charlotte', N'', N'', N'', N'',
N'704', N'943', N'8320', N'', N'', N'', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'hea_04_LeadingZeros', N'STAGE', N'random', N'hea', N'', N'&igen=1&esourceid=14349', N'HOMEEQUITY', N'Condominium w/4 or fewer stories', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'02215', N'Massachusetts', N'Boston', N'Primary Home', N'$350,001 - $375,000', 
N'$300,001 - $325,000', N'1998', N'First mortgage only', N'$160,001 - $170,000', N'', N'', N'', N'$45,001 - $50,000', N'stated-credit-history-poor', N'01', N'01', N'1955', 
N'Never Foreclosed', N'Never/Not in the last 7 years', N'Otto', N'', N'Tester', N'', N'4 Yawkey Way', N'02215', N'Massachusetts', N'Boston', N'', N'', N'', N'',
N'407', N'939', N'3463', N'', N'', N'', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'hea_05_LtvTest', N'STAGE', N'random', N'hea', N'', N'&igen=1&esourceid=14349', N'HOMEEQUITY', N'Single-Family Detached', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'92802', N'California', N'Anaheim', N'Primary Home', N'$190,001 - $200,000', 
N'$90,001 - $100,000', N'2004', N'First & second mortgages', N'$160,001 - $170,000', N'', N'$20,001 - $30,000', N'', N'$10,001 - $15,000', N'stated-credit-history-good', N'12', N'31', N'1960', 
N'Never Foreclosed', N'Never/Not in the last 7 years', N'Otto', N'', N'Tester', N'', N'1313 S Harbor Blvd', N'92802', N'California', N'Anaheim', N'', N'', N'', N'',
N'407', N'939', N'3463', N'', N'', N'', N'default', N'default', N'default', N'default', N'default', N'')
GO

-- ************************************
-- ***** End of HEA Tests         *****
-- ************************************


-- ************************************
-- ***** Insert Home Equity Tests *****
-- ************************************

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'homeequity_01_Loan', N'STAGE', N'random', N'homeequity', N'', N'&esourceid=14349', N'HOMEEQUITY', N'Single-Family Detached', N'Loan', N'20 years', 
N'Y', N'N', N'N', N'N', N'Y', N'N', N'N', N'', N'34747', N'Florida', N'Kissimmee', N'Primary Home', N'$350,001 - $375,000', 
N'$300,001 - $325,000', N'2004', N'First & Second Mortgages', N'$160,001 - $170,000', N'$901 - $1,000', N'$20,001 - $30,000', N'$400 or less', N'$45,001 - $50,000', N'Excellent (720+)', N'01', N'02', N'1960', 
N'Never Foreclosed', N'Never/Not in the last 7 years', N'Otto', N'B', N'Tester', N'', N'225 Celebration Pl', N'34747', N'Florida', N'Kissimmee', N'4', N'06', N'', N'', 
N'407', N'939', N'3463', N'407', N'939', N'7675', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'homeequity_02_LineOfCredit', N'STAGE', N'random', N'homeequity', N'', N'&igen=1&esourceid=14349', N'HOMEEQUITY', N'Town House', N'Line of Credit', N'10 years', 
N'N', N'N', N'N', N'N', N'N', N'N', N'Y', N'Emergency Fund', N'34747', N'Florida', N'Kissimmee', N'Secondary Home', N'$200,001 - $225,000', 
N'$200,001 - $225,000', N'2010', N'First Mortgage Only', N'$190,001 - $200,000', N'$1,001 - $1,500', N'', N'', N'$20,001 - $25,000', N'Good (680-719)', N'12', N'31', N'1960', 
N'Over 84 months ago', N'73-84 months ago', N'Marie Clair', N'', N'Van Der Beek', N'', N'11115 Rushmore Dr', N'28277', N'North Carolina', N'Charlotte', N'1', N'11', N'123 Previous Ln', N'28226',
N'704', N'943', N'8320', N'704', N'541', N'5351', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'homeequity_03_NoPreference', N'STAGE', N'random', N'homeequity', N'', N'&igen=1&esourceid=14349', N'HOMEEQUITY', N'Condominium w/5 or more stories', N'I’d like offers for both', N'No Preference', 
N'N', N'Y', N'N', N'N', N'Y', N'N', N'N', N'', N'33139', N'Florida', N'Miami Beach', N'Investment Property', N'Over $1,000,000', 
N'Over $1,000,000', N'1991', N'No current mortgage', N'', N'', N'', N'', N'Over $400,000', N'Fair (640-679)', N'10', N'31', N'1955', 
N'61-72 months ago', N'73-84 months ago', N'Abdul-Khaaliq', N'', N'El-Sayed', N'', N'11115-01 Rushmore Dr', N'28277', N'North Carolina', N'Charlotte', N'20', N'00', N'', N'',
N'704', N'943', N'8320', N'704', N'541', N'5351', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'homeequity_04_RequiredFieldTest', N'STAGE', N'random', N'homeequity', N'', N'&igen=1&esourceid=14349', N'HOMEEQUITY', N'Single-Family Detached', N'Loan', N'20 years', 
N'Y', N'N', N'N', N'N', N'Y', N'N', N'N', N'', N'34747', N'Florida', N'Kissimmee', N'Primary Home', N'$350,001 - $375,000', 
N'$300,001 - $325,000', N'1998', N'First Mortgage Only', N'$160,001 - $170,000', N'$901 - $1,000', N'', N'', N'$45,001 - $50,000', N'Excellent (720+)', N'01', N'01', N'1955', 
N'Never Foreclosed', N'Never/Not in the last 7 years', N'Otto', N'B', N'Tester', N'', N'225 Celebration Pl', N'34747', N'Florida', N'Kissimmee', N'14', N'00', N'', N'',
N'407', N'939', N'3463', N'407', N'939', N'7675', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'homeequity_05_LtvTest', N'STAGE', N'random', N'homeequity', N'', N'&igen=1&esourceid=14349', N'HOMEEQUITY', N'Single-Family Detached', N'Loan', N'20 years', 
N'Y', N'N', N'N', N'N', N'Y', N'N', N'N', N'', N'34747', N'Florida', N'Kissimmee', N'Primary Home', N'$190,001 - $200,000', 
N'$90,001 - $100,000', N'2004', N'First & Second Mortgages', N'$160,001 - $170,000', N'$901 - $1,000', N'$20,001 - $30,000', N'$400 or less', N'$10,001 - $15,000', N'Good (680-719)', N'12', N'31', N'1960', 
N'Never Foreclosed', N'Never/Not in the last 7 years', N'Otto', N'B', N'Tester', N'', N'225 Celebration Pl', N'34747', N'Florida', N'Kissimmee', N'5', N'00', N'', N'',
N'407', N'939', N'3463', N'407', N'939', N'7675', N'default', N'default', N'default', N'default', N'default', N'')
GO

-- ************************************
-- ***** End of Home Equity Tests *****
-- ************************************


