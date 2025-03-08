using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace test_salephone.PageObjects
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        private readonly By usernameField = By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[1]/input"); 
        private readonly By passwordField = By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[2]/span/input");
        private readonly By loginButton = By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[3]/div/div/button");
        // private readonly By errorMessage = By.ClassName("error-message");

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void EnterUsername(string username)
        {
            driver.FindElement(usernameField).SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            driver.FindElement(passwordField).SendKeys(password);
        }

        public void ClickLoginButton()
        {
            driver.FindElement(loginButton).Click();
        }

        // public string GetErrorMessage()
        // {
        //     return driver.FindElement(errorMessage).Text;
        // }

        public bool IsLoginSuccessful()
        {
            return !driver.Url.Contains("login"); 
        }
    }
}
