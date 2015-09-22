--********************************************************
--* TestDataInsert_ProdTests_Forms_HomeEquity.sql
--********************************************************

-- Script to insert values for Form tests in Production - HomeEquity
-- This data is designed so that it does not receive referrals in Production
--		Million Dollar Mobile Home in Montana
--		Current Bankruptcy
--		Current Foreclosure

USE [AutomatedTesting]
GO

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
VALUES (N'Prod_hea_01', N'PROD', N'firefox', N'hea', N'', N'&esourceid=14349&mpid=8550&devicemonitor=selenium', N'HOMEEQUITY', N'Mobile/Manufactured Home', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'59602', N'Montana', N'Helena', N'Secondary home', N'Over $1,000,000', 
N'Over $1,000,000', N'2007', N'First mortgage only', N'$600,001 - $650,000', N'', N'', N'', N'Over $400,000', N'stated-credit-history-poor', N'01', N'02', N'1983', 
N'Currently in Foreclosure', N'1-12 months ago', N'Otto', N'', N'Tester', N'', N'11115 Rushmore Dr', N'28277', N'North Carolina', N'Charlotte', N'', N'', N'', N'', 
N'407', N'939', N'3463', N'', N'', N'', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Template], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'Prod_hea_02', N'PROD', N'IE', N'hea', N'', N'&esourceid=14349&mpid=8550&devicemonitor=selenium', N'HOMEEQUITY', N'Mobile/Manufactured Home', N'', N'', 
N'', N'', N'', N'', N'', N'', N'', N'', N'59602', N'Montana', N'Helena', N'Investment Property', N'Over $1,000,000', 
N'Over $1,000,000', N'1999', N'First & second mortgages', N'$500,001 - $550,000', N'', N'$100,001 - $110,000', N'', N'Over $400,000', N'stated-credit-history-poor', N'12', N'31', N'1970', 
N'Currently in Foreclosure', N'1-12 months ago', N'Otto', N'', N'Tester', N'', N'11115 Rushmore Dr', N'28277', N'North Carolina', N'Charlotte', N'', N'', N'', N'', 
N'407', N'939', N'3463', N'', N'', N'', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'Prod_HomeEquity_Loan', N'PROD', N'firefox', N'homeequity', N'&esourceid=14349&mpid=8550&devicemonitor=selenium', N'HOMEEQUITY', N'Mobile/Manufactured Home', N'Loan', N'20 years', 
N'N', N'N', N'Y', N'N', N'N', N'N', N'Y', N'Just Testing...Please Ignore', N'59602', N'Montana', N'Helena', N'Secondary Home', N'Over $1,000,000', 
N'Over $1,000,000', N'2007', N'First Mortgage Only', N'$550,001 - $600,000', N'Over $7,501', N'', N'', N'Over $400,000', N'Poor (580-639)', N'11', N'30', N'1960', 
N'Currently in Foreclosure', N'1-12 months ago', N'Otto', N'B', N'Tester', N'', N'11115 Rushmore Dr', N'28277', N'North Carolina', N'Charlotte', N'4', N'06', N'', N'', 
N'704', N'541', N'5351', N'407', N'939', N'3463', N'default', N'default', N'default', N'default', N'default', N'')
GO

INSERT [dbo].tTestData_HomeEquity ([TestCaseName], [TestEnvironment], [BrowserType], [Variation], [QueryString], [LoanType], [PropertyType], [HELoanType], [HELoanTerm],
 [LoanPurposeDebtYesNo], [LoanPurposeBoatYesNo], [LoanPurposeRvYesNo], [LoanPurposeMotorcycleYesNo], [LoanPurposeImprovementYesNo], [LoanPurposeAutoYesNo], [LoanPurposeOtherYesNo], [LoanPurposeOtherReason], 
 [PropertyZipCode], [PropertyState], [PropertyCity], [PropertyUse], [PropertyValue], [PurchasePrice], [PurchaseYear], 
 [CurrentMortgages], [FirstMortgageBalance], [FirstMortgagePayment], [SecondMortgageBalance], [SecondMortgagePayment], [RequestedLoanAmount], [CreditProfile], [DateOfBirthMonth], [DateOfBirthDay], 
 [DateOfBirthYear], [ForeclosureHistory], [BankruptcyHistory], [BorrowerFirstName], [BorrowerMiddleName], [BorrowerLastName], [BorrowerSuffix], [BorrowerStreetAddress], 
 [BorrowerZipCode], [BorrowerState], [BorrowerCity], [YearsAtAddress], [MonthsAtAddress], [PreviousStreetAddress], [PreviousZipCode], [BorrowerHomePhone1], [BorrowerHomePhone2], 
 [BorrowerHomePhone3], [BorrowerWorkPhone1], [BorrowerWorkPhone2], [BorrowerWorkPhone3], [BorrowerSsn1], [BorrowerSsn2], [BorrowerSsn3], [EmailAddress], [Password], [ConfirmPassword]) 
VALUES (N'Prod_HomeEquity_LineOfCredit', N'PROD', N'IE', N'homeequity', N'&esourceid=14349&mpid=8550&devicemonitor=selenium', N'HOMEEQUITY', N'Mobile/Manufactured Home', N'Line of Credit', N'10 years', 
N'Y', N'N', N'N', N'N', N'N', N'N', N'Y', N'Just Testing...Please Ignore', N'59602', N'Montana', N'Helena', N'Investment Property', N'Over $1,000,000', 
N'Over $1,000,000', N'1999', N'First Mortgage Only', N'$550,001 - $600,000', N'Over $7,501', N'', N'', N'Over $400,000', N'Poor (580-639)', N'11', N'30', N'1960', 
N'Currently in Foreclosure', N'1-12 months ago', N'Otto', N'B', N'Tester', N'', N'11115 Rushmore Dr', N'28277', N'North Carolina', N'Charlotte', N'4', N'06', N'', N'', 
N'704', N'541', N'5351', N'407', N'939', N'3463', N'default', N'default', N'default', N'default', N'default', N'')
GO

-- ************************************
-- ***** End of Home Equity Tests *****
-- ************************************


