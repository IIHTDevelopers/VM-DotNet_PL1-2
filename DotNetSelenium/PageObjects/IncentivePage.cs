using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using DotNetSelenium.Utilities;

namespace DotNetSelenium.PageObjects
{
    public class IncentivePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public IncentivePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        private IWebElement IncentiveLink => driver.FindElement(By.CssSelector("a[href='#/Incentive']"));
        private IWebElement SettingsTab => driver.FindElement(By.CssSelector("ul[class='page-breadcrumb'] a[href='#/Incentive/Setting']"));
        private IWebElement SearchBar => driver.FindElement(By.CssSelector("input#quickFilterInput"));
        private IWebElement EditTDSButton => driver.FindElement(By.CssSelector("a[danphe-grid-action='edit-tds']"));
        private IWebElement EditTDSModal => driver.FindElement(By.CssSelector("div.modal[title='Edit TDS Percent']"));
        private IWebElement TDSInputField => driver.FindElement(By.CssSelector("input[type='number']"));
        private IWebElement UpdateTDSButton => driver.FindElement(By.CssSelector("button#btn_GroupDistribution"));
        private IWebElement TDSValueInTable => driver.FindElement(By.CssSelector("div[col-id='TDSPercent']"));

        /// <summary>
        /// @Test9
        /// @description This method updates the TDS% for a specific employee and verifies the updated value in the table.
        /// @expected
        /// The updated TDS% value is displayed correctly in the corresponding row of the table.
        /// </summary>
        public void EditTDSForEmployee()
        {
            // Read JSON file for employee names
            JObject testData = TestDataReader.LoadJson("PatientName.json");
            string patientName = testData["PatientNames"][2]["Patient3"].ToString() ?? ""; ;
            int updatedTDS = new Random().Next(1, 99);

            // Step 1: Click on Incentive link
            wait.Until(ExpectedConditions.ElementToBeClickable(IncentiveLink)).Click();

            // Step 2: Click on the "Settings" tab
            wait.Until(ExpectedConditions.ElementToBeClickable(SettingsTab)).Click();

            // Step 3: Search for employee name
            SearchBar.SendKeys(patientName);
            System.Threading.Thread.Sleep(100);  // Simulating {delay: 100} from Playwright

            // Step 4: Click "Edit TDS%" button
            wait.Until(ExpectedConditions.ElementToBeClickable(EditTDSButton)).Click();

            // Step 5: Wait for modal & enter TDS value
            //wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.modal[title='Edit TDS Percent']")));
            TDSInputField.Clear();
            TDSInputField.SendKeys(updatedTDS.ToString());

            // Step 6: Click on "Update TDS" button
            wait.Until(ExpectedConditions.ElementToBeClickable(UpdateTDSButton)).Click();

            // Step 7: Refresh search & validate updated TDS
            SearchBar.Clear();
            SearchBar.SendKeys(patientName);
            System.Threading.Thread.Sleep(2000); // Wait for table to update

            // Step 8: Verify updated TDS% value
            string displayedTDS = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//div[@col-id='TDSPercent'])[2]"))).Text.Trim();
            if (!displayedTDS.Equals(updatedTDS.ToString()))
            {
                throw new Exception($"TDS value mismatch! Expected: {updatedTDS}, Found: {displayedTDS}");
            }
        }
    }
}
