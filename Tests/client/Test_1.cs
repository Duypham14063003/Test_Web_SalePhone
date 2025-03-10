using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumTests
{
    [TestFixture]
    public class ProductDetailTests
    {
        IWebDriver driver;
        WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Url = "https://frontend-salephones.vercel.app";
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void ID_XemCTSP_1_CheckProductDetail()
        {
            string testCaseID = "ID_XemCTSP_1";
            string actualResult, status;

            try
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                // Chọn sản phẩm "iPhone 15"
                var productElement = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@class, 'sc-cEzcPc') and contains(text(), 'iPhone 15')]")));
                string selectedProductName = productElement.Text; 
                productElement.Click();

                // Chờ trang chi tiết sản phẩm load xong
                wait.Until(drv => drv.Url.Contains("product"));

                // ten sp trong trang chi tieets
                IWebElement productTitleElement = wait.Until(
                    ExpectedConditions.ElementIsVisible(By.XPath("//h1[contains(@class, 'sc-iRLAEC')]"))
                );

                string displayedProductName = productTitleElement.Text; 

                if (displayedProductName.Contains(selectedProductName))
                {
                    actualResult = $"Hiển thị đúng sản phẩm: {displayedProductName}";
                    status = "Pass";
                }
                else
                {
                    actualResult = $"Sai sản phẩm hiển thị! Chờ: {selectedProductName}, nhưng nhận: {displayedProductName}";
                    status = "Fail";
                }
            }
            catch (Exception ex)
            {
                actualResult = $"Lỗi: {ex.Message}";
                status = "Fail";
            }

            // Ghi kết quả vào file Excel
            ExcelHelper.WriteResult(testCaseID, actualResult, status);
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




























//    [Test]
//    public void ID_XemCTSP_4_CheckProductDeleted()
//    {
//        // 1️⃣ Mở trang khách hàng
//        IWebDriver customerDriver = new ChromeDriver();
//        customerDriver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/");
//        customerDriver.Manage().Window.Maximize();
//        Thread.Sleep(2000);

//        // Người dùng chọn sản phẩm để xem
//        customerDriver.FindElement(By.XPath("//div[contains(text(),'iPhone 15 | 128GB | Đen')]")).Click();
//        Thread.Sleep(2000);

//        // 2️⃣ Mở trang admin trong một cửa sổ mới
//        IWebDriver adminDriver = new ChromeDriver();
//        adminDriver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");
//        adminDriver.Manage().Window.Maximize();
//        Thread.Sleep(2000);

//        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

//        // login admin
//        var loginButton = wait.Until(d => d.FindElement(By.XPath("//div[contains(text(),'Đăng Nhập')]")));
//        loginButton.Click();

//        var emailField = wait.Until(d => d.FindElement(By.XPath("//input[@placeholder='Email']")));
//        emailField.SendKeys("ngotran1@gmail.com");

//        var passwordField = driver.FindElement(By.XPath("//input[@type='password']"));
//        passwordField.SendKeys("123123A");

//        var signInButton = wait.Until(d => d.FindElement(By.XPath("//div[@class='ant-spin-container']//button//span[text()='Đăng nhập']")));
//        signInButton.Click();

//        // Chờ một chút để đăng nhập hoàn tất
//        System.Threading.Thread.Sleep(4000);


//        // Admin xóa sản phẩm
//        adminDriver.FindElement(By.XPath("//div[contains(text(),'iPhone 15 | 128GB | Đen')]//following-sibling::button[contains(text(),'Xóa')]")).Click();
//        Thread.Sleep(1000);
//        adminDriver.SwitchTo().Alert().Accept();
//        Thread.Sleep(2000);

//        // 3️⃣ Người dùng reload trang chi tiết sản phẩm
//        customerDriver.Navigate().Refresh();
//        Thread.Sleep(2000);

//        // Kiểm tra xem có thông báo "Sản phẩm đã bị xóa" hay không
//        bool isDeletedMessageDisplayed = customerDriver.FindElements(By.XPath("//*[contains(text(),'Sản phẩm này đã bị xóa')]")).Count > 0;
//        Assert.That(isDeletedMessageDisplayed, Is.True, "Không hiển thị thông báo 'Sản phẩm này đã bị xóa'.");

//        // 4️⃣ Người dùng quay lại trang chủ
//        customerDriver.Navigate().GoToUrl("https://yourwebsite.com");
//        Thread.Sleep(2000);

//        bool isProductStillThere = customerDriver.FindElements(By.XPath("//div[contains(text(),'iPhone 15 | 128GB | Đen')]")).Count > 0;
//        Assert.That(isProductStillThere, Is.False, "Sản phẩm vẫn hiển thị trên trang chủ sau khi bị xóa.");

//        // Đóng trình duyệt sau khi kiểm tra xong
//        customerDriver.Quit();
//        adminDriver.Quit();
//    }


//    // Test Case 5: Kiểm tra trạng thái khi admin cập nhật sản phẩm
//    [Test]
//    public void ID_XemCTSP_5_CheckProductUpdated()
//    {
//        driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/admin");
//        Thread.Sleep(2000);

//        driver.FindElement(By.XPath("//div[contains(text(),'iPhone 15 | 128GB | Đen')]//following-sibling::button[contains(text(),'Sửa')]")).Click();
//        Thread.Sleep(1000);

//        IWebElement nameInput = driver.FindElement(By.XPath("//input[@id='product-name']"));
//        nameInput.Clear();
//        nameInput.SendKeys("iPhone 15 | 256GB | Đỏ");
//        driver.FindElement(By.XPath("//button[contains(text(),'Lưu')]")).Click();
//        Thread.Sleep(2000);

//        driver.Navigate().Refresh();
//        Thread.Sleep(2000);

//        string updatedProductTitle = driver.FindElement(By.XPath("//h1[contains(@class,'product-title')]")).Text;
//        Assert.That(updatedProductTitle, Is.EqualTo("iPhone 15 | 256GB | Đỏ"), "Thông tin sản phẩm chưa được cập nhật.");
//    }
//}

