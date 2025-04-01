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
using System.Text;
using System.Diagnostics;

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
            Console.WriteLine("Đang đăng nhập User...");
            loginPage.EnterUsername("user1@gmail.com");
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

            string testCaseName = "Testcase Trân";
            string testCaseId = "ID_XemLSMH_1";
            string status = "Pass";
            string message = "Thông tin chi tiết đơn hàng: " + orderDetails;

            ExcelReportHelper_Trân.WriteToExcel(testCaseName, testCaseId, status, message);

        }



        [Test]
        public void ID_XemLSMH_2()
        {
            purchaseHistoryPage.NavigateToPurchaseHistory();

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Tìm đơn hàng có trạng thái "Đang xử lý"
                var processingOrder = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//div[contains(@class, 'ant-alert-message') and text()='Đang xử lý']/ancestor::div[contains(@class, 'ant-card-body')]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", processingOrder);
                Thread.Sleep(1000);

                // Tìm nút "Hủy đơn hàng"
                var cancelButton = processingOrder.FindElement(By.XPath(".//button[span[text()='Hủy đơn hàng']]"));

                if (!cancelButton.Displayed || !cancelButton.Enabled)
                {
                    ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_2", "Fail", "Nút 'Hủy đơn hàng' không khả dụng.");
                    return;
                }

                cancelButton.Click();

                // Xác nhận hủy đơn hàng
                var okButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[contains(@class, 'ant-btn-primary') and span[text()='OK']]")));

                okButton.Click();
                Thread.Sleep(3000); // Chờ hệ thống cập nhật

                // Kiểm tra trạng thái đơn hàng
                var orderStatus = processingOrder.FindElement(By.XPath(".//div[contains(@class, 'ant-alert-message')]")).Text;

                if (orderStatus == "Đã hủy")
                {
                    ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_2", "Pass", "Trạng thái đơn hàng đã đổi thành 'Đã hủy'.");
                }
                else
                {
                    ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_2", "Fail", $"Trạng thái đơn hàng vẫn là '{orderStatus}', nút hủy không hoạt động.");
                }
            }
            catch (NoSuchElementException)
            {
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_2", "Fail", "Không tìm thấy đơn hàng hoặc nút hủy.");
            }
        }





        [Test]
        public void ID_XemLSMH_3()
        {
            purchaseHistoryPage.NavigateToPurchaseHistory();

            // Chờ và lấy đơn hàng đầu tiên có trạng thái "Đã giao hàng thành công"
            var orderStatus = wait.Until(ExpectedConditions.ElementExists(
                By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[1]/div[1]//div[contains(@class, 'ant-alert-message') and text()='Đã giao hàng thành công']")));


            try
            {
                // Tìm phần tử cha của đơn hàng
                var orderContainer = orderStatus.FindElement(By.XPath("./ancestor::*[@id='root']/div/div/div/div/div[2]/div/div[1]/div[1]"));

                // Tìm nút "Hủy đơn hàng" trong đơn hàng này
                var cancelButton = orderContainer.FindElement(By.XPath(".//button[span[text()='Hủy đơn hàng']]"));

                if (cancelButton.Displayed && cancelButton.Enabled)
                {
                    cancelButton.Click();
                    ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_3", "Fail", "Nút 'Hủy đơn hàng' có thể click khi đơn hàng đã giao thành công.");
                }
                else
                {
                    ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_3", "Pass", "Nút 'Hủy đơn hàng' không thể click khi đơn hàng đã giao thành công.");
                }
            }
            catch (NoSuchElementException)
            {
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_3", "PASS", "Không có nút 'Hủy đơn hàng' khi đơn hàng đã giao thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi khi tìm phần tử: {ex.Message}");
            }
        }




        [Test]
        public void ID_XemLSMH_4()
        {
            try
            {
                purchaseHistoryPage.NavigateToPurchaseHistory();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                var viewDetailsButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[span[text()='Xem chi tiết']]")));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", viewDetailsButton);
                Thread.Sleep(1000);

                viewDetailsButton.Click();

                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_4", "Pass", "Đã bấm vào 'Xem chi tiết' thành công.");
            }
            catch (WebDriverTimeoutException)
            {
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_4", "Fail", "Không thể tìm thấy nút 'Xem chi tiết' sau 10 giây.");
            }
            catch (Exception ex)
            {
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_4", "Fail", "Lỗi xảy ra: " + ex.Message);
            }
        }




        [Test]
        public void ID_XemLSMH_5()
        {
            try
            {
                purchaseHistoryPage.NavigateToPurchaseHistory();
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                var viewDetailsButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[span[text()='Xem chi tiết']]")));
                viewDetailsButton.Click();
                Thread.Sleep(1000);

                var exportExcelButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//button[span[text()='Xuất file Excel']]")));
                exportExcelButton.Click();

                Thread.Sleep(5000);
                FileInfo latestFile = CustomFileHelper.GetLatestExcelFile();
                if (latestFile != null)
                {
                    Console.WriteLine("File đã tải về: " + latestFile.FullName);
                    CustomFileHelper.OpenFile(latestFile);
                    ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_5", "Pass", "Đã xuất file Excel và mở thành công.");
                }
                else
                {
                    ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_5", "Fail", "Không tìm thấy file Excel.");
                }
            }
            catch (Exception ex)
            {
                ExcelReportHelper_Trân.WriteToExcel("Testcase Trân", "ID_XemLSMH_5", "Fail", "Lỗi: " + ex.Message);
            }
        }

    }
}