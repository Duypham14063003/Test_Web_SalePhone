//using test_salephone.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;

namespace test_salephone.Tests
{
    [TestFixture]
    public class Test_Order
    {
        private IWebDriver driver;
        //private string baseURL = "https://frontend-salephones.vercel.app/";


        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/");
            driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(1000);
        }

        [Test]
        public void Test_Login_Success()
        {
            driver.FindElement(By.XPath("//div[@class='ant-row sc-dgjgUn ittrMP css-qnu6hi']//div[4]//div[1]")).Click();
            Thread.Sleep(2000);
            
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");
            Console.WriteLine("Current URL: " + driver.Url);


            driver.FindElement(By.XPath("//input[@placeholder='Email']")).SendKeys("sela@gmail.com");
            driver.FindElement(By.CssSelector("input[placeholder='Nhập mật khẩu']")).SendKeys("123456");

            driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
            Thread.Sleep(2000);

        }

        [Test]
        public void Test_ViewOrder()
        {   
            Test_Login_Success();
            Thread.Sleep(3000);

            Actions action = new Actions(driver);
            IWebElement avatar = driver.FindElement(By.XPath("//img[@alt='avatar']"));
            action.ClickAndHold(avatar).Perform(); // Click và giữ
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click();;
            Thread.Sleep(2000);
            IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Đơn hàng')]"));
            sanPhamMenu.Click();
            Thread.Sleep(6000);
        }

        [Test]
        public void Test_ViewOrderDetails()
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Chọn chi tiết đơn hàng đầu tiên
            driver.FindElement(By.XPath("//tr[td[normalize-space()='67cd30de61ca00c79e87e675']]//button[@aria-label='Expand row']")).Click();
            Thread.Sleep(2000);

        }

        [Test]
        public void Test_Sort_Orderlist_Status()
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left') and .//span[text()='Tình trạng']]")).Click();
            Thread.Sleep(3000); 

            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left') and .//span[text()='Tình trạng']]")).Click();
            Thread.Sleep(3000); 

            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left') and .//span[text()='Tình trạng']]")).Click();
            Thread.Sleep(3000); 
        }

        [Test]
        public void Test_Sort_Orderlist_Payment()
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left') and .//span[text()='Thanh toán']]")).Click();
            Thread.Sleep(3000); 
        }

        [Test]
        public void Test_Sort_Orderlist_Name()
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Nhấp vào tiêu đề cột "Tên người mua" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tên người mua']]")).Click();
            Thread.Sleep(3000); 
        }

        [Test]
        public void Test_Sort_Orderlist_PaymentMethod()
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Nhấp vào tiêu đề cột "Thanh toán" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Phương thức thanh toán']]")).Click();
            Thread.Sleep(3000); 
        }

        [Test]
        public void Test_Sort_Orderlist_TotalOrder()
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Nhấp vào tiêu đề cột "Thanh toán" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tổng tiền đơn hàng']]")).Click();
            Thread.Sleep(3000); 
        }

        [Test]
        public void Test_VerifyOrderQuantity()
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            
            var orderRows = driver.FindElements(By.CssSelector("table tbody tr"));
            if (orderRows.Count > 0)
            {
                Console.WriteLine($" Hệ thống có {orderRows.Count} đơn hàng.");
            }
            else
            {
                Console.WriteLine(" Không có đơn hàng nào trong hệ thống.");
            }

        }

        [Test] 
        public void Test_CheckOrdersPerPage()
        {
            Test_ViewOrder();
            // Hàm đếm số lượng đơn hàng hiển thị trong bảng
            int CountOrdersOnPage()
            {
                return driver.FindElements(By.CssSelector("table tbody tr")).Count;
            }

            // Kiểm tra số lượng đơn trên trang đầu tiên
            int firstPageCount = CountOrdersOnPage();
            Console.WriteLine($"Số đơn hàng trên trang 1: {firstPageCount}");
            Console.WriteLine(firstPageCount == 10 ? "Trang 1 hiển thị đúng 10 đơn hàng." : "Trang 1 hiển thị sai số lượng đơn hàng!");

            // Chuyển sang trang 2
            driver.FindElement(By.XPath("//span[@aria-label='right']//*[name()='svg']")).Click();
            Thread.Sleep(3000); 

            // Kiểm tra số lượng đơn trên trang 2
            int secondPageCount = CountOrdersOnPage();
            Console.WriteLine($"Số đơn hàng trên trang 2: {secondPageCount}");
            Console.WriteLine(secondPageCount == 10 ? "Trang 2 hiển thị đúng 10 đơn hàng." : "Trang 2 hiển thị sai số lượng đơn hàng!");
        }

        [Test] 
        public void Test_SelectAnyPage()
        {
            Test_ViewOrder();

            // Chuyển sang trang số 3
            driver.FindElement(By.XPath("//a[normalize-space()='3']")).Click();
            Thread.Sleep(3000);

            // Kiểm tra số lượng đơn hàng trên trang 3
            int ordersOnPage3 = driver.FindElements(By.CssSelector("table tbody tr")).Count;
            Console.WriteLine($"Số đơn hàng trên trang 3: {ordersOnPage3}");
            Console.WriteLine(ordersOnPage3 == 10 ? "Trang 3 hiển thị đúng 10 đơn hàng." : "Trang 3 hiển thị sai số lượng đơn hàng!");
        }

        [Test]
        public void Test_Verify8Orders()
        {
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

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
        }

        [Test]
        public void Test_ButtonNextPrevious() 
        {
            Test_ViewOrder();

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
        }

        [Test]
        public void Test_ManyproductsinOrder() 
        {
            Test_ViewOrder();

            // Chọn đơn hàng đầu tiên để mở chi tiết
            driver.FindElement(By.XPath("//tr[td[normalize-space()='67cd7a2a61ca00c79e87fe44']]//button[@aria-label='Expand row']")).Click();
            Thread.Sleep(3000); 

            // Lấy danh sách sản phẩm trong chi tiết đơn hàng
            var productElements = driver.FindElements(By.CssSelector(".order-details .product-list .product-item"));
            List<string> displayedProducts = productElements.Select(el => el.Text.Trim()).ToList();

            // In danh sách sản phẩm ra console
            Console.WriteLine("Danh sách sản phẩm hiển thị trong đơn hàng:");
            displayedProducts.ForEach(product => Console.WriteLine($"- {product}"));

            // Kiểm tra danh sách không rỗng
            if (displayedProducts.Count > 0)
            {
                Console.WriteLine("Danh sách sản phẩm hiển thị đúng.");
            }
            else
            {
                Console.WriteLine("LỖI: Không có sản phẩm nào hiển thị trong đơn hàng!");
            }
        }

        [Test]
        public void Test_Change_StatusOrder() 
        {
            Test_ViewOrder();

            // Tìm đơn hàng có trạng thái "Đang xử lý"
            var orderRow = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đang xử lý']"));
            if (orderRow == null)
            {
                Console.WriteLine("Không có đơn hàng 'Đang xử lý'");
                return;
            }

            // Nhấn vào trạng thái "Đang xử lý" để mở dropdown
            var statusButton = orderRow.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Đang xử lý']")); 
            statusButton.Click();
            Thread.Sleep(1000);

            // Chọn "Đã giao hàng" từ danh sách dropdown
            driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã giao hàng']")).Click();
            Thread.Sleep(1000);
        }

        [Test]
        public void Test_Change_PaymentOrder() 
        {
            Test_ViewOrder();

            // Tìm đơn hàng có trạng thái "Chưa thanh toán"
            var orderRow = driver.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Chưa thanh toán']"));
            if (orderRow == null)
            {
                Console.WriteLine("Không có đơn hàng 'Chưa thanh toán'");
                return;
            }

            // Nhấn vào trạng thái "Chưa thanh toán" để mở dropdown
            var statusButton = orderRow.FindElement(By.XPath("//td[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left')]//button[contains(@class, 'ant-btn')]//span[text()='Chưa thanh toán']")); 
            statusButton.Click();
            Thread.Sleep(2000);

            // Chọn "Đã thanh toán" từ danh sách dropdown
            driver.FindElement(By.XPath("//li[contains(@class, 'ant-dropdown-menu-item') and contains(@class, 'ant-dropdown-menu-item-only-child') and @role='menuitem']//span[text()='Đã thanh toán']")).Click();
            Thread.Sleep(2000);
        }


        
        [TearDown]
        public void CleanUp()
        {
            driver.Dispose();
            driver.Quit();
        }
    }
}

