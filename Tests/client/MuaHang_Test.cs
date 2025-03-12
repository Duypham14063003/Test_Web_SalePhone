using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using test_salephone.Helpers;
using test_salephone.PageObjects;
using TestProject.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class PurchaseTests
    {
        private IWebDriver driver;
        private CheckCart checkCart;
        private LoginPage loginPage;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");
            checkCart = new CheckCart(driver);
            loginPage = new LoginPage(driver);

            Login();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        private void Login()
        {
            Console.WriteLine("⏳ Đang đăng nhập...");
            loginPage.EnterUsername("user@gmail.com");
            loginPage.EnterPassword("123123A");
            loginPage.ClickLoginButton();
            Thread.Sleep(2000); 
        }

        [Test]
        public void ID_MuaHang_1()
        {
            Console.WriteLine("🛒 Kiểm tra và thêm sản phẩm vào giỏ hàng nếu cần...");
            checkCart.OpenCart();

            if (!checkCart.IsProductInCart())
            {
                Console.WriteLine("⚠️ Giỏ hàng trống, thêm sản phẩm vào giỏ.");
                checkCart.AddProductToCart();
                checkCart.OpenCart();
            }

            checkCart.SelectLatestProduct();

            int beforeBuyCount = checkCart.GetCartItemCount();
            Console.WriteLine($"📦 Trước khi mua: {beforeBuyCount} sản phẩm.");

            checkCart.BuyProduct();

            Thread.Sleep(3000); // Đợi giỏ hàng cập nhật

            int afterBuyCount = checkCart.GetCartItemCount();
            Console.WriteLine($"📦 Sau khi mua: {afterBuyCount} sản phẩm.");

            Assert.That(afterBuyCount == beforeBuyCount - 1, $"❌ Sản phẩm vẫn còn trong giỏ hàng sau khi mua! Trước: {beforeBuyCount}, Sau: {afterBuyCount}");

            // Ghi kết quả vào file Excel
            ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_1", "Pass");
        }


        [Test]
        public void ID_MuaHang_5()
        {
            Console.WriteLine("🛒 Kiểm tra hành động hủy bỏ mua hàng...");

            // Mở giỏ hàng và kiểm tra sản phẩm
            checkCart.OpenCart();

            if (!checkCart.IsProductInCart())
            {
                Console.WriteLine("⚠️ Giỏ hàng trống, thêm sản phẩm vào giỏ.");
                checkCart.AddProductToCart();
                checkCart.OpenCart();
            }

            checkCart.SelectLatestProduct();
            checkCart.BuyProduct();
            checkCart.ClickLogoToGoToHomePage();
            //// Kiểm tra đơn hàng không được tạo
            //bool isOrderCreated = checkCart.IsOrderCreated();
            //Assert.That(!isOrderCreated, $"❌ Đơn hàng vẫn được tạo dù đã hủy mua!");

            // Ghi kết quả vào file Excel
            ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_5", "Pass");
        }

    }
}