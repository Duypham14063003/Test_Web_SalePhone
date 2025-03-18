using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using test_salephone.Helpers;

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
    }

}

    //public bool VerifyOrderDetails(string orderDetails)
    //{
    //    // Thông tin mong đợi (Expected Result) từ file Excel
    //    string expectedProductName = "SP Test";
    //    string expectedPriceString = "10.999 VND";

    //    // Loại bỏ " VND" và dấu phân cách hàng nghìn
    //    string priceString = expectedPriceString.Replace(" VND", "").Replace(".", "");

    //    // Chuyển đổi chuỗi thành decimal
    //    if (decimal.TryParse(priceString, out decimal expectedPrice))
    //    {
    //        // Chuyển đổi thành công, expectedPrice chứa giá trị decimal
    //        Console.WriteLine($"Giá trị decimal: {expectedPrice}");
    //    }
    //    else
    //    {
    //        // Chuyển đổi thất bại, xử lý lỗi
    //        Console.WriteLine("Lỗi: Không thể chuyển đổi chuỗi thành decimal.");
    //        return false; // Trả về false nếu chuyển đổi thất bại
    //    }

    //    int expectedQuantity = 1;

    //    // Regular Expressions để trích xuất thông tin
    //    string productNamePattern = @"Tên sản phẩm:\s*(.*?)\s*"; // Sửa regex để không dùng dấu nháy kép
    //    string pricePattern = @"Giá sản phẩm:\s*([\d.]+)\s*"; // Sửa regex để trích xuất cả dấu chấm
    //    string quantityPattern = @"Số lượng:\s*(\d+)\s*"; // Sửa regex để không dùng dấu nháy kép

    //    // Trích xuất thông tin
    //    string actualProductName = Regex.Match(orderDetails, productNamePattern).Groups[1].Value.Trim();
    //    string priceStringFromOrder = Regex.Match(orderDetails, pricePattern).Groups[1].Value.Trim(); // Đổi tên biến tránh trùng lặp
    //    string quantityString = Regex.Match(orderDetails, quantityPattern).Groups[1].Value.Trim();

    //    // Kiểm tra và so sánh
    //    bool isProductNameCorrect = string.Equals(expectedProductName, actualProductName, StringComparison.Ordinal);
    //    decimal actualPrice;
    //    bool isPriceCorrect = decimal.TryParse(priceStringFromOrder.Replace(".", ""), out actualPrice) && actualPrice == expectedPrice; // loại bỏ dấu chấm để so sánh
    //    int actualQuantity;
    //    bool isQuantityCorrect = int.TryParse(quantityString, out actualQuantity) && actualQuantity == expectedQuantity;

    //    // Trả về kết quả
    //    return isProductNameCorrect && isPriceCorrect && isQuantityCorrect;
    //}


    