using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using test_salephone.Helpers;

namespace test_salephone.Tests
{
    [TestFixture]
    public class ProductDetailTest
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/");
            driver.Manage().Window.Maximize();
        }

        

        [Test]
        public void Test_XemChiTietSanPham()
        {
            IWebElement firstProduct = driver.FindElement(By.XPath("//div[@class='product-item'][1]"));
            firstProduct.Click();
            Thread.Sleep(2000);

            string productName = driver.FindElement(By.XPath("//h1[@class='product-title']")).Text;
            Assert.That(productName.Contains("iPhone 15"));
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
            driver = null;

        }
    }
}
