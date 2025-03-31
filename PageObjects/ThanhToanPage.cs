
using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace test_salephone.PageObjects
{
    public class ThanhToanPageObject
    {

        private readonly IWebDriver driver;
        // Danh sách các phần tử của trang Thanh Toán
        // private readonly By tenNguoiNhan = By.XPath("//input[@placeholder='Họ và tên']");

        // button thanh toan
        private readonly By thanhToanButton = By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[1]/div/div[2]/button");

        private readonly By muaHangButton = By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[2]/button");

        private readonly By buttonthanhToanBangPayPal = By.XPath("//*[@id='buttons-container']/div/div[1]/div");
        // button chon all san pham
        private readonly By chonTatCaSanPhamButton = By.CssSelector(".ant-checkbox-input");

        //container thong tin dat hang
        private readonly By containerThongTinDatHang = By.XPath("//div[@class='ant-modal-content']");

        // ten trong thong tin dat hang
        private readonly By tenTrongThongTinDatHang = By.Id("basic_name");

        // ten thanh pho trong thong tin dat hang
        private readonly By tenThanhPhoTrongThongTinDatHang = By.Id("basic_city");

        // so dien thoai trong thong tin dat hang
        private readonly By soDienThoaiTrongThongTinDatHang = By.Id("basic_phone");

        // dia chi nha trong thong tin dat hang
        private readonly By diaChiNhaTrongThongTinDatHang = By.Id("basic_address");

        // phuong thuc giao hang
        private readonly By phuongThucGiaoHangDefault = By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[1]/div/div[1]/div[1]/div/div/label[1]/span[1]");

        // phuong thuc thanh toan
        private readonly By phuongThucThanhToanKhiNhanHang = By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[1]/div/div[1]/div[2]/div/div/label[1]/span[1]");

        private readonly By phuongThucThanhToanBangPayPal = By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[1]/div/div[1]/div[2]/div/div/label[2]/span[1]");

        private readonly By Text_DatHang_ThanhCong = By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div/h1");
        public ThanhToanPageObject(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void InputTenNguoiNhan(string tennguoinhan)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement tenNguoiNhanElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(tenTrongThongTinDatHang));
            tenNguoiNhanElement.Click();
            tenNguoiNhanElement.SendKeys(Keys.Command + "a");
            tenNguoiNhanElement.SendKeys(Keys.Delete);
            tenNguoiNhanElement.SendKeys(tennguoinhan);
        }
        public void InputTenThanhPho(string tenthanhpho)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement tenThanhPhoElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(tenThanhPhoTrongThongTinDatHang));
            tenThanhPhoElement.Click();
            tenThanhPhoElement.SendKeys(Keys.Command + "a");
            tenThanhPhoElement.SendKeys(Keys.Delete);
            tenThanhPhoElement.SendKeys(tenthanhpho);
        }
        public void InputSoDienThoai(string sodienthoai)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement soDienThoaiElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(soDienThoaiTrongThongTinDatHang));
            soDienThoaiElement.Click();
            soDienThoaiElement.SendKeys(Keys.Command + "a");
            soDienThoaiElement.SendKeys(Keys.Delete);
            soDienThoaiElement.SendKeys(sodienthoai);
        }
        public void InputDiaChiNha(string diachinha)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement diaChiNhaElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(diaChiNhaTrongThongTinDatHang));
            diaChiNhaElement.Click();
            diaChiNhaElement.SendKeys(Keys.Command + "a");
            diaChiNhaElement.SendKeys(Keys.Delete);
            diaChiNhaElement.SendKeys(diachinha);
        }
        public bool ClickThanhToanButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement thanhToanButtonElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(thanhToanButton));
            thanhToanButtonElement.Click();
            return true;
        }
        public void ClickChonTatCaSanPhamButton()
        {
            IWebElement checkbox = driver.FindElement(By.CssSelector(".ant-checkbox-input"));
            checkbox.Click();
            // chonTatCaSanPhamButtonElement.Click();
        }

        public bool IsThanhToanSuccess()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement containerThongTinDatHangElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(Text_DatHang_ThanhCong));
            return containerThongTinDatHangElement.Displayed;
        }

        public void chonPhuongThucGH()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement phuongThucThanhToanKhiNhanHangElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(phuongThucGiaoHangDefault));
            phuongThucThanhToanKhiNhanHangElement.Click();
        }

        public void chonPhuongThucTT()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement phuongThucThanhToanKhiNhanHangElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(phuongThucThanhToanKhiNhanHang));
            phuongThucThanhToanKhiNhanHangElement.Click();
        }
        public void chonPhuongThucPayPal()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement phuongThucThanhToanKhiNhanHangElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(phuongThucThanhToanBangPayPal));
            phuongThucThanhToanKhiNhanHangElement.Click();
        }

        public void ClickMuaHangButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement muaHangButtonElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(muaHangButton));
            muaHangButtonElement.Click();
        }

        public void thanhToanBangPayPal()
        {
            chonPhuongThucPayPal();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement thanhToanBangPayPalElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(buttonthanhToanBangPayPal));
            thanhToanBangPayPalElement.Click();

        }
        public void IsDiscountApplied()
        {

        }
        public bool ThayDoiDiaChi(string ten, string tp, string sdt, string dc)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[2]/div[1]/div[1]/div/span[3]")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(containerThongTinDatHang));
            InputTenNguoiNhan(ten);
            InputTenThanhPho(tp);
            InputSoDienThoai(sdt);
            InputDiaChiNha(dc);
            // ClickThanhToanButton();
            // IWebElement dad = driver.FindElement(containerThongTinDatHang);
            driver.FindElement(By.XPath("/html/body/div[3]/div/div[2]/div/div[2]/div/div[3]/button[2]")).Click();

            IWebElement notification = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(".ant-message-notice-content")));
            string toastContent = notification.Text.ToLower();
            Console.WriteLine("toast mess: " + toastContent);
            bool isSuccess = toastContent.Contains("thêm sản phẩm vào giỏ hàng thành công!");
            return isSuccess;
        }

        public bool MuaHangVoiThanhToanKhiNhanHang()
        {
            ClickChonTatCaSanPhamButton();
            ClickMuaHangButton();
            chonPhuongThucGH();
            chonPhuongThucTT();
            ClickThanhToanButton();
            return IsThanhToanSuccess();
        }
    };

}