using System.Collections.Generic;
using NUnit.Framework;
using test_salephone.Utilities;
using test_salephone;
using OpenQA.Selenium;
using test_salephone.PageObjects;

namespace test_salephone.client{
    [TestFixture]
    class Test_QLGH : TestBase
    {


        public void login(){
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[4]/div")).Click();
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[1]/input")).SendKeys("ngocduy1423@gmail.com");
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[2]/span/input")).SendKeys("Duy123123123");

            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[3]/div/div/button")).Click();
            Thread.Sleep(5000);

            // kiem tra da vao trnag home chua?
            // IWebElement home = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[3]/div/div[2]"));
            // Assert.IsTrue(home.Displayed);
        }

        public bool themMotSanPham(string number, string numberCart, int numberProduct){
            
            // int number_cart = int.Parse(driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[3]/div/span/sup/bdi/span")).Text);
            try{
                IWebElement container_product = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div[1]"));

                IList<IWebElement> products = container_product.FindElements(By.ClassName("ant-card"));
                
                if (products.Count > 1)  // Kiểm tra có nhiều sản phẩm không
                {
                    products[numberProduct].Click();  // Chọn sản phẩm thứ 2 (index = 1)
                }
                else
                {
                    Console.WriteLine("Không có đủ sản phẩm để chọn.");
                }

                // product.Click();

                IWebElement inputNumber = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[1]/div/div/div/div/div[2]/div[1]/div[1]/div[3]/div/div/input"));
                inputNumber.Click();
                inputNumber.SendKeys(Keys.Control + "a");  // Chọn toàn bộ văn bản
                inputNumber.SendKeys(Keys.Delete);  // Xóa nội dung đã chọn
                inputNumber.SendKeys(number);  // Nhập giá trị mới

                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[1]/div/div/div/div/div[2]/div[2]/button")).Click();
                // Assert.That(number_cart == number_cart + number);
                Thread.Sleep(5000);
                IWebElement cart = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[3]/div"));
                
                IWebElement sup = cart.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[3]/div/span/sup"));
                string itemCount = sup.GetAttribute("title");

                
                Assert.That(itemCount == numberCart, "Số lượng sản phẩm trong giỏ hàng đúng!");

                Thread.Sleep(5000);
                return true;
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
            
        }
        [Category("QLGH")]
        [Test]
        public void ID_QLGioHang_01_ThemSanPhamKhiChuaDangNhap(){
            string res = "fail";
            // login();
            // Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[4]/div/div/div[2]")).Click();
            Thread.Sleep(5000);
            if(themMotSanPham("2", "1",0)){
                res = "pass";
                ExcelReportHelper.WriteToExcel("testCase_Duy", "ID_QLGioHang_01", res);
            }
            else{
                ExcelReportHelper.WriteToExcel("testCase_Duy", "ID_QLGioHang_01", res);
            }
        }
        [Test]

        [TestCase("2","1","ID_QLGioHang_02", TestName = "ID_QLGioHang_02(ThemKhiDaDangNhap)")]
        [TestCase("9999","0","ID_QLGioHang_03",TestName = "ID_QLGioHang_03(ThemSanPhamVuotQuaKho)")]
        [TestCase("0","0","ID_QLGioHang_05",TestName = "ID_QLGioHang_05(ThemSanPhamVoiSoLuong=0)")]
        [TestCase("","","ID_QLGioHang_06",TestName = "ID_QLGioHang_06(ThemNhieuSanPhamKhacNhauVaoGio)")]
        [TestCase("-2","0","ID_QLGioHang_07",TestName = "ID_QLGioHang_07(ThemSanPhamVoiSoLuongAm)")]
        [TestCase("","","ID_QLGioHang_08",TestName = "ID_QLGioHang_08(ThemMotSanPhamNhieuLan)")]
        public void ThemSanPham(string numbercart, string solanthem, string IDTest){
            string res = "fail";
            login();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[4]/div/div/div[2]")).Click();
            Thread.Sleep(5000);
            if(IDTest == "ID_QLGioHang_06"){
                if(themMotSanPham("1","1",0) && themMotSanPham("1","2",1)){
                    res = "pass";
                    ExcelReportHelper.WriteToExcel("testCase_Duy", IDTest, res);
                }
                else{
                    ExcelReportHelper.WriteToExcel("testCase_Duy", IDTest, res);
                }
            }if(IDTest == "ID_QLGioHang_08"){
                if(themMotSanPham("1","1",0) && themMotSanPham("1","2",0) && themMotSanPham("1","3",0)){
                    res = "pass";
                    ExcelReportHelper.WriteToExcel("testCase_Duy", IDTest, res);
                }
                else{
                    ExcelReportHelper.WriteToExcel("testCase_Duy", IDTest, res);
                }
            }
            else{
                if(themMotSanPham(numbercart, solanthem,0)){
                    res = "pass";
                    ExcelReportHelper.WriteToExcel("testCase_Duy", IDTest, res);
                }
                else{
                    ExcelReportHelper.WriteToExcel("testCase_Duy", IDTest, res);
                }
            }

        }
    }
}