using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace DotNetSelenium.PageObjects
{
    public class SettingsPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public SettingsPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        private IWebElement SettingsLink => driver.FindElement(By.CssSelector("a[href='#/Settings']"));
        private IWebElement MoreDropdown => driver.FindElement(By.XPath("//a[contains(text(),'More...')]"));
        private IWebElement PriceCategoryTab => driver.FindElement(By.CssSelector("ul.dropdown-menu a[href='#/Settings/PriceCategory']"));

        private IWebElement GetDisableButton(string code) =>
            driver.FindElement(By.XPath($"//div[text()='{code}']/../div/span/a[@danphe-grid-action='deactivatePriceCategorySetting']"));

        private IWebElement GetEnableButton(string code) =>
            driver.FindElement(By.XPath($"//div[text()='{code}']/../div/span/a[@danphe-grid-action='activatePriceCategorySetting']"));

        private IWebElement ActivateSuccessMessage => driver.FindElement(By.XPath("//p[contains(text(),'success')]/../p[text()='Activated.']"));
        private IWebElement DeactivateSuccessMessage => driver.FindElement(By.XPath("//p[contains(text(),'success')]/../p[text()='Deactivated.']"));

        /// <summary>
        /// @Test10
        /// @description This method verifies disabling and enabling a price category code in the table.
        /// @expected
        /// A success message is displayed for both actions: "Deactivated." for disabling and "Activated." for enabling.
        /// </summary>
        public void TogglePriceCategoryStatus()
        {
            // Step 1: Navigate to Settings
            wait.Until(ExpectedConditions.ElementToBeClickable(SettingsLink)).Click();

            // Step 2: Open "More..." dropdown and click "Price Category" tab
            wait.Until(ExpectedConditions.ElementToBeClickable(MoreDropdown)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(PriceCategoryTab)).Click();

            // Step 3: Disable the specified code (e.g., NHIF-1)
            string priceCategoryCode = "NHIF-1";
            wait.Until(ExpectedConditions.ElementToBeClickable(GetDisableButton(priceCategoryCode))).Click();

            // Step 4: Verify "Deactivated." success message
            string deactivateMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[contains(text(),'success')]/../p[text()='Deactivated.']"))).Text.Trim();
            if (!deactivateMessage.Equals("Deactivated."))
            {
                throw new Exception($"Expected 'Deactivated.' message not found. Found: {deactivateMessage}");
            }

            // Step 5: Enable the same code
            wait.Until(ExpectedConditions.ElementToBeClickable(GetEnableButton(priceCategoryCode))).Click();

            // Step 6: Verify "Activated." success message
            string activateMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[contains(text(),'success')]/../p[text()='Activated.']"))).Text.Trim();
            if (!activateMessage.Equals("Activated."))
            {
                throw new Exception($"Expected 'Activated.' message not found. Found: {activateMessage}");
            }
        }
    }

}
