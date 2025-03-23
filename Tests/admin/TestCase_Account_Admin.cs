//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using test_salephone.Helpers;
//using OpenQA.Selenium.Support.UI;
//using test_salephone.Utilities;
//namespace test_salephone.Tests
//{
//    [TestFixture]
//    public class TestCase_Account_Admin
//    {
//        private IWebDriver driver;
//        private IWebElement element;

//        [SetUp]
//        public void Setup()
//        {
//            driver = new ChromeDriver();
//            driver.Manage().Window.Maximize();
//            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

//            // Đăng nhập
//            driver.FindElement(By.XPath("//input[@placeholder='Email']")).SendKeys("sela@gmail.com");
//            driver.FindElement(By.CssSelector("input[placeholder='Nhập mật khẩu']")).SendKeys("123456");
//            driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
//            Thread.Sleep(3000);
//            driver.FindElement(By.XPath("//img[@alt='avatar']")).Click();

//            //Vào trang quản lý người dùng
//            Thread.Sleep(2000);
//            driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click();
//            Thread.Sleep(2000);
//            IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Người dùng')]"));
//            sanPhamMenu.Click();
//            Thread.Sleep(4000);
//        }


//        [Test]
//        [Description("Test Cập nhật người dùng")]
//        [Category("User Management")]
//        [TestCase("ID_TaiKhoan_11", "Cập nhật thành công")]
//        [TestCase("ID_TaiKhoan_12", "Tên không được vượt quá 25 ký tự.")]
//        [TestCase("ID_TaiKhoan_13", "Cập nhật thành công")]
//        [TestCase("ID_TaiKhoan_14", "Số điện thoại phải là số")]
//        [TestCase("ID_TaiKhoan_15", "Số điện thoại phải có từ 7 đến 20 số")]
//        [TestCase("ID_TaiKhoan_16", "Số điện thoại phải có từ 7 đến 20 số")]
//        [TestCase("ID_TaiKhoan_17", "Cập nhật thành công")]
//        [TestCase("ID_TaiKhoan_18", "Cập nhật thành công")]
//        [TestCase("ID_TaiKhoan_19", "Địa chỉ quá dài, tối đa 100 ký tự")]
//        [TestCase("ID_TaiKhoan_20", "Cập nhật thành công")]
//        [TestCase("ID_TaiKhoan_21", "Ảnh đại diện không phù hợp, ảnh phải là một định dạng đuôi .png, .jpg,...")]
//        [TestCase("ID_TaiKhoan_22", "Vai trò quá dài.")]
//        [TestCase("ID_TaiKhoan_23", "Cập nhật thành công")]
//        [TestCase("ID_TaiKhoan_24", "Cập nhật thành công")]
//        [TestCase("ID_TaiKhoan_25", "isAdmin phải là true hoặc false")]

//        public void Test_CapNhatTaiKhoan(String testCaseID, String thongBao)
//        {
//            string status = "Fail";
//            try
//            {
//                // Đọc data test từ Excel
//                string dataTest = ReadTestDataToExcel.ReadDataToExcel($"TestCase Hoàng Phúc", testCaseID);
//                string[] testFields = dataTest.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
//                // Console.WriteLine("✅ Parsed Data: ");
//                // foreach (var field in testFields)
//                // {
//                //     Console.WriteLine(field);
//                // }
//                driver.FindElement(By.XPath($"//tr[td[contains(normalize-space(.), '{testFields[1]}')]]//span[@aria-label='edit']")).Click();
//                Thread.Sleep(2000);

//                // Nhập "Họ và tên"
//                var fullNameInput = driver.FindElement(By.Id("basic_name"));
//                Thread.Sleep(500);
//                fullNameInput.SendKeys(Keys.Control + "a" + Keys.Delete);
//                Thread.Sleep(500);
//                fullNameInput.SendKeys(testFields[0]);

//                // Nhập "Email"
//                var emailInput = driver.FindElement(By.Id("basic_email"));
//                Thread.Sleep(500);
//                emailInput.SendKeys(Keys.Control + "a" + Keys.Delete);
//                Thread.Sleep(500);
//                emailInput.SendKeys(testFields[1]);

//                // Nhập "Số điện thoại"
//                var phoneInput = driver.FindElement(By.Id("basic_phone"));
//                Thread.Sleep(500);
//                phoneInput.SendKeys(Keys.Control + "a" + Keys.Delete);
//                Thread.Sleep(500);
//                phoneInput.SendKeys(testFields[2]);

//                // Nhập "Địa chỉ"
//                var addressInput = driver.FindElement(By.Id("basic_address"));
//                Thread.Sleep(500);
//                addressInput.SendKeys(Keys.Control + "a" + Keys.Delete);
//                Thread.Sleep(500);
//                addressInput.SendKeys(testFields[3]);

//                // Nhập "Vai trò"
//                var roleInput = driver.FindElement(By.Id("basic_role"));
//                Thread.Sleep(500);
//                roleInput.SendKeys(Keys.Control + "a" + Keys.Delete);
//                Thread.Sleep(500);
//                roleInput.SendKeys(testFields[4]);

//                // Nhập "IsAdmin"
//                var isAdminInput = driver.FindElement(By.Id("basic_isAdmin"));
//                Thread.Sleep(500);
//                isAdminInput.SendKeys(Keys.Control + "a" + Keys.Delete);
//                Thread.Sleep(500);
//                isAdminInput.SendKeys(testFields[5]);

//                // Click nút submit
//                driver.FindElement(By.XPath("//button[@type='submit']")).Click();
//                //chờ hiện thông báo cập nhật thành công


//                WebDriverWait waitMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
//                IWebElement element = waitMessage.Until(driver =>
//                {
//                    try
//                    {
//                        var elements = driver.FindElements(By.XPath($"//span[contains(text(), '{thongBao}')]"));
//                        return elements.Count > 0 && elements[0].Displayed ? elements[0] : null;
//                    }
//                    catch (TimeoutException ex)
//                    {
//                        Console.WriteLine($"⚠️ Phát hiện lỗi timeout: {ex.Message}");
//                        return null;
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine($"Lỗi thông báo: {ex.Message}");
//                        return null;
//                    }


//                });
//                if (element != null)
//                {
//                    Console.WriteLine($"✅ Thông báo: {thongBao}");
//                    status = "Pass";
//                }
//                else
//                {
//                    status = "Fail";
//                }
//                Thread.Sleep(2000);
//                var elements = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
//                if (elements.Count > 0)
//                {
//                    try
//                    {
//                        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
//                        wait.Until(d =>
//                        {
//                            var element = d.FindElement(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
//                            return element.Displayed && element.Enabled ? element : null;
//                        }).Click();
//                        Console.WriteLine("Đã nhấp vào nút đóng.");
//                    }
//                    catch (Exception ex)
//                    {
//                        //không làm gì
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Không tìm thấy nút đóng.");
//                }
//                status = "Pass";

//            }

//            catch (WebDriverTimeoutException ex)
//            {
//                Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
//                status = "Fail";
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
//                status = "Fail";
//            }
//            //Ghi trạng thái test ra Excel nếu cần
//            ExcelReportHelper.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
//        }
        
//        [TearDown]
//        public void TearDown()
//        {
//            if (driver != null)
//            {
//                driver.Quit(); // Đóng trình duyệt
//                driver.Dispose(); // Giải phóng tài nguyên
//            }
//        }
//    }
//}
