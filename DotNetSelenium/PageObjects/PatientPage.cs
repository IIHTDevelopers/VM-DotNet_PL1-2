using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;

namespace DotNetSelenium.PageObjects
{
    public class PatientPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public PatientPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        private IWebElement PatientLink => driver.FindElement(By.CssSelector("a[href='#/Patient']"));
        private IWebElement RegisterPatient => driver.FindElement(By.CssSelector("ul.page-breadcrumb a[href='#/Patient/RegisterPatient']"));
        private IWebElement ProfilePictureIcon => driver.FindElement(By.CssSelector("a[title='Profile Picture']"));
        private IWebElement NewPhotoButton => driver.FindElement(By.XPath("//button[contains(text(),'New Photo')]"));
        private IWebElement UploadButton => driver.FindElement(By.CssSelector("label[for='fileFromLocalDisk']"));
        private IWebElement DoneButton => driver.FindElement(By.XPath("//button[text()='Done']"));
        private IWebElement UploadedImg => driver.FindElement(By.CssSelector("div.wrapper img"));

        /// <summary>
        /// @Test8
        /// @description This method verifies the successful upload of a profile picture for a patient 
        /// by navigating to the "Register Patient" tab and completing the upload process.
        /// @expected
        /// Verify that the uploaded image is displayed successfully in the patient's profile.
        /// </summary>
        public void UploadProfilePicture()
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestImage", "UploadImage.png");

            // Click on "Patient" link
            wait.Until(ExpectedConditions.ElementToBeClickable(PatientLink)).Click();

            // Click on "Register Patient" tab
            wait.Until(ExpectedConditions.ElementToBeClickable(RegisterPatient)).Click();

            // Click on "Profile Picture" icon
            wait.Until(ExpectedConditions.ElementToBeClickable(ProfilePictureIcon)).Click();

            // Click on "New Photo" button
            wait.Until(ExpectedConditions.ElementToBeClickable(NewPhotoButton)).Click();

            // Upload image
            IWebElement fileInput = driver.FindElement(By.CssSelector("input[type='file']"));
            fileInput.SendKeys(imagePath);

            // Wait for upload to complete
            System.Threading.Thread.Sleep(2000);

            // Click on "Done" button
            wait.Until(ExpectedConditions.ElementToBeClickable(DoneButton)).Click();

            // Verify success confirmation or image upload
            bool isImageDisplayed = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.wrapper img"))).Displayed;
            if (!isImageDisplayed)
            {
                throw new Exception("Profile picture upload failed!");
            }
        }
    }
}
