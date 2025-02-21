using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DotNetSelenium.PageObjects
{
    public class OperationTheatrePage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public OperationTheatrePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        // Locators for Operation Theatre Page Elements
        public By OperationTheatreLink => By.CssSelector("a[href='#/OperationTheatre']");
        public By NewOtBookingButton => By.XPath("//button[contains(text(),'New OT Booking')]");
        public By AddNewOtButton => By.CssSelector("input[value='Add New OT']");
        public By ModalHeading => By.CssSelector("div.modelbox-div");

        /**
         * @Test2
         * @description This method verifies and handles the alert for OT booking without patient selection.
         */
        public void HandleOtBookingAlert()
        {
            // Click on the Operation Theatre link
            wait.Until(ExpectedConditions.ElementToBeClickable(OperationTheatreLink)).Click();

            // Click on the "New OT Booking" button
            wait.Until(ExpectedConditions.ElementToBeClickable(NewOtBookingButton)).Click();

            // Verify the modal is displayed
            IWebElement modalElement = wait.Until(ExpectedConditions.ElementIsVisible(ModalHeading));
            Assert.That(modalElement.Displayed, Is.True, "Modal heading is not displayed.");

            // Click on the "Add New OT" button
            wait.Until(ExpectedConditions.ElementToBeClickable(AddNewOtButton)).Click();

            // Wait for and handle the alert
            IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
            string alertMessage = alert.Text;
            Console.WriteLine("Alert Message: " + alertMessage);
            Assert.That(alertMessage, Does.Contain("Patient not Selected! Please Select the patient first!"),
                "Unexpected alert message.");
            alert.Accept();
        }
    }
}
