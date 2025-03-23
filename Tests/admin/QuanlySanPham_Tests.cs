using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;
using SeleniumExtras.WaitHelpers;
using test_salephone.Utilities;
using test_salephone.Helpers;
using OpenQA.Selenium.DevTools.V131.Audits;

namespace QuanLySanPham_Tests 
{
    [TestFixture]
    public class QuanLySanPham_Tests
    {
        private IWebDriver driver;
      
        private string baseURL = "https://frontend-salephones.vercel.app/";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

            // Đăng nhập
            driver.FindElement(By.XPath("//input[@placeholder='Email']")).SendKeys("sela@gmail.com");
            driver.FindElement(By.CssSelector("input[placeholder='Nhập mật khẩu']")).SendKeys("123456");
            driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
            Thread.Sleep(6000);
            driver.FindElement(By.XPath("//img[@alt='avatar']")).Click();

            //Vào trang quản lý người dùng
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click();
            Thread.Sleep(2000);
            IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Sản phẩm')]"));
            sanPhamMenu.Click();
            Thread.Sleep(20000);
        }
        
    [Test]
    [Description("Test Thêm sản phẩm")]
    [TestCase("ID_QLSANPHAM_01", "Thêm sản phẩm thành công")]
    [TestCase("ID_QLSANPHAM_02", "Thêm sản phẩm không thành công")]
    [TestCase("ID_QLSANPHAM_03", "Thêm sản phẩm không thành công")]
public void Test_ThemSanPham(String testCaseID, String thongBao)
{
    string status = "Fail";
    try
    {   
        // Lấy dữ liệu test từ file Excel dựa theo testCaseID
        string testData_Add = string.Empty;
        if (testCaseID == "ID_QLSANPHAM_01")
        {
            testData_Add = ReadTestDataFromExcel.ReadDataRangeFromExcel("TestCase Anh Khôi", 12, 23, 6);
        }
        else if (testCaseID == "ID_QLSANPHAM_02")
        {
            testData_Add = ReadTestDataFromExcel.ReadDataRangeFromExcel("TestCase Anh Khôi", 24, 35, 6);
        }
        else if (testCaseID == "ID_QLSANPHAM_03")
        {
            testData_Add = ReadTestDataFromExcel.ReadDataRangeFromExcel("TestCase Anh Khôi", 36, 47, 6);
        }
        else
        {
            Console.WriteLine($"❌ TestCaseID {testCaseID} không được cấu hình phạm vi dòng.");
            status = "Fail";
            ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "TestCaseID không hợp lệ");
            return;
        }

        string[] testFields = testData_Add.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                
        // Thực hiện các thao tác nhập dữ liệu và upload file
        Thread.Sleep(6000);
        driver.FindElement(By.XPath("//body//div[@id='root']//div[@class='ant-spin-container']//div//div//div//div[1]//div[1]//button[1]")).Click();
        Thread.Sleep(2000);
                
        // Nhập "Họ và tên" (tên sản phẩm)
        var fullNameInput = driver.FindElement(By.Id("basic_name"));
        Thread.Sleep(3000);
        fullNameInput.Click();
        Thread.Sleep(500);
        fullNameInput.SendKeys(testFields[0]);

        // Chọn loại sản phẩm (ví dụ: Apple)
        IWebElement selectBox = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-selector')]"));
        selectBox.Click();
        Thread.Sleep(2000);
        IWebElement itemApple = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-item-option-content') and text()='Apple']"));
        itemApple.Click();

        // Nhập số lượng tồn kho
        var countinStockInput = driver.FindElement(By.Id("basic_countInStock"));
        Thread.Sleep(3000);
        countinStockInput.Click();
        Thread.Sleep(500);
        countinStockInput.SendKeys(testFields[2]);

        // Nhập giá sản phẩm
        var priceInput = driver.FindElement(By.Id("basic_price"));
        Thread.Sleep(3000);
        priceInput.Click();
        Thread.Sleep(500);
        priceInput.SendKeys(testFields[3]);

        // Nhập mô tả sản phẩm
        var desInput = driver.FindElement(By.Id("basic_description"));
        Thread.Sleep(3000);
        desInput.Click();
        Thread.Sleep(500);
        desInput.SendKeys(testFields[4]);

        // Click chọn file upload
        driver.FindElement(By.XPath("//span[text()='Select Files']")).Click();
        Thread.Sleep(2000);
        IWebElement fileInput = driver.FindElement(By.CssSelector("input[type='file']"));
        fileInput.SendKeys(@"C:\Users\Admin\Downloads\iphone16.jpg");
        Thread.Sleep(2000);

        // Nhấn checkbox (nếu cần)
        IWebElement checkBox = driver.FindElement(By.CssSelector("input[type='checkbox']"));
        checkBox.Click();
        Thread.Sleep(2000);

        // Nhập thông số kỹ thuật
        var screenSizeInput = driver.FindElement(By.Id("basic_screenSize"));
        Thread.Sleep(3000);
        screenSizeInput.Click();
        Thread.Sleep(500);
        screenSizeInput.SendKeys(testFields[6]);
                
        var chipsetInput = driver.FindElement(By.Id("basic_chipset"));
        Thread.Sleep(3000);
        chipsetInput.Click();
        Thread.Sleep(500);
        chipsetInput.SendKeys(testFields[7]);

        var ramInput = driver.FindElement(By.Id("basic_ram"));
        Thread.Sleep(3000);
        ramInput.Click();
        Thread.Sleep(500);
        ramInput.SendKeys(testFields[8]);
                
        var storageInput = driver.FindElement(By.Id("basic_storage"));
        Thread.Sleep(3000);
        storageInput.Click();
        Thread.Sleep(500);
        storageInput.SendKeys(testFields[9]);

        var batteryInput = driver.FindElement(By.Id("basic_battery"));
        Thread.Sleep(3000);
        batteryInput.Click();
        Thread.Sleep(500);
        batteryInput.SendKeys(testFields[10]);

        var screemResolutionInput = driver.FindElement(By.Id("basic_screenResolution"));
        Thread.Sleep(3000);
        screemResolutionInput.Click();
        Thread.Sleep(500);
        screemResolutionInput.SendKeys(testFields[11]);

        // Click nút "Thêm sản phẩm"
        IWebElement btntthemSanPham = driver.FindElement(By.XPath("//span[contains(text(),'Thêm sản phẩm')]"));
        btntthemSanPham.Click();
        Thread.Sleep(12000); 
                
        // Dùng WebDriverWait để bắt thông báo (dựa vào container thông báo)
        WebDriverWait waitMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                IWebElement element = waitMessage.Until(driver =>
                {
                    try
                    {
                        var elements = driver.FindElements(By.XPath("//div[contains(@class, 'ant-message-notice-content')]//span[text()='Thêm sản phẩm thành công!']"));
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


        // So sánh thông báo lấy được với thông báo mong đợi
        if (element.Text.Trim() != thongBao)
                {
                    status = "Fail";
                }

            else
                {
                    if (isErrorDisplayed)
                    {
                        // Actions action = new Actions(driver);
                        // action.MoveByOffset(0, 0).Click().Perform(); // Click ra ngoài để tắt popup
                        Console.WriteLine("Đã hiển thị thông báo lỗi! và dữ liệu không cập nhật");
                        status = "Pass";
                    }
                    else
                    {
                        // CheckInformationAfterUpdate(testFields[0], testFields[1], testFields[2], testFields[3], testFields[4], testFields[5], testFields[6]);
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
            ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status);

}

        // Test cập nhật sản phẩm (dữ liệu được đọc từ Excel theo testCaseID)
        [Test]
        [Description ("Test Cập nhật sản phẩm")]
        [TestCase ("ID_QLSANPHAM_04","Cập nhật thành công")]
        [TestCase ("ID_QLSANPHAM_05","Giá sản phẩm phải là một số hợp lệ lớn hơn 0")]

         public void Test_CapNhatSanPham(String testCaseID, String thongBao)
        {
            string status = "Fail";
    try
    {
        // Sử dụng hàm mới để lấy dữ liệu từ cột F (cột 6) của sheet "TestCase Anh Khôi"
        // từ dòng 12 đến dòng 23.
        string testData_Add = string.Empty;
        // Kiểm tra testCaseID để xác định phạm vi dòng cần lấy dữ liệu
        if (testCaseID == "ID_QLSANPHAM_04")
        { 
            // Lấy dữ liệu từ cột F (cột 6) của sheet "TestCase Anh Khôi" từ dòng 48 đến 51
            testData_Add = ReadTestDataFromExcel.ReadDataRangeFromExcel("TestCase Anh Khôi", 48, 51, 6);
        }
        else if (testCaseID == "ID_QLSANPHAM_05")
        {
            // Lấy dữ liệu từ cột F (cột 6) của sheet "TestCase Anh Khôi" từ dòng 54 đến 57
            testData_Add = ReadTestDataFromExcel.ReadDataRangeFromExcel("TestCase Anh Khôi", 54, 57, 6);
        }
            else
        {
            Console.WriteLine($"❌ TestCaseID {testCaseID} không được cấu hình phạm vi dòng.");
            status = "Fail";
            ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "TestCaseID không hợp lệ");
            return;
        }

        string[] testFields = testData_Add.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        
        // Debug: in ra dữ liệu test
        //foreach (var field in testFields)
        //{
        //    Console.WriteLine(field);
        //}
        
        // Click vào nút edit của sản phẩm cần cập nhật
        driver.FindElement(By.XPath("//tr[td[contains(normalize-space(.), 'iPhone 15 | 128GB | Đen')]]//span[@aria-label='edit']")).Click();
        Thread.Sleep(6000);

        // Nhập "Họ và tên" (ví dụ đây là tên sản phẩm)
        var fullNameInput = driver.FindElement(By.XPath("//div[@class='ant-drawer-body']//input[@id='basic_name']"));
        Thread.Sleep(3000);
        fullNameInput.Click();
        Thread.Sleep(500);
        fullNameInput.SendKeys(Keys.Control + "a" + Keys.Delete);
        Thread.Sleep(500);
        fullNameInput.SendKeys(testFields[0]);

        // Nhập "Thương hiệu"
        var brandInput = driver.FindElement(By.XPath("//div[@class='ant-drawer-body']//input[@id='basic_type']"));
        Thread.Sleep(3000);
        brandInput.Click();
        Thread.Sleep(500);
        brandInput.SendKeys(Keys.Control + "a" + Keys.Delete);
        Thread.Sleep(500);
        brandInput.SendKeys(testFields[1]);

        // Nhập "Số lượng"
        var countInStockInput = driver.FindElement(By.XPath("//div[@class='ant-drawer-body']//input[@id='basic_countInStock']"));
        Thread.Sleep(3000);
        countInStockInput.Click();
        Thread.Sleep(500);
        countInStockInput.SendKeys(Keys.Control + "a" + Keys.Delete);
        Thread.Sleep(500);
        countInStockInput.SendKeys(testFields[2]);

        // Nhập "Giá"
        var priceInput = driver.FindElement(By.XPath("//div[@class='ant-drawer-body']//input[@id='basic_price']"));
        Thread.Sleep(3000);
        priceInput.Click();
        Thread.Sleep(500);
        priceInput.SendKeys(Keys.Control + "a" + Keys.Delete);
        Thread.Sleep(500);
        priceInput.SendKeys(testFields[3]);

        // Click nút submit cập nhật
        driver.FindElement(By.XPath("//span[contains(text(),'Xác nhận')]")).Click();

        string excelMessage = "Cập nhật sản phẩm không thành công."; // Mặc định

        // Chờ hiện thông báo
        WebDriverWait waitMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        IWebElement element = waitMessage.Until(driver =>
        {
            try
            {   
                var successElements = driver.FindElements(By.XPath("//span[contains(text(),'Cập nhật sản phẩm thành công.')]"));
                var errorElements = driver.FindElements(By.XPath("//span[contains(text(),'Giá sản phẩm phải là một số hợp lệ và lớn hơn 0.')]"));

                if (errorElements.Count > 0 && errorElements[0].Displayed)
                {
                    Console.WriteLine("❌ Thông báo: Giá sản phẩm phải là một số hợp lệ và lớn hơn 0.");
                    status = "Pass"; 
                    excelMessage = "Giá sản phẩm phải là một số hợp lệ và lớn hơn 0.";
                    return errorElements[0];
                }
                else if (successElements.Count > 0 && successElements[0].Displayed)
                {
                    Console.WriteLine("✅ Thông báo: Cập nhật sản phẩm thành công.");
                    status = "Pass"; 
                    excelMessage = "Cập nhật sản phẩm thành công.";
                    return successElements[0];
                }
                return null;
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"⚠️ Phát hiện lỗi timeout: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi thông báo: {ex.Message}");
                return null;
            }
        });
                
            if (element == null)
            {
                status = "Fail";
            }   
            // Xử lý nút đóng nếu có
            var elements = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
            if (elements.Count > 0)
            {
                try
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    wait.Until(d =>
                    {
                        var closeButton = d.FindElement(By.XPath("//span[contains(@class, 'anticon-close')]//svg//path[contains(@d, 'M799.86 16')]"));
                        return closeButton.Displayed && closeButton.Enabled ? closeButton : null;
                    }).Click();
                    Console.WriteLine("🔘 Đã nhấp vào nút đóng.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("⚠️ Lỗi khi nhấp vào nút đóng: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("⚠️ Không tìm thấy nút đóng.");
            }

            Thread.Sleep(2000);
            }
                catch (WebDriverTimeoutException ex)
                {
                    Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
                    status = "Fail";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
                    status = "Fail";
                }
            // Ghi kết quả vào file Excel
                if (status == "Pass")
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status);
                }
                else
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status);
                }
}   
        

      

        [Test]
        public void Test_XoaSanPham()
        {
            string testCaseID = "ID_QLSANPHAM_06";
            string status = "Fail";
            try
            {
                Thread.Sleep(8000);
                driver.FindElement(By.XPath("//a[normalize-space()='2']")).Click();
                Thread.Sleep(6000);
                driver.FindElement(By.XPath("//tr[td[contains(text(), 'Iphone 16')]]//span[@aria-label='delete']//*[name()='svg']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//span[normalize-space()='OK']")).Click();
                Thread.Sleep(2000);
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
             if (status == "Pass")
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Xóa sản phẩm thành công");
                }
                else
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Xóa sản phẩm không thành công");
                }
        }

        [Test]
        public void Test_Phantrang()
        {
            string testCaseID = "ID_QLSANPHAM_07";
            string status = "Fail";
            try
            {
                
                Thread.Sleep(8000);
                // Lấy tổng số đơn hàng
                var totalProductsText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                int totalProducts = int.Parse(totalProductsText.Split(':')[1].Trim());
                Console.WriteLine($"Tổng số đơn hàng: {totalProducts}");

                int ordersPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)totalProducts / ordersPerPage);
                Console.WriteLine($"Tổng số trang: {totalPages}");

                if (totalPages == 1)
                {
                    var products = driver.FindElements(By.XPath("//tr[contains(@class, 'ant-table-row')]"));
                    Console.WriteLine($"Số đơn hàng trên trang đầu tiên: {products.Count}");
                    if (products.Count == totalProducts)
                        Console.WriteLine("Tất cả đơn hàng được hiển thị trên 1 trang.");
                    else
                        Console.WriteLine($"LỖI: Số đơn hàng không khớp (hiển thị: {products.Count}, mong đợi: {totalProducts}).");
                }
                else
                {
                    Console.WriteLine($"LỖI: Có nhiều hơn 1 trang mặc dù tổng đơn hàng là {totalProducts}.");
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Phân trang chính xác");
                }
                else
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Phân trang không chính xác");
                }
        }

        [Test]
        public void Test_ButtonNext()
        {
            string testCaseID = "ID_QLSANPHAM_08";
            string status = "Fail";
            try
            {
                
                Thread.Sleep(6000);
                var totalProductText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                int totalProducts = int.Parse(totalProductText.Split(':')[1].Trim());
                Console.WriteLine($"Tổng số đơn hàng: {totalProducts}");

                int ordersPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)totalProducts / ordersPerPage);
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
                            ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status);
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Phân trang chính xác");
                }
                else
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Phân trang không chính xác");
                }
        }
        [Test]
        public void Test_ButtonPrevious()
        {
            string testCaseID = "ID_QLSANPHAM_09";
            string status = "Fail";
            try
            {
                
                Thread.Sleep(6000);
                var totalProductText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                int totalProducts = int.Parse(totalProductText.Split(':')[1].Trim());
                Console.WriteLine($"Tổng số đơn hàng: {totalProducts}");

                int ordersPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)totalProducts / ordersPerPage);
                Console.WriteLine($"Tổng số trang: {totalPages}");

                if (totalPages > 1)
                {     for (int currentPage = 1; currentPage < totalPages; currentPage++)
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
                            ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status);
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
                            ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status);
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Chuyển trang chính xác");
                }
                else
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Chuyển trang không chính xác");
                }
        }

        [Test]
        public void Test_SelectAnyPage()
        {
            string testCaseID = "ID_QLSANPHAM_10";
            string status = "Fail";
            try
            {
                
                Thread.Sleep(6000);
                // Chuyển sang trang số 2 (ví dụ)
                driver.FindElement(By.XPath("//a[normalize-space()='2']")).Click();
                Thread.Sleep(3000);
                int productsOnPage = driver.FindElements(By.CssSelector("table tbody tr")).Count;
                Console.WriteLine($"Số đơn hàng trên trang 2: {productsOnPage}");
                if (productsOnPage == 10)
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Chuyển trang chính xác");
                }
                else
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Chuyển trang không chính xác");
                }
        }

        [Test]
        public void Test_TimKiemSanPhamBangMa()
        {
            string testCaseID = "ID_QLSANPHAM_12";
            string status = "Fail";
            try
            {
                
                Thread.Sleep(6000);
                driver.FindElement(By.XPath("//th[1]//div[1]//span[2]//span[1]//*[name()='svg']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//input[@placeholder='Search key']")).SendKeys("6734167b494d02e6fab57ea2");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//span[normalize-space()='Search']")).Click();
                Thread.Sleep(3000);
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
               if (status == "Pass")
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Tìm kiếm hoạt động chính xác");
                }
                else
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Tìm kiếm hoạt động không chính xác");
                }
        }

        [Test]
        public void Test_TimKiemSanPhamBangTen()
        {
            string testCaseID = "ID_QLSANPHAM_13";
            string status = "Fail";
            try
            {
                Thread.Sleep(6000);
                driver.FindElement(By.XPath("//th[@aria-label='Tên sản phẩm']//span[@aria-label='search']//*[name()='svg']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//input[@placeholder='Search name']")).SendKeys("iPhone 15 | 128GB | Đen");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//span[normalize-space()='Search']")).Click();
                Thread.Sleep(3000);
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
             if (status == "Pass")
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Tìm kiếm hoạt động chính xác");
                }
                else
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Tìm kiếm hoạt động không chính xác");
                }
        }

        [Test]
        public void Test_TimKiemSanPhamBangTheLoai()
        {
            string testCaseID = "ID_QLSANPHAM_14";
            string status = "Fail";
            try
            {
                Thread.Sleep(6000);
                driver.FindElement(By.XPath("//th[4]//div[1]//span[2]//span[1]//*[name()='svg']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//input[@placeholder='Search type']")).SendKeys("Apple");
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//span[normalize-space()='Search']")).Click();
                Thread.Sleep(3000);
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ {testCaseID} Lỗi: {ex.Message}");
                status = "Fail";
            }
              if (status == "Pass")
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Tìm kiếm hoạt động chính xác");
                }
                else
                {
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, "Tìm kiếm hoạt động không chính xác");
                }
        }

        [Test]
        public void Test_SapXepGia()
        {
            string testCaseID = "ID_QLSANPHAM_15" + "ID_QLSANPHAM_16";
            string status = "Fail";
            try
            {
                
                Thread.Sleep(9000);
                // Nhấp vào tiêu đề cột "Giá" 3 lần để sắp xếp
                for (int i = 0; i < 3; i++)
                {
                    driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Giá']]")).Click();
                    Thread.Sleep(3000);
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_15", status, "Sắp xếp hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_16", status, "Sắp xếp hoạt động chính xác");
                }
                else
                {
                   ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_15", status, "Sắp xếp không hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_16", status, "Sắp xếp không hoạt động chính xác");
                }
        }

        [Test]
        public void Test_SapXepTonKho()
        {
            string testCaseID = "ID_QLSANPHAM_17" + "ID_QLSANPHAM_18";
            string status = "Fail";
            try
            {
               
                Thread.Sleep(6000);
                for (int i = 0; i < 3; i++)
                {
                    driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tồn kho']]")).Click();
                    Thread.Sleep(3000);
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_17", status, "Sắp xếp hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_18", status, "Sắp xếp hoạt động chính xác");
                }
                else
                {
                   ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_17", status, "Sắp xếp không hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_18", status, "Sắp xếp không hoạt động chính xác");
                }
        }

        [Test]
        public void Test_SapXepDaBan()
        {
            string testCaseID = "ID_QLSANPHAM_19" + "ID_QLSANPHAM_20";
            string status = "Fail";
            try
            {
               
                Thread.Sleep(10000);
                for (int i = 0; i < 3; i++)
                {
                    driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Đã bán']]")).Click();
                    Thread.Sleep(3000);
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_19", status, "Sắp xếp hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_20", status, "Sắp xếp hoạt động chính xác");
                }
                else
                {
                   ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_19", status, "Sắp xếp không hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_20", status, "Sắp xếp không hoạt động chính xác");
                }
        }


        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
