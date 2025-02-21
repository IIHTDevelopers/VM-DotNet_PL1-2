using DotNetSelenium.Utilities;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

namespace DotNetSelenium.PageObjects
{
    public class ADTPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public ADTPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        private IWebElement ADTLink => driver.FindElement(By.CssSelector("a[href='#/ADTMain']"));
        private IWebElement SearchBar => driver.FindElement(By.Id("quickFilterInput"));
        private IWebElement AdmittedPatientsTab => driver.FindElement(By.CssSelector("ul.page-breadcrumb a[href='#/ADTMain/AdmittedList']"));
        private IWebElement MoreOptionsButton => driver.FindElement(By.XPath("//button[contains(text(),'...')]"));
        private IWebElement ChangeDoctorOption => driver.FindElement(By.CssSelector("a[danphe-grid-action='changedr']"));
        private IWebElement ChangeDoctorModal => driver.FindElement(By.CssSelector("div.modelbox-div"));
        private IWebElement UpdateButton => driver.FindElement(By.XPath("//button[text()='Update']"));
        private IWebElement FieldErrorMessage => driver.FindElement(By.XPath("//span[text()='Select doctor from the list.']"));
        private IList<IWebElement> CounterItems => driver.FindElements(By.CssSelector("div.counter-item"));

        /// <summary>
        /// @Test14
        /// @description This test verifies that the error message "Select doctor from the list." is displayed 
        ///              when the user attempts to update the doctor without selecting a value.
        /// @expected The error message "Select doctor from the list." is shown near the field.
        /// </summary>
        public void VerifyFieldLevelErrorMessage()
        {
            // Read JSON file for employee names
            JObject testData = TestDataReader.LoadJson("PatientName.json");
            string patientName = testData["PatientNames"][0]["Patient1"].ToString() ?? "";

            wait.Until(ExpectedConditions.ElementToBeClickable(ADTLink)).Click();

            // Wait for counter items and select the first one if available
            System.Threading.Thread.Sleep(3000);
            if (CounterItems.Count > 0)
            {
                CounterItems[0].Click();
            }
            else
            {
                Console.WriteLine("No counter items available");
            }

            // Navigate to "Admitted Patients" tab
            wait.Until(ExpectedConditions.ElementToBeClickable(AdmittedPatientsTab)).Click();

            // Search for the patient
            SearchBar.SendKeys(patientName);
            SearchBar.SendKeys(Keys.Enter);

            // Click on the "..." button for the patient
            wait.Until(ExpectedConditions.ElementToBeClickable(MoreOptionsButton)).Click();

            // Select "Change Doctor" from the options
            wait.Until(ExpectedConditions.ElementToBeClickable(ChangeDoctorOption)).Click();

            // Wait for the "Change Doctor" modal to appear
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.modelbox-div")));

            // Click on the "Update" button without selecting a doctor
            wait.Until(ExpectedConditions.ElementToBeClickable(UpdateButton)).Click();

            // Verify the error message is displayed
            bool isErrorMessageDisplayed = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Select doctor from the list.']"))).Displayed;
            if (!isErrorMessageDisplayed)
            {
                throw new Exception("Expected error message was not displayed.");
            }
        }
    }

}
