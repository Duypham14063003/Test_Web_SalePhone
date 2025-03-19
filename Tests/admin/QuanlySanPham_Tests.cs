using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Threading;
using SeleniumExtras.WaitHelpers;
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
            driver.Navigate().GoToUrl(baseURL);
        }

        [Test]
        public void DangNhapAdmin_Test()
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
        public void Test_HienThiQuanLySanPham()
        {     
            DangNhapAdmin_Test();
            Thread.Sleep(4000); 
            Actions action = new Actions(driver);
            IWebElement avatar = driver.FindElement(By.XPath("//img[@alt='avatar']"));
            action.ClickAndHold(avatar).Perform(); // Click và giữ
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//p[contains(text(),'Quản lý hệ thống')]")).Click();;
            Thread.Sleep(2000);
            IWebElement sanPhamMenu = driver.FindElement(By.XPath("//span[@class='ant-menu-title-content' and text()='Sản phẩm']"));
            sanPhamMenu.Click();
            Thread.Sleep(6000);
        }    

        [Test]
        public void Test_ThemSanPham()
        {
            Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
           driver.FindElement(By.XPath("//body//div[@id='root']//div[@class='ant-spin-container']//div//div//div//div[1]//div[1]//button[1]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("basic_name")).SendKeys("Iphone 16");

             IWebElement selectBox = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-selector')]"));
            selectBox.Click();
            Thread.Sleep(2000);

            IWebElement itemApple = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-item-option-content') and text() ='Apple']"));
            itemApple.Click();

            driver.FindElement(By.Id("basic_countInStock")).SendKeys("10");

            
            driver.FindElement(By.Id("basic_price")).SendKeys("30000000");
            
            driver.FindElement(By.Id("basic_description")).SendKeys("Điện thoại Iphone 16 chất từng điểm chạm");
            Thread.Sleep(2000);
            
            driver.FindElement(By.XPath("//span[text()='Select Files']")).Click();
            Thread.Sleep(2000);
            
            IWebElement fileInput = driver.FindElement(By.CssSelector("input[type='file']"));
            fileInput.SendKeys("C:\\Users\\Admin\\Downloads\\iphone16.jpg");
            Thread.Sleep(2000);
            
            IWebElement checkBox = driver.FindElement(By.CssSelector("input[type='checkbox']"));

            checkBox.Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("basic_screenSize")).SendKeys("6.1 inch");
            driver.FindElement(By.Id("basic_chipset")).SendKeys("Apple A18 6 nhân");
            driver.FindElement(By.Id("basic_ram")).SendKeys("8 GB");
            driver.FindElement(By.Id("basic_storage")).SendKeys("128 GB");
            driver.FindElement(By.Id("basic_battery")).SendKeys("Li-ion");
            driver.FindElement(By.Id("basic_screenResolution")).SendKeys("Super Retina XDR (1179 x 2566 Pixels)");
            IWebElement btntthemSanPham = driver.FindElement(By.XPath("//span[contains(text(),'Thêm sản phẩm')]"));
            btntthemSanPham.Click();
            Thread.Sleep(10000); 
        }


        [Test]
        public void Test_ThemSanPhamThongTinKhongHopLe()
        {
            Test_HienThiQuanLySanPham();
            IWebElement btnThemSanPham = driver.FindElement(By.XPath("//span[@aria-label='plus-square']/ancestor::button"));
            btnThemSanPham.Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("basic_name")).SendKeys("Iphone 17");
            IWebElement selectBox = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-selector')]"));
            selectBox.Click();
            Thread.Sleep(2000);

            IWebElement itemApple = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-item-option-content') and text() ='Apple']"));
            itemApple.Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("basic_countInStock")).SendKeys("10");
            driver.FindElement(By.Id("basic_price")).SendKeys("Không có");
            driver.FindElement(By.Id("basic_description")).SendKeys("Điện thoại Iphone 16 chất từng điểm chạm");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[text()='Select Files']")).Click();
            Thread.Sleep(2000);
            IWebElement fileInput = driver.FindElement(By.CssSelector("input[type='file']"));
            fileInput.SendKeys("C:\\Users\\Admin\\Downloads\\iphone16.jpg");
            Thread.Sleep(2000);
            IWebElement checkBox = driver.FindElement(By.CssSelector("input[type='checkbox']"));
            checkBox.Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("basic_screenSize")).SendKeys("6.1 inch");
            driver.FindElement(By.Id("basic_chipset")).SendKeys("Apple A18 6 nhân");
            driver.FindElement(By.Id("basic_ram")).SendKeys("8 GB");
            driver.FindElement(By.Id("basic_storage")).SendKeys("128 GB");
            driver.FindElement(By.Id("basic_battery")).SendKeys("Li-ion");
            driver.FindElement(By.Id("basic_screenResolution")).SendKeys("Super Retina XDR (1179 x 2566 Pixels)");
            IWebElement btntthemSanPham = driver.FindElement(By.XPath("//button[.//span[text()='Thêm sản phẩm']]"));
            btntthemSanPham.Click();
            Thread.Sleep(10000);
        }

         [Test]
        public void Test_ThemSanPhamThongTinBiThieu()
        {
            Test_HienThiQuanLySanPham();
            IWebElement btnThemSanPham = driver.FindElement(By.XPath("//span[@aria-label='plus-square']/ancestor::button"));
            btnThemSanPham.Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("basic_name")).SendKeys("Iphone 17");
            IWebElement selectBox = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-selector')]"));
            selectBox.Click();
            Thread.Sleep(2000);
            //IWebElement itemApple = driver.FindElement(By.XPath("//div[contains(@class, 'ant-select-item-option-content') and text() ='']"));
            //itemApple.Click();
            //Thread.Sleep(2000);
            driver.FindElement(By.Id("basic_countInStock")).SendKeys("10");
            driver.FindElement(By.Id("basic_price")).SendKeys("30000000");
            driver.FindElement(By.Id("basic_description")).SendKeys("Điện thoại Iphone 16 chất từng điểm chạm");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[text()='Select Files']")).Click();
            Thread.Sleep(2000);
            IWebElement fileInput = driver.FindElement(By.CssSelector("input[type='file']"));
            fileInput.SendKeys("C:\\Users\\Admin\\Downloads\\iphone16.jpg");
            Thread.Sleep(2000);
            IWebElement checkBox = driver.FindElement(By.CssSelector("input[type='checkbox']"));
            checkBox.Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("basic_screenSize")).SendKeys("6.1 inch");
            driver.FindElement(By.Id("basic_chipset")).SendKeys("Apple A18 6 nhân");
            driver.FindElement(By.Id("basic_ram")).SendKeys("8 GB");
            driver.FindElement(By.Id("basic_storage")).SendKeys("128 GB");
            driver.FindElement(By.Id("basic_battery")).SendKeys("Li-ion");
            driver.FindElement(By.Id("basic_screenResolution")).SendKeys("Super Retina XDR (1179 x 2566 Pixels)");
            IWebElement btntthemSanPham = driver.FindElement(By.XPath("//button[.//span[text()='Thêm sản phẩm']]"));
            btntthemSanPham.Click();
            Thread.Sleep(10000); 
        }

        [Test]
        public void Test_SuaThongTinGiaTienSanPham()
        {
            Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            driver.FindElement(By.XPath("//tr[td[contains(normalize-space(.), 'iPhone 15 | 128GB | Đen')]]//span[@aria-label='edit']")).Click();
            Thread.Sleep(6000);
            driver.FindElement(By.Id("basic_price")).Click();
            driver.FindElement(By.Id("basic_price")).SendKeys(Keys.Control + "a");
            driver.FindElement(By.Id("basic_price")).SendKeys(Keys.Delete);
            driver.FindElement(By.Id("basic_price")).SendKeys("30500000");
            Thread.Sleep(2000);
        }

        [Test]
        public void Test_SuaThongTinKhongHopLeChoSanPham()
        {
            Test_HienThiQuanLySanPham();
        }
         [Test]
        public void Test_XoaSanPham()
        {
            Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            driver.FindElement(By.XPath("//tr[td[normalize-space()='iPhone 15 Plus | 128GB | Xanh lá']]//span[@aria-label='delete']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[normalize-space()='OK']")).Click();
            Thread.Sleep(2000);
        } 

        [Test]
        public void Test_Phantrang()
        {   Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            try
            {
                // Lấy tổng số đơn hàng từ phần tử hiển thị "Tổng số lượng đơn hàng"
                var totalProductsText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                int totalProducts = int.Parse(totalProductsText.Split(':')[1].Trim());
                Console.WriteLine($"Tổng số đơn hàng: {totalProducts}");

                // Tính tổng số trang dựa trên 10 đơn hàng/trang
                int ordersPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)totalProducts / ordersPerPage);
                Console.WriteLine($"Tổng số trang: {totalPages}");

                // Xác minh rằng chỉ có 1 trang hiển thị tất cả các đơn hàng
                if (totalPages == 1)
                {
                    // Đếm số lượng đơn hàng trên trang
                    var products = driver.FindElements(By.XPath("//tr[contains(@class, 'ant-table-row')]"));
                    Console.WriteLine($"Số lượng đơn hàng trên trang đầu tiên: {products.Count}");

                    // Kiểm tra xem số lượng đơn hàng có khớp với tổng đơn hàng hay không
                    if (products.Count == totalProducts)
                    {
                        Console.WriteLine("Tất cả đơn hàng được hiển thị trên một trang.");
                    }
                    else
                    {
                        Console.WriteLine($"LỖI: Số lượng đơn hàng trên trang không khớp: {products.Count} (mong đợi {totalProducts}).");
                    }
                }
                else
                {
                    Console.WriteLine($"LỖI: Có nhiều hơn 1 trang mặc dù tổng đơn hàng là {totalProducts}.");
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
            Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            try
            {
                // Lấy tổng số đơn hàng từ phần tử hiển thị "Tổng số lượng đơn hàng"
                var totalProductText = driver.FindElement(By.XPath("//div[@class='ant-table-title']//div[1]")).Text;
                int totalProducts = int.Parse(totalProductText.Split(':')[1].Trim());
                Console.WriteLine($"Tổng số đơn hàng: {totalProducts}");

                // Kiểm tra tổng số trang (với 10 đơn/trang)
                int ordersPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)totalProducts / ordersPerPage);
                Console.WriteLine($"Tổng số trang: {totalPages}");

                if (totalPages > 1)
                {// Kiểm tra nút "Next" hoạt động đúng
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
        public void Test_SelectAnyPage()
        {
            Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            
            // Chuyển sang trang số 3
            driver.FindElement(By.XPath("//a[normalize-space()='2']")).Click();
            Thread.Sleep(3000); // Chờ trang tải

            // Kiểm tra số lượng đơn hàng trên trang 3
            int productsOnPage2 = driver.FindElements(By.CssSelector("table tbody tr")).Count;
            Console.WriteLine($"Số đơn hàng trên trang 2: {productsOnPage2}");
            Console.WriteLine(productsOnPage2 == 10 ? "✅ Trang 2 hiển thị đúng 10 đơn hàng." : "❌ Trang 3 hiển thị sai số lượng đơn hàng!");
        }
        [Test]  
        public void Test_TimKiemSanPhamBangMa()
        {
            Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            driver.FindElement(By.XPath("//th[1]//div[1]//span[2]//span[1]//*[name()='svg']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//input[@placeholder='Search key']")).SendKeys("6734167b494d02e6fab57ea2");  
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[normalize-space()='Search']")).Click();
            Thread.Sleep(3000);
        }
            [Test]  
        public void Test_TimKiemSanPhamBangTen()
        {
            Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            driver.FindElement(By.XPath("//th[@aria-label='Tên sản phẩm']//span[@aria-label='search']//*[name()='svg']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//input[@placeholder='Search name']")).SendKeys("iPhone 15 | 128GB | Đen");  
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[normalize-space()='Search']")).Click();
            Thread.Sleep(3000);
        }
         [Test]  
        public void Test_TimKiemSanPhamBangTheLoai()
        {
            Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            driver.FindElement(By.XPath("//th[4]//div[1]//span[2]//span[1]//*[name()='svg']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//input[@placeholder='Search type']")).SendKeys("Apple");  
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[normalize-space()='Search']")).Click();
            Thread.Sleep(3000);
        }
             [Test]  
        public void Test_SapXepGia()
        {
            // Xem trang quản lý đơn hàng
            Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
               // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Giá']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong

            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Giá']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong

            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Giá']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong
        }
        [Test]
        public void Test_SapXepTonKho()
        {   Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tồn kho']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong

            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tồn kho']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong

            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tồn kho']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong
        }

        [Test]
        public void Test_SapXepDaBan()
        {   Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Đã bán']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong

            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Đã bán']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong

            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Đã bán']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong
        }
         [Test]
        public void Test_HienThiCotTonKho()
        {   Test_HienThiQuanLySanPham();
            Thread.Sleep(6000);
            // Nhấp vào tiêu đề cột "Tình trạng" để sắp xếp
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tồn kho']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong
             driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tồn kho']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong
            driver.FindElement(By.XPath("//th[contains(@class, 'ant-table-cell') and contains(@class, 'ant-table-column-has-sorters') and .//span[text()='Tồn kho']]")).Click();
            Thread.Sleep(3000); // Chờ sắp xếp xong
        }
        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
