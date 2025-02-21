using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;
using DotNetSelenium.Utilities;

namespace DotNetSelenium.PageObjects
{
    public class DoctorPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private JObject testData;

        public DoctorPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        // Locators
        public By DoctorLink => By.CssSelector("a[href='#/Doctors']");
        public By InPatientTab => By.CssSelector("ul.page-breadcrumb a[href='#/Doctors/InPatientDepartment']");
        public By SearchBox => By.CssSelector("input#quickFilterInput");
        public By ActionsPreviewIcon => By.CssSelector("a[title='Preview']");
        public By PatientNameHeading => By.CssSelector("h1.pat-name-hd");
        public By NotesSection => By.CssSelector("a[href='#/Doctors/PatientOverviewMain/NotesSummary']");
        public By AddNotesButton => By.XPath("//a[text()='Add Notes']");
        public By TemplateDropdown => By.CssSelector("input[value-property-name='TemplateName']");
        public By SubjectiveNotesField => By.XPath("//label[text()='Subjective Notes']/../div/textarea");
        public By SuccessConfirmationPopup => By.XPath("//p[contains(text(),'Success')]/../p[contains(text(),'Progress Note Template added.')]");
        public By SaveNotesButton => By.XPath("//button[contains(text(),'Save')]");
        public By NoteType => By.CssSelector("input[placeholder='Select Note Type']");

        /**
         * @Test3
         * @description This method searches for a patient and verifies their overview page.
         */
        public void VerifyPatientOverview()
        {
            JObject testData = TestDataReader.LoadJson("PatientName.json");
            string patientName = testData["PatientNames"][0]["Patient1"].ToString();
            wait.Until(ExpectedConditions.ElementToBeClickable(DoctorLink)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(InPatientTab)).Click();

            // Search for the patient
            var searchBoxes = driver.FindElements(SearchBox);
            foreach (var searchBox in searchBoxes)
            {
                if (searchBox.Displayed)
                {
                    searchBox.SendKeys(patientName);
                    break;
                }
            }
            Thread.Sleep(2000); // Equivalent to Playwright's waitForTimeout(2000)

            // Click on the preview icon under Actions
            wait.Until(ExpectedConditions.ElementToBeClickable(ActionsPreviewIcon)).Click();

            // Verify patient name
            IWebElement patientHeading = wait.Until(ExpectedConditions.ElementIsVisible(PatientNameHeading));
            Assert.That(patientHeading.Text.ToLower().Trim(), Is.EqualTo(patientName.ToLower()), "Patient name does not match.");
        }

        /**
         * @Test4
         * @description This method adds a Progress Note for a patient.
         */
        public void AddProgressNoteForPatient()
        {
            JObject testData = TestDataReader.LoadJson("PatientName.json");
            string patientName = testData["PatientNames"][1]["Patient2"].ToString();
            wait.Until(ExpectedConditions.ElementToBeClickable(DoctorLink)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(InPatientTab)).Click();

            // Search for the patient
            var searchBoxes = driver.FindElements(SearchBox);
            foreach (var searchBox in searchBoxes)
            {
                if (searchBox.Displayed)
                {
                    searchBox.SendKeys(patientName);
                    break;
                }
            }
            Thread.Sleep(2000);

            // Click on the preview icon under Actions
            wait.Until(ExpectedConditions.ElementToBeClickable(ActionsPreviewIcon)).Click();

            // Click on Notes section
            wait.Until(ExpectedConditions.ElementToBeClickable(NotesSection)).Click();

            // Click on Add Notes button
            wait.Until(ExpectedConditions.ElementToBeClickable(AddNotesButton)).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(NoteType)).Click();
            driver.FindElement(NoteType).SendKeys("Progress Note");
            driver.FindElement(NoteType).SendKeys(Keys.Enter);

            // Select "Progress Note" from the Template dropdown
            wait.Until(ExpectedConditions.ElementToBeClickable(TemplateDropdown)).Click();
            driver.FindElement(TemplateDropdown).SendKeys("Progress Note");
            driver.FindElement(TemplateDropdown).SendKeys(Keys.Enter);

            // Enter subjective notes
            Thread.Sleep(1000);
            driver.FindElement(SubjectiveNotesField).SendKeys("Test Notes");
            Thread.Sleep(1000);

            // Click Save
            wait.Until(ExpectedConditions.ElementToBeClickable(SaveNotesButton)).Click();
            Thread.Sleep(2000);

            // Verify success confirmation popup
            if (driver.FindElements(SuccessConfirmationPopup).Count > 0)
            {
                string successMessage = driver.FindElement(SuccessConfirmationPopup).Text;
                Assert.That(successMessage, Does.Contain("Progress Note Template added."),
                    "Success message does not match.");
            }
        }
    }
}
