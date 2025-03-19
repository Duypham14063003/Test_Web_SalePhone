using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace test_salephone.PageObjects
{
    public class LoginPage : TestBase
    {
        public void login(){
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[4]/div")).Click();
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[1]/input")).SendKeys("ngocduy1423@gmail.com");
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[2]/span/input")).SendKeys("Duy123123123");

            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[3]/div/div/button")).Click();
            Thread.Sleep(5000);

            // kiem tra da vao trnag home chua?
            IWebElement home = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[3]/div/div[2]"));
            Assert.IsTrue(home.Displayed);
        }
    }
}
