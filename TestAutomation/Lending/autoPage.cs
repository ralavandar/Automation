using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestAutomation.LendingTree.tl
{
    public class autoPage : PageBase
    {
        private readonly IWebDriver autoDriver;
        private const String strTid = "auto";
        private const uint VARIATION_AUTOADVANCE = 5;
        private const uint VARIATION_VALIDATION = 12;

        private const int WAIT_TIME_ANIMATE = 2; //s
        private const int WAIT_TIME_AJAX = 10; //s
        private const int WAIT_TIME_STEP_ADVANCE = 5; //s

        //TODO: This delay before ending or beginning a step prevents certain mysterious issues like IE not hitting radio buttons. Increasing it might help if those issues resurface.
        private const int WAIT_TIME_INPUT_DELAY = 2000; //ms

        protected string currStep;
        private bool autoAdvanceReported = false;
        public bool shouldValidateHeader = true;
        public bool shouldAdvanceSteps = true;
        public bool ShouldAutoAdvance
        {
            get
            {
                var autoAdvanceVariation = GetVariation(VARIATION_AUTOADVANCE);
                if (autoAdvanceVariation == 0)
                {
                    if (!autoAdvanceReported) Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is turned OFF (0) for this test.");
                    autoAdvanceReported = true;
                    return false;
                }
                else if (autoAdvanceVariation == 1)
                {
                    if (!autoAdvanceReported) Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is turned ON (1) for this test.");
                    autoAdvanceReported = true;
                    return true;
                }
                else
                {
                    if (!autoAdvanceReported) Common.ReportEvent(Common.INFO, "The 'autoAdvance' variation is ON by default for this test.");
                    autoAdvanceReported = true;
                    return true;     //default value
                }
            }
        }
        private bool didLastFieldTriggerAdvance;

        private bool validationReported = false;
        public bool ShouldValidationStopProgress
        {
            get
            {
                var validationVariation = GetVariation(VARIATION_VALIDATION);
                if (validationVariation == 0)
                {
                    if (!validationReported) Common.ReportEvent(Common.INFO, "The 'validation stops progress' variation is turned ON (0) for this test.");
                    validationReported = true;
                    return true;
                }
                else if (validationVariation == 1)
                {
                    if (!validationReported) Common.ReportEvent(Common.INFO, "The 'validation stops progress' variation is turned OFF (1) for this test.");
                    validationReported = true;
                    return false;
                }
                else if (validationVariation == 2)
                {
                    if (!validationReported) Common.ReportEvent(Common.INFO, "The 'validation stops progress' variation is turned OFF (2) for this test.");
                    validationReported = true;
                    return false;
                }
                else
                {
                    if (!validationReported) Common.ReportEvent(Common.INFO, "The 'validation stops progress' variation is ON by default for this test.");
                    validationReported = true;
                    return true;     //default value
                }
            }
        }

        public Dictionary<string, string> testData;

        // Constructor
        public autoPage(IWebDriver driver, Dictionary<string, string> testData)
            : base(driver)
        {
            autoDriver = driver;
            this.testData = testData;
        }

        public void FillOutValidQF()
        {
            NavigateToFossaForm(testData["TestEnvironment"], "tl.aspx", strTid, testData["Variation"]);
            CompleteStep1();
            CompleteStep2();
            CompleteStep3();
            CompleteStep4();
            CompleteStep4a();
            CompleteStep4b();
            CompleteStep4c();
            CompleteStep5();
            CompleteStep6();
            CompleteStep7();
            CompleteStep8();
            CompleteStep9();
            CompleteStep10();
            CompleteStep11();
            CompleteStep12();
            CompleteStep13();
            CompleteStep14();
            CompleteStep15();
            CompleteStep16();
            
            // if there is a coborrower, then complete the remaining steps
            if (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                CompleteStep17();
                CompleteStep18();
                CompleteStep19();
                CompleteStep20();
                CompleteStep21();
                CompleteStep22();
                CompleteStep23();
                CompleteStep24();
                CompleteStep25();
                CompleteStep26();
                CompleteStep27();
            }
        }

        public void FillOutValidationStep()
        {
            shouldValidateHeader = false;
            shouldAdvanceSteps = false;
            CompleteStep1();
            CompleteStep2();
            CompleteStep3();
            CompleteStep4();
            CompleteStep4a();
            CompleteStep4b();
            CompleteStep4c();
            CompleteStep5();
            CompleteStep6();
            CompleteStep7();
            CompleteStep8();
            CompleteStep9();
            CompleteStep10();
            CompleteStep11();
            CompleteStep12();
            CompleteStep13();
            CompleteStep14();
            CompleteStep15();
            CompleteStep16();

            // if there is a coborrower, then complete the remaining steps
            if (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                CompleteStep17();
                CompleteStep18();
                CompleteStep19();
                CompleteStep20();
                CompleteStep21();
                CompleteStep22();
                CompleteStep23();
                CompleteStep24();
                CompleteStep25();
                CompleteStep26();
                CompleteStep27();
            }
        }

        public void CompleteStep1()
        {
            PrepareStep("step-1");

            // Capture/Report the GUID and QFVersion
            strQFormUID = autoDriver.FindElement(By.Id("GUID")).Text;
            Common.ReportEvent(Common.INFO, String.Format("The QForm GUID = {0}", strQFormUID));

            // Fill out the fields & click Continue
            SelectByValue("loan-type", testData["AutoLoanType"]);
            SelectByValue("loan-period", testData["AutoLoanTerm"]);
            
            ContinueToNextStep();
        }

        public void CompleteStep2(Boolean setCoborrowerOnly = false)
        {
            PrepareStep("step-2");

            if (!setCoborrowerOnly)
            {
                // New Car Purchase OR Used Car Purchase
                if (!testData["AutoLoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase))
                {
                    SelectByText("vehicle-state", testData["VehicleState"]);
                }
            }

            if (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("cb-relationship-yes");
            }
            else
            {
                ClickRadio("cb-relationship-no");
            }

            // Work-around: auto-advance is not working when the borrower selects Yes for cobo question;
            //              so hard-coding click on 'next' in this case
            if (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Common.ReportEvent(Common.INFO, "***** clicking next *****" + DateTime.Now);
                ClickButton("next");
            }
            else
            {
                ContinueToNextStep();
            }
        }

        public void CompleteStep3()
        {
            // MPS, 4/17 - per LTAUTO-1234, step 3 was removed for REFINANCE.
            // So converting this step to a switch-case to account for the various scenarios
            switch (testData["AutoLoanType"].ToUpper())
            {
                case "NEWCARPURCHASE":
                case "USEDCARPURCHASE":
                case "BUYOUTLEASE":
                    PrepareStep("step-3");
                    if (testData["AutoLoanType"].Equals("NewCarPurchase", StringComparison.OrdinalIgnoreCase)
                        || testData["AutoLoanType"].Equals("UsedCarPurchase", StringComparison.OrdinalIgnoreCase))
                    {
                        Fill("down-payment", testData["PurchaseDownPayment"]);
                    }
                    Fill("requested-loan-amount", testData["RequestedLoanAmount"]);
                    ContinueToNextStep();
                    break;
                case "REFINANCE":
                    Common.ReportEvent(Common.INFO, "***** Skipping Step 3 for " + testData["AutoLoanType"] + " *****");
                    break;
                default:
                    Common.ReportEvent(Common.WARNING, "An unknown value was passed for AutoLoanType: '" + testData["AutoLoanType"] + "'.");
                    Common.ReportEvent(Common.INFO, "***** Skipping Step 3 for " + testData["AutoLoanType"] + " *****");
                    break;
            }
        }

        public void CompleteStep4()
        {
            PrepareStep("step-4");

            SelectByText("vehicle-year", testData["VehicleYear"]);
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            SelectByText("vehicle-make", testData["VehicleMake"]);
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            SelectByText("vehicle-model", testData["VehicleModel"]);
            WaitForAjaxToComplete(WAIT_TIME_AJAX);
            SelectByText("vehicle-trim", testData["VehicleTrim"]);

            ContinueToNextStep();
        }

        public void CompleteStep4a()
        {
            if (!testData["AutoLoanType"].Equals("NewCarPurchase", StringComparison.OrdinalIgnoreCase))
            {
                PrepareStep("step-4a");

                Fill("vehicle-identification-number", testData["VehicleIdNumber"]);
                Fill("vehicle-mileage", testData["VehicleMileage"]);

                ContinueToNextStep();
            }
            else
            {
                Common.ReportEvent(Common.INFO, "***** Skipping Step 4a *****");
            }
        }

        public void CompleteStep4b()
        {
            if (testData["AutoLoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase)
                || testData["AutoLoanType"].Equals("BUYOUTLEASE", StringComparison.OrdinalIgnoreCase))
            {
                PrepareStep("step-4b");


                Fill("current-interest-rate", testData["CurrentRate"]);
                Fill("current-lender", testData["CurrentLender"]);

                ContinueToNextStep();
            }
            else
            {
                Common.ReportEvent(Common.INFO, "***** Skipping Step 4b *****");
            }
        }

        public void CompleteStep4c()
        {
            if (testData["AutoLoanType"].Equals("REFINANCE", StringComparison.OrdinalIgnoreCase)
                || testData["AutoLoanType"].Equals("BUYOUTLEASE", StringComparison.OrdinalIgnoreCase))
            {
                PrepareStep("step-4c");

                Fill("payoff-amount", testData["CurrentPayoffAmount"]);
                SelectByText("remaining-terms", testData["CurrentRemainingTerms"]);
                Fill("current-vehicle-payment", testData["CurrentPayment"]);

                ContinueToNextStep();
            }
            else
            {
                Common.ReportEvent(Common.INFO, "***** Skipping Step 4c *****");
            }
        }

        public void CompleteStep5()
        {
            PrepareStep("step-5");

            SelectByText("birth-month", testData["DateOfBirthMonth"]);
            SelectByText("birth-day", testData["DateOfBirthDay"]);
            SelectByText("birth-year", testData["DateOfBirthYear"]);

            ContinueToNextStep();
        }

        public void CompleteStep6()
        {
            PrepareStep("step-6");

            if (testData["BankruptcyHistory"].Equals("N", StringComparison.OrdinalIgnoreCase)
                || testData["BankruptcyHistory"].Equals("", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("declared-bankruptcy-no");
            }
            else
            {
                ClickRadio("declared-bankruptcy-yes");
                SelectByText("bankruptcy-discharged", testData["BankruptcyHistory"]);
            }

            ContinueToNextStep();
        }

        public void CompleteStep7()
        {
            PrepareStep("step-7");

            SelectByText("employment-status", testData["BorrowerEmploymentStatus"]);
            // If Employment status is one of the 'working' statuses, then complete these questions.  Otherwise, skip them.
            if ("FULLTIME, PARTTIME, SELFEMPLOYED".Contains(GetElement("employment-status").GetAttribute("value")))
            {
                int jobMonths = (Int16.Parse(testData["BorrowerEmploymentTimeYears"]) * 12) + Int16.Parse(testData["BorrowerEmploymentTimeMonths"]);
                DateTime jobStartDate = DateTime.Now;
                jobStartDate = jobStartDate.AddMonths(-jobMonths);
                SelectByText("job-start-month", jobStartDate.Month.ToString().PadLeft(2, '0'));
                SelectByText("job-start-year", jobStartDate.Year.ToString());
            }

            ContinueToNextStep();
        }

        public void CompleteStep8()
        {
            if ("FULLTIME, PARTTIME, SELFEMPLOYED".Contains(GetElement("employment-status").GetAttribute("value")))
            {
                PrepareStep("step-8");

                Fill("employer-name", testData["BorrowerEmployerName"]);
                Fill("job-title", testData["BorrowerJobTitle"]);

                ContinueToNextStep();
            }
            else
            {
                Common.ReportEvent(Common.INFO, "***** Skipping Step 8 *****");
            }
        }

        public void CompleteStep9()
        {
            PrepareStep("step-9");

            Fill("income", testData["BorrowerIncome"]);
            Fill("other-income", testData["BorrowerOtherIncome"]);

            if (Int32.Parse(testData["BorrowerIncome"]) < 12000)
            {
                System.Threading.Thread.Sleep(2000);
                Validation.IsTrue(DoesPageContainText("You entered a value less than $12,000 a year. Please verify and click the button below if this is correct."));
            }
            ContinueToNextStep();
        }

        public void CompleteStep10()
        {
            PrepareStep("step-10");

            Fill("asset-value", testData["BorrowerAssets"]);

            ContinueToNextStep();
        }

        public void CompleteStep11()
        {
            PrepareStep("step-11");

            Fill("first-name", testData["BorrowerFirstName"]);
            Fill("last-name", testData["BorrowerLastName"]);
            Fill("email", testData["EmailAddress"]);
            Fill("password", testData["Password"]);

            ContinueToNextStep();
        }

        public void CompleteStep12()
        {
            PrepareStep("step-12");

            Fill("street1", testData["BorrowerStreetAddress"]);
            if (testData["CoborrowerYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase) && testData["CoSameAddressYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Check("cb-same-addr");
            }
            Fill("zip-code", testData["BorrowerZipCode"]);
            //force a blur by clicking back to the street1 field
            driver.FindElement(By.Id("street1")).Click();
            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
            ContinueToNextStep();
        }

        public void CompleteStep13()
        {
            PrepareStep("step-13");

            if (testData["BorrowerRentOwn"].Equals("OWN", StringComparison.OrdinalIgnoreCase))
                ClickRadio("own-or-rent-own");
            else if (testData["BorrowerRentOwn"].Equals("RENT", StringComparison.OrdinalIgnoreCase))
                ClickRadio("own-or-rent-rent");
            else
                ClickRadio("own-or-rent-other");


            SelectByText("time-at-address-year", testData["BorrowerYearsAtAddress"]);
            SelectByText("time-at-address-month", testData["BorrowerMonthsAtAddress"]);
            
            ContinueToNextStep();
        }

        public void CompleteStep14()
        {
            PrepareStep("step-14");

            Fill("current-housing-payment", testData["BorrowerHousingPayment"]);

            ContinueToNextStep();
        }

        public void CompleteStep15()
        {
            PrepareStep("step-15");

            Fill("home-phone-one", testData["BorrowerHomePhone1"]);
            Fill("home-phone-two", testData["BorrowerHomePhone2"]);
            Fill("home-phone-three", testData["BorrowerHomePhone3"]);
            Fill("work-phone-one", testData["BorrowerWorkPhone1"]);
            Fill("work-phone-two", testData["BorrowerWorkPhone2"]);
            Fill("work-phone-three", testData["BorrowerWorkPhone3"]);

            ContinueToNextStep();
        }

        public void CompleteStep16()
        {
            PrepareStep("step-16");

            Fill("social-security-one", testData["BorrowerSsn1"]);
            Fill("social-security-two", testData["BorrowerSsn2"]);
            Fill("social-security-three", testData["BorrowerSsn3"]);

           ContinueToNextStep();
            
        }

        public void CompleteStep17()
        {
            PrepareStep("step-17");

            Fill("cb-first-name", testData["CoborrowerFirstName"]);
            Fill("cb-last-name", testData["CoborrowerLastName"]);
            Fill("cb-email", testData["CoEmailAddress"]);

            ContinueToNextStep();
        }

        public void CompleteStep18()
        {
            PrepareStep("step-18");

            SelectByText("cb-birth-month", testData["CoDateOfBirthMonth"]);
            SelectByText("cb-birth-day", testData["CoDateOfBirthDay"]);
            SelectByText("cb-birth-year", testData["CoDateOfBirthYear"]);

            ContinueToNextStep();
        }

        public void CompleteStep19()
        {
            PrepareStep("step-19");

            if (testData["CoBankruptcyHistory"].Equals("N", StringComparison.OrdinalIgnoreCase)
                || testData["CoBankruptcyHistory"].Equals("", StringComparison.OrdinalIgnoreCase))
            {
                ClickRadio("cb-declared-bankruptcy-no");
            }
            else
            {
                ClickRadio("cb-declared-bankruptcy-yes");
                SelectByText("cb-bankruptcy-discharged", testData["CoBankruptcyHistory"]);
            }

            ContinueToNextStep();
        }

        public void CompleteStep20()
        {
            PrepareStep("step-20");

            SelectByText("cb-employment-status", testData["CoEmploymentStatus"]);

            // If Employment status is one of the 'working' statuses, then complete these questions.  Otherwise, skip them.
            if ("FULLTIME, PARTTIME, SELFEMPLOYED".Contains(GetElement("cb-employment-status").GetAttribute("value")))
            {
                int jobMonths = (Int16.Parse(testData["CoEmploymentTimeYears"]) * 12) + Int16.Parse(testData["CoEmploymentTimeMonths"]);
                DateTime jobStartDate = DateTime.Now;
                jobStartDate = jobStartDate.AddMonths(-jobMonths);
                SelectByText("cb-job-start-month", jobStartDate.Month.ToString().PadLeft(2, '0'));
                SelectByText("cb-job-start-year", jobStartDate.Year.ToString());
            }

            ContinueToNextStep();
        }

        public void CompleteStep21()
        {
            if ("FULLTIME, PARTTIME, SELFEMPLOYED".Contains(GetElement("cb-employment-status").GetAttribute("value")))
            {
                PrepareStep("step-21");

                Fill("cb-employer-name", testData["CoEmployerName"]); 
                Fill("cb-job-title", testData["CoJobTitle"]);
                
                ContinueToNextStep();
            }
            else
            {
                Common.ReportEvent(Common.INFO, "***** Skipping Step 21 *****");
            }
        }

        public void CompleteStep22()
        {
            PrepareStep("step-22");

            Fill("cb-income", testData["CoborrowerIncome"]);
            Fill("cb-other-income", testData["CoborrowerOtherIncome"]);

            ContinueToNextStep();
        }

        public void CompleteStep23()
        {
            PrepareStep("step-23");

            Fill("cb-asset-value", testData["CoborrowerAssets"]);

            ContinueToNextStep();
        }

        public void CompleteStep24()
        {
            if (!testData["CoSameAddressYesNo"].Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                PrepareStep("step-24");

                Fill("cb-street1", testData["CoStreetAddress"]);
                Fill("cb-zip-code", testData["CoborrowerZipCode"]);
                WaitForAjaxToComplete(WAIT_TIME_AJAX);
                ContinueToNextStep();
            }
            else
            {
                Common.ReportEvent(Common.INFO, "***** Skipping Step 24 *****");
            }
        }

        public void CompleteStep25()
        {
            PrepareStep("step-25");

            if (testData["CoborrowerRentOwn"].Equals("OWN", StringComparison.OrdinalIgnoreCase))
                ClickRadio("cb-own-or-rent-own");
            else if (testData["CoborrowerRentOwn"].Equals("RENT", StringComparison.OrdinalIgnoreCase))
                ClickRadio("cb-own-or-rent-rent");
            else
                ClickRadio("cb-own-or-rent-other");

            SelectByText("cb-time-at-address-year", testData["CoYearsAtAddress"]);
            SelectByText("cb-time-at-address-month", testData["CoMonthsAtAddress"]);

            ContinueToNextStep();
        }

        public void CompleteStep26()
        {
            PrepareStep("step-26");

            Fill("cb-home-phone-one", testData["CoborrowerHomePhone1"]);
            Fill("cb-home-phone-two", testData["CoborrowerHomePhone2"]);
            Fill("cb-home-phone-three", testData["CoborrowerHomePhone3"]);
            Fill("cb-work-phone-one", testData["CoborrowerWorkPhone1"]);
            Fill("cb-work-phone-two", testData["CoborrowerWorkPhone2"]);
            Fill("cb-work-phone-three", testData["CoborrowerWorkPhone3"]);
           
            ContinueToNextStep();
        }

        public void CompleteStep27()
        {
            PrepareStep("step-27");


            Fill("cb-social-security-one", testData["CoborrowerSsn1"]);
            Fill("cb-social-security-two", testData["CoborrowerSsn2"]);
            Fill("cb-social-security-three", testData["CoborrowerSsn3"]);

            ContinueToNextStep();
        }

        public void PrepareStep(string step)
        {
            currStep = step;
            didLastFieldTriggerAdvance = false;
            Common.ReportEvent(Common.INFO, "***** Starting " + step + " *****");
            if (shouldValidateHeader)
            {
                
                try
                {
                    WaitForElementDisplayed(By.Id(step), WAIT_TIME_STEP_ADVANCE);
                }
                catch (WebDriverTimeoutException)
                {
                    Common.ReportEvent("ERROR", "Step " + step + " did not appear before the timeout; the form may not have advanced to the next step as expected.");
                    throw;
                }
            }

            WaitUntilNoAnimation(WAIT_TIME_STEP_ADVANCE);

            System.Threading.Thread.Sleep(WAIT_TIME_INPUT_DELAY);
        }


        public void ContinueToNextStep()
        {
            System.Threading.Thread.Sleep(WAIT_TIME_STEP_ADVANCE);
            if (!shouldAdvanceSteps)
            {
                Common.ReportEvent("INFO", "Validation step, not advancing steps.");
                return;
            }
            if (!(ShouldAutoAdvance && didLastFieldTriggerAdvance))
            {
                Validation.IsTrue(IsElementDisplayed(By.Id(currStep)));
                By nextBtn= By.Id("next");
                
                if (IsElementDisplayed(nextBtn))
                {
                    if (driver.FindElement(nextBtn).GetAttribute("class").ToString().Contains("disabled"))
                    {
                        Common.ReportEvent(Common.INFO, "***** next disabled, waiting *****" + DateTime.Now);
                        System.Threading.Thread.Sleep(7000);
                    }
                    Common.ReportEvent(Common.INFO, "***** clicking next *****" + DateTime.Now);
                    ClickButton("next");
                }
            }
            else
            {
                Common.ReportEvent("INFO", "Auto-advance expected.");
            }
            
        }

        public void ClickThroughSteps(Int32 intNumSteps)
        {
            if (!ShouldValidationStopProgress)
            {
                Common.ReportEvent(Common.INFO, "The 'validation stops progress' variation is turned OFF for this test. Clicking through " + intNumSteps +" steps");
                // Loop through steps - Verify on expected step, delay for 1 sec, then click Continue/Submit
                for (int i = 1; i <= intNumSteps; i++)
                {
                    var strStepNum = "step-" + i.ToString();
                    Common.ReportEvent(Common.INFO, "waiting for step id " + strStepNum + " to show");
                    WaitForElement(By.Id(strStepNum), 10);
                    Assert.IsTrue(IsElementPresent(By.Id(strStepNum)), "Unable to verify the script is on Step "
                        + i.ToString() + ".  Cannot locate element with an id of '" + strStepNum + "'.");
                    System.Threading.Thread.Sleep(1000);
                    Common.ReportEvent(Common.INFO, "proceeding from " + strStepNum + " to next step");
                    // Click the continue button
                    ContinueToNextStep();
                }
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The 'validation stops progress' variation is turned ON for this test. Cannot click through all steps");
            }
        }

        /// <summary>
        /// Verifies redirect to MyLendingTree Express page
        /// </summary>
        /// <param name="testData"></param>
        public void VerifyRedirectToMyLtExpress(Dictionary<string, string> testData)
        {
            System.Threading.Thread.Sleep(5000);

            // Check for redirect to mc.aspx / offers page
            if (IsElementDisplayed(By.ClassName("user-icon"), 25))    // element inside nav bar at very top of MyLendingTree page
            {
                // Specific checks on MyLendingTree
                System.Threading.Thread.Sleep(1000);

                try
                {
                    Assert.IsTrue(driver.Url.Contains("/express-offers"));

                    Common.ReportEvent(Common.PASS, String.Format
                        ("The TestString contains the expected value.  Expected: '/express-offers'.  TestString: \"{0}\".",
                           driver.Url));
                }
                catch (AssertionException)
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The TestString does not contain the expected value.  Expected: '/express-offers'.  TestString: \"{0}\".",
                            driver.Url));
                }

                try
                {
                    Assert.IsTrue(driver.Url.Contains("&guid=" + strQFormUID));

                    Common.ReportEvent(Common.PASS, String.Format
                        ("The TestString contains the expected value.  Expected: '&guid=<QFormUID>'.  TestString: \"{0}\".",
                           driver.Url));
                }
                catch (AssertionException)
                {
                    Common.ReportEvent(Common.FAIL, String.Format
                        ("The TestString does not contain the expected value.  Expected: '&guid=<QFormUID>'.  TestString: \"{0}\".",
                            driver.Url));
                }

                // Validate page header/nav contains text <firstname> + " " + <lastname>
                Validation.StringContains(testData["BorrowerFirstName"] + " " + testData["BorrowerLastName"],
                    driver.FindElement(By.ClassName("user-icon")).FindElement(By.TagName("a")).Text);
            }
            else
            {
                Common.ReportEvent(Common.FAIL, "The redirect to My LendingTree did not display within 30 seconds. "
                    + "Expecting an element with ClassName 'user-icon'.");
                RecordScreenshot("MyLendingTreeException");
                Assert.Fail();
            }
        }

        public void BypassAutoCrossSells()
        {
            // Check for Processing Dialog display
            // CheckForProcessing();
            // System.Threading.Thread.Sleep(500);

            //check for xsellModal info box
            if (IsElementDisplayed(By.Id("xsellModal"), 10))    // xsell modal dialog
            {
                Common.ReportEvent(Common.INFO, String.Format("Clicking close button on xsellModal dialog: {0}",
                    driver.FindElement(By.Id("xsellModal")).FindElement(By.ClassName("modalTitle")).Text));

                driver.FindElement(By.Id("xsellModal")).FindElement(By.ClassName("close")).Click();
            }
            else
            {
                Common.ReportEvent(Common.INFO, "The xsellModal did not display within 10 seconds.  This may be expected behavior depending on template and vid.");
            }
            
            // Click the 'next' link on the 3 distinct cross sells
            try
            {
                WaitForElement(By.Id("txtCrossSellZipcode"), 5);  // Auto Insurance cross-sell
                System.Threading.Thread.Sleep(500);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on Auto Insurance cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No Auto Insurance cross-sell shown, this may be expected behavior");
            }

            try
            {
                //WaitForElement(By.Id("btnExperianSubmit"), 5);     // Experian Credit Score cross-sell - this one retired.
                WaitForElement(By.CssSelector("[alt='Continue to Free Score']"), 5);  //New Credit Score cross-sell
                System.Threading.Thread.Sleep(500);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on Experian Credit Score cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No Experian Credit Score cross-sell shown, this may be expected behavior");
            }

            try
            {
                WaitForElement(By.ClassName("skip_three"), 5);     // Carfax Report cross-sell
                System.Threading.Thread.Sleep(500);
                Common.ReportEvent(Common.INFO, "Clicking 'No thanks' on Carfax Report cross-sell");
                ClickButton("demoBtn");
            }
            catch (Exception)
            {
                Common.ReportEvent(Common.INFO, "No Carfax Report cross-sell shown, this may be expected behavior");
            }
        }

        public override void ClickRadio(String strId)
        {
            base.ClickRadio(strId);
            didLastFieldTriggerAdvance = true;
        }

        public override void Fill(String strId, String strValue)
        {
            base.Fill(strId, strValue);
            didLastFieldTriggerAdvance = false;
        }

        public override void SelectByText(String ID, String text)
        {
            WrapSelectWithAutoadvanceCheck(ID, text, base.SelectByText);
        }

        public override void SelectByValue(String ID, String value)
        {
            WrapSelectWithAutoadvanceCheck(ID, value, base.SelectByValue);
        }

        private void WrapSelectWithAutoadvanceCheck(String ID, String value, Action<string, string> toWrap)
        {
            SelectElement element = new SelectElement(driver.FindElement(By.Id(ID)));
            IWebElement oldSelection = null;
            if (element.AllSelectedOptions.Count > 0) oldSelection = element.SelectedOption;

            toWrap(ID, value);

            if (oldSelection == null) didLastFieldTriggerAdvance = (null != (element.AllSelectedOptions.Count > 0 ? element.SelectedOption : null));
            else didLastFieldTriggerAdvance = !oldSelection.GetAttribute("value").Equals(element.AllSelectedOptions.Count > 0 ? element.SelectedOption.GetAttribute("value") : null);
            //Common.ReportEvent("DEBUG", "SelectByText autoAdvance trigger " + oldSelection.GetAttribute("value") + " ?= " + element.SelectedOption.GetAttribute("value"));
        }

        public void WaitUntilNoAnimation(Int32 intSeconds)
        {
            WebDriverWait objWait = new WebDriverWait(driver, TimeSpan.FromSeconds(intSeconds));

            Boolean blnDisplayed = objWait.Until<Boolean>((d) =>
            {
                //Use jQuery to see if there are any elements with the pseudoclass :animated. Probably a pretty slow check, so let's not use this in performance-critical code.
                return ((IJavaScriptExecutor)d).ExecuteScript("return $(':animated').length").ToString() == "0";
            });
        }

        private uint? GetVariation(uint variationIndex)
        {
            string VID = testData["Variation"];

            //If the VID isn't long enough, return null to indicate the default value.
            if (VID.Length < (2 * variationIndex) + 1) return null;

            return uint.Parse(VID.Substring((int)(2 * variationIndex), 1));
        }
    }
}



