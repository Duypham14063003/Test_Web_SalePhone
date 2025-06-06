﻿using OpenQA.Selenium;
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
using test_salephone.Utilities;

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
            Console.WriteLine("Đang đăng nhập User...");
            loginPage.EnterUsername("user@gmail.com");
            loginPage.EnterPassword("123123A");
            loginPage.ClickLoginButton();
            Thread.Sleep(2000);
        }

        private void LoginAdmin()
        {
            Console.WriteLine("Đang đăng nhập Admin...");
            loginPage.EnterUsername("ngotran2@gmail.com");
            loginPage.EnterPassword("123123A");
            loginPage.ClickLoginButton();
            Thread.Sleep(2000);
        }

        [Test]
        public void ID_MuaHang_1()
        {
            var testData = TestDataHelper.GetTestData("ID_MuaHang_1");

            if (testData == null)
            {
                Assert.Fail("Không có dữ liệu testcase ID_MuaHang_1!");
                return;
            }

            string productName = testData["ProductName"];
            StringBuilder log = new StringBuilder();

            log.AppendLine("Kiểm tra và thêm sản phẩm vào giỏ hàng nếu cần...");
            checkCart = new CheckCart(driver);
            checkCart.OpenCart();

            if (!checkCart.IsProductInCart())
            {
                log.AppendLine("Giỏ hàng trống, thêm sản phẩm vào giỏ.");
                checkCart.AddProductToCart();
                checkCart.OpenCart();
            }

            checkCart.SelectLatestProduct();

            int beforeBuyCount = checkCart.GetCartItemCount();
            log.AppendLine($"Trước khi mua: {beforeBuyCount} sản phẩm.");

            checkCart.BuyProduct();
            Thread.Sleep(3000);

            int afterBuyCount = checkCart.GetCartItemCount();
            log.AppendLine($"Sau khi mua: {afterBuyCount} sản phẩm.");

            bool result = afterBuyCount == beforeBuyCount - 1;
            log.AppendLine(result ? "Mua hàng thành công" : "Sản phẩm vẫn còn trong giỏ hàng sau khi mua!");

            Assert.That(result, log.ToString());

            ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_MuaHang_1", result ? "Pass" : "Fail", log.ToString());
        }

        [Test]
        public void ID_MuaHang_2()
        {
            var testData = TestDataHelper.GetTestData("ID_MuaHang_2");

            string productName = testData["ProductName"];
            string brand = testData["Brand"];
            string quantity = testData["Quantity"];
            string price = testData["Price"];
            string description = testData["Description"];
            string firstImage = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "assets", "AnhSP.png"));

            Console.WriteLine("Đăng nhập Admin...");

            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[1]);
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

            loginPage = new LoginPage(driver);
            LoginAdmin();

            adminPage = new AdminPage(driver);

            Console.WriteLine("Điều hướng đến trang 'Sản phẩm'...");
            adminPage.OpenAdminDropdown();
            adminPage.SelectSystemManagement();
            adminPage.NavigateToProductPage();

            Console.WriteLine("Đang thêm sản phẩm mới...");
            adminPage.AddNewProduct(productName, brand, quantity, price, description, firstImage);
            Thread.Sleep(3000);

            Console.WriteLine("Chuyển sang trang User...");
            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(5000);

            Console.WriteLine("Tìm sản phẩm và thêm vào giỏ hàng...");
            checkCart = new CheckCart(driver);
            checkCart.FindAndClickProductByName(productName);
            Thread.Sleep(6000);

            Console.WriteLine("Quay lại tab Admin để xóa sản phẩm...");
            driver.SwitchTo().Window(tabs[1]);
            Thread.Sleep(3000);

            adminPage.DeleteProduct(productName);
            Console.WriteLine("Đã xóa sản phẩm thành công!");
            Thread.Sleep(3000);

            Console.WriteLine("Quay lại trang User...");
            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(3000);

            Console.WriteLine("Click vào nút Mua Hàng cuối cùng...");
            checkCart.ClickLastBuyButtonOnly();

            string currentUrl = driver.Url;
            if (currentUrl == "https://frontend-salephones.vercel.app/orderSuccess")
            {
                Console.WriteLine("Chuyển hướng đến trang orderSuccess thành công.");
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_MuaHang_2", "Fail", "Mua hàng và chuyển hướng đến trang orderSuccess thành công. Không thông báo giá mới cho người dùng.");
            }
            else
            {
                Console.WriteLine("Chuyển hướng đến trang orderSuccess không thành công. URL hiện tại: " + currentUrl);
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_MuaHang_2", "Pass", "Không thể mua hàng do sản phẩm đã bị xóa");
            }

        }




        [Test]
        public void ID_MuaHang_3()
        {
            var testData = TestDataHelper.GetTestData("ID_MuaHang_3");


            string productName = testData["ProductName"];
            string brand = testData["Brand"];
            string quantity = testData["Quantity"];
            string price = testData["Price"];
            string description = testData["Description"];
            string newPrice = testData["ExtraField"]; 
            string firstImage = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "assets", "AnhSP.png"));


            Console.WriteLine("Đăng nhập Admin...");
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[1]);
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

            loginPage = new LoginPage(driver);
            LoginAdmin();

            adminPage = new AdminPage(driver);
            Console.WriteLine("Điều hướng đến trang 'Sản phẩm'...");
            adminPage.OpenAdminDropdown();
            Thread.Sleep(4000);
            adminPage.SelectSystemManagement();
            adminPage.NavigateToProductPage();

            adminPage.AddNewProduct(productName, brand, quantity, price, description, firstImage);
            Console.WriteLine("Đã thêm sản phẩm mới thành công!");
            Thread.Sleep(5000);

            Console.WriteLine("Chuyển sang trang User...");
            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(5000);

            Console.WriteLine("Tìm sản phẩm và thêm vào giỏ hàng...");
            checkCart = new CheckCart(driver);
            checkCart.FindAndClickProductByName(productName);
            Thread.Sleep(6000);

            Console.WriteLine("Quay lại tab Admin để cập nhật sản phẩm...");
            driver.SwitchTo().Window(tabs[1]);
            Thread.Sleep(3000);

            bool isUpdated = adminPage.UpdateProduct(productName, newPrice);
            if (isUpdated)
            {
                Console.WriteLine($"Giá sản phẩm '{productName}' đã cập nhật thành {newPrice}!");
            }
            else
            {
                Console.WriteLine($"Không thể cập nhật giá sản phẩm '{productName}'.");
            }

            Console.WriteLine("Quay lại trang User...");
            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(3000);

            Console.WriteLine("Click vào nút Mua Hàng cuối cùng...");
            checkCart.ClickLastBuyButtonOnly();

            string currentUrl = driver.Url;
            if (currentUrl == "https://frontend-salephones.vercel.app/orderSuccess")
            {
                Console.WriteLine("Chuyển hướng đến trang orderSuccess thành công.");
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_MuaHang_3", "Fail", "Mua hàng và chuyển hướng đến trang orderSuccess thành công. Không thông báo giá mới cho người dùng.");
            }
            else
            {
                Console.WriteLine("Chuyển hướng đến trang orderSuccess không thành công. URL hiện tại: " + currentUrl);
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_MuaHang_3", "Pass", "Mua hàng thất bại");
            }
        }

        [Test]
        public void ID_MuaHang_4()
        {
            var testData = TestDataHelper.GetTestData("ID_MuaHang_4");


            string productName = testData["ProductName"];

            Console.WriteLine("Tìm sản phẩm và thêm vào giỏ hàng...");
            checkCart = new CheckCart(driver);
            checkCart.FindAndClickProductByName(productName);
            Thread.Sleep(6000);

            Console.WriteLine("Đăng nhập Admin...");
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs[1]);
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

            loginPage = new LoginPage(driver);
            LoginAdmin();

            adminPage = new AdminPage(driver);

            Console.WriteLine("Điều hướng đến trang 'Sản phẩm'...");
            adminPage.OpenAdminDropdown();
            adminPage.SelectSystemManagement();
            adminPage.NavigateToProductPage();

            Console.WriteLine("Cập nhật số lượng sản phẩm về 0...");
            bool isUpdated = adminPage.UpdateSLProduct(productName);
            if (isUpdated)
            {
                Console.WriteLine($"Số lượng sản phẩm '{productName}' đã cập nhật về 0!");
            }
            else
            {
                Console.WriteLine($"Không thể cập nhật số lượng sản phẩm '{productName}'.");
            }

            Console.WriteLine("Quay lại trang User...");
            driver.SwitchTo().Window(tabs[0]);
            Thread.Sleep(3000);

            checkCart.ClickLastBuyButtonOnly();

            Console.WriteLine("Kiểm tra chuyển hướng...");
            string currentUrl = driver.Url;

            if (currentUrl == "https://frontend-salephones.vercel.app/orderSuccess")
            {
                Console.WriteLine("Chuyển hướng đến trang orderSuccess thành công.");
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_MuaHang_4", "Fail", "Mua hàng và chuyển hướng đến trang orderSuccess thành công. /n Không kiểm tra số lượng sản phẩm tồn kho.");
            }
            else
            {
                Console.WriteLine("Chuyển hướng đến trang orderSuccess không thành công. URL hiện tại: " + currentUrl);
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_MuaHang_4", "Pass", "Mua hàng thất bại do không đủ sản phẩm");
            }
        }

        [Test]
        public void ID_MuaHang_5()
        {
            var testData = TestDataHelper.GetTestData("ID_MuaHang_5");

            if (testData == null)
            {
                Assert.Fail("Không có dữ liệu testcase ID_MuaHang_5!");
                return;
            }

            string productName = testData["ProductName"];
            Console.WriteLine("Tìm sản phẩm và thêm vào giỏ hàng...");
            checkCart = new CheckCart(driver);
            checkCart.FindAndClickProductByName(productName);
            Thread.Sleep(6000);

            Console.WriteLine("Trở về trang chủ...");
            checkCart.ClickLogoToGoToHomePage();
            wait.Until(ExpectedConditions.UrlToBe("https://frontend-salephones.vercel.app/"));

            Console.WriteLine("Kiểm tra giỏ hàng...");
            checkCart.OpenCart();
            bool isProductInCart = checkCart.IsProductInCart();

            Console.WriteLine($"Giỏ hàng có sản phẩm: {isProductInCart}");

            if (isProductInCart)
            {
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_MuaHang_5", "Pass", "Mua hàng không thành công");
            }
            else
            {
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_MuaHang_5", "Fail", "Mua hàng thành công");
            }
        }


    }
}
