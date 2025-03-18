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
        public void Test_Login_Success()//01
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
        public void Test_ViewOrder()//02 và 03
        {   
            Test_Login_Success();
            Thread.Sleep(3000);

            Actions action = new Actions(driver);
            IWebElement avatar = driver.FindElement(By.XPath("//img[@alt='avatar']"));
            action.ClickAndHold(avatar).Perform(); 
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click();;
            Thread.Sleep(2000);
            IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[contains(text(),'Đơn hàng')]"));
            sanPhamMenu.Click();
            Thread.Sleep(6000);
        }

        [Test]
        public void Test_ViewOrderDetails() //04
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Chọn chi tiết đơn hàng đầu tiên
            driver.FindElement(By.XPath("//tr[td[normalize-space()='67cd30de61ca00c79e87e675']]//button[@aria-label='Expand row']")).Click();
            Thread.Sleep(2000);

        }

        [Test]
        public void Test_InValidLogin()//05
        {
            driver.FindElement(By.XPath("//div[@class='ant-row sc-dgjgUn ittrMP css-qnu6hi']//div[4]//div[1]")).Click();
            Thread.Sleep(2000);
            
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");
            Console.WriteLine("Current URL: " + driver.Url);


            driver.FindElement(By.XPath("//input[@placeholder='Email']")).SendKeys("sela@gmail.com");
            driver.FindElement(By.CssSelector("input[placeholder='Nhập mật khẩu']")).SendKeys("1234");

            driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
            Thread.Sleep(2000);

            var errorMessage = driver.FindElement(By.XPath("//span[contains(text(),'Email hoặc Mật khẩu sai!')]")).Text;
            Console.WriteLine("Thông báo lỗi: " + errorMessage);
        }

        [Test]
        public void Test_NoAccount()//06
        {
            driver.FindElement(By.XPath("//div[@class='ant-row sc-dgjgUn ittrMP css-qnu6hi']//div[4]//div[1]")).Click();
            Thread.Sleep(2000);
            
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");
            Console.WriteLine("Current URL: " + driver.Url);

            driver.FindElement(By.XPath("//button[.//span[text()='Đăng nhập']]")).Click();
            Thread.Sleep(2000);

            var errorMessage = driver.FindElement(By.XPath("//div[@class='sc-FFETS kqYrLy']//div//span[contains(text(),'The input is required')]")).Text;
            Console.WriteLine("Thông báo lỗi: " + errorMessage);
        }

        [Test]
        public void Test_Sort_Orderlist_Status()//07
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
        public void Test_Sort_Orderlist_Payment()//08
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-cell-fix-left') and .//span[text()='Thanh toán']]")).Click();
            Thread.Sleep(3000); 
        }

        [Test]
        public void Test_Sort_Orderlist_Name()//09
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Nhấp vào tiêu đề cột "Tên người mua" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tên người mua']]")).Click();
            Thread.Sleep(3000); 
        }

        [Test]
        public void Test_Sort_Orderlist_PaymentMethod()//10
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Nhấp vào tiêu đề cột "Thanh toán" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Phương thức thanh toán']]")).Click();
            Thread.Sleep(3000); 
        }

        [Test]
        public void Test_Sort_Orderlist_TotalOrder()//11
        {
           
            // Xem trang quản lý đơn hàng
            Test_ViewOrder();

            // Nhấp vào tiêu đề cột "Tổng tiền" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tổng tiền đơn hàng']]")).Click();
            Thread.Sleep(3000); 
        }

        // [Test]
        // public void Test_Sort_Orderlist_OrderDate()//12
        // {
           
        //     // Xem trang quản lý đơn hàng
        //     Test_ViewOrder();

        //     // Tìm container bảng
        //     var tableContainer = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[2]/div/div/div/div/div/div/div[2]"));

        //     // In thông tin kiểm tra
        //     Console.WriteLine(tableContainer.GetAttribute("outerHTML"));

        //     // Cuộn dọc đến cuối bảng
        //     ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollTop = arguments[0].scrollHeight;", tableContainer);
        //     Thread.Sleep(1000);

        //     // Cuộn ngang sang phải
        //     ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollLeft = arguments[0].scrollWidth;", tableContainer);
        //     Thread.Sleep(1000);

        //     // Nhấp vào tiêu đề cột "Ngày đặt"
        //     var ngayDatColumn = driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and .//span[text()='Ngày đặt']]"));
        //     ngayDatColumn.Click();
        //     Thread.Sleep(3000); 
        // }

        [Test]
        public void Test_VerifyOrderQuantity()//13
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
        public void Test_CheckOrdersPerPageAndSelectAnyPage()//14
        {
            Test_ViewOrder();

            // Hàm đếm số lượng đơn hàng hiển thị trong bảng
            int CountOrdersOnPage()
            {
                return driver.FindElements(By.CssSelector("table tbody tr")).Count;
            }

            // Kiểm tra số lượng đơn hàng trên trang đầu tiên
            int firstPageCount = CountOrdersOnPage();
            Console.WriteLine($"Số đơn hàng trên trang 1: {firstPageCount}");
            Console.WriteLine(firstPageCount == 10 ? "Trang 1 hiển thị đúng 10 đơn hàng." : "Trang 1 hiển thị sai số lượng đơn hàng!");

            // Chuyển sang trang 2 bằng nút điều hướng
            driver.FindElement(By.XPath("//span[@aria-label='right']//*[name()='svg']")).Click();
            Thread.Sleep(3000);

            // Kiểm tra số lượng đơn hàng trên trang 2
            int secondPageCount = CountOrdersOnPage();
            Console.WriteLine($"Số đơn hàng trên trang 2: {secondPageCount}");
            Console.WriteLine(secondPageCount == 10 ? "Trang 2 hiển thị đúng 10 đơn hàng." : "Trang 2 hiển thị sai số lượng đơn hàng!");

            // Chuyển sang trang số 3 bằng cách nhấp vào số trang
            driver.FindElement(By.XPath("//a[normalize-space()='3']")).Click();
            Thread.Sleep(3000);

            // Kiểm tra số lượng đơn hàng trên trang 3
            int thirdPageCount = CountOrdersOnPage();
            Console.WriteLine($"Số đơn hàng trên trang 3: {thirdPageCount}");
            Console.WriteLine(thirdPageCount == 10 ? "Trang 3 hiển thị đúng 10 đơn hàng." : "Trang 3 hiển thị sai số lượng đơn hàng!");
        }


        [Test]
        public void Test_Verify8Orders()//15
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
        public void Test_ButtonNextPrevious() //16, 17
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
        public void Test_ManyproductsinOrder() //18
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
        public void Test_Change_StatusOrder() //19
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
        public void Test_Change_PaymentOrder() //20
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

        [Test]
        public void Test_Change_DeliveredOrderLogic() //21
        {
            // Thực hiện kiểm tra danh sách đơn hàng
            Test_ViewOrder();

            IWebElement orderRow = null;

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
                        Thread.Sleep(2000); // Đợi trang mới tải xong
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
        }

        [Test]
        public void Test_Change_PaymentOrderLogic() //22
        {
            // Thực hiện kiểm tra danh sách đơn hàng
            Test_ViewOrder();

            IWebElement orderRow = null;

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
                        Thread.Sleep(2000); // Đợi trang mới tải xong
                    }
                    else
                    {
                        // Không còn trang nào để tìm
                        Console.WriteLine("Không có đơn hàng 'Đã thanh toán' trên tất cả các trang.");
                        return; // Kết thúc vì không tìm thấy
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
        }

        
        [TearDown]
        public void CleanUp()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}

