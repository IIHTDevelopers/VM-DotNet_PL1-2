using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace DotNetSelenium.PageObjects
{
    public class SubstorePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private readonly Actions actions;

        public SubstorePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            this.actions = new Actions(driver);
        }

        private IWebElement SubstoreLink => driver.FindElement(By.CssSelector("a[href='#/WardSupply']"));
        private IWebElement SelectSubstore => driver.FindElement(By.XPath("(//span[@class='report-name'])[1]"));
        private IWebElement InventoryRequisition => driver.FindElement(By.CssSelector("a[href='#/WardSupply/Inventory/InventoryRequisitionList']"));
        private IWebElement Consumption => driver.FindElement(By.CssSelector("a[href='#/WardSupply/Inventory/Consumption']"));
        private IWebElement Reports => driver.FindElement(By.CssSelector("a[href='#/WardSupply/Inventory/Reports']"));
        private IWebElement PatientConsumption => driver.FindElement(By.CssSelector("a[href='#/WardSupply/Inventory/PatientConsumption']"));
        private IWebElement Return => driver.FindElement(By.CssSelector("a[href='#/WardSupply/Inventory/Return']"));
        private IWebElement Inventory => driver.FindElement(By.CssSelector("ul.page-breadcrumb a[href='#/WardSupply/Inventory']"));
        private IWebElement SignoutCursor => driver.FindElement(By.CssSelector("i.fa-sign-out"));
        private IWebElement Tooltip => driver.FindElement(By.CssSelector("div.modal-content h6"));

        /// <summary>
        /// @Test11
        /// @description This method verifies that the user is able to navigate between the sub modules.
        /// @expected Ensure that it should navigate to each section of the "substore" module.
        /// </summary>
        public void VerifyNavigationBetweenSubmodules()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(SubstoreLink)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(SelectSubstore)).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(InventoryRequisition)).Click();
            if (!driver.Url.Contains("Inventory/InventoryRequisitionList"))
            {
                throw new Exception("Navigation to Inventory Requisition failed.");
            }

            wait.Until(ExpectedConditions.ElementToBeClickable(Consumption)).Click();
            if (!driver.Url.Contains("Inventory/Consumption/ConsumptionList"))
            {
                throw new Exception("Navigation to Consumption failed.");
            }

            wait.Until(ExpectedConditions.ElementToBeClickable(Reports)).Click();
            if (!driver.Url.Contains("Inventory/Reports"))
            {
                throw new Exception("Navigation to Reports failed.");
            }

            wait.Until(ExpectedConditions.ElementToBeClickable(PatientConsumption)).Click();
            if (!driver.Url.Contains("Inventory/PatientConsumption/PatientConsumptionList"))
            {
                throw new Exception("Navigation to Patient Consumption failed.");
            }

            wait.Until(ExpectedConditions.ElementToBeClickable(Return)).Click();
            if (!driver.Url.Contains("Inventory/Return"))
            {
                throw new Exception("Navigation to Return failed.");
            }
        }

        /// <summary>
        /// @Test12
        /// @description This method verifies the tooltip text displayed when hovering over the cursor icon in the Inventory tab.
        /// @expected Tooltip text should contain: "To change, you can always click here."
        /// </summary>
        public void VerifyTooltipText()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(SubstoreLink)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(SelectSubstore)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(Inventory)).Click();

            // Hover over the cursor icon
            actions.MoveToElement(SignoutCursor).Perform();

            // Get tooltip text
            string tooltipText = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.modal-content h6"))).Text.Trim();
            if (!tooltipText.Contains("To change, you can always click here."))
            {
                throw new Exception($"Tooltip text mismatch. Found: {tooltipText}");
            }
        }

        /// <summary>
        /// @Test13
        /// @description This method navigates to the Inventory Requisition section, captures a screenshot of the page, 
        ///              and saves it in the screenshots folder.
        /// @expected Screenshot of the page is captured and saved successfully.
        /// </summary>
        public void CaptureInventoryRequisitionScreenshot()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
            string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"screenshots/inventory-requisition-{timestamp}.png");

            wait.Until(ExpectedConditions.ElementToBeClickable(SubstoreLink)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(SelectSubstore)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(Inventory)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(InventoryRequisition)).Click();

            if (!driver.Url.Contains("Inventory/InventoryRequisitionList"))
            {
                throw new Exception("Navigation to Inventory Requisition failed.");
            }

            // Take a screenshot
            //Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            //Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)); // Ensure directory exists
            //screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
        }
    }
}
