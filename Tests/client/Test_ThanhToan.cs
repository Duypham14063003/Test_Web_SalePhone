using test_salephone.Utilities;
using OpenQA.Selenium;
using test_salephone.PageObjects; // Ensure this namespace contains the LoginPage class
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;
using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using TestProject.PageObjects;

namespace test_salephone.client
{
    [TestFixture]
    class Test_ThanhToan : TestBase
    {
        private WebDriverWait wait;
        private IWebElement notification;


        [SetUp]
        public override void Setup()
        {
            base.Setup();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            LoginPage loginPage = new LoginPage(driver);
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[4]/div")).Click();
            loginPage.EnterUsername("ngocduy1423@gmail.com");
            loginPage.EnterPassword("Duy123123123");
            loginPage.ClickLoginButton();
        }

        [Category("ThanhToan")]
        [TestCase("ID_ThanhToan_01")]
        [TestCase("ID_ThanhToan_02")]
        [TestCase("ID_ThanhToan_03")]
        [TestCase("ID_ThanhToan_04")]
        [TestCase("ID_ThanhToan_05")]
        [TestCase("ID_ThanhToan_06")]
        [TestCase("ID_ThanhToan_07")]
        [TestCase("ID_ThanhToan_08")]
        [TestCase("ID_ThanhToan_09")]
        public void ThanhToan(string id)
        {
            CheckCart checkCart = new CheckCart(driver);
            Console.WriteLine($"🚀 Bắt đầu test case: {id}");

            try
            {
                // Nếu test case không phải ID_ThanhToan_03 thì thêm sản phẩm vào giỏ
                if (id != "ID_ThanhToan_03")
                {
                    checkCart.AddProductToCart();
                }

                checkCart.OpenCart();
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/h3")));
                Console.WriteLine("Đã vào giỏ hàng");

                ThanhToanPageObject thanhToanPage = new ThanhToanPageObject(driver);
                // Sử dụng FindElements để tránh exception nếu phần tử không tồn tại
                IList<IWebElement> diaChiElements = driver.FindElements(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[2]/div[1]/div[1]/div/span[2]"));
                bool isDiaChiDisplayed = diaChiElements.Count > 0 && diaChiElements.First().Displayed;

                switch (id)
                {
                    case "ID_ThanhToan_01":
                        bool res = thanhToanPage.ThayDoiDiaChi("Pham Ngoc Duy", "HCMC", "0365968196", "");
                        if (!res)
                        {
                            Console.WriteLine("Địa chỉ không được để trống");
                            ExcelReportHelper.WriteToExcel("Testcase Duy", "ID_ThanhToan_01", "pass", "Địa chỉ bị bỏ trống");

                        }
                        thanhToanPage.ClickMuaHangButton();
                        return;
                    // Assert.That(isDiaChiDisplayed, Is.False, );

                    case "ID_ThanhToan_02":
                        if (!thanhToanPage.ThayDoiDiaChi("Pham Ngoc Duy", "HCMC", "0365968196", "TanPhu"))
                        {
                            Console.WriteLine("Địa chỉ không được để trống");
                            ExcelReportHelper.WriteToExcel("Testcase Duy", "ID_ThanhToan_01", "pass", "Địa chỉ bị bỏ trống");
                        }
                        bool ketqua = thanhToanPage.MuaHangVoiThanhToanKhiNhanHang();
                        ExcelReportHelper.WriteToExcel("Testcase Duy", "ID_ThanhToan_01", ketqua.ToString(), "");

                        // Assert.That(isDiaChiDisplayed, Is.True, "Test ID_ThanhToan_02: Địa chỉ được hiển thị khi người dùng đã nhập địa chỉ hợp lệ.");
                        break;

                    case "ID_ThanhToan_03":
                        Assert.That(checkCart.IsProductInCart(), Is.False, "Test ID_ThanhToan_03: Giỏ hàng nên trống.");
                        thanhToanPage.ClickMuaHangButton();
                        notification = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(".ant-message-notice-content")));
                        string toastContent = notification.Text.ToLower();
                        Console.WriteLine("Toast content:", toastContent);
                        Assert.That(toastContent.Contains("vui lòng chọn sản phẩm"), Is.True, "Test ID_ThanhToan_03: Phải hiển thị thông báo l��i khi tìm thấy sản phẩm.");
                        // IWebElement errorMessage = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[2]/div[2]/div/span")); // Update XPath as needed
                        // Assert.That(errorMessage.Displayed, Is.True, "Test ID_ThanhToan_03: Phải hiển thị thông báo lỗi khi giỏ hàng trống.");
                        break;

                    case "ID_ThanhToan_04":
                    case "ID_ThanhToan_05":
                        thanhToanPage.ClickChonTatCaSanPhamButton();
                        thanhToanPage.ClickThanhToanButton();
                        thanhToanPage.chonPhuongThucTT();
                        thanhToanPage.ClickMuaHangButton();
                        Assert.That(thanhToanPage.IsThanhToanSuccess(), Is.True, "Test ID_ThanhToan_04/05: Thanh toán nên thành công.");
                        break;

                    case "ID_ThanhToan_06":
                        Assert.That(checkCart.IsProductInCart(), Is.False, "Test ID_ThanhToan_06: Giỏ hàng nên trống do sản phẩm đã hết hàng.");
                        break;

                    case "ID_ThanhToan_07":
                        Assert.That(isDiaChiDisplayed, Is.False, "Test ID_ThanhToan_07: Địa chỉ không hợp lệ nên không được hiển thị.");
                        break;

                    case "ID_ThanhToan_08":
                        // Implement logic for ID_ThanhToan_08 or add a break statement
                        Console.WriteLine("Test case ID_ThanhToan_08 logic not implemented yet.");
                        break;

                        //     thanhToanPage.ClickChonTatCaSanPhamButton();
                        // case "ID_ThanhToan_09":
                        //     thanhToanPage.ClickChonTatCaSanPhamButton();
                        //     thanhToanPage.ClickThanhToanButton();
                        //     Assert.That(thanhToanPage.IsDiscountApplied(), Is.True, "Test ID_ThanhToan_09: Mã giảm giá hợp lệ nên được áp dụng.");
                        //     break;
                }

                Console.WriteLine("✅ Test case thành công.");
            }
            catch (Exception e)
            {
                Console.WriteLine("❌ Lỗi: " + e.Message);
                Assert.Fail(e.Message);
            }
        }
    }
}
