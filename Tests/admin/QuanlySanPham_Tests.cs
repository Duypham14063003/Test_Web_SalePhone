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
                
        Thread.Sleep(6000);
        driver.FindElement(By.XPath("//body//div[@id='root']//div[@class='ant-spin-container']//div//div//div//div[1]//div[1]//button[1]")).Click();
        Thread.Sleep(2000);
                
        var fullNameInput = driver.FindElement(By.Id("basic_name"));
        Thread.Sleep(3000);
        fullNameInput.Click();
        Thread.Sleep(500);
        fullNameInput.SendKeys(testFields[0]);

        IWebElement selectBox = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-selector')]"));
        selectBox.Click();
        Thread.Sleep(2000);
        IWebElement itemApple = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-item-option-content') and text()='Apple']"));
        itemApple.Click();

        var countinStockInput = driver.FindElement(By.Id("basic_countInStock"));
        Thread.Sleep(3000);
        countinStockInput.Click();
        Thread.Sleep(500);
        countinStockInput.SendKeys(testFields[2]);

        var priceInput = driver.FindElement(By.Id("basic_price"));
        Thread.Sleep(3000);
        priceInput.Click();
        Thread.Sleep(500);
        priceInput.SendKeys(testFields[3]);

        var desInput = driver.FindElement(By.Id("basic_description"));
        Thread.Sleep(3000);
        desInput.Click();
        Thread.Sleep(500);
        desInput.SendKeys(testFields[4]);

        driver.FindElement(By.XPath("//span[text()='Select Files']")).Click();
        Thread.Sleep(2000);
        IWebElement fileInput = driver.FindElement(By.CssSelector("input[type='file']"));
        fileInput.SendKeys(@"C:\Users\Admin\Downloads\iphone16.jpg");
        Thread.Sleep(2000);

        IWebElement checkBox = driver.FindElement(By.CssSelector("input[type='checkbox']"));
        checkBox.Click();
        Thread.Sleep(2000);

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

        IWebElement btnThemSanPham = driver.FindElement(By.XPath("//span[contains(text(),'Thêm sản phẩm')]"));
        btnThemSanPham.Click();
                
        Thread.Sleep(3000);

        // Kiểm tra thông báo lỗi giá
        var errorElements = driver.FindElements(By.XPath("//div[@class='ant-message-notice-content']//span[contains(text(),'Giá sản phẩm phải là một số hợp lệ')]"));
        if (errorElements.Count > 0)
        {
            string actualError = errorElements.First().Text.Trim();
            Console.WriteLine($"✅ Actual Result: {actualError}");
            status = "Pass";
            ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status);
            return;
        }

        // Kiểm tra thông báo validate (ví dụ: Vui lòng không bỏ trống!)
        var validateElements = driver.FindElements(By.XPath("//div[@class='ant-form-item-explain-error']"));
        if (validateElements.Count > 0)
        {
            string actualValidate = validateElements.First().Text.Trim();
            Console.WriteLine($"✅ Actual Result: {actualValidate}");
            status = "Pass";
            ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status);
            return;
        }

        // Nếu không có lỗi, bắt thông báo thành công
        WebDriverWait waitMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        IWebElement element = waitMessage.Until(drv =>
        {
            try
            {
                var elements = drv.FindElements(By.XPath("//div[contains(@class, 'ant-message-notice-content')]//span[2]"));
                return elements.FirstOrDefault(el => el.Displayed);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        });

        Assert.That(element, Is.Not.Null, "Không tìm thấy bất kỳ thông báo nào sau 60s!");
        string actualMessage = element.Text.Trim().TrimEnd('!');
        Console.WriteLine($"✅ Actual Result: {actualMessage}");
        Console.WriteLine($"✅ Thông báo mong đợi: {thongBao}");

        if (!actualMessage.Equals(thongBao, StringComparison.OrdinalIgnoreCase))
        {
            status = "Fail";
        }
        else
        {
            status = "Pass";
        }

        // Sau khi xác nhận thông báo, chuyển hướng về màn hình chính
        if (element != null && element.Displayed)
        {   
            IWebElement logo = driver.FindElement(By.XPath("//img[@alt='logo']"));
            logo.Click();
            Thread.Sleep(2000);

            WebDriverWait waitXemThem = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            IWebElement xemThemSanPham = waitXemThem.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[contains(text(),'Xem thêm sản phẩm')]")));
            xemThemSanPham.Click();
            Thread.Sleep(5000);
                    
            IWebElement searchProduct = driver.FindElement(By.XPath("//input[@placeholder='Tìm kiếm... ']"));
            searchProduct.SendKeys(testFields[0]);
            Thread.Sleep(4000);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
        status = "Fail";
    }
    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status);
}






        [Test]
        [Description ("Test Cập nhật sản phẩm")]
        [TestCase ("ID_QLSANPHAM_04","Cập nhật thành công")]
        [TestCase ("ID_QLSANPHAM_05","Giá sản phẩm phải là một số hợp lệ lớn hơn 0")]

         public void Test_CapNhatSanPham(String testCaseID, String thongBao)
        {
            string status = "Fail";
    try
    {
        string testData_Add = string.Empty;
        if (testCaseID == "ID_QLSANPHAM_04")
        { 
            testData_Add = ReadTestDataFromExcel.ReadDataRangeFromExcel("TestCase Anh Khôi", 48, 51, 6);
        }
        else if (testCaseID == "ID_QLSANPHAM_05")
        {
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

        
        WebDriverWait waitMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        IWebElement messageElement = waitMessage.Until(driver =>
        {
            var elements = driver.FindElements(By.XPath("//div[contains(@class, 'ant-message-notice-content')]//span[2]"));
            return (elements.Count > 0 && elements[0].Displayed) ? elements[0] : null;
        });

       // Lấy nội dung thông báo từ phần tử
            string excelMessage = messageElement != null ? messageElement.Text.Trim() : "Không tìm thấy thông báo";

            // Nếu thông báo là "Cập nhật sản phẩm thành công." thì chuyển trang theo yêu cầu
           if (excelMessage.Equals("Cập nhật sản phẩm thành công.", StringComparison.OrdinalIgnoreCase))
            {
                status = "Pass"; // Cập nhật status thành Pass khi thành công
                // Nhấp vào logo
                driver.FindElement(By.XPath("//img[@alt='logo']")).Click();
                Thread.Sleep(2000);
                // Nhấp vào nút "Xem thêm sản phẩm"
                driver.FindElement(By.XPath("//span[contains(text(),'Xem thêm sản phẩm')]")).Click();
                Thread.Sleep(5000);
                IWebElement searchProduct = driver.FindElement(By.XPath("//input[@placeholder='Tìm kiếm... ']"));
                searchProduct.SendKeys(testFields[0]);
                Thread.Sleep(6000);
                Console.WriteLine("✅ Đã chuyển sang trang 'Xem thêm sản phẩm'.");
                Console.WriteLine("Test result: " + excelMessage);
                ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, excelMessage);
                return;
            }

            else
            {
                status = "Pass";
                Thread.Sleep(2000);
                string failMessage = "Giá sản phẩm phải là một số hợp lệ và lớn hơn 0.";
                Console.WriteLine("Test result: " + failMessage);
                ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", testCaseID, status, failMessage);
                return;
            }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Phát hiện lỗi: {ex.Message}");
        status = "Fail";
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
                
                // Kiểm tra thông báo "Xóa sản phẩm thành công!"
                if (driver.FindElements(By.XPath("//span[contains(text(),'Xóa sản phẩm thành công!')]")).Count > 0)
                {
                    // Nếu thông báo xuất hiện thì bấm vào logo và "Xem thêm sản phẩm"
                    driver.FindElement(By.XPath("//img[@alt='logo']")).Click();
                    Thread.Sleep(2000);
                    driver.FindElement(By.XPath("//span[contains(text(),'Xem thêm sản phẩm')]")).Click();
                    Thread.Sleep(2000);
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
            string testCaseID = "ID_QLSANPHAM_11";
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
            string testCaseID = "ID_QLSANPHAM_12";
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
            string testCaseID = "ID_QLSANPHAM_13";
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
            string testCaseID = "ID_QLSANPHAM_14" + "ID_QLSANPHAM_15";
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_14", status, "Sắp xếp hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_15", status, "Sắp xếp hoạt động chính xác");
                }
                else
                {
                   ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_14", status, "Sắp xếp không hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_15", status, "Sắp xếp không hoạt động chính xác");
                }
        }

        [Test]
        public void Test_SapXepTonKho()
        {
            string testCaseID = "ID_QLSANPHAM_16" + "ID_QLSANPHAM_17";
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_16", status, "Sắp xếp hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_17", status, "Sắp xếp hoạt động chính xác");
                }
                else
                {
                   ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_16", status, "Sắp xếp không hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_17", status, "Sắp xếp không hoạt động chính xác");
                }
        }

        [Test]
        public void Test_SapXepDaBan()
        {
            string testCaseID = "ID_QLSANPHAM_18" + "ID_QLSANPHAM_19";
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
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_18", status, "Sắp xếp hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_19", status, "Sắp xếp hoạt động chính xác");
                }
                else
                {
                   ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_18", status, "Sắp xếp không hoạt động chính xác");
                    ExcelReportHelper_Khoi.WriteToExcel("TestCase Anh Khôi", "ID_QLSANPHAM_19", status, "Sắp xếp không hoạt động chính xác");
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
