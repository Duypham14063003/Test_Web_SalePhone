using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace QuanlySanPham_Tests
{
    [TestFixture]
    public class QuanlySanPham_Tests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
       
        [SetUp]
        public void SetupTest()
        {   

            // Mo trang web
            driver = new ChromeDriver();
            baseURL = "https://frontend-salephones.vercel.app/";
            verificationErrors = new StringBuilder();
            driver.Manage().Window.Maximize();
            Thread.Sleep(1000);
            //
        }
        
         [Test]
        public void TheQuanlySanPham_Test()
        {
            driver.Navigate().GoToUrl(baseURL + "/system/admin");
            driver.Manage().Window.Maximize();
            Thread.Sleep(2000);
        }

        [TearDown]
        public void TeardownTest()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
    }
}