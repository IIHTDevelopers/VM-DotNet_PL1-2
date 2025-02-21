using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;

namespace DotNetSelenium.PageObjects
{
    public class AdminPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public AdminPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        private IWebElement AdminDropdown => wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@class='dropdown dropdown-user']")));
        private IWebElement MyProfileOption => driver.FindElement(By.CssSelector("a[routerlink='Employee/ProfileMain']"));
        private IWebElement UserProfileHeader => driver.FindElement(By.CssSelector("a[routerlink='UserProfile']"));

        /**
        * @Test7
        * @description This method verifies that the user is successfully navigated to the "User Profile" page 
        *              after selecting the "My Profile" option from the Admin dropdown.
        * @expected
        * Verify that the user is redirected to the "User Profile" page and the page header or title confirms this.
        */
        public void VerifyUserProfileNavigation()
        {
            // Click on Admin dropdown
            AdminDropdown.Click();

            // Select "My Profile" option
            wait.Until(ExpectedConditions.ElementToBeClickable(MyProfileOption)).Click();

            // Wait for User Profile page to load
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a[routerlink='UserProfile']")));

            // Verify that the User Profile page is displayed
            string headerText = UserProfileHeader.Text.Trim();
            Assert.AreEqual("User Profile", headerText, "User Profile page did not load as expected.");
        }
    }

}
