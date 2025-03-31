using test_salephone.Utilities;
using OpenQA.Selenium;
using test_salephone.PageObjects;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports.Model;
// dung de hover
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;


namespace test_salephone.client
{
    [TestFixture]
    class Test_QLGH : TestBase
    {
        private WebDriverWait wait;
        [SetUp]
        public override void Setup()
        {
            // driver.Navigate().GoToUrl("https://salephone.mysapo.vn/");
            base.Setup();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
        }
        public void login()
        {
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[4]/div")).Click();
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[1]/input")).SendKeys("ngocduy1423@gmail.com");
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[2]/span/input")).SendKeys("Duy123123123");

            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[3]/div/div/button")).Click();
            Thread.Sleep(5000);

            // kiem tra da vao trnag home chua?
            // IWebElement home = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[3]/div/div[2]"));
            // Assert.IsTrue(home.Displayed);
        }

        public bool themMotSanPham(string number, string numberCart, int numberProduct)
        {

            // int number_cart = int.Parse(driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[3]/div/span/sup/bdi/span")).Text);
            try
            {
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }
        [Category("QLGH")]
        [Test]
        public void ID_QLGioHang_01_ThemSanPhamKhiChuaDangNhap()
        {
            string res = "fail";
            // login();
            // Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[4]/div/div/div[2]")).Click();
            Thread.Sleep(5000);
            if (themMotSanPham("2", "1", 0))
            {
                ExcelReportHelper.WriteToExcel("testCase Duy", "ID_QLGioHang_01", res, "Thêm sản phẩm thành công");

            }
            else
            {
                res = "pass";
                ExcelReportHelper.WriteToExcel("testCase Duy", "ID_QLGioHang_01", res, "Thêm sản phẩm thất bại");
            }
        }
        [Test]

        [TestCase("2", "1", "ID_QLGioHang_02", TestName = "ID_QLGioHang_02(ThemKhiDaDangNhap)")]
        [TestCase("9999", "0", "ID_QLGioHang_03", TestName = "ID_QLGioHang_03(ThemSanPhamVuotQuaKho)")]
        [TestCase("0", "0", "ID_QLGioHang_05", TestName = "ID_QLGioHang_05(ThemSanPhamVoiSoLuong=0)")]
        [TestCase("", "", "ID_QLGioHang_06", TestName = "ID_QLGioHang_06(ThemNhieuSanPhamKhacNhauVaoGio)")]
        [TestCase("-2", "0", "ID_QLGioHang_07", TestName = "ID_QLGioHang_07(ThemSanPhamVoiSoLuongAm)")]
        [TestCase("", "", "ID_QLGioHang_08", TestName = "ID_QLGioHang_08(ThemMotSanPhamNhieuLan)")]
        public void ThemSanPham(string numbercart, string solanthem, string IDTest)
        {
            string res = "fail";
            login();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[4]/div/div/div[2]")).Click();
            Thread.Sleep(5000);
            if (IDTest == "ID_QLGioHang_06")
            {
                if (themMotSanPham("1", "1", 0) && themMotSanPham("1", "2", 1))
                {
                    res = "pass";
                    ExcelReportHelper.WriteToExcel("testCase Duy", IDTest, res);
                }
                else
                {
                    ExcelReportHelper.WriteToExcel("testCase Duy", IDTest, res);
                }
            }
            if (IDTest == "ID_QLGioHang_08")
            {
                if (themMotSanPham("1", "1", 0) && themMotSanPham("1", "2", 0) && themMotSanPham("1", "3", 0))
                {
                    res = "pass";
                    ExcelReportHelper.WriteToExcel("testCase Duy", IDTest, res);
                }
                else
                {
                    ExcelReportHelper.WriteToExcel("testCase Duy", IDTest, res);
                }
            }
            else
            {
                if (themMotSanPham(numbercart, solanthem, 0))
                {
                    res = "pass";
                    ExcelReportHelper.WriteToExcel("testCase Duy", IDTest, res);
                }
                else
                {
                    ExcelReportHelper.WriteToExcel("testCase Duy", IDTest, res);
                }
            }

        }


        [Test]
        [Category("update_GH")]
        //
        [TestCase("3", "ID_QLGioHang_09", TestName = "ID_QLGioHang_09(Tăng số lượng)")]
        [TestCase("-6", "ID_QLGioHang_10", TestName = "ID_QLGioHang_10(Giảm số lượng)")]
        [TestCase("-10", "ID_QLGioHang_11", TestName = "ID_QLGioHang_11(Giảm về 0)")]
        [TestCase("-10", "ID_QLGioHang_11", TestName = "ID_QLGioHang_11(Giảm về 0)")]
        [TestCase("100", "ID_QLGioHang_12", TestName = "ID_QLGioHang_12(Tăng số lượng sao cho hơn số lượng trong kho)")]
        [TestCase("-12", "ID_QLGioHang_13", TestName = "ID_QLGioHang_13(Giảm số lượng về âm)")]
        [TestCase(" ", "ID_QLGioHang_14", TestName = "ID_QLGioHang_14(Nhập với dữ liệu trống)")]
        [TestCase("QDADWEDASs", "ID_QLGioHang_15", TestName = "ID_QLGioHang_15(Nhập vào ký tự)")]


        public void CapNhapSoLuong(string number, string ID)
        {

            CheckCart cart = new CheckCart(driver);
            string res = "fail";
            login();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[4]/div/div/div[2]")).Click();
            Thread.Sleep(5000);
            if (themMotSanPham("10", "1", 0))
            {
                Console.WriteLine("Đã thêm sản phẩm vào giỏ hàng");
            }
            else
            {
                Console.WriteLine("Đặt thất bại");
                return;
            }

            cart.OpenCart();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/h3")));
            bool hasProduct = cart.IsProductInCart();
            Console.WriteLine($"Giỏ hàng có sản phẩm không?{hasProduct}");
            if (hasProduct)
            {
                IWebElement container = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[1]/div[2]"));
                IList<IWebElement> products = container.FindElements(By.ClassName("sc-hJRrWL"));
                // cap nhap so luong
                Console.WriteLine("Product count: " + products.Count());
                IWebElement product = products[products.Count() - 1];
                // IWebElement inputNumber = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[1]/div/div/div/div/div[2]/div[1]/div[1]/div[3]/div/div/input"));
                IWebElement inputNumber = product.FindElement(By.ClassName("ant-input-number-input"));

                if (ID != "ID_QLGioHang_14" && ID != "ID_QLGioHang_15")
                {
                    // hover
                    Actions actions = new Actions(driver);
                    actions.MoveToElement(inputNumber).Perform();
                    Thread.Sleep(1000);
                    IWebElement buttonUp = product.FindElement(By.ClassName("ant-input-number-handler-up"));
                    IWebElement buttonDown = product.FindElement(By.ClassName("ant-input-number-handler-down"));
                    if (int.Parse(number) > 0)
                    {
                        for (int i = 0; i < int.Parse(number); i++)
                        {
                            buttonUp.Click();

                            if (ID != "ID_QLGioHang_12")
                            {
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                Thread.Sleep(1);
                            }
                        }
                        Console.WriteLine($"SL la: ${inputNumber.GetAttribute("value")}");
                        if ((int.Parse(inputNumber.GetAttribute("value")) + int.Parse(number) - int.Parse(inputNumber.GetAttribute("value"))) == int.Parse(number))
                        {
                            ExcelReportHelper.WriteToExcel("testCase Duy", ID, res, "Cập nhập Thành công");
                            return;
                        }
                    }
                    else
                    {

                        for (int i = 0; i < -int.Parse(number); i--)
                        {
                            string buttonClass = buttonDown.GetAttribute("class");
                            Console.WriteLine("Class của button: " + buttonClass);

                            if (buttonClass.Contains("ant-input-number-handler-down-disabled"))
                            {
                                ExcelReportHelper.WriteToExcel("testCase Duy", ID, res, "Không thể click về 0");
                                Console.WriteLine("Button is disabled.");
                                return;
                            }
                            else
                            {
                                buttonDown.Click();
                            }
                        }
                        Console.WriteLine($"SL la: ${inputNumber.GetAttribute("value")}");

                        if (int.Parse(inputNumber.GetAttribute("value")) + int.Parse(number) - int.Parse(number) == int.Parse(inputNumber.GetAttribute("value")))
                        {
                            ExcelReportHelper.WriteToExcel("testCase Duy", ID, res, "Cập nhập Thành công");
                            return;
                        }
                    }

                    // driver.Navigate().Refresh();
                    //
                    ExcelReportHelper.WriteToExcel("testCase Duy", ID, res, "Cập nhập thất bại");

                    // wait.Until(Ex)
                }
                else
                {
                    nhapSoLuongVaoInput(inputNumber, number);
                    Thread.Sleep(2000);
                    inputNumber.SendKeys(Keys.Enter);
                    Console.WriteLine("da submit " + inputNumber.GetAttribute("value"));
                    Thread.Sleep(2000);
                    if (inputNumber.GetAttribute("value").Equals(number.Trim()))
                    {
                        ExcelReportHelper.WriteToExcel("testCase Duy", ID, res, "Dữ liệu nhập vào đúng với tham số đầu vô");
                    }
                    else if (inputNumber.GetAttribute("value").Equals("10"))
                    {
                        res = "pass";
                        ExcelReportHelper.WriteToExcel("testCase Duy", ID, res, "Dữ liệu bị default về giá trị cũ");
                    }
                    else
                    {
                        Console.WriteLine("Lỗi code");
                        ExcelReportHelper.WriteToExcel("testCase Duy", ID, res, "Cập nhập thất bại");
                    }

                }
            }
            else
            {
                // IWebElement inputNumber = product.FindElement(By.ClassName("ant-input-number-input"));
                Console.WriteLine("Sản phẩm không tồn tại trong giỏ hàng");
                ExcelReportHelper.WriteToExcel("testCase Duy", ID, res, "Sản phẩm không tồn tại");
            }
        }

        void nhapSoLuongVaoInput(IWebElement input, string thamso)
        {
            Actions actions = new Actions(driver);
            actions.Click(input)
                   .KeyDown(Keys.Control)
                   .SendKeys("a")
                   .KeyUp(Keys.Control)
                   .SendKeys(Keys.Delete)
                   .SendKeys(thamso)   // 'thamso' là chuỗi giá trị bạn muốn nhập
                   .Perform();
        }

        // [Test]
        // [Category("Delete_GH")]
        [TestCase("1", "ID_QLGioHang_16", TestName = "ID_QLGioHang_16(Xóa 1 sản phẩm)")]
        [TestCase("full", "ID_QLGioHang_18", TestName = "ID_QLGioHang_18(Xóa tất sản phẩm)")]
        public void Delete_GH(string sl, string id)
        {
            string res = "Fail";
            login();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[4]/div/div/div[2]")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#root > div > div > div > div > div.sc-bbQqnZ.kdmDEE > div.ant-spin-nested-loading.css-qnu6hi > div > div.sc-lnsxGb.cnbrWm")));
            themMotSanPham("1", "1", 0);
            Thread.Sleep(2000);
            driver.Navigate().Back();


            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#root > div > div > div > div > div.sc-bbQqnZ.kdmDEE > div.ant-spin-nested-loading.css-qnu6hi > div > div.sc-lnsxGb.cnbrWm")));

            themMotSanPham("2", "2", 2);
            Thread.Sleep(2000);
            int i = 3;
            if (id == "ID_QLGioHang_17")
            {
                driver.Navigate().Back();
                for (int j = 0; j < 2; j++)
                {
                    wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#root > div > div > div > div > div.sc-bbQqnZ.kdmDEE > div.ant-spin-nested-loading.css-qnu6hi > div > div.sc-lnsxGb.cnbrWm")));
                    themMotSanPham("1", "1", i);
                    Thread.Sleep(2000);
                    driver.Navigate().Back();
                    i++;
                }
            }
            Console.WriteLine("Them san pham thanh cong !");
            CheckCart cart = new CheckCart(driver);
            cart.OpenCart();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/h3")));
            bool hasProduct = cart.IsProductInCart();
            Console.WriteLine($"Giỏ hàng có sản phẩm không?{hasProduct}");
            if (hasProduct)
            {
                IWebElement container = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[1]/div[2]"));
                IList<IWebElement> products = container.FindElements(By.ClassName("sc-hJRrWL"));
                // cap nhap so luong
                Console.WriteLine("Product count: " + products.Count());
                var preNumber = products.Count();
                if (sl == "full")
                {
                    choiceProduct(products, preNumber.ToString());
                }
                else if (preNumber >= int.Parse(sl))
                {
                    choiceProduct(products, sl);
                }
                else
                {
                    Console.WriteLine("Số lượng sản phẩm trong giỏ hàng không đủ");
                    return;
                }

                // IWebElement product = products[products.Count() - 1];

                // IWebElement buttonDelete = product.FindElement(By.ClassName("anticon-delete"));
                // buttonDelete.Click();
                // Thread.Sleep(2000);

                driver.Navigate().Refresh();
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/h3")));
                IWebElement container_check = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div[1]/div[2]"));
                IList<IWebElement> products_check = container_check.FindElements(By.ClassName("sc-hJRrWL"));
                int soluong = sl == "full" ? preNumber : int.Parse(sl);
                if (products_check.Count() == preNumber - soluong)
                {
                    res = "pass";
                    ExcelReportHelper.WriteToExcel("testCase Duy", id, res, "Xóa sản phẩm thành công");
                    Console.WriteLine("Đã kiểm tra là xác nhận đã xóa");
                    Console.WriteLine("so luong sau khi xoa la: ", products_check.Count());
                }
                else
                {
                    ExcelReportHelper.WriteToExcel("testCase Duy", id, res, "Xóa sản phẩm thất bại");
                    Console.WriteLine("Kiểm tra và xác nhận thất bại");
                }
            }
            else
            {
                ExcelReportHelper.WriteToExcel("testCase Duy", id, res, "Sản phẩm không tồn tại trong giỏ hàng !");

                Console.WriteLine("Sản phẩm không tồn tại trong giỏ hàng");
                return;
            }
        }

        void choiceProduct(IList<IWebElement> products, string sl)
        {
            Console.WriteLine("so luong san pham: " + products.Count());

            for (int i = 0; i < int.Parse(sl); i++)
            {
                IList<IWebElement> products_check = driver.FindElements(By.ClassName("sc-hJRrWL"));

                int index = products_check.Count();
                IWebElement product = products[index - 1];
                IWebElement buttonDelete = product.FindElement(By.ClassName("anticon-delete"));
                buttonDelete.Click();
            }
            Console.WriteLine("Xóa sản phẩm thành công");
        }


        [Category("View_GH")]
        [TestCase("ID_QLGioHang_23", TestName = "ID_QLGioHang_19(Xem giỏ hàng khi logout)")]
        [TestCase("ID_QLGioHang_24", TestName = "ID_QLGioHang_24(Xem giỏ hàng khi đã đăng nhập)")]
        public void View_GH(string id)
        {
            login();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[4]/div/div/div[2]")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#root > div > div > div > div > div.sc-bbQqnZ.kdmDEE > div.ant-spin-nested-loading.css-qnu6hi > div > div.sc-lnsxGb.cnbrWm")));
            themMotSanPham("1", "1", 0);
            Thread.Sleep(2000);

            //hover
            if (id != "ID_QLGioHang_24")
            {
                Actions actions = new Actions(driver);
                IWebElement userIcon = driver.FindElement(By.ClassName("GGVwt"));
                actions.MoveToElement(userIcon).Perform();
                // Thread.Sleep(2000);
                // WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                IWebElement container_hover = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ant-popover-inner")));

                IWebElement logoutButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//p[contains(text(), 'Đăng xuất')]")));
                logoutButton.Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[4]/div")));
                Console.WriteLine("Đăng xuất thành công");
            }


            CheckCart cart = new CheckCart(driver);
            cart.OpenCart();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/h3")));
            Console.WriteLine("Da vao trang gio hang");
            bool hasProduct = cart.IsProductInCart();
            if (hasProduct)
            {
                if (id != "ID_QLGioHang_24")
                {
                    ExcelReportHelper.WriteToExcel("testCase Duy", id, "fail", "Giỏ hàng vẫn xem được");
                    Console.WriteLine("Giỏ hàng vẫn xem được");

                }
                else
                {
                    ExcelReportHelper.WriteToExcel("testCase Duy", id, "pass", "Xem thành công");
                    Console.WriteLine("Xem giỏ hàng thành công");
                }
            }
            else
            {
                if (id == "ID_QLGioHang_24")
                {
                    ExcelReportHelper.WriteToExcel("testCase Duy", id, "pass", "Không xem được giỏ hàng");
                    Console.WriteLine("Không xem được giỏ hàng");
                }
                else
                {
                    ExcelReportHelper.WriteToExcel("testCase Duy", id, "fail", "Không xem được giỏ hàng");
                    Console.WriteLine("Không xem được giỏ hàng");
                }

            }
        }
    }
}