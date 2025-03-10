using DocumentFormat.OpenXml.Math;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V131.Debugger;
using OpenQA.Selenium.Support.UI;
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
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click(); ;
            Thread.Sleep(2000);
            IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Người dùng')]"));
            sanPhamMenu.Click();
            Thread.Sleep(4000);
        }
        public void Test_CapNhatTaiKhoan(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc data test từ Excel
                string dataTest = ReadTestDataToExcel.ReadDataToExcel($"TestCase Hoàng Phúc", testCaseID);
                string[] testFields = dataTest.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                // Console.WriteLine("✅ Parsed Data: ");
                // foreach (var field in testFields)
                // {
                //     Console.WriteLine(field);
                // }

                driver.FindElement(By.XPath($"//tr[td[contains(normalize-space(.), '{testFields[1]}')]]//span[@aria-label='edit']")).Click();
                Thread.Sleep(2000);

                // Gán dữ liệu cho các trường trên form

                // Nhập "Họ và tên"
                var fullNameInput = driver.FindElement(By.Id("basic_name"));
                Thread.Sleep(500);
                fullNameInput.SendKeys(Keys.Control + "a" + Keys.Delete);
                Thread.Sleep(500);
                fullNameInput.SendKeys(testFields[0]);

                // Nhập "Email"
                var emailInput = driver.FindElement(By.Id("basic_email"));
                Thread.Sleep(500);
                emailInput.SendKeys(Keys.Control + "a" + Keys.Delete);
                Thread.Sleep(500);
                emailInput.SendKeys(testFields[1]);

                // Nhập "Số điện thoại"
                var phoneInput = driver.FindElement(By.Id("basic_phone"));
                Thread.Sleep(500);
                phoneInput.SendKeys(Keys.Control + "a" + Keys.Delete);
                Thread.Sleep(500);
                phoneInput.SendKeys(testFields[2]);

                // Nhập "Địa chỉ"
                var addressInput = driver.FindElement(By.Id("basic_address"));
                Thread.Sleep(500);
                addressInput.SendKeys(Keys.Control + "a" + Keys.Delete);
                Thread.Sleep(500);
                addressInput.SendKeys(testFields[3]);

                // Nhập "Vai trò"
                var roleInput = driver.FindElement(By.Id("basic_role"));
                Thread.Sleep(500);
                roleInput.SendKeys(Keys.Control + "a" + Keys.Delete);
                Thread.Sleep(500);
                roleInput.SendKeys(testFields[4]);

                // Nhập "IsAdmin"
                var isAdminInput = driver.FindElement(By.Id("basic_isAdmin"));
                Thread.Sleep(500);
                isAdminInput.SendKeys(Keys.Control + "a" + Keys.Delete);
                Thread.Sleep(500);
                isAdminInput.SendKeys(testFields[5]);

                // Click nút submit
                driver.FindElement(By.XPath("//button[@type='submit']")).Click();
                //chờ hiện thông báo cập nhật thành công
                WebDriverWait waitMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                IWebElement element = waitMessage.Until(driver =>
                {
                    try
                    {
                        var elements = driver.FindElements(By.XPath($"//span[contains(text(), '{thongBao}')]"));
                        return elements.Count > 0 && elements[0].Displayed ? elements[0] : null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Phát hiện lỗi popup: {ex.Message}");
                        return null;
                    }
                });
                status = "Pass";

            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"⚠️ Phát hiện lỗi timeout: {ex.Message}");
                status = "Fail";
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"⚠️ Phát hiện lỗi timeout: {ex.Message}");
                status = "Fail";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
                status = "Fail";
            }
            //Ghi trạng thái test ra Excel nếu cần
            ExcelReportHelper.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
        }
        [Test]
        public void Test_CapNhatTaiKhoan_TungTruongHop()
        {
            VaoTrangQuanLyTaiKhoan();
            Test_CapNhatTaiKhoan("ID_TaiKhoan_11", "Cập nhật thành công");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_12", "Tên không được vượt quá 25 ký tự.");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_13", "Cập nhật thành công");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_14", "Số điện thoại phải là số");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_15", "Số điện thoại phải có từ 7 đến 20 số");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_16", "Số điện thoại phải có từ 7 đến 20 số");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_17", "Cập nhật thành công");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_18", "Cập nhật thành công");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_19", "Địa chỉ quá dài, tối đa 100 ký tự");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_20", "Cập nhật thành công");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_21", "Ảnh đại diện không phù hợp, ảnh phải là một định dạng đuôi .png, .jpg,...");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_22", "Vai trò quá dài.");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_23", "Cập nhật thành công");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_24", "Cập nhật thành công");
            Thread.Sleep(2000);
            Test_CapNhatTaiKhoan("ID_TaiKhoan_25", "isAdmin phải là true hoặc false");

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
