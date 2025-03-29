using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using test_salephone.Utilities;

namespace test_salephone.PageObjects
{
    public class PurchaseHistoryPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;


        // Khai báo locators
        private readonly By menuTaiKhoanLocator = By.XPath("//div[@style='cursor: pointer; padding: 5px;']");
        private readonly By lichSuMuaHangLocator = By.XPath("//p[text()='Lịch sử mua hàng']");
        private readonly By ngayDatHangLocator = By.XPath("//div[@class='ant-card-body']//span[contains(text(),'Ngày đặt hàng:')]");
        private readonly By tinhTrangGiaoHangLocator = By.XPath("//div[@class='ant-card-body']//p[contains(text(),'Tình trạng giao hàng:')]//div[@class='ant-alert-message']");
        private readonly By tinhTrangThanhToanLocator = By.XPath("//div[@class='ant-card-body']//p[contains(text(),'Tình trạng thanh toán:')]//div[@class='ant-alert-message']");
        private readonly By tenSanPhamLocator = By.XPath("//div[@class='ant-card-body']//p[1]");
        private readonly By giaSanPhamLocator = By.XPath("//div[@class='ant-card-body']//p[2]");
        private readonly By soLuongLocator = By.XPath("//div[@class='ant-card-body']//p[3]");
        private readonly By tongTienLocator = By.XPath("//span[@class='ant-typography order-total css-qnu6hi']");


        public PurchaseHistoryPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateToPurchaseHistory()
        {
            try
            {
                var menuTaiKhoan = wait.Until(ExpectedConditions.ElementToBeClickable(menuTaiKhoanLocator));
                menuTaiKhoan.Click();

                var lichSuMuaHang = wait.Until(ExpectedConditions.ElementToBeClickable(lichSuMuaHangLocator));
                lichSuMuaHang.Click();

                Console.WriteLine("Đã chuyển đến trang lịch sử mua hàng.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                throw;
            }
        }

        public string GetOrderDetailsOfFirstItem()
        {
            try
            {
                WebDriverWait waitExtended = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                Console.WriteLine("Bắt đầu lấy dữ liệu đơn hàng đầu tiên.");

                // Lấy Ngày đặt hàng
                Console.WriteLine($"Tìm kiếm phần tử Ngày đặt hàng sử dụng locator: {ngayDatHangLocator}");
                string ngayDatHang = waitExtended.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(ngayDatHangLocator)).First().Text;
                Console.WriteLine($"Đã tìm thấy Ngày đặt hàng: {ngayDatHang}");


                // Lấy Tên sản phẩm
                Console.WriteLine($"Tìm kiếm phần tử Tên sản phẩm sử dụng locator: {tenSanPhamLocator}");
                string tenSanPham = waitExtended.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(tenSanPhamLocator)).First().Text;
                Console.WriteLine($"Đã tìm thấy Tên sản phẩm: {tenSanPham}");

                // Lấy Giá sản phẩm
                Console.WriteLine($"Tìm kiếm phần tử Giá sản phẩm sử dụng locator: {giaSanPhamLocator}");
                string giaSanPham = waitExtended.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(giaSanPhamLocator)).First().Text;
                Console.WriteLine($"Đã tìm thấy Giá sản phẩm: {giaSanPham}");

                // Lấy Số lượng
                Console.WriteLine($"Tìm kiếm phần tử Số lượng sử dụng locator: {soLuongLocator}");
                string soLuong = waitExtended.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(soLuongLocator)).First().Text;
                Console.WriteLine($"Đã tìm thấy Số lượng: {soLuong}");

                // Lấy Tổng tiền
                Console.WriteLine($"Tìm kiếm phần tử Tổng tiền sử dụng locator: {tongTienLocator}");
                string tongTien = waitExtended.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(tongTienLocator)).First().Text;
                Console.WriteLine($"Đã tìm thấy Tổng tiền: {tongTien}");

                return $"Ngày đặt hàng: {ngayDatHang}\n" +
                       $"    {tenSanPham}\n" +
                       $"Giá sản phẩm: {giaSanPham}\n" +
                       $"Số lượng: {soLuong}\n" +
                       $"Tổng tiền: {tongTien}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy dữ liệu: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return $"Lỗi khi lấy dữ liệu: {ex.Message}";
            }
        }

        public int GetOrderCount()
        {
            return driver.FindElements(By.XPath("//div[@class='order-item']")).Count; // Thay thế XPath bằng locator phù hợp
        }
        

        public void CancelFirstOrder()
        {
            ReadOnlyCollection<IWebElement> huyDonHangButtons = driver.FindElements(By.XPath("//button[span[text()='Hủy đơn hàng']]"));

            if (huyDonHangButtons.Count == 0)
            {
                ExcelReportHelper.WriteToExcel("Testcase Trân", "ID_XemLSMH_2", "WARNING", "Không tìm thấy đơn hàng có nút 'Hủy đơn hàng'.");
                Console.WriteLine("⚠️ Không tìm thấy đơn hàng có nút 'Hủy đơn hàng'.");
                return;
            }

            IWebElement huyDonHangButton = huyDonHangButtons[0];
            huyDonHangButton.Click();

            try
            {
                IWebElement xacNhanButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[span[text()='OK']]")));
                xacNhanButton.Click();
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//button[span[text()='OK']]")));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("⚠️ Không tìm thấy popup xác nhận hủy đơn hàng hoặc đã bấm ok không thành công.");
            }
        }
        public string GetOrderStatus(IWebElement orderItem)
        {
            // Logic để lấy trạng thái đơn hàng (ví dụ)
            return orderItem.FindElement(By.XPath(".//span[@class='order-status']")).Text;
        }

        public bool IsCancelButtonEnabled(IWebElement orderItem)
        {
            try
            {
                IWebElement cancelButton = orderItem.FindElement(By.XPath(".//button[span[text()='Hủy đơn hàng']]"));
                return cancelButton.Enabled; // Kiểm tra xem nút có hoạt động không
            }
            catch (NoSuchElementException)
            {
                return false; // Nút không tồn tại, tức là không hoạt động
            }
        }
        public bool ClickViewDetailsButton()
        {
            try
            {
                var viewDetailsButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[span[text()='Xem chi tiết']]")));
                viewDetailsButton.Click();
                return true; // ✅ Click thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Không thể bấm vào nút 'Xem chi tiết': " + ex.Message);
                return false; // ❌ Click thất bại
            }
        }

        public void ClickExportExcelButton()
        {
            var exportButton = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//span[text()='Xuất file Excel']")));
            exportButton.Click();
        }

        public string GetOrderDetails()
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                var modalContent = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ant-modal-content")));

                // Trích xuất thông tin người nhận
                string tenNguoiNhan = modalContent.FindElement(By.XPath(".//div[contains(text(), 'Tên người nhận:')]/span")).Text;
                string soDienThoai = modalContent.FindElement(By.XPath(".//div[contains(text(), 'Số điện thoại:')]/span")).Text;
                string diaChiGiaoHang = modalContent.FindElement(By.XPath(".//div[contains(text(), 'Địa chỉ giao hàng:')]/span")).Text;

                // Trích xuất thông tin đơn hàng
                string ngayDatHang = modalContent.FindElement(By.XPath(".//p[contains(text(), 'Ngày đặt hàng:')]/following-sibling::text()[1]")).Text.Trim();
                string trangThaiGiaoHang = modalContent.FindElement(By.XPath(".//p[contains(text(), 'Trạng thái giao hàng:')]/following-sibling::text()[1]")).Text.Trim();
                string trangThaiThanhToan = modalContent.FindElement(By.XPath(".//p[contains(text(), 'Trạng thái thanh toán:')]/following-sibling::text()[1]")).Text.Trim();

                // Trích xuất thông tin sản phẩm
                string tenSanPham = modalContent.FindElement(By.XPath(".//div[contains(@style, 'min-width: 150px;')]")).Text;
                string soLuong = modalContent.FindElement(By.XPath(".//div[contains(@style, 'width: 50px; text-align: center;')]")).Text;
                string donGia = modalContent.FindElement(By.XPath(".//div[contains(@style, 'width: 120px; text-align: right; padding: 0px 13px;')]")).Text;

                // Trích xuất thông tin thanh toán
                string tongTienHang = modalContent.FindElement(By.XPath(".//span[contains(text(), 'Tổng tiền hàng:')]/following-sibling::span")).Text;
                string phiVanChuyen = modalContent.FindElement(By.XPath(".//span[contains(text(), 'Phí vận chuyển:')]/following-sibling::span")).Text;
                string thanhTien = modalContent.FindElement(By.XPath(".//span[contains(text(), 'Thành tiền:')]/following-sibling::span")).Text;
                string phuongThucThanhToan = modalContent.FindElement(By.XPath(".//span[contains(text(), 'Phương thức thanh toán:')]/following-sibling::span")).Text;

                // Tạo chuỗi thông tin chi tiết
                string orderDetails = $"Tên người nhận: {tenNguoiNhan}\n" +
                                       $"Số điện thoại: {soDienThoai}\n" +
                                       $"Địa chỉ giao hàng: {diaChiGiaoHang}\n\n" +
                                       $"Ngày đặt hàng: {ngayDatHang}\n" +
                                       $"Trạng thái giao hàng: {trangThaiGiaoHang}\n" +
                                       $"Trạng thái thanh toán: {trangThaiThanhToan}\n\n" +
                                       $"Tên sản phẩm: {tenSanPham}\n" +
                                       $"Số lượng: {soLuong}\n" +
                                       $"Đơn giá: {donGia}\n\n" +
                                       $"Tổng tiền hàng: {tongTienHang}\n" +
                                       $"Phí vận chuyển: {phiVanChuyen}\n" +
                                       $"Thành tiền: {thanhTien}\n" +
                                       $"Phương thức thanh toán: {phuongThucThanhToan}";

                return orderDetails.Trim();
            }
            catch (WebDriverTimeoutException timeoutEx)
            {
                Console.WriteLine($"TimeoutException: Không tìm thấy element sau 20 giây: {timeoutEx.Message}");
                return null;
            }
            catch (NoSuchElementException noElementEx)
            {
                Console.WriteLine($"NoSuchElementException: Không tìm thấy element: {noElementEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: Lỗi khi lấy thông tin đơn hàng: {ex.Message}");
                return null;
            }
        }
        public bool FindCancelableOrder()
        {
            try
            {
                var orderElements = driver.FindElements(By.XPath("//div[contains(@class, 'order-item')]"));
                foreach (var order in orderElements)
                {
                    var statusElement = order.FindElement(By.XPath(".//span[contains(@class, 'order-status')]"));
                    string status = statusElement.Text.Trim();

                    if (status == "Đang xử lý" || status == "Đang giao hàng")
                    {
                        Console.WriteLine($"✅ Tìm thấy đơn hàng có thể hủy với trạng thái: {status}");

                        // Nhấn vào nút "Hủy đơn hàng" của đơn hàng này
                        var cancelButton = order.FindElement(By.XPath(".//button[contains(text(), 'Hủy đơn hàng')]"));
                        cancelButton.Click();
                        return true;
                    }
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("❌ Không tìm thấy đơn hàng nào có thể hủy.");
            }
            return false;
        }


        public void ClickCancelOrderButton()
        {
            var cancelButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(), 'Hủy đơn hàng')]")));
            cancelButton.Click();
        }

        public void ConfirmCancelOrder()
        {
            var confirmButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(), 'Xác nhận')]")));
            confirmButton.Click();
        }

        public string GetOrderStatus()
        {
            var statusElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(@class, 'order-status')]")));
            return statusElement.Text.Trim();
        }

        public bool IsCancelOrderButtonVisible()
        {
            try
            {
                var cancelButton = driver.FindElement(By.XPath("//button[contains(text(), 'Hủy đơn hàng')]"));
                return cancelButton.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsCancelOrderButtonClickable()
        {
            try
            {
                var cancelButton = driver.FindElement(By.XPath("//button[contains(text(), 'Hủy đơn hàng')]"));
                return cancelButton.Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public IWebElement FindDeliveredOrder()
        {
            try
            {
                Console.WriteLine("🔍 Đang tìm đơn hàng có trạng thái 'Đã giao hàng thành công'...");

                // Xác định tất cả các đơn hàng trong trang lịch sử
                var orders = driver.FindElements(By.CssSelector("div[data-show='true'].ant-alert-success"));

                foreach (var order in orders)
                {
                    if (order.Text.Contains("Đã giao hàng thành công"))
                    {
                        Console.WriteLine("✅ Đã tìm thấy đơn hàng đã giao thành công.");
                        return order;
                    }
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("❌ Không tìm thấy đơn hàng đã giao thành công.");
            }

            return null;
        }
        public bool CancelProcessingOrder(string orderDate, StringBuilder logOutput)
        {
            try
            {
                logOutput.AppendLine($"🔍 Tìm đơn hàng có ngày đặt: {orderDate}...");
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                // Tìm đơn hàng có ngày đặt hàng cụ thể
                var orderElement = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//span[contains(text(), '{orderDate}')]/ancestor::div[contains(@class, 'order-container')]")));

                if (orderElement != null)
                {
                    logOutput.AppendLine("✅ Tìm thấy đơn hàng. Kiểm tra trạng thái...");

                    // Kiểm tra trạng thái "Đang xử lý"
                    var statusElement = orderElement.FindElement(By.XPath(".//div[contains(@class, 'ant-alert-info') and .//div[text()='Đang xử lý']"));

                    if (statusElement != null)
                    {
                        logOutput.AppendLine("✅ Đơn hàng đang ở trạng thái 'Đang xử lý'. Tiến hành hủy...");

                        // Tìm nút "Hủy đơn hàng"
                        var cancelButton = orderElement.FindElement(By.XPath(".//button[span[text()='Hủy đơn hàng']]"));

                        if (cancelButton.Displayed && cancelButton.Enabled)
                        {
                            logOutput.AppendLine("🖱 Click vào nút 'Hủy đơn hàng'...");
                            cancelButton.Click();
                            Thread.Sleep(3000);

                            // Xác nhận hủy đơn hàng
                            var confirmButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[span[text()='OK']]")));
                            confirmButton.Click();
                            Thread.Sleep(5000);

                            // Kiểm tra lại xem đơn hàng có còn tồn tại không
                            driver.Navigate().Refresh();
                            Thread.Sleep(5000);

                            try
                            {
                                wait.Until(ExpectedConditions.ElementExists(By.XPath($"//span[contains(text(), '{orderDate}')]")));
                                logOutput.AppendLine("❌ Đơn hàng vẫn tồn tại. Hủy đơn thất bại!");
                                return false;
                            }
                            catch (WebDriverTimeoutException)
                            {
                                logOutput.AppendLine("✅ Đơn hàng đã bị xóa khỏi danh sách. Hủy thành công!");
                                return true;
                            }
                        }
                        else
                        {
                            logOutput.AppendLine("❌ Nút 'Hủy đơn hàng' không khả dụng!");
                        }
                    }
                    else
                    {
                        logOutput.AppendLine("⚠️ Đơn hàng không ở trạng thái 'Đang xử lý'.");
                    }
                }
                else
                {
                    logOutput.AppendLine("⚠️ Không tìm thấy đơn hàng với ngày đặt hàng cụ thể.");
                }
            }
            catch (NoSuchElementException)
            {
                logOutput.AppendLine("⚠️ Không tìm thấy đơn hàng hoặc nút hủy.");
            }

            return false;
        }


        public IWebElement GetCancelButton(IWebElement orderItem)
        {
            try
            {
                return orderItem.FindElement(By.XPath(".//button[contains(text(), 'Hủy đơn hàng')]"));
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

    }




}



    