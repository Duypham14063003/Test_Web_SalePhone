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
            Thread.Sleep(4000);
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
        [TestCase("ID_TaiKhoan_18", "Địa chỉ quá dài.")]
        [TestCase("ID_TaiKhoan_19", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_20", "Ảnh đại diện không phù hợp, ảnh phải là một định dạng đuôi .png, .jpg,...")]
        [TestCase("ID_TaiKhoan_21", "Vai trò quá dài.")]
        [TestCase("ID_TaiKhoan_22", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_23", "Cập nhật thành công")]
        [TestCase("ID_TaiKhoan_24", "isAdmin phải là true hoặc false.")]

        public void Test_CapNhatTaiKhoan(String testCaseID, String thongBao)
        {
            string actual = null;
            string status = "Fail";
            string intergration = null;
            Setup();
            Thread.Sleep(20000);

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
                        status = "Fail";
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
                // Thêm điều kiện kiểm tra "Cập nhật thành công" và so sánh với testFields
                if (element.Text.Trim() == "Cập nhật thành công")
                {
                    string localImagePath = $"{testFields[6]}";
                    string base64Local = ImageToBase64(localImagePath);
                    string shortBase64Local = base64Local.Substring(0, 50);
                    Console.WriteLine($"Giá trị của ảnh trên máy: {base64Local.Substring(0, 50)}");

                    driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app");
                    Thread.Sleep(2000);
                    driver.FindElement(By.XPath("//img[@alt='avatar']")).Click();

                    // Đăng xuất tài khoản admin
                    Thread.Sleep(4000);
                    driver.FindElement(By.XPath("//p[contains(text(),'Đăng xuất')]")).Click();

                    Thread.Sleep(1000);
                    driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

                    // Đăng nhập
                    driver.FindElement(By.XPath("//input[@placeholder='Email']")).SendKeys(testFields[1]);
                    driver.FindElement(By.CssSelector("input[placeholder='Nhập mật khẩu']")).SendKeys("123456");
                    driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();

                    Thread.Sleep(3000);
                    driver.FindElement(By.XPath("//img[@alt='avatar']")).Click();

                    // Vào trang quản lý người dùng
                    Thread.Sleep(2000);
                    driver.FindElement(By.XPath("//p[contains(text(),'Thông tin người dùng')]")).Click();

                    // Lấy giá trị từ giao diện
                    Thread.Sleep(500);
                    string textName = driver.FindElement(By.XPath("//input[@id='name']")).GetAttribute("value");
                    string textEmail = driver.FindElement(By.XPath("//input[@id='email']")).GetAttribute("value");
                    string textPhone = driver.FindElement(By.XPath("//input[@id='phone']")).GetAttribute("value");
                    string textAddress = driver.FindElement(By.XPath("//input[@id='address']")).GetAttribute("value");
                    string imgSrc = driver.FindElement(By.XPath("//img[@alt='avatar']")).GetAttribute("src");


                    string base64WebImage = imgSrc.Split(',')[1];
                    string shortBase64 = base64WebImage.Substring(0, 50);
                    // So sánh với testFields
                    if (textName != testFields[0] || textEmail != testFields[1] || textPhone != testFields[2] ||
                        textAddress != testFields[3])
                    {
                        if (shortBase64Local != shortBase64)
                        {
                            status = "Fail";
                            intergration = $"Tên: {textName}\nEmail: {textEmail}\nĐiện thoại: {textPhone}\nĐịa chỉ: {textAddress}\nẢnh: {shortBase64} (KHÔNG khớp ở định dạng base64)\nHệ thống cập nhật lại tài khoản không chính xác với thông tin sau chỉnh sửa!";
                        }
                        else
                        {
                            status = "Fail";
                            // Console.WriteLine("❌ Dữ liệu không khớp:");
                            // Console.WriteLine($"Tên: {textName} (Kỳ vọng: {testFields[0]})");
                            // Console.WriteLine($"Email: {textEmail} (Kỳ vọng: {testFields[1]})");
                            // Console.WriteLine($"Điện thoại: {textPhone} (Kỳ vọng: {testFields[2]})");
                            // Console.WriteLine($"Địa chỉ: {textAddress} (Kỳ vọng: {testFields[3]})");
                            // Console.WriteLine($"hình ảnh: {shortBase64Local} (Kỳ vọng: {shortBase64})");
                            intergration = $"Tên: {textName}\nEmail: {textEmail}\nĐiện thoại: {textPhone}\nĐịa chỉ: {textAddress}\nẢnh: {shortBase64}(Khớp ở định dạng base64)\nHệ thống cập nhật lại tài khoản không chính xác với thông tin sau chỉnh sửa!";
                        }


                    }
                    else
                    {
                        status = "Pass";
                        // Console.WriteLine("✅ Dữ liệu khớp với đầu vào:");
                        // Console.WriteLine($"Tên: {textName}");
                        // Console.WriteLine($"Email: {textEmail}");
                        // Console.WriteLine($"Điện thoại: {textPhone}");
                        // Console.WriteLine($"Địa chỉ: {textAddress}");
                        // Console.WriteLine($"Ảnh: {imgSrc.Substring(0, 50)}");
                        intergration = $"Tên: {textName}\nEmail: {textEmail}\nĐiện thoại: {textPhone}\nĐịa chỉ: {textAddress}\nẢnh: {shortBase64}\nHệ thống cập nhật lại tài khoản chính xác với thông tin sau chỉnh sửa!";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
                status = "Fail";
            }
            ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status, actual, intergration);
        }

        [Test]
        [Description("Test Đăng ký")]
        [TestCase("ID_TaiKhoan_1", "Hãy nhập email hợp lệ và có đuôi @gmail.com")]
        [TestCase("ID_TaiKhoan_2", "Hãy nhập đầy đủ thông tin")]
        [TestCase("ID_TaiKhoan_3", "Email này đã được đăng ký, vui lòng chọn email khác")]
        [TestCase("ID_TaiKhoan_4", "Đăng ký tài khoản thành công")]
        [TestCase("ID_TaiKhoan_5", "Mật khẩu phải có ít nhất 1 ký tự viết hoa và ít nhất 6 ký tự")]
        [TestCase("ID_TaiKhoan_6", "Mật khẩu phải có tối thiểu 6 ký tự")]
        [TestCase("ID_TaiKhoan_7", "Mật khẩu phải có ít nhất 1 ký tự viết hoa và ít nhất 6 ký tự")]
        [TestCase("ID_TaiKhoan_8", "Mật khẩu là trường bắt buộc")]
        [TestCase("ID_TaiKhoan_9", "Nhập lại mật khẩu là trường bắt buộc")]
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
            Setup();
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
            string actual = "N/A";
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

                if (totalPages >= 1)
                {
                    for (int i = 1; i <= totalPages; i++)
                    {
                        var products = driver.FindElements(By.XPath("//tr[contains(@class, 'ant-table-row')]"));
                        Console.WriteLine($"Số người dùng trên trang {i}: {products.Count}");

                        if (i < totalPages && products.Count != 10)
                        {
                            Console.WriteLine($"LỖI: Trang {i} không đủ 10 người dùng.");
                            status = "Fail";
                            break;
                        }
                        else if (i == totalPages && products.Count != totalUsers % 10)
                        {
                            Console.WriteLine($"LỖI: Trang cuối cùng có {products.Count} người dùng, mong đợi {totalUsers % 10}.");
                            status = "Fail";
                            break;
                        }

                        // Nhấn nút chuyển trang nếu chưa đến trang cuối
                        if (i < totalPages)
                        {
                            driver.FindElement(By.XPath("//li[@title='Next Page']//button[@type='button']")).Click();
                            Thread.Sleep(3000);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Chỉ có 1 trang, không cần kiểm tra phân trang.");
                }
                actual = "Số trang đúng với số lượng sản phẩm";
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
            if (status == "Pass")
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status, actual);
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
            string actual = "";
            Setup();
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
                actual = "Hiển thị đầy đủ danh sách của những trang tiếp theo";
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
            if (status == "Pass")
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status, actual);
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
            string actual = "";
            Setup();
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
                actual = "Hiển thị đầy đủ danh sách của trang trước";
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
            if (status == "Pass")
            {
                ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status, actual);
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
            string actual = "";
            Setup();
            try
            {
                Thread.Sleep(6000);

                // Lấy tổng số trang từ giao diện
                var totalUsersText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                int totalUsers = int.Parse(totalUsersText.Split(':')[1].Trim());

                int usersPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)totalUsers / usersPerPage);
                Console.WriteLine($"Tổng số trang: {totalPages}");

                // Random chọn 1 trang bất kỳ từ 1 đến totalPages
                Random rnd = new Random();
                int randomPage = rnd.Next(1, totalPages + 1);
                Console.WriteLine($"🔹 Chuyển đến trang số: {randomPage}");

                // Click vào trang ngẫu nhiên
                driver.FindElement(By.XPath($"//a[normalize-space()='{randomPage}']")).Click();
                Thread.Sleep(3000);

                // Kiểm tra số đơn hàng trên trang được chọn
                int UserOnPage = driver.FindElements(By.CssSelector("table tbody tr")).Count;
                Console.WriteLine($"Số đơn hàng trên trang {randomPage}: {UserOnPage}");

                if (randomPage < totalPages && UserOnPage == 10)
                    Console.WriteLine($"✅ Trang {randomPage} hiển thị đúng 10 đơn hàng.");
                else if (randomPage == totalPages && (UserOnPage > 0 && UserOnPage <= 10))
                    Console.WriteLine($"✅ Trang cuối ({randomPage}) hiển thị đúng số đơn hàng còn lại.");
                else
                    Console.WriteLine($"❌ LỖI: Trang {randomPage} hiển thị sai số lượng đơn hàng!");

                actual = $"Hiển thị danh sách sản phẩm của trang {randomPage}";
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }

            // Ghi kết quả vào file Excel
            ExcelReportHelper_Phuc.WriteToExcel("TestCase Hoàng Phúc", testCaseID, status, actual);
        }
        string ImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }
        string GetImageBase64FromElement(IWebDriver driver, string xpath)
        {
            string base64Src = driver.FindElement(By.XPath(xpath)).GetAttribute("src");
            return base64Src.Split(',')[1]; // Loại bỏ "data:image/png;base64,"
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