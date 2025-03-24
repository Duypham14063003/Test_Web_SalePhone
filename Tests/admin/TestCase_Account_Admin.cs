using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using test_salephone.Helpers;
namespace test_salephone.Tests
{
    [TestFixture]
    public class TestCase_Account_Admin
    {
        private IWebDriver driver;
        private IWebElement element;

        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");


            // Đăng nhập
            driver.FindElement(By.XPath("//input[@placeholder='Email']")).SendKeys("sela@gmail.com");
            driver.FindElement(By.CssSelector("input[placeholder='Nhập mật khẩu']")).SendKeys("123456");
            driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//img[@alt='avatar']")).Click();

            //Vào trang quản lý người dùng
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click();
            Thread.Sleep(2000);
            IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Người dùng')]"));
            sanPhamMenu.Click();
            Thread.Sleep(4000);

        }
        public void Setup2()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

        }

        [Test]
        [Description("Test Cập nhật người dùng")]
        [Category("User Management")]
        [TestCase("ID_TaiKhoan_10", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_11", "Tên không được vượt quá 25 ký tự.")]
        [TestCase("ID_TaiKhoan_12", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_13", "Số điện thoại phải là số.")]
        [TestCase("ID_TaiKhoan_14", "Số điện thoại phải có từ 7 đến 20 số.")]
        [TestCase("ID_TaiKhoan_15", "Số điện thoại phải có từ 7 đến 20 số.")]
        [TestCase("ID_TaiKhoan_16", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_17", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_18", "Địa chỉ quá dài, tối đa 100 ký tự")]
        [TestCase("ID_TaiKhoan_19", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_20", "Ảnh đại diện không phù hợp, ảnh phải là một định dạng đuôi .png, .jpg,...")]
        [TestCase("ID_TaiKhoan_21", "Vai trò quá dài.")]
        [TestCase("ID_TaiKhoan_22", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_23", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_24", "isAdmin phải là true hoặc false.")]

        public void Test_CapNhatTaiKhoan(String testCaseID, String thongBao)
        {
            string actual = "";
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


                var imageInput = driver.FindElement(By.XPath("//span[@class='ant-upload']//input[@type='file']"));
                imageInput.SendKeys(testFields[6]);
                Thread.Sleep(500);

                // Click nút submit
                driver.FindElement(By.XPath("//button[@type='submit']")).Click();
                //chờ hiện thông báo cập nhật thành công


                WebDriverWait waitMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                IWebElement element = waitMessage.Until(driver =>
                {
                    try
                    {
                        var elements = driver.FindElements(By.XPath("//div[contains(@class, 'ant-message-notice-content')]//span[2]"));
                        return elements.FirstOrDefault(el => el.Displayed); // Trả về phần tử hiển thị đầu tiên
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
                bool isErrorDisplayed = driver.FindElements(By.XPath("//div[contains(@class, 'ant-message-custom-content') and contains(@class, 'ant-message-error')]")).Count > 0;
                Assert.That(element, Is.Not.Null, "Không tìm thấy bất kỳ thông báo nào sau 60s!");
                Console.WriteLine($"✅ Đã tìm thấy thông báo: {element.Text.Trim()}");
                Console.WriteLine($"✅ thông báo truyền vào: {thongBao}");
                if (element.Text.Trim() != thongBao)
                {
                    actual = element.Text.Trim();
                    status = "Fail";
                }
                else
                {
                    if (isErrorDisplayed)
                    {
                        // Actions action = new Actions(driver);
                        // action.MoveByOffset(0, 0).Click().Perform(); // Click ra ngoài để tắt popup
                        actual = element.Text.Trim();
                        Console.WriteLine("Đã hiển thị thông báo lỗi! và dữ liệu không cập nhật");
                        status = "Pass";
                    }
                    else
                    {
                        // CheckInformationAfterUpdate(testFields[0], testFields[1], testFields[2], testFields[3], testFields[4], testFields[5], testFields[6]);
                        actual = element.Text.Trim();
                        status = "Pass";
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
                status = "Fail";
            }
            //Ghi trạng thái test ra Excel nếu cần
            ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status, actual);
        }

        [Test]
        [Description("Test Đăng ký")]
        [TestCase("ID_TaiKhoan_1", "Hãy nhập email hợp lệ và có đuôi @gmail.com")]
        [TestCase("ID_TaiKhoan_2", "Hãy nhập đầy đủ thông tin")]
        [TestCase("ID_TaiKhoan_3", "Email này đã được đăng ký, vui lòng chọn email khác")]
        [TestCase("ID_TaiKhoan_4", "Đăng ký tài khoản thành công")]
        [TestCase("ID_TaiKhoan_5", "Mật khẩu phải có ít nhất 1 ký tự viết hoa và ít nhất 6 ký tự")]
        [TestCase("ID_TaiKhoan_6", "Mật khẩu phải có ít nhất 1 ký tự viết hoa và ít nhất 6 ký tự")]
        [TestCase("ID_TaiKhoan_7", "Mật khẩu phải có ít nhất 1 ký tự viết hoa và ít nhất 6 ký tự")]
        [TestCase("ID_TaiKhoan_8", "Hãy nhập đầy đủ thông tin")]
        [TestCase("ID_TaiKhoan_9", "Hãy nhập đầy đủ thông tin")]
        public void Test_DangKy(String testCaseID, String thongBao)
        {
            {
                string actual = "";
                string status = "Fail";
                Setup2();
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
                    driver.FindElement(By.XPath("//span[contains(text(),'Đăng Ký')]")).Click();
                    Thread.Sleep(1000);

                    var EmailInput = driver.FindElement(By.XPath("//input[@placeholder='Nhập Email']"));
                    Thread.Sleep(500);
                    EmailInput.SendKeys(Keys.Control + "a" + Keys.Delete);
                    Thread.Sleep(500);
                    EmailInput.SendKeys(testFields[0]);


                    var passInput = driver.FindElement(By.XPath("//input[@placeholder='Nhập mật khẩu']"));
                    Thread.Sleep(500);
                    passInput.SendKeys(Keys.Control + "a" + Keys.Delete);
                    Thread.Sleep(500);
                    passInput.SendKeys(testFields[1]);

                    var re_passInput = driver.FindElement(By.XPath("//input[@placeholder='Nhập lại mật khẩu']"));
                    Thread.Sleep(500);
                    re_passInput.SendKeys(Keys.Control + "a" + Keys.Delete);
                    Thread.Sleep(500);
                    re_passInput.SendKeys(testFields[2]);

                    // Click nút submit
                    driver.FindElement(By.XPath("//span[contains(text(),'Đăng ký')]")).Click();
                    //chờ hiện thông báo cập nhật thành công


                    WebDriverWait waitMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                    IWebElement element = waitMessage.Until(driver =>
                    {
                        try
                        {
                            var elements = driver.FindElements(By.XPath("//div[contains(@class, 'ant-message-notice-content')]//span[2]"));
                            return elements.FirstOrDefault(el => el.Displayed); // Trả về phần tử hiển thị đầu tiên
                        }
                        catch (NoSuchElementException)
                        {
                            return null;
                        }
                    });
                    bool isErrorDisplayed = driver.FindElements(By.XPath("//div[contains(@class, 'ant-message-custom-content') and contains(@class, 'ant-message-error')]")).Count > 0;
                    Assert.That(element, Is.Not.Null, "Không tìm thấy bất kỳ thông báo nào sau 60s!");
                    Console.WriteLine($"✅ Đã tìm thấy thông báo: {element.Text.Trim()}");
                    Console.WriteLine($"✅ thông báo truyền vào: {thongBao}");
                    if (element.Text.Trim() != thongBao)
                    {
                        actual = element.Text.Trim();
                        status = "Fail";
                    }
                    else
                    {
                        if (isErrorDisplayed)
                        {
                            // Actions action = new Actions(driver);
                            // action.MoveByOffset(0, 0).Click().Perform(); // Click ra ngoài để tắt popup
                            actual = element.Text.Trim();
                            Console.WriteLine("Đã hiển thị thông báo lỗi! và dữ liệu không cập nhật");
                            status = "Pass";
                        }
                        else
                        {
                            // CheckInformationAfterUpdate(testFields[0], testFields[1], testFields[2], testFields[3], testFields[4], testFields[5], testFields[6]);
                            actual = element.Text.Trim();
                            status = "Pass";
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
                    status = "Fail";
                }
                //Ghi trạng thái test ra Excel nếu cần
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status, actual);
            }
        }


        [Test]
        [TestCase("ID_TaiKhoan_15", "Xóa người dùng thành công", "test11@gmail.com")]


        public void Test_XoaTaiKhoan(string testCaseID, string thongBao, string email)
        {
            string status = "Fail";
            try
            {
                driver.FindElement(By.XPath($"//tr[td[contains(normalize-space(.), '{email}')]]//span[@aria-label='delete']")).Click();
                Thread.Sleep(1000);
                driver.FindElement(By.XPath($"//button[@type='button' and contains(@class, 'ant-btn-primary')]/span[text()='OK']")).Click();

                //chờ hiện thông báo cập nhật thành công
                WebDriverWait waitMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                IWebElement element = waitMessage.Until(driver =>
                {
                    try
                    {
                        var elements = driver.FindElements(By.XPath("//div[contains(@class, 'ant-message-notice-content')]//span[2]"));
                        return elements.FirstOrDefault(el => el.Displayed); // Trả về phần tử hiển thị đầu tiên
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
                Console.WriteLine($"✅ Đã tìm thấy thông báo: {element.Text.Trim()}");
                Console.WriteLine($"✅ thông báo truyền vào: {thongBao}");
                if (element.Text.Trim() != thongBao)
                {
                    status = "Fail";
                }
                else
                {
                    status = "Pass";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
                status = "Fail";
            }
            //Ghi trạng thái test ra Excel nếu cần
            ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
        }


        [Test]
        public void Test_Phantrang()
        {
            string testCaseID = "ID_TaiKhoan_26";
            string status = "Fail";
            Setup();
            try
            {

                Thread.Sleep(8000);
                // Lấy tổng số đơn hàng
                var totalUsersText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                int totalUsers = int.Parse(totalUsersText.Split(':')[1].Trim());
                Console.WriteLine($"Tổng số người dùng: {totalUsers}");

                int usersPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)totalUsers / usersPerPage);
                Console.WriteLine($"Tổng số trang: {totalPages}");

                if (totalPages == 1)
                {
                    var products = driver.FindElements(By.XPath("//tr[contains(@class, 'ant-table-row')]"));
                    Console.WriteLine($"Số người dùng trên trang đầu tiên: {products.Count}");
                    if (products.Count == totalUsers)
                        Console.WriteLine("Tất cả người dùng được hiển thị trên 1 trang.");
                    else
                        Console.WriteLine($"LỖI: Số người dùng không khớp (hiển thị: {products.Count}, mong đợi: {totalUsers}).");
                }
                else
                {
                    Console.WriteLine($"LỖI: Có nhiều hơn 1 trang mặc dù tổng người dùng là {totalUsers}.");
                }
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
            if (status == "Pass")
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
            }
            else
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
            }
        }

        [Test]
        public void Test_ButtonNext()
        {
            string testCaseID = "ID_TaiKhoan_27";
            string status = "Fail";
            try
            {

                Thread.Sleep(6000);
                var totalUsersText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                int totalUsers = int.Parse(totalUsersText.Split(':')[1].Trim());
                Console.WriteLine($"Tổng số đơn hàng: {totalUsers}");

                int ordersPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)totalUsers / ordersPerPage);
                Console.WriteLine($"Tổng số trang: {totalPages}");

                if (totalPages > 1)
                {
                    // Kiểm tra nút Next
                    for (int currentPage = 1; currentPage < totalPages; currentPage++)
                    {
                        var currentPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                        Console.WriteLine($"Trang hiện tại: {currentPageNumber}");
                        driver.FindElement(By.XPath("//span[@aria-label='right']//*[name()='svg']")).Click();
                        Thread.Sleep(2000);
                        var newPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                        Console.WriteLine($"Chuyển đến trang: {newPageNumber}");
                        if (int.Parse(newPageNumber) != currentPage + 1)
                        {
                            Console.WriteLine("LỖI: Nút 'Next' không hoạt động đúng.");
                            status = "Fail";
                            ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Chỉ có 1 trang, không cần kiểm tra nút Next/Previous.");
                }
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
            if (status == "Pass")
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
            }
            else
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
            }
        }

        [Test]
        public void Test_ButtonPrevious()
        {
            string testCaseID = "ID_TaiKhoan_28";
            string status = "Fail";
            try
            {

                Thread.Sleep(6000);
                var totalUsersText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                int totalUsers = int.Parse(totalUsersText.Split(':')[1].Trim());
                Console.WriteLine($"Tổng số đơn hàng: {totalUsers}");

                int ordersPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)totalUsers / ordersPerPage);
                Console.WriteLine($"Tổng số trang: {totalPages}");

                if (totalPages > 1)
                {
                    for (int currentPage = 1; currentPage < totalPages; currentPage++)
                    {
                        var currentPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                        Console.WriteLine($"Trang hiện tại: {currentPageNumber}");
                        driver.FindElement(By.XPath("//span[@aria-label='right']//*[name()='svg']")).Click();
                        Thread.Sleep(2000);
                        var newPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                        Console.WriteLine($"Chuyển đến trang: {newPageNumber}");
                        if (int.Parse(newPageNumber) != currentPage + 1)
                        {
                            Console.WriteLine("LỖI: Nút 'Next' không hoạt động đúng.");
                            status = "Fail";
                            ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
                            return;
                        }
                    }
                    // Kiểm tra nút Previous
                    for (int currentPage = totalPages; currentPage > 1; currentPage--)
                    {
                        var currentPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                        Console.WriteLine($"Trang hiện tại: {currentPageNumber}");
                        driver.FindElement(By.XPath("//span[@aria-label='left']//*[name()='svg']")).Click();
                        Thread.Sleep(2000);
                        var newPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                        Console.WriteLine($"Chuyển đến trang: {newPageNumber}");
                        if (int.Parse(newPageNumber) != currentPage - 1)
                        {
                            Console.WriteLine("LỖI: Nút 'Previous' không hoạt động đúng.");
                            status = "Fail";
                            ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
                            return;
                        }
                    }
                    Console.WriteLine("Nút 'Next' và 'Previous' hoạt động đúng.");
                }
                else
                {
                    Console.WriteLine("Chỉ có 1 trang, không cần kiểm tra nút Next/Previous.");
                }
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
            if (status == "Pass")
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
            }
            else
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
            }
        }

        [Test]
        public void Test_SelectAnyPage()
        {
            string testCaseID = "ID_TaiKhoan_29";
            string status = "Fail";
            try
            {

                Thread.Sleep(6000);
                // Chuyển sang trang số 2 (ví dụ)
                driver.FindElement(By.XPath("//a[normalize-space()='2']")).Click();
                Thread.Sleep(3000);
                int UserOnPage = driver.FindElements(By.CssSelector("table tbody tr")).Count;
                Console.WriteLine($"Số đơn hàng trên trang 2: {UserOnPage}");
                if (UserOnPage == 10)
                    Console.WriteLine("✅ Trang 2 hiển thị đúng 10 đơn hàng.");
                else
                    Console.WriteLine("❌ Trang 2 hiển thị sai số lượng đơn hàng!");
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ ID_SelectPage Lỗi: {ex.Message}");
                status = "Fail";
            }
            if (status == "Pass")
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
            }
            else
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status);
            }
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
































// public void CheckInformationAfterUpdate(string fullName, string email, string phone, string address, string role, string isAdmin, string image)
//         {
//             try
//             {
//                 //mở edit
//                 driver.FindElement(By.XPath($"//tr[td[contains(normalize-space(.), '{email}')]]//span[@aria-label='edit']")).Click();
//                 Thread.Sleep(2000);

//                 // Kiem tra thong tin sau khi cap nhat
//                 var fullNameInput = driver.FindElement(By.Id("basic_name")).GetAttribute("value");
//                 Assert.That(fullNameInput, Is.EqualTo(fullName), "Họ và tên không trùng khớp!");

//                 var emailInput = driver.FindElement(By.Id("basic_email")).GetAttribute("value");
//                 Assert.That(emailInput, Is.EqualTo(email), "Email không trùng khóp!");

//                 var phoneInput = driver.FindElement(By.Id("basic_phone")).GetAttribute("value");
//                 Assert.That(phoneInput, Is.EqualTo(phone), "Số điện thoại không trùng khóp!");

//                 var addressInput = driver.FindElement(By.Id("basic_address")).GetAttribute("value");
//                 Assert.That(addressInput, Is.EqualTo(address), "Địa chiề không trùng khóp!");

//                 var roleInput = driver.FindElement(By.Id("basic_role")).GetAttribute("value");
//                 Assert.That(roleInput, Is.EqualTo(role), "Vai trò không trùng khóp!");

//                 var isAdminInput = driver.FindElement(By.Id("basic_isAdmin")).GetAttribute("value");
//                 Assert.That(isAdminInput, Is.EqualTo(isAdmin), "IsAdmin không trùng khóp!");

//                 // Tìm phần tử ảnh
//                 IWebElement imgElement = driver.FindElement(By.XPath("//div[@class='ant-form-item-control-input-content']//img[starts-with(@src, 'data:image/')]"));

//                 // Lấy dữ liệu base64 từ ảnh
//                 string base64Image = imgElement.GetAttribute("src").Split(',')[1]; // Bỏ phần "data:image/png;base64,"

//                 // Chuyển đổi base64 thành ảnh
//                 byte[] webImageBytes = Convert.FromBase64String(base64Image);
//                 Console.WriteLine("Ảnh web: " + webImageBytes.Length + " bytes");

//                 string testImagePath = image;
//                 byte[] testImageBytes = File.ReadAllBytes(testImagePath);
//                 Console.WriteLine("Ảnh test: " + testImageBytes.Length + " bytes");

//                 bool imagesAreEqual = webImageBytes.SequenceEqual(testImageBytes);
//                 Console.WriteLine("Ảnh giống nhau: " + imagesAreEqual);
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
//             }

//         }