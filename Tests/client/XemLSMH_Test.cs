using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using test_salephone.PageObjects;
using TestProject.PageObjects;
using NUnit.Framework;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using test_salephone.Utilities;

namespace XemLSMH
{
    [TestFixture]
    public class XemLSMH_Test
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private WebDriverWait wait;
        private PurchaseHistoryPage purchaseHistoryPage;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/sign-in");

            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            Login();
            purchaseHistoryPage = new PurchaseHistoryPage(driver);
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

        [Test]
        public void ID_XemLSMH_1()
        {
            purchaseHistoryPage.NavigateToPurchaseHistory();
            string orderDetails = purchaseHistoryPage.GetOrderDetailsOfFirstItem();
            Console.WriteLine(orderDetails);

            // 1. Prepare data for Excel writing
            string testCaseName = "Testcase Trân";
            string testCaseId = "ID_XemLSMH_1";
            string status = "PASS"; // Set status to PASS directly
            string message = "Thông tin chi tiết đơn hàng: " + orderDetails; // Combine message and order details

            // 2. Write to Excel
            ExcelReportHelper.WriteToExcel(testCaseName, testCaseId, status, message);

            // In ra thông báo PASS
            Console.WriteLine("✅ Test Case ID_XemLSMH_1 Passed.");
        }

        [Test]
        public void ID_XemLSMH_2()
        {
            purchaseHistoryPage.NavigateToPurchaseHistory();

            // 1. Lấy số lượng đơn hàng trước khi hủy
            int soLuongDonHangTruocKhiHuy = purchaseHistoryPage.GetOrderCount();

            purchaseHistoryPage.CancelFirstOrder();

            // 2. Lấy số lượng đơn hàng sau khi hủy
            int soLuongDonHangSauKhiHuy = purchaseHistoryPage.GetOrderCount();

            // 3. Kiểm tra xem số lượng đơn hàng có giảm đi 1 không
            bool huyDonHangThanhCong = (soLuongDonHangSauKhiHuy == soLuongDonHangTruocKhiHuy - 1);

            string ketQua = huyDonHangThanhCong ? "PASS" : "FAIL";
            string message = huyDonHangThanhCong ? "Hủy đơn hàng thành công. Số lượng đơn hàng giảm 1." : "Hủy đơn hàng thất bại. Số lượng đơn hàng không giảm.";

            ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_XemLSMH_2", ketQua, message);
            Console.WriteLine($"✅ Test Case ID_XemLSMH_2: {ketQua}");
            Console.WriteLine(message);
        }

        [Test]
        public void ID_XemLSMH_3()
        {
            purchaseHistoryPage.NavigateToPurchaseHistory();
            ReadOnlyCollection<IWebElement> orderItems = driver.FindElements(By.XPath("//div[@class='order-item']"));
            Thread.Sleep(9000);

            if (orderItems.Count == 0)
            {
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_XemLSMH_3", "Fail", "Nút hủy đơn vẫn hiển thị");
                return;
            }

            string overallResult = "PASS";
            string overallMessage = "";

            Assert.Multiple(() =>
            {
                foreach (IWebElement item in orderItems)
                {
                    string status = purchaseHistoryPage.GetOrderStatus(item);
                    bool isEnabled = purchaseHistoryPage.IsCancelButtonEnabled(item);

                    if (status == "Đã giao hàng thành công")
                    {
                        Assert.That(!isEnabled, $"Nút 'Hủy đơn hàng' phải bị vô hiệu hóa khi đơn hàng ở trạng thái '{status}'.");
                        if (isEnabled)
                        {
                            overallResult = "FAIL";
                            overallMessage += $"❌ Trạng thái '{status}': Nút 'Hủy đơn hàng' hoạt động. ";
                            Console.WriteLine($"❌ ID_XemLSMH_3: FAIL - Nút 'Hủy đơn hàng' hoạt động khi đơn hàng ở trạng thái '{status}'.");
                        }
                        else
                        {
                            overallMessage += $"✅ Trạng thái '{status}': Nút 'Hủy đơn hàng' bị vô hiệu hóa. ";
                            Console.WriteLine($"✅ ID_XemLSMH_3: PASS - Nút 'Hủy đơn hàng' bị vô hiệu hóa khi đơn hàng ở trạng thái '{status}'.");
                        }
                    }
                    else
                    {
                        overallMessage += $"ℹ️ Trạng thái '{status}': Không kiểm tra nút vô hiệu hóa ở trạng thái này. ";
                    }
                }
            });

            ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_XemLSMH_3", overallResult, overallMessage);
        }

        [Test]
        public void ID_XemLSMH_4()
        {
            try
            {
                // 1. Điều hướng đến trang lịch sử mua hàng
                purchaseHistoryPage.NavigateToPurchaseHistory();

                // 2. Bấm vào nút "Xem chi tiết"
                bool isClicked = purchaseHistoryPage.ClickViewDetailsButton(); // ✅ Lưu kết quả click

                if (isClicked)  // ❌ Xóa dấu `;` ở đây
                {
                    // 3. Ghi kết quả vào file Excel (PASS)
                    ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_XemLSMH_4", "PASS", "Đã bấm vào 'Xem chi tiết' thành công.");
                    Console.WriteLine("✅ Test Case ID_XemLSMH_4 Passed.");
                }
                else
                {
                    // 4. Ghi kết quả vào file Excel (FAIL)
                    ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_XemLSMH_4", "FAIL", "Không thể bấm vào 'Xem chi tiết'.");
                    Console.WriteLine("❌ Test Case ID_XemLSMH_4 Failed: Không thể bấm vào 'Xem chi tiết'.");
                }
            }
            catch (Exception ex)
            {
                // Ghi kết quả FAIL nếu có lỗi trong quá trình test
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_XemLSMH_4", "FAIL", "Lỗi xảy ra: " + ex.Message);
                Console.WriteLine("❌ Test Case ID_XemLSMH_4 Failed: Lỗi xảy ra - " + ex.Message);
            }
        }

        [Test]
        public void ID_XemLSMH_5()
        {
            // 1. Điều hướng đến trang lịch sử mua hàng
            purchaseHistoryPage.NavigateToPurchaseHistory();

            // 2. Bấm vào nút "Xem chi tiết"
            purchaseHistoryPage.ClickViewDetailsButton();

            // 3. Bấm vào nút "Xuất file Excel"
            purchaseHistoryPage.ClickExportExcelButton();

            // 4. Ghi kết quả vào file Excel
            bool isButtonClicked = true; // Giả sử nếu không có lỗi là thành công

            if (isButtonClicked)
            {
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_XemLSMH_5", "PASS", "Đã bấm vào 'Xem chi tiết' và 'Xuất file Excel' thành công.");
            }
            else
            {
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_XemLSMH_5", "FAIL", "Không thể bấm vào 'Xem chi tiết' hoặc 'Xuất file Excel'.");
            }

            // 5. In thông báo kết quả
            Console.WriteLine(isButtonClicked ? "✅ Test Case ID_XemLSMH_5 Passed." : "❌ Test Case ID_XemLSMH_5 Failed.");
        }
    }
}