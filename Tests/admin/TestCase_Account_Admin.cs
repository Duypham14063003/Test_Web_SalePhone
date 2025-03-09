using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using test_salephone.Helpers;

namespace test_salephone.Tests
{
    [TestFixture] // Giúp NUnit nhận diện class test
    public class TestCase_Account_Admin
    {
        private IWebDriver driver;
        private IWebElement element;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");
            Console.WriteLine("Current URL: " + driver.Url);

            // Đăng nhập
            driver.FindElement(By.XPath("//input[@placeholder='Email']")).SendKeys("sela@gmail.com");
            driver.FindElement(By.CssSelector("input[placeholder='Nhập mật khẩu']")).SendKeys("123456");
            driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
            Thread.Sleep(2000);


        }

        public void VaoTrangQuanLyTaiKhoan()
        {
            //vào trang quản lý tài khoản
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//img[@alt='avatar']")).Click(); ;
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click(); ;
            Thread.Sleep(2000);
            IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Người dùng')]"));
            sanPhamMenu.Click();
            Thread.Sleep(4000);
        }
        [Test]
        public void Test_CapNhatTaiKhoan()
        {
            string status = "Pass";
            try
            {
                 // Đọc data test từ Excel
                string dataTest = ExcelHelper.ReadDataToExcel("TestCase Hoàng Phúc", "ID_TaiKhoan_11", status);
                string[] testFields = dataTest.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                Console.WriteLine("✅ Parsed Data: ");
                foreach (var field in testFields)
                {
                    Console.WriteLine(field);
                }
                // Vào trang quản lý tài khoản (nếu cần)
                VaoTrangQuanLyTaiKhoan();

                // Click vào nút cập nhật (ví dụ đã có sẵn bước này)
                driver.FindElement(By.XPath($"//tr[td[contains(normalize-space(.), '{testFields[1]}')]]//span[@aria-label='edit']")).Click();
                Thread.Sleep(2000);

                // Gán dữ liệu cho các trường trên form
                driver.FindElement(By.Id("fullName")).Clear();
                driver.FindElement(By.Id("fullName")).SendKeys(testFields[0]);

                driver.FindElement(By.Id("email")).Clear();
                driver.FindElement(By.Id("email")).SendKeys(testFields[1]);

                driver.FindElement(By.Id("phone")).Clear();
                driver.FindElement(By.Id("phone")).SendKeys(testFields[2]);

                driver.FindElement(By.Id("address")).Clear();
                driver.FindElement(By.Id("address")).SendKeys(testFields[3]);

                driver.FindElement(By.Id("role")).Clear();
                driver.FindElement(By.Id("role")).SendKeys(testFields[4]);

                // Giả sử có checkbox cho IsAdmin
                bool isAdmin = testFields[5].ToLower() == "true";
                var isAdminCheckbox = driver.FindElement(By.Id("isAdmin"));
                if (isAdmin && !isAdminCheckbox.Selected)
                {
                    isAdminCheckbox.Click();
                }
                else if (!isAdmin && isAdminCheckbox.Selected)
                {
                    isAdminCheckbox.Click();
                }

                Thread.Sleep(2000); // Đợi thao tác xong, sau đó có thể xác nhận kết quả cập nhật

            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
                status = "Fail";
            }
            //Ghi trạng thái test ra Excel nếu cần
            // ExcelReportHelper.WriteToExcel("TestCase Hoàng Phúc", "ID_TaiKhoan_11", status);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit(); // Đóng trình duyệt
                driver.Dispose(); // Giải phóng tài nguyên
            }
        }
    }
}
