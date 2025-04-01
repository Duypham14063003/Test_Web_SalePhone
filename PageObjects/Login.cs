using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace TestProject.PageObjects
{
    public class LoginPage : TestBase
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private readonly By usernameField = By.CssSelector("input[placeholder='Email']");
        private readonly By passwordField = By.CssSelector("input[placeholder='Nhập mật khẩu']");
        private readonly By loginButton = By.XPath("//div[@class='ant-spin-container']//button//span[text()='Đăng nhập']");

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void EnterUsername(string username)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(usernameField)).SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(passwordField)).SendKeys(password);
        }

        public void ClickLoginButton()
        {
            var button = wait.Until(ExpectedConditions.ElementIsVisible(loginButton));
            wait.Until(ExpectedConditions.ElementToBeClickable(loginButton)).Click();

            System.Threading.Thread.Sleep(3000);
        }


        public bool IsLoginSuccessful()
        {
            bool isLoggedIn = !driver.Url.Contains("sign-in");
            Console.WriteLine(isLoggedIn ? "Đăng nhập thành công!" : "Đăng nhập thất bại.");
            return isLoggedIn;
        }
    }
}