using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;

namespace DotNetSelenium.PageObjects
{
    public class UtilitiesPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public UtilitiesPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement UtilitiesLink => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Utilities']")));
        private IWebElement SchemeRefundTab => wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("ul.page-breadcrumb a[href='#/Utilities/SchemeRefund']")));
        private IReadOnlyCollection<IWebElement> CounterItems => driver.FindElements(By.XPath("//div[@class='counter-item']"));
        private IWebElement NewSchemeRefundEntryButton => wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'New Scheme Refund Entry')]")));
        private IWebElement SaveSchemeRefundButton => wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("savebutton")));
        private IWebElement WarningPopup => wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[contains(text(),'warning')]/../p[contains(text(),'Please fill all the mandatory fields.')]")));

        /**
        * @Test6
        * @description This method verifies that a warning popup is displayed when attempting to save a new 
        *              Scheme Refund Entry without filling in mandatory fields.
        * @expected
        * A warning popup should appear with the message: "Please fill all the mandatory fields."
        */
        public void VerifyMandatoryFieldsWarning()
        {
            // Navigate to Utilities and open Scheme Refund tab
            UtilitiesLink.Click();
            SchemeRefundTab.Click();

            // Select first counter item if available
            System.Threading.Thread.Sleep(3000); // Consider replacing with an explicit wait
            if (CounterItems.Count > 0)
            {
                Console.WriteLine("Counter count is: " + CounterItems.Count);
                CounterItems.First().Click();
            }
            else
            {
                Console.WriteLine("No counter items available");
            }

            // Click "New Scheme Refund Entry" button
            NewSchemeRefundEntryButton.Click();

            // Click Save without filling any fields
            SaveSchemeRefundButton.Click();

            // Verify warning message
            string popupMessage = WarningPopup.Text.Trim();
            Console.WriteLine("Popup message: " + popupMessage);
            Assert.AreEqual("Please fill all the mandatory fields.", popupMessage);
        }
    }

}
