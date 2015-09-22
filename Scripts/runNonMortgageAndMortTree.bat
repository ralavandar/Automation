@echo off
REM This should be a comma-separated list with NO SPACES.
set tests_to_run=TestAutomation.LendingTree.tl.autoTests,TestAutomation.LendingTree.tl.ccreturnTests,TestAutomation.LendingTree.tl.gsmortgageTests,TestAutomation.LendingTree.tl.gsmortgageVariationTests,TestAutomation.LendingTree.tl.homeequityTests,TestAutomation.LendingTree.tl.ltreturnTests,TestAutomation.LendingTree.tl.reverseMortgageTests

set log_label=NonMortMortTree

call runtesthelper.bat