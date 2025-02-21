using System;
using NUnit.Framework;
using DotNetSelenium.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DotNetSelenium
{
    public class Tests
    {

        private IWebDriver? driver;
        private LoginPage? loginPage;
        private AppointmentPage? appointmentPage;
        private OperationTheatrePage operationTheatrePage;
        private DoctorPage doctorPage;
        private ProcurementPage procurementPage;
        private UtilitiesPage utilitiesPage;
        private AdminPage adminPage;
        private PatientPage patientPage;
        private IncentivePage incentivePage;
        private SettingsPage settingsPage;
        private SubstorePage substorePage;
        private ADTPage adtPage;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://healthapp.yaksha.com/");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

            // Initialize the login page
            loginPage = new LoginPage(driver);
            appointmentPage = new AppointmentPage(driver);
            operationTheatrePage = new OperationTheatrePage(driver);
            doctorPage = new DoctorPage(driver);
            procurementPage = new ProcurementPage(driver);
            utilitiesPage = new UtilitiesPage(driver); 
            adminPage = new AdminPage(driver);
            patientPage = new PatientPage(driver);
            incentivePage = new IncentivePage(driver);
            settingsPage = new SettingsPage(driver);
            substorePage = new SubstorePage(driver);
            adtPage = new ADTPage(driver);

            // Perform login before every test
            loginPage.PerformLogin();
        }

        [Test]
        public void TestVerifyVisitTypeDropdown()
        {
            // Calling the method to verify visit type dropdown functionality
            appointmentPage.VerifyVisitTypeDropdown();
            VerifyVisitType(driver);
        }

        [Test]
        public void Test_HandleOtBookingAlert()
        {
            operationTheatrePage.HandleOtBookingAlert();
            OTBookingModalIsDisplayed(driver);
        }

        [Test]
        public void Test_VerifyPatientOverview()
        {
            doctorPage.VerifyPatientOverview();
            VerifyUserIsOnCorrectURL(driver, "Doctors/PatientOverviewMain/PatientOverview");
        }

        [Test]
        public void Test_AddProgressNoteForPatient()
        {
            doctorPage.AddProgressNoteForPatient();
            VerifyUserIsOnCorrectURL(driver, "Doctors/PatientOverviewMain/NotesSummary/NotesList");
        }

        [Test]
        public void Test_AddCurrencyAndVerify()
        {
            procurementPage.AddCurrencyAndVerify();
            VerifyUserIsOnCorrectURL(driver, "ProcurementMain/Settings/CurrencyList");
        }

        [Test]
        public void Test_VerifyMandatoryFieldsWarning()
        {
            utilitiesPage.VerifyMandatoryFieldsWarning();
            VerifyUserIsOnCorrectURL(driver, "/Utilities/SchemeRefund");
        }

        [Test]
        public void Test_VerifyUserProfileNavigation()
        {
            adminPage.VerifyUserProfileNavigation();
            VerifyUserIsOnCorrectURL(driver, "Employee/ProfileMain/UserProfile");
        }

        [Test]
        public void Test_UploadProfilePicture()
        {
            patientPage.UploadProfilePicture();
            VerifyImageIsUploaded(driver);
        }

        [Test]
        public void Test_EditTDSForEmployee()
        {
            incentivePage.EditTDSForEmployee();
            VerifyTdsTest(driver);
        }

        [Test]
        public void Test_TogglePriceCategoryStatus()
        {
            settingsPage.TogglePriceCategoryStatus();
            VerifyUserIsOnCorrectURL(driver, "Settings/PriceCategory");
        }

        [Test]
        public void Test_VerifyNavigationBetweenSubmodules()
        {
            substorePage.VerifyNavigationBetweenSubmodules();
            VerifyUserIsOnCorrectURL(driver, "Inventory/Return");
        }

        [Test]
        public void Test_VerifyTooltipText()
        {
            substorePage.VerifyTooltipText();
            IsTooltipDisplayed(driver);
        }

        [Test]
        public void Test_CaptureInventoryRequisitionScreenshot()
        {
            substorePage.CaptureInventoryRequisitionScreenshot();
            VerifyUserIsOnCorrectURL(driver, "Inventory/InventoryRequisitionList");
        }

        [Test]
        public void Test_VerifyFieldLevelErrorMessage()
        {
            adtPage.VerifyFieldLevelErrorMessage();
            VerifyErrorMessage(driver);
        }

        [Test]
        public void Test_VerifyLogoutFunctionality()
        {
            loginPage.VerifyLogoutFunctionality();
            VerifyUserIsLoggedOut(driver);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }


        /**
 * ------------------------------------------------------Helper Methods----------------------------------------------------
 */

        public void VerifyUserIsLoggedIn(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@class='dropdown dropdown-user']")));
            Assert.IsTrue(driver.FindElement(By.XPath("//li[@class='dropdown dropdown-user']")).Displayed);
        }

        public void VerifyUserIsLoggedOut(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.Id("login")).Displayed);
        }

        public void VerifyVisitType(IWebDriver driver)
        {
            var tableElements = driver.FindElements(By.CssSelector("div[col-id='AppointmentType']"));
            Assert.Greater(tableElements.Count, 1);
        }

        public void VerifyUserIsOnCorrectURL(IWebDriver driver, string expectedURL)
        {
            string actualURL = driver.Url;
            Assert.IsTrue(actualURL.Contains(expectedURL));
        }

        public void VerifyImageIsUploaded(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.CssSelector("div.wrapper img")).Displayed);
        }

        public void IsTooltipDisplayed(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.CssSelector("div.modal-content")).Displayed);
        }

        public void VerifyErrorMessage(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.XPath("//span[text()='Select doctor from the list.']")).Displayed);
        }

        public void OTBookingModalIsDisplayed(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.CssSelector("div.modelbox-div")).Displayed);
        }

        public void VerifyIfRecordsArePresent(IWebDriver driver)
        {
            var records = driver.FindElements(By.CssSelector("div[col-id='PatientName']"));
            Assert.Greater(records.Count, 1);
        }

        public void VerifyTdsTest(IWebDriver driver)
        {
            var patientNames = driver.FindElements(By.CssSelector("div[col-id='FullName']"));
            Assert.IsTrue(patientNames[1].Text.Contains("Rakesh"));
        }


    }
}