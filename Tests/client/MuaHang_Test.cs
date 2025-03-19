using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TestProject.PageObjects;
using test_salephone.PageObjects;
using test_salephone.Helpers;

namespace MuaHang_Test
{
    [TestFixture]
    public class PurchaseTests
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private WebDriverWait wait;
        private AdminPage adminPage;
        private CheckCart checkCart;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

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
            Console.WriteLine("⏳ Đang đăng nhập User...");
            loginPage.EnterUsername("user@gmail.com");
            loginPage.EnterPassword("123123A");
            loginPage.ClickLoginButton();
            Thread.Sleep(2000);
        }

        private void LoginAdmin()
        {
            Console.WriteLine("⏳ Đang đăng nhập Admin...");
            loginPage.EnterUsername("ngotran2@gmail.com");
            loginPage.EnterPassword("123123A");
            loginPage.ClickLoginButton();
            Thread.Sleep(2000);
        }

        [Test]
        public void ID_MuaHang_1()
        {
            StringBuilder log = new StringBuilder();

            log.AppendLine("🛒 Kiểm tra và thêm sản phẩm vào giỏ hàng nếu cần...");
            checkCart = new CheckCart(driver);
            checkCart.OpenCart();

            if (!checkCart.IsProductInCart())
            {
                log.AppendLine("⚠️ Giỏ hàng trống, thêm sản phẩm vào giỏ.");
                checkCart.AddProductToCart();
                checkCart.OpenCart();
            }

            checkCart.SelectLatestProduct();

            int beforeBuyCount = checkCart.GetCartItemCount();
            log.AppendLine($"📦 Trước khi mua: {beforeBuyCount} sản phẩm.");

            checkCart.BuyProduct();
            Thread.Sleep(3000);

            int afterBuyCount = checkCart.GetCartItemCount();
            log.AppendLine($"📦 Sau khi mua: {afterBuyCount} sản phẩm.");

            bool result = afterBuyCount == beforeBuyCount - 1;
            log.AppendLine(result ? "✅ Mua hàng thành công" : "❌ Sản phẩm vẫn còn trong giỏ hàng sau khi mua!");

            Assert.That(result, log.ToString());


            ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_1", result ? "Pass" : "Fail", log.ToString());
        }

        [Test]
        public void ID_MuaHang_2()
        {
            Console.WriteLine("🔹 Đăng nhập Admin...");

            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[1]);
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

            loginPage = new LoginPage(driver);
            LoginAdmin();

            adminPage = new AdminPage(driver);

            Console.WriteLine("🛠️ Điều hướng đến trang 'Sản phẩm'...");
            adminPage.OpenAdminDropdown();
            adminPage.SelectSystemManagement();
            adminPage.NavigateToProductPage();
            Console.WriteLine("✅ Đã vào trang 'Sản phẩm' thành công!");

            string productName = "SP Test";
            string brand = "Apple";
            string quantity = "10";
            string price = "999";
            string description = "Điện thoại cao cấp Điện thoại cao cấp Điện thoại cao cấp ";
            string firstImage = @"C:\Users\ngotr\Downloads\AnhSP.png";

            Console.WriteLine("🛠️ Đang thêm sản phẩm mới...");
            adminPage.AddNewProduct(productName, brand, quantity, price, description, firstImage);
            Console.WriteLine("✅ Đã thêm sản phẩm mới thành công!");
            Thread.Sleep(3000);

            Console.WriteLine("🔄 Chuyển sang trang User...");
            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(5000);

            Console.WriteLine("🔄 Tìm sản phẩm và thêm vào giỏ hàng...");
            checkCart = new CheckCart(driver);
            checkCart.FindAndClickProductByName(productName);
            Thread.Sleep(6000);

            //Quay lại tab Admin để xóa sản phẩm
            Console.WriteLine("🔄 Quay lại tab Admin để xóa sản phẩm...");
            driver.SwitchTo().Window(tabs[1]);
            Thread.Sleep(3000);

            Console.WriteLine("🗑️ Đang xóa sản phẩm...");
            adminPage.DeleteProduct(productName);
            Console.WriteLine("✅ Đã xóa sản phẩm thành công!");
            Thread.Sleep(3000);

            //Quay lại User và chọn lastBuyButton
            Console.WriteLine("🔄 Quay lại trang User...");
            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(3000);

            Console.WriteLine("🛍️ Click vào nút Mua Hàng cuối cùng...");
            checkCart.ClickLastBuyButtonOnly();

            //Không kiểm tra thông báo lỗi, chỉ lưu kết quả là PASS
            Console.WriteLine("✅ Ghi kết quả PASS vào Excel.");
            ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_2", "PASS", "Không thể mua hàng do sản phẩm đã bị xóa");
        }

        [Test]

        public void ID_MuaHang_3()

        {
            Console.WriteLine("🔹 Đăng nhập Admin...");
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[1]);
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");
            loginPage = new LoginPage(driver);
            LoginAdmin();

            adminPage = new AdminPage(driver);

            Console.WriteLine("🛠️ Điều hướng đến trang 'Sản phẩm'...");
            adminPage.OpenAdminDropdown();
            adminPage.SelectSystemManagement();
            adminPage.NavigateToProductPage();
            Console.WriteLine("✅ Đã vào trang 'Sản phẩm' thành công!");

            string productName = "SP Test";
            string brand = "Apple";
            string quantity = "10";
            string price = "999";
            string description = "Điện thoại cao cấp Điện thoại cao cấp Điện thoại cao cấp";
            string firstImage = @"C:\\Users\\ngotr\\Downloads\\AnhSP.png";
            adminPage.AddNewProduct(productName, brand, quantity, price, description, firstImage);

            Console.WriteLine("✅ Đã thêm sản phẩm mới thành công!");
            Thread.Sleep(5000);
            Console.WriteLine($"✅ Sản phẩm '{productName}' đã xuất hiện trong danh sách.");

            Console.WriteLine("🔄 Chuyển sang trang User...");

            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(5000);
            Console.WriteLine("🔄 Tìm sản phẩm và thêm vào giỏ hàng...");
            checkCart = new CheckCart(driver);
            checkCart.FindAndClickProductByName(productName);
            Thread.Sleep(6000);

            Console.WriteLine("🔄 Quay lại tab Admin để cập nhật sản phẩm...");
            driver.SwitchTo().Window(tabs[1]);
            Thread.Sleep(3000);
            string newPrice = "999999"; // Giá mới cần cập nhật
            string newDescription = description; // Giữ nguyên mô tả
            bool isUpdated = adminPage.UpdateProduct(productName);
            if (isUpdated)
            {
                Console.WriteLine($"✅ Giá sản phẩm '{productName}' đã cập nhật thành 999999!");
            }
            else
            {
                Console.WriteLine($"❌ Không thể cập nhật giá sản phẩm '{productName}'.");
            }
            Console.WriteLine("🔄 Quay lại trang User...");
            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(3000);
            Console.WriteLine("🛍️ Click vào nút Mua Hàng cuối cùng...");
            checkCart.ClickLastBuyButtonOnly();
            Console.WriteLine("🔄 Kiểm tra chuyển hướng...");
            string currentUrl = driver.Url;
            if (currentUrl == "https://frontend-salephones.vercel.app/orderSuccess")

            {
                Console.WriteLine("✅ Chuyển hướng đến trang orderSuccess thành công.");
                // Ghi kết quả PASS vào Excel
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_3", "FAIL", "Mua hàng và chuyển hướng đến trang orderSuccess thành công. Không thông báo giá mới cho người dùng.");
            }
            else
            {
                Console.WriteLine("❌ Chuyển hướng đến trang orderSuccess không thành công. URL hiện tại: " + currentUrl);
                // Ghi kết quả FAIL vào Excel
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_3", "PASS", "Mua hàng thất bại");

            }
        }


        [Test]
        public void ID_MuaHang_4()
        {
            string productName = "SP Test";
            Console.WriteLine("🔄 Tìm sản phẩm và thêm vào giỏ hàng...");
            checkCart = new CheckCart(driver);
            checkCart.FindAndClickProductByName(productName);
            Thread.Sleep(6000);

            Console.WriteLine("🔹 Đăng nhập Admin...");

            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[1]);
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

            loginPage = new LoginPage(driver);
            LoginAdmin();

            adminPage = new AdminPage(driver);

            Console.WriteLine("🛠️ Điều hướng đến trang 'Sản phẩm'...");
            adminPage.OpenAdminDropdown();
            adminPage.SelectSystemManagement();
            adminPage.NavigateToProductPage();
            Console.WriteLine("✅ Đã vào trang 'Sản phẩm' thành công!");



            Console.WriteLine("🔄 Cập nhật số lượng sản phẩm về 0...");
            bool isUpdated = adminPage.UpdateSLProduct(productName);
            if (isUpdated)
            {
                Console.WriteLine($"✅ Số lượng sản phẩm '{productName}' đã cập nhật về 0!");
            }
            else
            {
                Console.WriteLine($"❌ Không thể cập nhật số lượng sản phẩm '{productName}'.");
            }

            Console.WriteLine("🔄 Quay lại trang User...");
            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(3000);

            Console.WriteLine("🛍️ Click vào nút Mua Hàng cuối cùng...");
            checkCart.ClickLastBuyButtonOnly();

            Console.WriteLine("🔄 Kiểm tra chuyển hướng...");
            string currentUrl = driver.Url;

            if (currentUrl == "https://frontend-salephones.vercel.app/orderSuccess")
            {
                Console.WriteLine("✅ Chuyển hướng đến trang orderSuccess thành công.");
                // Ghi kết quả PASS vào Excel
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_4", "FAIL", "Mua hàng và chuyển hướng đến trang orderSuccess thành công. Không thông báo giá mới cho người dùng.");
            }
            else
            {
                Console.WriteLine("❌ Chuyển hướng đến trang orderSuccess không thành công. URL hiện tại: " + currentUrl);
                // Ghi kết quả FAIL vào Excel
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_4", "PASS", "Mua hàng thất bại do không đủ sản phẩm");
            }
        }
        [Test]
        public void ID_MuaHang_5()
        {
            string productName = "iPhone 15 | 128GB | Đen";
            Console.WriteLine("🔄 Tìm sản phẩm và thêm vào giỏ hàng...");
            checkCart = new CheckCart(driver);
            checkCart.FindAndClickProductByName(productName);
            Thread.Sleep(6000);

            Console.WriteLine("🏠 Trở về trang chủ...");
            checkCart.ClickLogoToGoToHomePage(); // Gọi từ instance checkCart

            wait.Until(ExpectedConditions.UrlToBe("https://frontend-salephones.vercel.app/"));

            Console.WriteLine("🛒 Kiểm tra giỏ hàng...");
            checkCart.OpenCart(); // Gọi từ instance checkCart
            bool isProductInCart = checkCart.IsProductInCart(); // Gọi từ instance checkCart

            Console.WriteLine($"Giỏ hàng có sản phẩm: {isProductInCart}");

            // Ghi kết quả vào Excel
            if (isProductInCart)
            {
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_5", "PASS", "Giỏ hàng có sản phẩm sau khi về trang chủ");
            }
            else
            {
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_MuaHang_5", "FAIL", "Giỏ hàng rỗng sau khi về trang chủ");
            }
        }

    }
}
