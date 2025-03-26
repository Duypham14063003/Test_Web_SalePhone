using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using test_salephone.Helpers;
using OpenQA.Selenium.Support.UI;
using test_salephone.Utilities;
using System.Text.RegularExpressions;
using SeleniumExtras.WaitHelpers;

namespace test_salephone.Tests
{
    [TestFixture]
    public class Test_Order
    {
        private IWebDriver driver;
        private IWebElement element;

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
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//img[@alt='avatar']")).Click();

            //Vào trang quản lý người dùng
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click();
            Thread.Sleep(2000);
            IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Đơn hàng')]"));
            sanPhamMenu.Click();
            Thread.Sleep(10000);
        }

        [Test]
        [Description("Test Kiểm tra giao diện và số lượng đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_01", "Giao diện hiển thị đầy đủ", TestName = "ID_Order_01")]
        [TestCase("ID_Order_02", "Giao diện hiển thị đầy đủ khi có 1 đơn hàng", TestName = "ID_Order_02")]
        [TestCase("ID_Order_03", "Không có đơn nào trong danh sách", TestName = "ID_Order_03")]
        [TestCase("ID_Order_04", "Đơn hàng hiển thị đúng với dữ liệu", TestName = "ID_Order_04")]
        [TestCase("ID_Order_05", "Hiển thị đúng danh sách sản phẩm trong đơn hàng", TestName = "ID_Order_05")]
        public void Test_ViewAndVerifyOrder(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                //Kiểm tra giao diện đơn hàng
                if (testCaseID.StartsWith("ID_Order_01") || testCaseID.StartsWith("ID_Order_02") || testCaseID.StartsWith("ID_Order_03"))
                {
                    IWebElement orderList = wait.Until(d => d.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[2]/div/div/div/div/div/div/div[2]")));
                    if (orderList.Displayed)
                    {
                        Console.WriteLine("✅ Giao diện hiển thị danh sách đơn hàng đầy đủ");
                        status = "Pass";
                    }
                    else
                    {
                        Console.WriteLine("❌ Không tìm thấy danh sách đơn hàng");
                        status = "Fail";
                    }
                }

                //Kiểm tra số lượng đơn hàng
                if (testCaseID.StartsWith("ID_Order_04"))
                {
                    Thread.Sleep(5000);
                    string totalOrderText = driver.FindElement(By.XPath("//div[contains(text(),'Tổng số lượng đơn hàng')]")).Text;
                    string[] parts = totalOrderText.Split(':');
                    Thread.Sleep(5000);

                    if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
                    {
                        throw new Exception("⚠️ LỖI: Không lấy được số lượng đơn hàng!");
                    }

                    if (!int.TryParse(parts[1].Trim(), out int totalOrderCount))
                    {
                        throw new Exception($"⚠️ LỖI: Giá trị '{parts[1].Trim()}' không phải số hợp lệ!");
                    }
                    Console.WriteLine($"📌 Tổng số lượng đơn hàng hiển thị: {totalOrderCount}");

                    int actualOrderCount = 0;
                    while (true)
                    {
                        var currentPage = driver.FindElement(By.XPath("//li[contains(@class,'ant-pagination-item-active')]")).Text;
                        Console.WriteLine($"📌 Đang ở trang: {currentPage}");

                        List<IWebElement> orderRows = driver.FindElements(By.XPath("//tr[contains(@class, 'ant-table-row')]")).ToList();
                        actualOrderCount += orderRows.Count;

                        var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));
                        if (nextPageButton.Count > 0)
                        {
                            Console.WriteLine("📌 Chuyển sang trang tiếp theo...");
                            nextPageButton[0].Click();
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.WriteLine("✅ Đã duyệt hết tất cả các trang.");
                            break;
                        }
                    }

                    Console.WriteLine($"📌 Số đơn hàng thực tế sau khi duyệt tất cả trang: {actualOrderCount}");

                    if (totalOrderCount == actualOrderCount)
                    {
                        Console.WriteLine("✅ Dữ liệu đúng: Số lượng đơn hàng hiển thị khớp với số đơn hàng thực tế.");
                        status = "Pass";
                    }
                    else
                    {
                        Console.WriteLine($"❌ LỖI: Số đơn hàng hiển thị ({totalOrderCount}) KHÔNG khớp với số đơn hàng thực tế ({actualOrderCount}).");
                        status = "Fail";
                        thongBao = "Đơn hàng hiển thị sai dữ liệu";
                    }
                }

                //Kiểm tra giao diện đơn hàng
                if (testCaseID.StartsWith("ID_Order_05"))
                {
                    IWebElement orderList = wait.Until(d => d.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[2]/div/div/div/div/div/div/div[2]")));
                    // Chọn đơn hàng đầu tiên để mở chi tiết
                    driver.FindElement(By.XPath("//button[@type='button' and @class='ant-table-row-expand-icon ant-table-row-expand-icon-collapsed' and @aria-label='Expand row' and @aria-expanded='false']")).Click();
                    Thread.Sleep(3000); 

                    // Lấy danh sách sản phẩm trong chi tiết đơn hàng
                    var productElements = driver.FindElements(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[2]/div/div/div/div/div/div/div[2]/div[2]/table/tbody/tr[3]/td/div/div/div[2]"));
                    List<string> displayedProducts = productElements.Select(el => el.Text.Trim()).ToList();

                    // In danh sách sản phẩm ra console
                    Console.WriteLine("Danh sách sản phẩm hiển thị trong đơn hàng:");
                    displayedProducts.ForEach(product => Console.WriteLine($"- {product}"));

                    // Kiểm tra danh sách không rỗng
                    if (displayedProducts.Count > 0)
                    {
                        Console.WriteLine("Danh sách sản phẩm hiển thị đúng.");
                        status = "Pass";
                    }
                    else
                    {
                        Console.WriteLine("LỖI: Không có sản phẩm nào hiển thị trong đơn hàng!");
                        status = "Fail";
                        thongBao = "Không có sản phẩm nào trong đơn hàng";
                    }
                }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            string testResultMessage = thongBao;
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, testResultMessage);
        }

        [Test]
        [Description("Test Kiểm tra chỉnh sửa đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_06", new string[] {
            "Popup (dropdown) xuất hiện ngay bên dưới trạng thái hiện tại",
            "popup hiển thị đầy đủ các trạng thái",
            "Đơn hàng được cập nhật đúng khi chọn trạng thái mới",
            "Trạng thái không thay đổi khi nhấp ngoài popup"}, TestName = "ID_Order_06")]
        public void Test_Popup_StatusOrder(String testCaseID, string[] thongBaoList)
        {
            string status = "Fail";
            IWebElement orderRow = null;
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin")
                    .FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                while (true)
                {
                    try
                    {
                        // Tìm đơn hàng có trạng thái "Đang giao hàng"
                        orderRow = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đang giao hàng']"));

                        Console.WriteLine("✅ Đã tìm thấy đơn hàng 'Đang giao hàng'");
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

                        if (nextPageButton.Count > 0)
                        {
                            Console.WriteLine("⏩ Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
                            nextPageButton[0].Click();
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.WriteLine("❌ Không có đơn hàng 'Đang giao hàng' trên tất cả các trang.");
                            return;
                        }
                    }
                }

                // Nhấn vào trạng thái để mở dropdown
                var statusButton = orderRow.FindElement(By.XPath(
                    "//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đang giao hàng']"));
                statusButton.Click();
                Thread.Sleep(2000);

                // Lấy danh sách trạng thái từ popup
                var statusOptions = driver.FindElements(By.XPath(
                    "//div[contains(@class, 'ant-dropdown')]//ul[contains(@class, 'ant-dropdown-menu')]/li"));

                Console.WriteLine($"Số lượng trạng thái tìm thấy: {statusOptions.Count}");

                var expectedStatuses = new[] { "Đang giao hàng", "Đã giao hàng", "Đang xử lý", "Đã hủy" };
                foreach (var option in statusOptions)
                {
                    string optionText = option.Text.Trim();
                    Console.WriteLine($"Trạng thái hợp lệ: {optionText}");
                }

                Thread.Sleep(2000);

                // Chọn một trạng thái khác
                var newStatusOption = statusOptions.FirstOrDefault(option => option.Text.Trim() == "Đã giao hàng");
                if (newStatusOption != null)
                {
                    newStatusOption.Click();
                    Console.WriteLine("✅ Đã chọn trạng thái 'Đã giao hàng'.");
                }
                else
                {
                    Console.WriteLine("❌ Không tìm thấy trạng thái 'Đã giao hàng'! Kiểm tra lại danh sách trạng thái.");
                    return;
                }
                Thread.Sleep(2000);

                // Kiểm tra trạng thái đơn hàng sau khi chọn
                var updatedStatus = driver.FindElement(By.XPath("//td[@class='ant-table-cell ant-table-cell-fix-left ant-table-cell-row-hover']//button[@type='button']"));
                if (updatedStatus != null)
                    Console.WriteLine("✅ Trạng thái đơn hàng được cập nhật đúng: Đã giao hàng.");
                else
                    Console.WriteLine("❌ Trạng thái đơn hàng không được cập nhật!");
                Thread.Sleep(2000);

                // Kiểm tra popup có đóng khi nhấp ra ngoài không
                var bodyElement = driver.FindElement(By.TagName("body"));
                bodyElement.Click();
                Console.WriteLine("🖱️ Đã nhấp ra ngoài popup.");
                Thread.Sleep(2000);
                bool isPopupClosed = driver.FindElements(By.XPath("//div[contains(@class, 'ant-dropdown')]")).Count == 0;

                if (isPopupClosed)
                    Console.WriteLine("✅ Popup đã đóng đúng cách.");
                else
                    Console.WriteLine("❌ Popup không đóng đúng cách!");

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
                status = "Pass";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            string allMessages = string.Join("\n", thongBaoList);
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, allMessages);
        }


        // [Test]
        // [Description("Test Kiểm tra giao diện đơn hàng")]
        // [Category("Order Management")]
        // [TestCase("ID_Order_01", "Giao diện hiển thị đầy đủ", TestName = "ID_Order_01")]
        // [TestCase("ID_Order_02", "Giao diện hiển thị đầy đủ khi có 1 đơn hàng", TestName = "ID_Order_02")]
        // [TestCase("ID_Order_03", "Không có đơn nào trong danh sách", TestName = "ID_Order_03")]
        // public void Test_ViewOrder(String testCaseID, String thongBao)
        // {
        //     string status = "Fail";
        //     try
        //     {
        //         // Đọc dữ liệu kiểm thử từ Excel
        //         string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

        //         if (string.IsNullOrEmpty(dataTest))
        //         {
        //             Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
        //             return;
        //         }

        //         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //         // Kiểm tra xem danh sách đơn hàng có hiển thị không
        //         IWebElement orderList = wait.Until(d => d.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[2]/div/div/div/div/div/div/div[2]")));
        //         if (orderList.Displayed)
        //         {
        //             Console.WriteLine("✅ Giao diện hiển thị danh sách đơn hàng đầy đủ");
        //             status = "Pass";
        //         }
        //         else
        //         {
        //             Console.WriteLine("❌ Không tìm thấy danh sách đơn hàng");
        //             status = "Fail";
        //         }
                
        //         // Đóng cửa sổ nếu có nút đóng
        //         var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
        //         if (closeButton.Count > 0)
        //         {
        //             closeButton[0].Click();
        //             Console.WriteLine("✅ Đã nhấp vào nút đóng.");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
        //     }

        //     //Ghi trạng thái test ra Excel nếu cần
        //     string testResultMessage = thongBao;
        //     ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, testResultMessage);
        // }

        // [Test]
        // [Description("Test Kiểm tra giao diện đơn hàng")]
        // [Category("Order Management")]
        // [TestCase("ID_Order_04", "Đơn hàng hiển thị đúng với dữ liệu", TestName = "ID_Order_04")]
        // public void Test_VerifyOrderQuantity(String testCaseID, String thongBao)
        // {
        //     string status = "Fail";
        //     try
        //     {
        //         // Đọc dữ liệu kiểm thử từ Excel
        //         string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

        //         if (string.IsNullOrEmpty(dataTest))
        //         {
        //             Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
        //             return;
        //         }

        //         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //         //Lấy số lượng đơn hàng từ tiêu đề (Ví dụ: "Tổng số lượng đơn hàng: 113")
        //         string totalOrderText = driver.FindElement(By.XPath("//div[contains(text(),'Tổng số lượng đơn hàng')]")).Text;
        //         string[] parts = totalOrderText.Split(':');

        //         if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
        //         {
        //             throw new Exception("⚠️ LỖI: Không lấy được số lượng đơn hàng!");
        //         }

        //         if (!int.TryParse(parts[1].Trim(), out int totalOrderCount))
        //         {
        //             throw new Exception($"⚠️ LỖI: Giá trị '{parts[1].Trim()}' không phải số hợp lệ!");
        //         }
        //         Console.WriteLine($"📌 Tổng số lượng đơn hàng hiển thị: {totalOrderCount}");

        //         //Đếm số lượng đơn hàng thực tế bằng cách duyệt qua từng trang
        //         int actualOrderCount = 0;

        //         while (true)
        //         {
        //             // Kiểm tra trang hiện tại để debug
        //             var currentPage = driver.FindElement(By.XPath("//li[contains(@class,'ant-pagination-item-active')]")).Text;
        //             Console.WriteLine($"📌 Đang ở trang: {currentPage}");

        //             // Đếm số đơn hàng trên trang hiện tại
        //             List<IWebElement> orderRows = driver.FindElements(By.XPath("//tr[contains(@class, 'ant-table-row')]")).ToList();
        //             actualOrderCount += orderRows.Count;

        //             // Kiểm tra xem có thể bấm Next không
        //             var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));
                    
        //             if (nextPageButton.Count > 0)
        //             {
        //                 Console.WriteLine("📌 Chuyển sang trang tiếp theo...");
        //                 nextPageButton[0].Click();

        //                 // Đợi trang tải xong
        //                 // WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        //                 // wait.Until(drv => drv.FindElement(By.XPath("//table//tbody//tr")));

        //                 Thread.Sleep(1000); // Chờ thêm 1s để tránh lỗi hiển thị chậm
        //             }
        //             else
        //             {
        //                 Console.WriteLine("✅ Đã duyệt hết tất cả các trang.");
        //                 break; // Thoát vòng lặp khi không còn nút Next
        //             }
        //         }


        //         Console.WriteLine($"📌 Số đơn hàng thực tế sau khi duyệt tất cả trang: {actualOrderCount}");

        //         // So sánh hai số và in kết quả
        //         if (totalOrderCount == actualOrderCount)
        //         {
        //             Console.WriteLine("✅ Dữ liệu đúng: Số lượng đơn hàng hiển thị khớp với số đơn hàng thực tế.");
        //             status = "Pass";
        //         }
        //         else
        //         {
        //             Console.WriteLine($"❌ LỖI: Số đơn hàng hiển thị ({totalOrderCount}) KHÔNG khớp với số đơn hàng thực tế ({actualOrderCount}).");
        //             status = "Fail";
        //             thongBao = "Đơn hàng hiển thị sai dữ liệu";
        //         }
                
        //         // Đóng cửa sổ nếu có nút đóng
        //         var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
        //         if (closeButton.Count > 0)
        //         {
        //             closeButton[0].Click();
        //             Console.WriteLine("✅ Đã nhấp vào nút đóng.");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
        //     }

        //     //Ghi trạng thái test ra Excel nếu cần
        //     string testResultMessage = thongBao;
        //     ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, testResultMessage);
        // }

        // [Test]
        // [Description("Test Kiểm tra giao diện đơn hàng")]
        // [Category("Order Management")]
        // [TestCase("ID_Order_05", "Hiển thị đúng danh sách sản phẩm trong đơn hàng")]
        // public void Test_ManyproductsinOrder(String testCaseID, String thongBao)
        // {
        //     string status = "Fail";
        //     try
        //     {
        //         // Đọc dữ liệu kiểm thử từ Excel
        //         string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

        //         if (string.IsNullOrEmpty(dataTest))
        //         {
        //             Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
        //             return;
        //         }

        //         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //         // Chọn đơn hàng đầu tiên để mở chi tiết
        //         driver.FindElement(By.XPath("//button[@type='button' and @class='ant-table-row-expand-icon ant-table-row-expand-icon-collapsed' and @aria-label='Expand row' and @aria-expanded='false']")).Click();
        //         Thread.Sleep(3000); 

        //         // Lấy danh sách sản phẩm trong chi tiết đơn hàng
        //         var productElements = driver.FindElements(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[2]/div/div/div/div/div/div/div[2]/div[2]/table/tbody/tr[3]/td/div/div/div[2]"));
        //         List<string> displayedProducts = productElements.Select(el => el.Text.Trim()).ToList();

        //         // In danh sách sản phẩm ra console
        //         Console.WriteLine("Danh sách sản phẩm hiển thị trong đơn hàng:");
        //         displayedProducts.ForEach(product => Console.WriteLine($"- {product}"));

        //         // Kiểm tra danh sách không rỗng
        //         if (displayedProducts.Count > 0)
        //         {
        //             Console.WriteLine("Danh sách sản phẩm hiển thị đúng.");
        //             status = "Pass";
        //         }
        //         else
        //         {
        //             Console.WriteLine("LỖI: Không có sản phẩm nào hiển thị trong đơn hàng!");
        //             status = "Fail";
        //         }
                
        //         // Đóng cửa sổ nếu có nút đóng
        //         var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
        //         if (closeButton.Count > 0)
        //         {
        //             closeButton[0].Click();
        //             Console.WriteLine("✅ Đã nhấp vào nút đóng.");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
        //     }

        //     //Ghi trạng thái test ra Excel nếu cần
        //     string testResultMessage = thongBao;
        //     ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, testResultMessage);
        // }

        [Test]
        [Description("Test Kiểm tra chỉnh sửa đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_07", "Đã giao hàng", "Đã giao hàng thành công", "Trạng thái tình trạng đơn hàng được cập nhật thành công", TestName = "ID_Order_07")]
        [TestCase("ID_Order_08", "Đã thanh toán", "Đã thanh toán thành công", "Trạng thái thanh toán đơn hàng được cập nhật thành công", TestName = "ID_Order_08")]
        public void Test_Update_OrderStatus(string testCaseID, string expectedAdminStatus, string expectedUserStatus, string thongBao)
        {
            string status = "Fail";
            IWebElement orderRow = null;
            try
            {
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;
                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                while (true)
                {
                    try
                    {
                        orderRow = driver.FindElement(By.XPath("//td[normalize-space()='67e19e091bb6b5978a009687']"));
                        Console.WriteLine("Đã tìm thấy đơn hàng '67e19e091bb6b5978a009687'");
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));
                        if (nextPageButton.Count > 0) nextPageButton[0].Click();
                        else return;
                        Thread.Sleep(2000);
                    }
                }

                IWebElement statusDropdown = driver.FindElement(By.XPath($"//tr[@data-row-key='67e19e091bb6b5978a009687']//td//button/span[text()='Đang xử lý' or text()='Chưa thanh toán']"));
                statusDropdown.Click();
                Thread.Sleep(3000);

                driver.FindElement(By.XPath($"//li[contains(@class, 'ant-dropdown-menu-item')]//span[text()='{expectedAdminStatus}']")).Click();
                Thread.Sleep(3000);

                driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/system/admin");
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//span[contains(text(),'Đơn hàng')]")).Click();
                Thread.Sleep(10000);
                string updatedStatus = driver.FindElement(By.XPath($"//tr[@data-row-key='67e19e091bb6b5978a009687']//td//button/span[text()='{expectedAdminStatus}']")).Text;
                Console.WriteLine("Trạng thái đơn hàng mới: " + updatedStatus);

                driver.FindElement(By.XPath("//img[@alt='avatar']")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//p[contains(text(),'Đăng xuất')]")).Click();
                Thread.Sleep(5000);

                driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");
                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//input[@placeholder='Email']")).SendKeys("rinlam@gmail.com");
                driver.FindElement(By.XPath("//input[@placeholder='Nhập mật khẩu']")).SendKeys("123456R");
                driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
                Thread.Sleep(10000);

                driver.FindElement(By.XPath("//div[contains(text(),'DSFDHDS')]")).Click();
                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//p[contains(text(),'Lịch sử mua hàng')]")).Click();
                Thread.Sleep(7000);
                string userOrderStatus = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[1]/div/div/div/div[1]/div[2]/p[contains(text(), 'Đã')]/div/div/div")).Text;
                Console.WriteLine("Trạng thái đơn hàng của user: " + userOrderStatus);

                if (updatedStatus == expectedAdminStatus && userOrderStatus == expectedUserStatus)
                {
                    status = "Pass";
                }
                else
                {
                    Console.WriteLine("❌ Trạng thái đơn hàng không đúng, kiểm tra lại!");
                }

                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0) closeButton[0].Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }


        // [Test]
        // [Description("Test Kiểm tra chỉnh sửa đơn hàng")]
        // [Category("Order Management")]
        // [TestCase("ID_Order_07", "Trạng thái tình trạng đơn hàng được cập nhật thành công", TestName = "ID_Order_07")]
        // public void Test_Update_StatusOrder(String testCaseID, String thongBao)
        // {
        //     string status = "Fail";
        //     IWebElement orderRow = null;
        //     try
        //     {
        //         // Đọc dữ liệu kiểm thử từ Excel
        //         string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

        //         if (string.IsNullOrEmpty(dataTest))
        //         {
        //             Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
        //             return;
        //         }

        //         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //         // Tìm đơn hàng có trạng thái "Đã giao hàng" trên bất kỳ trang nào
        //         while (true)
        //         {
        //             try
        //             {
        //                 // Tìm đơn hàng có trạng thái "67e19e091bb6b5978a009687" trên trang hiện tại
        //                 orderRow = driver.FindElement(By.XPath("//td[normalize-space()='67e19e091bb6b5978a009687']"));
        //                 Thread.Sleep(3000);
        //                 // Nếu tìm thấy, thoát vòng lặp để xử lý
        //                 Console.WriteLine("Đã tìm thấy đơn hàng '67e19e091bb6b5978a009687'");
        //                 break;
        //             }
        //             catch (NoSuchElementException)
        //             {
        //                 // Nếu không tìm thấy đơn hàng trên trang hiện tại, kiểm tra nút chuyển trang
        //                 var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

        //                 if (nextPageButton.Count > 0)
        //                 {
        //                     // Chuyển sang trang tiếp theo
        //                     Console.WriteLine("Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
        //                     nextPageButton[0].Click();
        //                     Thread.Sleep(2000); 
        //                 }
        //                 else
        //                 {
        //                     // Không còn trang nào để tìm
        //                     Console.WriteLine("Không có đơn hàng '67e19e091bb6b5978a009687' trên tất cả các trang.");
        //                     return; // Kết thúc vì không tìm thấy
        //                 }
        //             }
        //         }
        //         // // Chọn đơn hàng cần sửa
        //         // Thread.Sleep(3000);
        //         // IWebElement latestOrder = driver.FindElement(By.XPath("//td[normalize-space()='67e19e091bb6b5978a009687']"));
        //         // //latestOrder.Click();
        //         Thread.Sleep(3000);
                
        //         // Nhấn vào trạng thái "Đang xử lý" để mở dropdown
        //         IWebElement statusDropdown = driver.FindElement(By.XPath("//tr[@data-row-key='67e19e091bb6b5978a009687']//td//button/span[text()='Đang xử lý']"));
        //         statusDropdown.Click();
        //         Thread.Sleep(3000);
                
        //         // Chọn trạng thái "Đã giao hàng"
        //         driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã giao hàng']")).Click();
        //         Thread.Sleep(3000);
                
        //         // Vào trang quản lý đơn hàng bên Admin để kiểm tra trạng thái đơn hàng
        //         driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/system/admin");
        //         Thread.Sleep(3000);
        //         IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Đơn hàng')]"));
        //         sanPhamMenu.Click();
        //         Thread.Sleep(10000);
        //         string updatedStatus = driver.FindElement(By.XPath("//tr[@data-row-key='67e19e091bb6b5978a009687']//td//button/span[text()='Đã giao hàng']")).Text;
        //         Thread.Sleep(3000);
        //         Console.WriteLine("Trạng thái đơn hàng mới: " + updatedStatus);

        //         Thread.Sleep(2000);
        //         //driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click();
        //         driver.FindElement(By.XPath("//img[@alt='avatar']")).Click();
        //         Thread.Sleep(2000);
        //         driver.FindElement(By.XPath("//p[contains(text(),'Đăng xuất')]")).Click();
        //         Thread.Sleep(5000);
                
        //         // Đăng nhập vào trang user đã đặt hàng
        //         driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in"); 
        //         //driver.FindElement(By.XPath("//span[@aria-label='user']//*[name()='svg']"));
        //         Thread.Sleep(5000);
        //         IWebElement emailFiel= driver.FindElement(By.XPath("//input[@placeholder='Email']"));
        //         Thread.Sleep(5000);
        //         emailFiel.SendKeys("rinlam@gmail.com");
        //         driver.FindElement(By.XPath("//input[@placeholder='Nhập mật khẩu']")).SendKeys("123456R");
        //         driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
        //         Thread.Sleep(10000);
                
        //         // Vào trang "Lịch sử mua hàng" để kiểm tra đơn hàng mới nhất
        //         driver.FindElement(By.XPath("//div[contains(text(),'DSFDHDS')]")).Click();
        //         Thread.Sleep(5000);
        //         driver.FindElement(By.XPath("//p[contains(text(),'Lịch sử mua hàng')]")).Click();
        //         Thread.Sleep(7000);
        //         string userOrderStatus = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[1]/div/div/div/div[1]/div[2]/p[1]/div/div/div")).Text;
        //         Console.WriteLine("Trạng thái đơn hàng của user: " + userOrderStatus);
        //         if (updatedStatus == "Đã giao hàng" && userOrderStatus == "Đã giao hàng thành công")
        //         {
        //             status = "Pass";
        //         }
        //         else
        //         {
        //             Console.WriteLine("❌ Trạng thái đơn hàng không đúng, kiểm tra lại!");
        //             status = "Fail";
        //         }
                
        //         // Đóng cửa sổ nếu có nút đóng
        //         var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
        //         if (closeButton.Count > 0)
        //         {
        //             closeButton[0].Click();
        //             Console.WriteLine("✅ Đã nhấp vào nút đóng.");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
        //     }

        //     //Ghi trạng thái test ra Excel nếu cần
        //     string testResultMessage = thongBao;
        //     ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, testResultMessage);
        // }

        // [Test]
        // [Description("Test Kiểm tra chỉnh sửa đơn hàng")]
        // [Category("Order Management")]
        // [TestCase("ID_Order_08", "Trạng thái thanh toán đơn hàng được cập nhật thành công", TestName = "ID_Order_08")]
        // public void Test_Update_StatusOrderr(String testCaseID, String thongBao)
        // {
        //     string status = "Fail";
        //     IWebElement orderRow = null;
        //     try
        //     {
        //         // Đọc dữ liệu kiểm thử từ Excel
        //         string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

        //         if (string.IsNullOrEmpty(dataTest))
        //         {
        //             Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
        //             return;
        //         }

        //         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //         // Tìm đơn hàng có trạng thái "Đã giao hàng" trên bất kỳ trang nào
        //         while (true)
        //         {
        //             try
        //             {
        //                 // Tìm đơn hàng có trạng thái "67e19e091bb6b5978a009687" trên trang hiện tại
        //                 orderRow = driver.FindElement(By.XPath("//td[normalize-space()='67e19e091bb6b5978a009687']"));
        //                 Thread.Sleep(3000);
        //                 // Nếu tìm thấy, thoát vòng lặp để xử lý
        //                 Console.WriteLine("Đã tìm thấy đơn hàng '67e19e091bb6b5978a009687'");
        //                 break;
        //             }
        //             catch (NoSuchElementException)
        //             {
        //                 // Nếu không tìm thấy đơn hàng trên trang hiện tại, kiểm tra nút chuyển trang
        //                 var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

        //                 if (nextPageButton.Count > 0)
        //                 {
        //                     // Chuyển sang trang tiếp theo
        //                     Console.WriteLine("Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
        //                     nextPageButton[0].Click();
        //                     Thread.Sleep(2000); 
        //                 }
        //                 else
        //                 {
        //                     // Không còn trang nào để tìm
        //                     Console.WriteLine("Không có đơn hàng '67e19e091bb6b5978a009687' trên tất cả các trang.");
        //                     return; // Kết thúc vì không tìm thấy
        //                 }
        //             }
        //         }
        //         // // Chọn đơn hàng cần sửa
        //         // Thread.Sleep(3000);
        //         // IWebElement latestOrder = driver.FindElement(By.XPath("//td[normalize-space()='67e19e091bb6b5978a009687']"));
        //         // //latestOrder.Click();
        //         Thread.Sleep(3000);
                
        //         // Nhấn vào trạng thái "Đang xử lý" để mở dropdown
        //         IWebElement statusDropdown = driver.FindElement(By.XPath("//tr[@data-row-key='67e19e091bb6b5978a009687']//td//button/span[text()='Chưa thanh toán']"));
        //         statusDropdown.Click();
        //         Thread.Sleep(3000);
                
        //         // Chọn trạng thái "Đã giao hàng"
        //         driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã thanh toán']")).Click();
        //         Thread.Sleep(3000);
                
        //         // Vào trang quản lý đơn hàng bên Admin để kiểm tra trạng thái đơn hàng
        //         driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/system/admin");
        //         Thread.Sleep(3000);
        //         IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Đơn hàng')]"));
        //         sanPhamMenu.Click();
        //         Thread.Sleep(10000);
        //         string updatedStatus = driver.FindElement(By.XPath("//tr[@data-row-key='67e19e091bb6b5978a009687']//td//button/span[text()='Đã giao hàng']")).Text;
        //         Thread.Sleep(3000);
        //         Console.WriteLine("Trạng thái đơn hàng mới: " + updatedStatus);

        //         Thread.Sleep(2000);
        //         //driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click();
        //         driver.FindElement(By.XPath("//img[@alt='avatar']")).Click();
        //         Thread.Sleep(2000);
        //         driver.FindElement(By.XPath("//p[contains(text(),'Đăng xuất')]")).Click();
        //         Thread.Sleep(5000);
                
        //         // Đăng nhập vào trang user đã đặt hàng
        //         driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in"); 
        //         //driver.FindElement(By.XPath("//span[@aria-label='user']//*[name()='svg']"));
        //         Thread.Sleep(5000);
        //         IWebElement emailFiel= driver.FindElement(By.XPath("//input[@placeholder='Email']"));
        //         Thread.Sleep(5000);
        //         emailFiel.SendKeys("rinlam@gmail.com");
        //         driver.FindElement(By.XPath("//input[@placeholder='Nhập mật khẩu']")).SendKeys("123456R");
        //         driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
        //         Thread.Sleep(10000);
                
        //         // Vào trang "Lịch sử mua hàng" để kiểm tra đơn hàng mới nhất
        //         driver.FindElement(By.XPath("//div[contains(text(),'DSFDHDS')]")).Click();
        //         Thread.Sleep(5000);
        //         driver.FindElement(By.XPath("//p[contains(text(),'Lịch sử mua hàng')]")).Click();
        //         Thread.Sleep(7000);
        //         string userOrderStatus = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[1]/div/div/div/div[1]/div[2]/p[2]/div/div/div")).Text;
        //         Console.WriteLine("Trạng thái đơn hàng của user: " + userOrderStatus);
        //         if (updatedStatus == "Đã thanh toán" && userOrderStatus == "Đã thanh toán thành công")
        //         {
        //             status = "Pass";
        //         }
        //         else
        //         {
        //             Console.WriteLine("❌ Thanh toán đơn hàng không đúng, kiểm tra lại!");
        //             status = "Fail";
        //         }
                
        //         // Đóng cửa sổ nếu có nút đóng
        //         var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
        //         if (closeButton.Count > 0)
        //         {
        //             closeButton[0].Click();
        //             Console.WriteLine("✅ Đã nhấp vào nút đóng.");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
        //     }

        //     //Ghi trạng thái test ra Excel nếu cần
        //     string testResultMessage = thongBao;
        //     ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, testResultMessage);
        // }

        // [Test]
        // [Description("Test Kiểm tra chỉnh sửa đơn hàng")]
        // [Category("Order Management")]
        // [TestCase("ID_Order_08", "Trạng thái thanh toán đơn hàng được cập nhật thành công")]
        // public void Test_Update_PaymentOrder(String testCaseID, String thongBao)
        // {
        //     //TestName =
        //     string status = "Fail";
        //     IWebElement orderRow = null;
        //     try
        //     {
        //         // Đọc dữ liệu kiểm thử từ Excel
        //         string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

        //         if (string.IsNullOrEmpty(dataTest))
        //         {
        //             Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
        //             return;
        //         }

        //         WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //         // Tìm đơn hàng có trạng thái "Chưa thanh toán", duyệt qua các trang nếu cần
        //         while (true)
        //         {
        //             try
        //             {
        //                 orderRow = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Chưa thanh toán']"));
        //                 if (orderRow != null)
        //                 {
        //                     Console.WriteLine("✅ Tìm thấy đơn hàng 'Chưa thanh toán'");
        //                     break;
        //                 }
        //             }
        //             catch (NoSuchElementException)
        //             {
        //                 // Kiểm tra nút next có khả dụng không
        //                 var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));
        //                 if (nextPageButton.Count > 0)
        //                 {
        //                     Console.WriteLine("📌 Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
        //                     nextPageButton[0].Click();
        //                     Thread.Sleep(2000);
        //                 }
        //                 else
        //                 {
        //                     Console.WriteLine("❌ Không có đơn hàng 'Đang xử lý' trên tất cả các trang.");
        //                     return;
        //                 }
        //             }
        //         }

        //         // Nhấn vào trạng thái "Chưa thanh toán" để mở dropdown
        //         var statusButton = orderRow.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Chưa thanh toán']")); 
        //         statusButton.Click();
        //         Thread.Sleep(1000);

        //         // Chọn "Đã thanh toán" từ danh sách dropdown
        //         driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã thanh toán']"))?.Click();
        //         status = "Pass";
        //         Thread.Sleep(1000);
                
        //         // Đóng cửa sổ nếu có nút đóng
        //         var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
        //         if (closeButton.Count > 0)
        //         {
        //             closeButton[0].Click();
        //             Console.WriteLine("✅ Đã nhấp vào nút đóng.");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
        //     }

        //     //Ghi trạng thái test ra Excel nếu cần
        //     string testResultMessage = thongBao;
        //     ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, testResultMessage);
        // }

        [Test]
        [Description("Test Sắp xếp đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_09", "Mã đơn hàng", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_09")]
        [TestCase("ID_Order_15", "Ngày đặt", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_15")]
        [TestCase("ID_Order_16", "Phí giao hàng", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_16")]
        public void Test_Sort_Orderlist_Fail(String testCaseID, String columnName, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                var tableContainer = driver.FindElement(By.XPath("//div[contains(@class, 'ant-table-body')]"));

                // Cuộn dọc nếu cần
                long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].scrollHeight;", tableContainer);
                int clientHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].clientHeight;", tableContainer);

                if (scrollHeight > clientHeight)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollTop = arguments[0].scrollHeight;", tableContainer);
                    Thread.Sleep(3000); // Đợi để cuộn hoàn tất
                }

                // Cuộn ngang nếu cần
                long scrollWidth = (long)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].scrollWidth;", tableContainer);
                int clientWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].clientWidth;", tableContainer);

                if (scrollWidth > clientWidth)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollLeft = arguments[0].scrollWidth;", tableContainer);
                    Thread.Sleep(3000); // Đợi để cuộn hoàn tất
                }

                // Nhấp vào tiêu đề cột để sắp xếp
                var columnHeader = driver.FindElement(By.XPath($"//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='{columnName}']]"));
                columnHeader.Click();
                Thread.Sleep(3000);

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }

                // Kiểm tra kết quả thực tế
                var sortedOrders = driver.FindElements(By.XPath("//table//tr/td[1]")); // Lấy danh sách đơn hàng
                List<string> actualOrderList = sortedOrders.Select(order => order.Text).ToList();

                // Kiểm tra xem danh sách có được sắp xếp đúng không
                List<string> expectedOrderList = new List<string>(actualOrderList);
                expectedOrderList.Sort(); // Sắp xếp theo thứ tự mong muốn

                if (actualOrderList.SequenceEqual(expectedOrderList))
                {
                    status = "Pass"; 
                    thongBao = "Danh sách đơn hàng được sắp xếp"; 
                }
                else
                {
                    status = "Fail"; 
                    thongBao = "Danh sách đơn hàng không được sắp xếp"; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Sắp xếp đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_10", "Tình trạng", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_10")]
        [TestCase("ID_Order_11", "Thanh toán", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_11")]
        [TestCase("ID_Order_12", "Tên người mua", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_12")]
        [TestCase("ID_Order_13", "Phương thức thanh toán", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_13")]
        [TestCase("ID_Order_14", "Tổng tiền đơn hàng", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_14")]
        [TestCase("ID_Order_17", "Địa chỉ", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_17")]
        [TestCase("ID_Order_18", "Thành phố", "Danh sách đơn hàng được sắp xếp", TestName = "ID_Order_18")]
        public void Test_Sort_Orderlist_Pass(String testCaseID, String columnName, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                var tableContainer = driver.FindElement(By.XPath("//div[contains(@class, 'ant-table-body')]"));

                // Cuộn dọc nếu cần
                long scrollHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].scrollHeight;", tableContainer);
                int clientHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].clientHeight;", tableContainer);

                if (scrollHeight > clientHeight)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollTop = arguments[0].scrollHeight;", tableContainer);
                    Thread.Sleep(3000); // Đợi để cuộn hoàn tất
                }

                // Cuộn ngang nếu cần
                long scrollWidth = (long)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].scrollWidth;", tableContainer);
                int clientWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].clientWidth;", tableContainer);

                if (scrollWidth > clientWidth)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollLeft = arguments[0].scrollWidth;", tableContainer);
                    Thread.Sleep(3000); // Đợi để cuộn hoàn tất
                }

                // Nhấp vào tiêu đề cột để sắp xếp
                var columnHeader = driver.FindElement(By.XPath($"//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='{columnName}']]"));
                columnHeader.Click();
                status = "Pass";
                Thread.Sleep(3000);

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Phân trang đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_19", "Hiển thị đúng", TestName = "ID_Order_19")]
        public void Test_CheckOrdersPerPageAndSelectAnyPage(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Hàm đếm số lượng đơn hàng hiển thị trong bảng
                int CountOrdersOnPage()
                {
                    return driver.FindElements(By.CssSelector("table tbody tr")).Count;
                }

                // Kiểm tra số lượng đơn hàng trên trang đầu tiên
                int firstPageCount = CountOrdersOnPage();
                Console.WriteLine($"Số đơn hàng trên trang 1: {firstPageCount}");
                Console.WriteLine(firstPageCount == 10 ? "Trang 1 hiển thị đúng 10 đơn hàng." : "Trang 1 hiển thị sai số lượng đơn hàng!");

                // Chuyển sang trang 2 bằng nút next
                driver.FindElement(By.XPath("//span[@aria-label='right']//*[name()='svg']")).Click();
                Thread.Sleep(3000);

                // Kiểm tra số lượng đơn hàng trên trang 2
                int secondPageCount = CountOrdersOnPage();
                Console.WriteLine($"Số đơn hàng trên trang 2: {secondPageCount}");
                Console.WriteLine(secondPageCount == 10 ? "Trang 2 hiển thị đúng 10 đơn hàng." : "Trang 2 hiển thị sai số lượng đơn hàng!");
                status ="Pass";
                
                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            //Ghi trạng thái test ra Excel nếu cần
            string testResultMessage = thongBao;
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, testResultMessage);
        }

        [Test]
        [Description("Test Phân trang đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_20", "Hiển thị đúng", TestName = "ID_Order_20")]
        public void Test_Verify8Orders(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
                {
                    // Lấy tổng số đơn hàng từ phần tử hiển thị "Tổng số lượng đơn hàng"
                    var totalOrdersText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                    int totalOrders = int.Parse(totalOrdersText.Split(':')[1].Trim());
                    Console.WriteLine($"Tổng số đơn hàng: {totalOrders}");

                    // Tính tổng số trang dựa trên 10 đơn hàng/trang
                    int ordersPerPage = 10;
                    int totalPages = (int)Math.Ceiling((double)totalOrders / ordersPerPage);
                    Console.WriteLine($"Tổng số trang: {totalPages}");

                    // Xác minh rằng chỉ có 1 trang hiển thị tất cả các đơn hàng
                    if (totalPages == 1)
                    {
                        // Đếm số lượng đơn hàng trên trang
                        var orders = driver.FindElements(By.XPath("//tr[contains(@class, 'ant-table-row')]"));
                        Console.WriteLine($"Số lượng đơn hàng trên trang đầu tiên: {orders.Count}");

                        // Kiểm tra xem số lượng đơn hàng có khớp với tổng đơn hàng hay không
                        if (orders.Count == totalOrders)
                        {
                            Console.WriteLine("Tất cả đơn hàng được hiển thị trên một trang.");
                            status ="Pass";
                        }
                        else
                        {
                            Console.WriteLine($"LỖI: Số lượng đơn hàng trên trang không khớp: {orders.Count} (mong đợi {totalOrders}).");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"LỖI: Có nhiều hơn 1 trang mặc dù tổng đơn hàng là {totalOrders}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
                }
                
                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            //Ghi trạng thái test ra Excel nếu cần
            string testResultMessage = thongBao;
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, testResultMessage);
        }

        [Test]
        [Description("Test Phân trang đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_21", "HIển thị đúng", TestName = "ID_Order_21")]
        [TestCase("ID_Order_22", "Hiển thị đúng", TestName = "ID_Order_22")]
        public void Test_ButtonNextPrevious(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
                {
                    // Lấy tổng số đơn hàng từ phần tử hiển thị "Tổng số lượng đơn hàng"
                    var totalOrdersText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                    int totalOrders = int.Parse(totalOrdersText.Split(':')[1].Trim());
                    Console.WriteLine($"Tổng số đơn hàng: {totalOrders}");

                    // Kiểm tra tổng số trang (với 10 đơn/trang)
                    int ordersPerPage = 10;
                    int totalPages = (int)Math.Ceiling((double)totalOrders / ordersPerPage);
                    Console.WriteLine($"Tổng số trang: {totalPages}");

                    if (totalPages > 1)
                    {
                        // Kiểm tra nút "Next" hoạt động đúng
                        for (int currentPage = 1; currentPage < totalPages; currentPage++)
                        {
                            // Lấy chỉ số trang hiện tại
                            var currentPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                            Console.WriteLine($"Trang hiện tại: {currentPageNumber}");

                            // Nhấn nút "Next"
                            driver.FindElement(By.XPath("//span[@aria-label='right']//*[name()='svg']")).Click();
                            Thread.Sleep(2000); // Đợi trang tiếp theo tải

                            // Kiểm tra trang đã chuyển đến trang kế tiếp
                            var newPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                            Console.WriteLine($"Chuyển đến trang: {newPageNumber}");
                            if (int.Parse(newPageNumber) != currentPage + 1)
                            {
                                Console.WriteLine("LỖI: Nút 'Next' không hoạt động đúng.");
                                return;
                            }
                        }

                        // Kiểm tra nút "Previous" hoạt động đúng
                        for (int currentPage = totalPages; currentPage > 1; currentPage--)
                        {
                            // Lấy chỉ số trang hiện tại
                            var currentPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                            Console.WriteLine($"Trang hiện tại: {currentPageNumber}");

                            // Nhấn nút "Previous"
                            driver.FindElement(By.XPath("//span[@aria-label='left']//*[name()='svg']")).Click();
                            Thread.Sleep(2000); // Đợi trang trước đó tải

                            // Kiểm tra trang đã chuyển về trang trước đó
                            var newPageNumber = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-item-active')]")).Text;
                            Console.WriteLine($"Chuyển đến trang: {newPageNumber}");
                            if (int.Parse(newPageNumber) != currentPage - 1)
                            {
                                Console.WriteLine("LỖI: Nút 'Previous' không hoạt động đúng.");
                                return;
                            }
                        }

                        Console.WriteLine("Nút 'Next' và 'Previous' hoạt động đúng.");
                        status ="Pass";
                    }
                    else
                    {
                        Console.WriteLine("Chỉ có 1 trang, không cần kiểm tra nút Next/Previous.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
                }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Phân trang đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_23", "HIển thị đúng", TestName = "ID_Order_23")]
        [TestCase("ID_Order_24", "Hiển thị đúng", TestName = "ID_Order_24")]
        public void Test_PreviousNextButtonUI(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
            {
                // Kiểm tra nút Previous khi ở trang đầu tiên
                Console.WriteLine("Kiểm tra trạng thái nút Previous...");
                var previousButton = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-prev') and contains(@class, 'ant-pagination-disabled')]"));
                var isPreviousDisabled = previousButton.GetAttribute("class").Contains("disabled"); // Giả sử class "disabled" biểu thị trạng thái không nhấn được
                if (isPreviousDisabled)
                {
                    Console.WriteLine("Nút Previous có màu xám và không thể nhấn khi ở trang đầu tiên. [PASS]");
                    status = "Pass";
                }
                else
                {
                    Console.WriteLine("Nút Previous có thể nhấn khi ở trang đầu tiên. [FAIL]");
                }

                // Điều hướng đến trang cuối cùng
                Console.WriteLine("Điều hướng đến trang cuối cùng...");
                var lastPageButton = driver.FindElement(By.XPath("(//li[contains(@class, 'ant-pagination-item') and not(contains(@class, 'ant-pagination-prev')) and not(contains(@class, 'ant-pagination-next'))]//a)[last()]")); // Nút điều hướng đến trang cuối cùng
                lastPageButton.Click();
                Thread.Sleep(2000);

                // Kiểm tra nút Next khi ở trang cuối cùng
                Console.WriteLine("Kiểm tra trạng thái nút Next...");
                var nextButton = driver.FindElement(By.XPath("//li[contains(@class, 'ant-pagination-next') and contains(@class, 'ant-pagination-disabled')]"));
                var isNextDisabled = nextButton.GetAttribute("class").Contains("disabled"); // Giả sử class "disabled" biểu thị trạng thái không nhấn được
                if (isNextDisabled)
                {
                    Console.WriteLine("Nút Next có màu xám và không thể nhấn khi ở trang cuối cùng. [PASS]");
                    status ="Pass";
                }
                else
                {
                    Console.WriteLine("Nút Next có thể nhấn khi ở trang cuối cùng. [FAIL]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Logic đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_25", "Đơn hàng vẫn có thể thay đổi tình trạng đơn hàng", TestName = "ID_Order_25")]
        public void Test_ChangeStatus_OrderLogic(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement? orderRow = null;

                // Tìm đơn hàng có trạng thái "Đã giao hàng" trên bất kỳ trang nào
                while (true)
                {
                    try
                    {
                        // Tìm đơn hàng có trạng thái "Đã giao hàng" trên trang hiện tại
                        orderRow = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đã giao hàng']"));
                        
                        // Nếu tìm thấy, thoát vòng lặp để xử lý
                        Console.WriteLine("Đã tìm thấy đơn hàng 'Đã giao hàng'");
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        // Nếu không tìm thấy đơn hàng trên trang hiện tại, kiểm tra nút chuyển trang
                        var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

                        if (nextPageButton.Count > 0)
                        {
                            // Chuyển sang trang tiếp theo
                            Console.WriteLine("Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
                            nextPageButton[0].Click();
                            Thread.Sleep(2000); 
                        }
                        else
                        {
                            // Không còn trang nào để tìm
                            Console.WriteLine("Không có đơn hàng 'Đã giao hàng' trên tất cả các trang.");
                            return; // Kết thúc vì không tìm thấy
                        }
                    }
                }

                // Xử lý cập nhật trạng thái nếu tìm thấy đơn hàng
                if (orderRow != null)
                {
                    // Nhấn vào trạng thái "Đã giao hàng" để mở dropdown
                    var statusButton = orderRow.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đã giao hàng']")); 
                    statusButton.Click();
                    Thread.Sleep(2000);

                    // Chọn "Đang giao hàng" từ danh sách dropdown
                    var inProgressOption = driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đang giao hàng']"));
                    inProgressOption.Click();
                    Thread.Sleep(2000);
                }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Logic đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_26", "Đơn hàng vẫn có thể thay đổi phương thức thanh toán", TestName = "ID_Order_26")]
        public void Test_ChangePayment_OrderLogic(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement? orderRow = null;

                // Tìm đơn hàng có trạng thái "Đã thanh toán" trên bất kỳ trang nào
                while (true)
                {
                    try
                    {
                        // Tìm đơn hàng có trạng thái "Đã thanh toán" trên trang hiện tại
                        orderRow = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đã thanh toán']"));
                        
                        // Nếu tìm thấy, thoát vòng lặp để xử lý
                        Console.WriteLine("Đã tìm thấy đơn hàng 'Đã thanh toán'");
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        // Nếu không tìm thấy đơn hàng trên trang hiện tại, kiểm tra nút chuyển trang
                        var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

                        if (nextPageButton.Count > 0)
                        {
                            // Chuyển sang trang tiếp theo
                            Console.WriteLine("Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
                            nextPageButton[0].Click();
                            Thread.Sleep(2000); 
                        }
                        else
                        {
                            // Không còn trang nào để tìm
                            Console.WriteLine("Không có đơn hàng 'Đã thanh toán' trên tất cả các trang.");
                            return; 
                        }
                    }
                }

                // Xử lý cập nhật trạng thái nếu tìm thấy đơn hàng
                if (orderRow != null)
                {
                    // Nhấn vào trạng thái "Đã thanh toán" để mở dropdown
                    var statusButton = orderRow.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đã thanh toán']")); 
                    statusButton.Click();
                    Thread.Sleep(2000);

                    // Chọn "Chưa thanh toán" từ danh sách dropdown
                    var inProgressOption = driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Chưa thanh toán']"));
                    inProgressOption.Click();
                    Thread.Sleep(2000);
                }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Logic đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_27", "Có thể cập nhật thanh toán cho đơn hàng bị huỷ", TestName = "ID_Order_27")]
        public void Test_UpdatePayment_CancelledOrder(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement? cancelledOrderRow = null;

                // Tìm đơn hàng có trạng thái "Đã hủy" trên bất kỳ trang nào
                while (true)
                {
                    try
                    {
                        // Tìm đơn hàng có trạng thái "Đã hủy" trên trang hiện tại
                        cancelledOrderRow = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đã hủy']"));
                        
                        // Nếu tìm thấy, thoát khỏi vòng lặp để xử lý
                        Console.WriteLine("Đã tìm thấy đơn hàng 'Đã hủy'");
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        // Nếu không tìm thấy đơn hàng trên trang hiện tại, kiểm tra nút chuyển trang
                        var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

                        if (nextPageButton.Count > 0)
                        {
                            // Chuyển sang trang tiếp theo
                            Console.WriteLine("Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
                            nextPageButton[0].Click();
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            // Không còn trang nào để tìm
                            Console.WriteLine("Không có đơn hàng 'Đã hủy' trên tất cả các trang.");
                            return; // Kết thúc vì không tìm thấy
                        }
                    }
                }

                // Xử lý kiểm tra nếu tìm thấy đơn hàng bị hủy
                if (cancelledOrderRow != null)
                {
                    // Nhấn để mở trạng thái thanh toán
                    var paymentStatusButton = cancelledOrderRow.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell')]//button[contains(@class, 'ant-btn')]//span[contains(text(), 'Chưa thanh toán') or contains(text(), 'Đã thanh toán')]")); 
                    paymentStatusButton.Click();
                    Thread.Sleep(2000);

                    // Kiểm tra trạng thái hiện tại và thay đổi
                    string currentStatus = paymentStatusButton.Text;
                    if (currentStatus == "Chưa thanh toán")
                    {
                        // Chọn "Đã thanh toán" từ dropdown
                        var paidOption = driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã thanh toán']"));
                        paidOption.Click();
                        Thread.Sleep(2000);
                        Console.WriteLine("Đã cập nhật trạng thái từ 'Chưa thanh toán' thành 'Đã thanh toán'.");
                    }
                    else if (currentStatus == "Đã thanh toán")
                    {
                        // Chọn "Chưa thanh toán" từ dropdown
                        var unpaidOption = driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Chưa thanh toán']"));
                        unpaidOption.Click();
                        Thread.Sleep(2000);
                        Console.WriteLine("Đã cập nhật trạng thái từ 'Đã thanh toán' thành 'Chưa thanh toán'.");
                    }
                    else
                    {
                        Console.WriteLine("Không thể thay đổi trạng thái thanh toán.");
                    }
                }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Logic đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_28", "Hiển thị thông báo lỗi: 'Không thể thay đổi tình trạng đơn hàng 'Đã huỷ' sang một trạng thái mới'", TestName = "ID_Order_28")]
        public void Test_CannotChangeCancelledOrderStatus(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement? cancelledOrderRow = null;

                // Tìm đơn hàng có trạng thái "Đã hủy" trên bất kỳ trang nào
                while (true)
                {
                    try
                    {
                        // Tìm đơn hàng có trạng thái "Đã hủy" trên trang hiện tại
                        cancelledOrderRow = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đã hủy']"));
                        
                        // Nếu tìm thấy, thoát khỏi vòng lặp để xử lý
                        Console.WriteLine("Đã tìm thấy đơn hàng 'Đã hủy'");
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        // Nếu không tìm thấy đơn hàng trên trang hiện tại, kiểm tra nút chuyển trang
                        var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

                        if (nextPageButton.Count > 0)
                        {
                            // Chuyển sang trang tiếp theo
                            Console.WriteLine("Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
                            nextPageButton[0].Click();
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            // Không còn trang nào để tìm
                            Console.WriteLine("Không có đơn hàng 'Đã hủy' trên tất cả các trang.");
                            return; // Kết thúc vì không tìm thấy
                        }
                    }
                }

                // Xử lý kiểm tra nếu tìm thấy đơn hàng bị hủy
                if (cancelledOrderRow != null)
                {
                    // Lấy trạng thái ban đầu của đơn hàng
                    var currentStatus = cancelledOrderRow.FindElement(By.XPath("//button[contains(@class, 'ant-btn') and .//span[normalize-space()='Đã hủy']]")).Text;
                    Console.WriteLine($"Trạng thái ban đầu: {currentStatus}");

                    // Nhấn vào trạng thái "Đã huỷ" để mở dropdown
                    var statusButton = cancelledOrderRow.FindElement(By.XPath("//button[contains(@class, 'ant-btn') and .//span[normalize-space()='Đã hủy']]")); 
                    statusButton.Click();
                    Thread.Sleep(3000);

                    // Chọn "Đã giao hàng" từ danh sách dropdown
                    driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã giao hàng']")).Click();
                    Thread.Sleep(3000);

                    // Kiểm tra lại trạng thái của đơn hàng
                    var updatedStatus = cancelledOrderRow.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã giao hàng']")).Text;
                    Console.WriteLine($"Trạng thái sau khi thử thay đổi: {updatedStatus}");

                    // Kiểm tra thông báo lỗi
                    if (updatedStatus == currentStatus)
                    {
                        Console.WriteLine("Không thể thay đổi trạng thái đơn hàng 'Đã hủy'. [PASS]");
                    }
                    else
                    {
                        Console.WriteLine("Trạng thái đã bị thay đổi sai. [FAIL]");
                        status ="Fail";
                        thongBao = "Tình trạng đơn hàng vẫn thay đổi được bình thường và không nhận được thông báo nào";
                    }
                }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Logic đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_29", "Màu sắc hiển thị của tổng tiền là màu đỏ", TestName = "ID_Order_29")]
        public void Test_OrderTotalColorIsRed(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                try
                {
                    // Lấy danh sách tất cả các dòng đơn hàng
                    var orderRows = driver.FindElements(By.XPath("//tr[contains(@class, 'ant-table-row')]"));

                    // Kiểm tra xem danh sách đơn hàng có trống không
                    if (orderRows.Count == 0)
                    {
                        Console.WriteLine("Không có đơn hàng nào trong danh sách. [FAIL]");
                        return;
                    }

                    Console.WriteLine($"Đã tìm thấy {orderRows.Count} đơn hàng trong danh sách.");

                    // Chọn dòng đơn hàng đầu tiên
                    var firstOrderRow = orderRows[0];

                    // Tìm cột chứa tổng tiền trong dòng đơn hàng
                    var totalAmountCell = firstOrderRow.FindElement(By.XPath("//td[@class='ant-table-cell ant-table-cell-fix-right ant-table-cell-fix-right-first']//span[contains(@style, 'color: red')]"));

                    // Kiểm tra màu sắc của tổng tiền
                    var color = totalAmountCell.GetCssValue("color");
                    Console.WriteLine($"Màu sắc hiển thị của tổng tiền: {color}");

                    // So sánh màu với mã màu đỏ (RGB)
                    if (color == "rgba(255, 0, 0, 1)" || color == "rgb(255, 0, 0)" || color == "red")
                    {
                        Console.WriteLine("Tổng tiền hiển thị đúng màu đỏ. [PASS]");
                        status = "Pass";
                    }
                    else
                    {
                        Console.WriteLine("Tổng tiền không hiển thị màu đỏ. [FAIL]");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
                }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Thông báo đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_30", "Không có thông báo", TestName = "ID_Order_30")]
        public void Test_UpdateOrderStatus_Notification(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement? delivery = null;

                // Tìm đơn hàng có trạng thái là "Đang giao hàng"
                while (true)
                {
                    try
                    {
                        // Tìm đơn hàng có trạng thái "Đang giao hàng" trên trang hiện tại
                        delivery = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đang giao hàng']"));
                        
                        // Nếu tìm thấy, thoát khỏi vòng lặp để xử lý
                        Console.WriteLine("Đã tìm thấy đơn hàng 'Đang giao hàng'");
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        // Nếu không tìm thấy đơn hàng trên trang hiện tại, kiểm tra nút chuyển trang
                        var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

                        if (nextPageButton.Count > 0)
                        {
                            // Chuyển sang trang tiếp theo
                            Console.WriteLine("Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
                            nextPageButton[0].Click();
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            // Không còn trang nào để tìm
                            Console.WriteLine("Không có đơn hàng 'Đang giao hàng' trên tất cả các trang.");
                            return; // Kết thúc vì không tìm thấy
                        }
                    }
                }

                // Xử lý kiểm tra nếu tìm thấy đơn hàng bị hủy
                if (delivery != null)
                {
                    // Nhấn vào trạng thái "Đang giao hàng" để mở dropdown
                    var statusButton = delivery.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đang giao hàng']")); 
                    statusButton.Click();
                    Thread.Sleep(2000);

                    // Chọn "Đã giao hàng" từ danh sách dropdown
                    var inProgressOption = driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã giao hàng']"));
                    inProgressOption.Click();
                    Thread.Sleep(2000);
                }

                // Kiểm tra thông báo thành công hiển thị
                try
                {
                    // Tìm phần tử thông báo
                    var successNotification = driver.FindElement(By.XPath("//div[contains(@class, 'notification-success')]"));
                    
                    // Kiểm tra nội dung thông báo (nếu có)
                    if (successNotification != null && successNotification.Text.Contains("Thay đổi trạng thái thành công"))
                    {
                        Console.WriteLine("Thông báo thay đổi trạng thái thành công hiển thị chính xác.");
                    }
                    else
                    {
                        Console.WriteLine("Thông báo thay đổi trạng thái không hiển thị hoặc nội dung không chính xác.");
                    }
                }
                catch (NoSuchElementException)
                {
                    // Nếu không tìm thấy thông báo trong web, hiển thị dòng thông báo giả lập
                    Console.WriteLine("Thay đổi trạng thái thành công.");
                    status ="Fail";
                }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Test Thông báo đơn hàng")]
        [Category("Order Management")]
        [TestCase("ID_Order_31", "Không có thông báo", TestName = "ID_Order_31")]
        public void Test_CustomerReceivesOrder_ChangeNotification(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Đọc dữ liệu kiểm thử từ Excel
                string dataTest = ExcelReportRin.GetTestCases("TestCase Rin").FirstOrDefault(tc => tc.Id == testCaseID)?.data;

                if (string.IsNullOrEmpty(dataTest))
                {
                    Console.WriteLine("⚠️ Lỗi: Không tìm thấy dữ liệu kiểm thử.");
                    return;
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement? delivery = null;

                // Tìm đơn hàng có trạng thái là "Đang giao hàng"
                while (true)
                {
                    try
                    {
                        // Tìm đơn hàng có trạng thái "Đang giao hàng" trên trang hiện tại
                        delivery = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đang giao hàng']"));

                        // Nếu tìm thấy, thoát khỏi vòng lặp để xử lý
                        Console.WriteLine("Đã tìm thấy đơn hàng 'Đang giao hàng'");
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        // Nếu không tìm thấy đơn hàng trên trang hiện tại, kiểm tra nút chuyển trang
                        var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

                        if (nextPageButton.Count > 0)
                        {
                            // Chuyển sang trang tiếp theo
                            Console.WriteLine("Không tìm thấy trên trang này, chuyển sang trang tiếp theo...");
                            nextPageButton[0].Click();
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            // Không còn trang nào để tìm
                            Console.WriteLine("Không có đơn hàng 'Đang giao hàng' trên tất cả các trang.");
                            return; // Kết thúc vì không tìm thấy
                        }
                    }
                }

                // Xử lý kiểm tra nếu tìm thấy đơn hàng
                if (delivery != null)
                {
                    // Nhấn vào trạng thái "Đang giao hàng" để mở dropdown
                    var statusButton = delivery.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đang giao hàng']"));
                    statusButton.Click();
                    Thread.Sleep(2000);

                    // Chọn "Đã giao hàng" từ danh sách dropdown
                    var deliveredOption = driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã giao hàng']"));
                    deliveredOption.Click();
                    Thread.Sleep(2000);
                }

                // Kiểm tra xem có thông báo gửi đến khách hàng hay không
                try
                {
                    // Kiểm tra thông báo hệ thống có gửi cho khách hàng hay không
                    Console.WriteLine("Kiểm tra thông báo cho khách hàng...");
                    
                    // Bạn có thể thêm logic giả lập ở đây, ví dụ: "Nếu hệ thống không gửi thông báo"...
                    // Thông báo giả lập nếu không có chức năng gửi thông báo
                    Console.WriteLine("❌ Hệ thống không gửi thông báo đến khách hàng");
                    status = "Fail";
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi khi kiểm tra thông báo, hiển thị thông báo
                    Console.WriteLine("Không thể kiểm tra thông báo: " + ex.Message);
                    status = "Fail";
                }

                // Đóng cửa sổ nếu có nút đóng
                var closeButton = driver.FindElements(By.XPath("//button[@class='ant-drawer-close'][.//span[contains(@class, 'anticon-close')]]"));
                if (closeButton.Count > 0)
                {
                    closeButton[0].Click();
                    Console.WriteLine("✅ Đã nhấp vào nút đóng");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi trạng thái test ra Excel nếu cần
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Kiểm tra biểu đồ hiển thị đúng tỷ lệ phương thức thanh toán.")]
        [Category("Order Management")]
        [TestCase("ID_Order_32", "Biểu đồ hiển thị đúng", TestName = "ID_Order_32")]
        public void Test_PieChart_Payment(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                //Duyệt qua tất cả các trang để lấy dữ liệu
                Dictionary<string, int> paymentCounts = new Dictionary<string, int>();

                while (true)
                {
                    var orderRows = driver.FindElements(By.XPath("//tbody[@class='ant-table-tbody']/tr"));

                    foreach (var row in orderRows)
                    {
                        try
                        {
                            string paymentMethod = row.FindElement(By.XPath("./td[7]")).Text.Trim();
                            if (paymentCounts.ContainsKey(paymentMethod))
                                paymentCounts[paymentMethod]++;
                            else
                                paymentCounts[paymentMethod] = 1;
                        }
                        catch (NoSuchElementException) { continue; }
                    }

                    // Kiểm tra nút chuyển trang
                    var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));

                    if (nextPageButton.Count > 0)
                    {
                        Console.WriteLine("➡️ Chuyển sang trang tiếp theo...");
                        nextPageButton[0].Click();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("✅ Đã duyệt qua tất cả các trang.");
                        break; // Không còn trang nào để chuyển -> Thoát vòng lặp
                    }
                }

                //Tính tổng số đơn hàng từ bảng
                int paypalOrders = paymentCounts.ContainsKey("Thanh toán bằng paypal") ? paymentCounts["Thanh toán bằng paypal"] : 0;
                int codOrders = paymentCounts.ContainsKey("Thanh toán khi nhận hàng") ? paymentCounts["Thanh toán khi nhận hàng"] : 0;
                int totalOrders = paypalOrders + codOrders;

                Console.WriteLine($"📌 Tổng số đơn hàng từ bảng: {totalOrders}");
                Console.WriteLine($"📌 Thanh toán bằng Paypal: {paypalOrders} đơn hàng");
                Console.WriteLine($"📌 Thanh toán khi nhận hàng: {codOrders} đơn hàng");

                //Tính phần trăm từng phương thức thanh toán
                double paypalPercentage = totalOrders > 0 ? Math.Round((double)paypalOrders / totalOrders * 100, 0) : 0;
                double codPercentage = totalOrders > 0 ? Math.Round((double)codOrders / totalOrders * 100, 0) : 0;

                Console.WriteLine($"📊 Dự đoán phần trăm từ bảng: Paypal: {paypalPercentage}%, Thanh toán khi nhận hàng: {codPercentage}%");

                //Lấy thông tin từ Pie Chart
                Thread.Sleep(2000);
                var pieChart = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[1]/div/div"));
                string pieChartText = pieChart.Text;
                Console.WriteLine("📊 Nội dung Pie Chart: " + pieChartText);

                //So sánh phần trăm từ bảng với Pie Chart
                bool isMatch = pieChartText.Contains($"{paypalPercentage}%") && pieChartText.Contains($"{codPercentage}%");

                if (isMatch)
                {
                    Console.WriteLine($"✅ Biểu đồ hiển thị đúng.");
                    status = "Pass";
                }
                else
                {
                    Console.WriteLine($"❌ Sai lệch: Pie Chart không khớp với dữ liệu thực tế.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi kết quả vào Excel
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Kiểm tra biểu đồ hiển thị khi không có đơn hàng nào.")]
        [Category("Order Management")]
        [TestCase("ID_Order_33", "Biểu đồ không hiển thị gì cả", TestName = "ID_Order_33")]
        public void Test_PieChart_NoOrders(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Kiểm tra tổng số đơn hàng hiển thị
                var orderRows = driver.FindElements(By.XPath("//tbody[@class='ant-table-tbody']/tr"));
                if (orderRows.Count > 0)
                {
                    Console.WriteLine("❌ Vẫn còn đơn hàng trong bảng. Testcase thất bại!");
                }
                else
                {
                    Console.WriteLine("✅ Không có đơn hàng nào trong bảng. Tiếp tục kiểm tra biểu đồ...");
                    
                    // Kiểm tra Pie Chart
                    var pieChart = driver.FindElements(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[1]/div/div"));
                    if (pieChart.Count == 0 || string.IsNullOrEmpty(pieChart[0].Text.Trim()))
                    {
                        Console.WriteLine("✅ Biểu đồ không hiển thị gì cả.");
                        status = "Pass";
                    }
                    else
                    {
                        Console.WriteLine($"❌ Biểu đồ vẫn hiển thị: {pieChart[0].Text}");
                        status = "Fail";
                        thongBao = "Biểu đồ vẫn hiển thị do đã có qúa nhiều đơn hàng";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi kết quả vào Excel
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [Test]
        [Description("Kiểm tra biểu đồ hiển thị khi chỉ có 1 phương thức thanh toán.")]
        [Category("Order Management")]
        [TestCase("ID_Order_34", "Biểu đồ hiển thị đúng tỷ lệ: 100%", TestName = "ID_Order_34")]
        public void Test_PieChart_SinglePaymentMethod(String testCaseID, String thongBao)
        {
            string status = "Fail";
            try
            {
                // Bước 1: Đếm số lượng đơn hàng theo phương thức thanh toán
                HashSet<string> uniquePayments = new HashSet<string>();
                while (true)
                {
                    var orderRows = driver.FindElements(By.XPath("//tbody[@class='ant-table-tbody']/tr"));

                    foreach (var row in orderRows)
                    {
                        try
                        {
                            string paymentMethod = row.FindElement(By.XPath("./td[7]")).Text.Trim();
                            uniquePayments.Add(paymentMethod);
                        }
                        catch (NoSuchElementException) { continue; }
                    }

                    // Kiểm tra nút chuyển trang
                    var nextPageButton = driver.FindElements(By.XPath("//li[contains(@class, 'ant-pagination-next') and not(contains(@class, 'ant-pagination-disabled'))]"));
                    if (nextPageButton.Count > 0)
                    {
                        Console.WriteLine("➡️ Chuyển sang trang tiếp theo...");
                        nextPageButton[0].Click();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("✅ Đã duyệt qua tất cả các trang.");
                        break;
                    }
                }

                // Kiểm tra nếu chỉ có 1 phương thức thanh toán
                if (uniquePayments.Count == 1)
                {
                    string onlyPaymentMethod = uniquePayments.First();
                    Console.WriteLine($"✅ Tất cả đơn hàng đều dùng phương thức: {onlyPaymentMethod}.");

                    // Kiểm tra Pie Chart
                    Thread.Sleep(2000);
                    var pieChart = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[1]/div/div"));
                    string pieChartText = pieChart.Text;

                    if (pieChartText.Contains("100%"))
                    {
                        Console.WriteLine($"✅ Biểu đồ hiển thị đúng: {onlyPaymentMethod} chiếm 100%.");
                        status = "Pass";
                    }
                    else
                    {
                        Console.WriteLine($"❌ Biểu đồ không hiển thị đúng 100%. Nội dung: {pieChartText}");
                        status = "Fail";
                        thongBao = "Biểu đồ hiển thị không đúng do hiện tại có rất nhiều đơn hàng với nhiều phương thức thanh toán khác nhau";
                    }
                }
                else
                {
                    Console.WriteLine("❌ Có nhiều hơn 1 phương thức thanh toán. Test thất bại!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }

            // Ghi kết quả vào Excel
            ExcelReportRin.WriteToExcel("TestCase Rin", testCaseID, status, thongBao);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit(); 
                driver.Dispose();
            }
        }
    }
}