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
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//td[normalize-space()='6734167b494d02e6fab57ea2']//span[@aria-label='edit']")).Click();
            IWebElement priceInput = driver.FindElement(By.Id("basic_price"));
            priceInput.Clear();
            priceInput.SendKeys("30500000");
        }

        [Test]
        public void Test_SuaThongTinKhongHopLeChoSanPham()
        {
            Test_HienThiQuanLySanPham();
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
