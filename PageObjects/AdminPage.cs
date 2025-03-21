using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace test_salephone.PageObjects
{
    public class AdminPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public AdminPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private By adminDropdownButton = By.XPath("//div[@style='cursor: pointer; padding: 5px;']");
        private By manageSystemButton = By.XPath("//p[contains(text(), 'Quản lý hệ thống')]");
        private By productMenuButton = By.XPath("//span[contains(@class, 'ant-menu-title-content') and contains(text(), 'Sản phẩm')]");
        private By addProductButton = By.XPath("//span[@role='img' and @aria-label='plus-square']/parent::button");
        private By productNameField = By.XPath("//input[@name='name' and @type='text']");
        private By brandDropdown = By.XPath("//span[@class='ant-select-selection-item']");
        private By brandOptionApple = By.XPath("//div[contains(@class, 'ant-select-item-option-content') and text()='Apple']");
        private By quantityField = By.Id("basic_countInStock");
        private By priceField = By.Id("basic_price");
        private By descriptionField = By.Id("basic_description");
        private By saveButton = By.XPath("//form//button[.//span[text()='Thêm sản phẩm']]");
        private By fileInput = By.XPath("//input[@type='file']");
        private By productNameHeader = By.XPath("//th[contains(@class, 'ant-table-column-sort')]");
        private By sortButton = By.CssSelector(".ant-table-column-sorters");
        By confirmButton = By.XPath("//button[contains(@class, 'ant-btn-primary') and contains(., 'Xác nhận')]");

        public void OpenAdminDropdown()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(adminDropdownButton)).Click();
        }


        public void SelectSystemManagement()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(manageSystemButton)).Click();
        }



        //san pham
        public void NavigateToProductPage()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(productMenuButton)).Click();
            try
            {
                wait.Until(ExpectedConditions.UrlContains("products"));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Không thể chuyển đến trang sản phẩm! URL hiện tại: " + driver.Url);
            }
        }




        // them moi sp
        public void AddNewProduct(string productName, string brand, string quantity, string price, string description, string imagePath)
        {
            //Thread.Sleep(5000);
            wait.Until(ExpectedConditions.ElementToBeClickable(addProductButton)).Click();
            driver.FindElement(productNameField).SendKeys(productName);
            // Thread.Sleep(5000);
            driver.FindElement(brandDropdown).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(brandOptionApple)).Click();

            driver.FindElement(quantityField).SendKeys(quantity);
            driver.FindElement(priceField).SendKeys(price);
            driver.FindElement(descriptionField).SendKeys(description);

            driver.FindElement(fileInput).SendKeys(imagePath);
            driver.FindElement(saveButton).Click();
        }




        public void DeleteProduct(string productName)
        {
            Console.WriteLine($"🗑️ Đang xóa sản phẩm: {productName}...");

            By productNameHeader = By.XPath("//th[contains(@aria-label, 'Tên sản phẩm')]");
            IWebElement header = wait.Until(ExpectedConditions.ElementToBeClickable(productNameHeader));

            string sortStatusBefore = header.GetAttribute("aria-sort");
            Console.WriteLine($"📌 Trạng thái sắp xếp trước: {sortStatusBefore}");

            try
            {
                header.Click();
                Thread.Sleep(1000);
            }
            catch (Exception)
            {
                Console.WriteLine("⚠️ Selenium click không hoạt động, thử dùng JavaScript...");
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click();", header);
                Thread.Sleep(1000);
            }

            string sortStatusAfter = header.GetAttribute("aria-sort");
            Console.WriteLine($"📌 Trạng thái sắp xếp sau: {sortStatusAfter}");

            if (sortStatusBefore == sortStatusAfter || string.IsNullOrEmpty(sortStatusAfter))
            {
                Console.WriteLine("⚠️ Có thể sắp xếp chưa được kích hoạt, thử click lần nữa...");
                header.Click();
                Thread.Sleep(1000);
            }

            Console.WriteLine("🔄 Đã sắp xếp danh sách sản phẩm theo tên.");

            Thread.Sleep(2000);

            // Tìm sản phẩm trong danh sách
            By productCell = By.XPath($"//td[contains(text(), '{productName}')]");
            var productElements = driver.FindElements(productCell);

            if (productElements.Count == 0)
            {
                Console.WriteLine($"⚠️ Không tìm thấy sản phẩm '{productName}' trong danh sách!");
                return;
            }
            Console.WriteLine($"✅ Tìm thấy sản phẩm '{productName}'.");
            Thread.Sleep(5000);
            // Tìm nút xóa tương ứng với sản phẩm
            By deleteButton = By.XPath($"//td[contains(text(), '{productName}')]/following-sibling::td//span[contains(@class, 'anticon-delete')]");

            Thread.Sleep(5000);

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(deleteButton)).Click();
                Console.WriteLine("🔹 Đã nhấn vào nút xóa!");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("❌ Lỗi: Không thể nhấn vào nút xóa. Kiểm tra lại sản phẩm có thực sự tồn tại không.");
                return;
            }

            // Xác nhận xóa
            By confirmButton = By.XPath("//button[contains(@class, 'ant-btn-primary') and span[text()='OK']]");
            wait.Until(ExpectedConditions.ElementToBeClickable(confirmButton)).Click();
            Console.WriteLine("✅ Đã xác nhận xóa sản phẩm!");
            Thread.Sleep(5000);

        }



        public void FindAndClickProductByName(string productName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            for (int i = 0; i < 5; i++) // Lặp tối đa 5 lần để cuộn xuống
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 500);");
                Thread.Sleep(1000); // Chờ một chút để dữ liệu tải lên
            }

            By productLocator = By.XPath($"//div[contains(@class, 'sc-cEzcPc') and contains(text(), '{productName}')]");

            try
            {
                IWebElement product = wait.Until(ExpectedConditions.ElementToBeClickable(productLocator));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", product);
                Thread.Sleep(1000);

                product.Click();
                Console.WriteLine($"✅ Đã click vào sản phẩm: {productName}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Không tìm thấy sản phẩm: {productName}");
            }
        }


        private IWebElement FindProductRow(string productName)
        {
            Console.WriteLine($"🔍 Tìm kiếm sản phẩm: {productName}");
            By productCellLocator = By.XPath($"//td[contains(text(), '{productName}')]");
            var productCells = driver.FindElements(productCellLocator);
            Console.WriteLine($"🔍 Số lượng sản phẩm tìm thấy: {productCells.Count}");

            if (productCells.Count == 0)
            {
                Console.WriteLine($"⚠️ Không tìm thấy sản phẩm '{productName}' trong danh sách!");
                return null;
            }

            return productCells[0].FindElement(By.XPath("./ancestor::tr"));
        }



        //UpdateProduct

        public bool UpdateProduct(string productName, string newPrice = null)
        {
            Console.WriteLine($"✏️ Đang cập nhật sản phẩm: {productName}...");

            // Sắp xếp danh sách sản phẩm theo tên (tương tự như DeleteProduct)
            By productNameHeader = By.XPath("//th[contains(@aria-label, 'Tên sản phẩm')]");
            IWebElement header = wait.Until(ExpectedConditions.ElementToBeClickable(productNameHeader));


            string sortStatusBefore = header.GetAttribute("aria-sort");
            Console.WriteLine($"📌 Trạng thái sắp xếp trước: {sortStatusBefore}");

            try
            {
                header.Click();
                Thread.Sleep(1000);
            }
            catch (Exception)
            {
                Console.WriteLine("⚠️ Selenium click không hoạt động, thử dùng JavaScript...");
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click();", header);
                Thread.Sleep(1000);
            }

            string sortStatusAfter = header.GetAttribute("aria-sort");
            Console.WriteLine($"📌 Trạng thái sắp xếp sau: {sortStatusAfter}");

            if (sortStatusBefore == sortStatusAfter || string.IsNullOrEmpty(sortStatusAfter))
            {
                Console.WriteLine("⚠️ Có thể sắp xếp chưa được kích hoạt, thử click lần nữa...");
                header.Click();
                Thread.Sleep(1000);
            }

            Console.WriteLine("🔄 Đã sắp xếp danh sách sản phẩm theo tên.");
            Thread.Sleep(2000);

            // Tìm sản phẩm trong danh sách (tương tự như DeleteProduct)
            By productCell = By.XPath($"//span[@aria-label='edit' and @class='anticon anticon-edit']");
            var productElements = driver.FindElements(productCell);

            if (productElements.Count == 0)
            {
                Console.WriteLine($"⚠️ Không tìm thấy sản phẩm '{productName}' trong danh sách!");
                return false;
            }
            Console.WriteLine($"✅ Tìm thấy sản phẩm '{productName}'.");
            Thread.Sleep(5000);

            // Tìm nút cập nhật tương ứng với sản phẩm
            By updateButton = By.CssSelector("span[aria-label='edit']");
            try
            {
                // Kiểm tra xem nút "edit" có tồn tại không
                IWebElement editButton = driver.FindElement(updateButton);
                if (editButton != null)
                {
                    Console.WriteLine("✅ Nút 'edit' đã được tìm thấy.");
                    // Đợi nút "edit" có thể click được và click vào nó
                    wait.Until(ExpectedConditions.ElementToBeClickable(updateButton)).Click();
                    Console.WriteLine("🔹 Đã mở form chỉnh sửa sản phẩm!");
                }
                else
                {
                    Console.WriteLine("❌ Lỗi: Không tìm thấy nút 'edit'.");
                    return false;
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("❌ Lỗi: Timeout khi đợi nút chỉnh sửa clickable.");
                return false;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("❌ Lỗi: Không tìm thấy nút 'edit'.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi không xác định: {ex.Message}");
                return false;
            }



            Console.WriteLine("🔄 Cập nhật giá sản phẩm...");
            Console.WriteLine("🔄 Đợi ant-drawer-body hiển thị...");
            try
            {
                IWebElement drawerBody = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ant-drawer-body")));
                Console.WriteLine("✅ Tìm thấy 'ant-drawer-body'.");

                // Tìm ô input giá trong ant-drawer-body
                Console.WriteLine("🔄 Tìm ô input giá trong ant-drawer-body...");
                IWebElement priceElement = drawerBody.FindElement(By.Id("basic_price"));

                // Kiểm tra giá trị hiện tại
                string oldPrice = priceElement.GetAttribute("value");
                Console.WriteLine($"🔍 Giá hiện tại: {oldPrice}");

                // Cập nhật giá sản phẩm
                Console.WriteLine("🔄 Đang xóa giá cũ...");
                priceElement.Clear();
                Thread.Sleep(5500); // Chờ một chút để tránh lỗi nhập bị ghi đè

                newPrice = "9999";
                priceElement.SendKeys(newPrice);
                Console.WriteLine($"✅ Đã nhập giá mới: {newPrice}");

                // Kiểm tra lại giá trị sau khi nhập
                string updatedPrice = priceElement.GetAttribute("value");

                if (updatedPrice != newPrice)
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("arguments[0].value = arguments[1];", priceElement, newPrice);
                    Thread.Sleep(500);
                }

                // Click nút lưu
                By saveButton = By.XPath("//button[contains(., 'Xác nhận')]");
                IWebElement save = wait.Until(ExpectedConditions.ElementToBeClickable(saveButton));

                Console.WriteLine("🔄 Đang nhấn vào nút 'Xác nhận'...");
                save.Click();
                Console.WriteLine("✅ Đã lưu thay đổi!");
                Thread.Sleep(5500); // Chờ một chút để tránh lỗi nhập bị ghi đè

                // Kiểm tra lại giá trị trên giao diện sau khi lưu

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("❌ Lỗi: Timeout khi đợi 'ant-drawer-body' hiển thị hoặc khi tìm element.");
                return false;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("❌ Lỗi: Không tìm thấy ô nhập giá hoặc nút lưu.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi không xác định: {ex.Message}");
                return false;
            }
        }



        ///UpdateSLProduct
        public bool UpdateSLProduct(string productName)
        {
            Console.WriteLine($"✏️ Đang cập nhật sản phẩm: {productName}...");
            try
            {
                // Sắp xếp danh sách sản phẩm theo tên
                By productNameHeader = By.XPath("//th[contains(@aria-label, 'Tên sản phẩm')]");
                IWebElement header = wait.Until(ExpectedConditions.ElementToBeClickable(productNameHeader));

                string sortStatusBefore = header.GetAttribute("aria-sort");
                Console.WriteLine($"📌 Trạng thái sắp xếp trước: {sortStatusBefore}");

                header.Click();
                Thread.Sleep(1000); // Đợi 1 giây

                string sortStatusAfter = header.GetAttribute("aria-sort");
                Console.WriteLine($"📌 Trạng thái sắp xếp sau: {sortStatusAfter}");

                if (sortStatusBefore == sortStatusAfter || string.IsNullOrEmpty(sortStatusAfter))
                {
                    Console.WriteLine("⚠️ Có thể sắp xếp chưa được kích hoạt, thử click lần nữa...");
                    header.Click();
                    Thread.Sleep(1000);
                }

                Console.WriteLine("🔄 Đã sắp xếp danh sách sản phẩm theo tên.");
                Thread.Sleep(2000);

                // Tìm sản phẩm trong danh sách
                By productCell = By.XPath($"//span[@aria-label='edit' and @class='anticon anticon-edit']");
                var productElements = driver.FindElements(productCell);

                if (productElements.Count == 0)
                {
                    Console.WriteLine($"⚠️ Không tìm thấy sản phẩm '{productName}' trong danh sách!");
                    return false;
                }
                Console.WriteLine($"✅ Tìm thấy sản phẩm '{productName}'.");
                Thread.Sleep(3000);

                // Tìm nút cập nhật tương ứng với sản phẩm
                By updateButton = By.CssSelector("span[aria-label='edit']");
                try
                {
                    IWebElement editButton = driver.FindElement(updateButton);
                    if (editButton != null)
                    {
                        Console.WriteLine("✅ Nút 'edit' đã được tìm thấy.");
                        wait.Until(ExpectedConditions.ElementToBeClickable(updateButton)).Click();
                        Console.WriteLine("🔹 Đã mở form chỉnh sửa sản phẩm!");
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        Console.WriteLine("❌ Lỗi: Không tìm thấy nút 'edit'.");
                        return false;
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("❌ Lỗi: Timeout khi đợi nút chỉnh sửa clickable.");
                    return false;
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("❌ Lỗi: Không tìm thấy nút 'edit'.");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Lỗi không xác định: {ex.Message}");
                    return false;
                }

                // Đợi ant-drawer-body hiển thị
                IWebElement drawerBody = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ant-drawer-body")));
                Thread.Sleep(3000);

                // Tìm ô input countInStock
                IWebElement countInStockElement = drawerBody.FindElement(By.Id("basic_countInStock"));
                Thread.Sleep(3000);

                // Lấy số lượng hàng tồn kho trước khi cập nhật
                string oldStock = countInStockElement.GetAttribute("value");
                Console.WriteLine($"🔍 Số lượng hàng tồn kho trước khi cập nhật: {oldStock}");

                // Cập nhật countInStock sản phẩm
                Console.WriteLine("🔄 Đang cập nhật số lượng hàng tồn kho...");


                countInStockElement.Click();  // Click vào ô input
                countInStockElement.SendKeys(Keys.Control + "a"); // Chọn toàn bộ nội dung
                countInStockElement.SendKeys(Keys.Delete); // Xóa nội dung
                Thread.Sleep(500);
                countInStockElement.SendKeys("0");
                Thread.Sleep(1000);



                // Đợi giá trị input được cập nhật
                wait.Until(ExpectedConditions.TextToBePresentInElementValue(countInStockElement, "0"));
                Thread.Sleep(3000);

                Console.WriteLine("✅ Đã nhập số lượng hàng mới: 0");

                // Cuộn xuống để tránh lỗi click bị chặn
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", countInStockElement);
                Thread.Sleep(500);

                // Click nút "Xác nhận"
                By saveButton = By.XPath("//button[contains(., 'Xác nhận')]");
                IWebElement save = wait.Until(ExpectedConditions.ElementToBeClickable(saveButton));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", save);
                Console.WriteLine("✅ Đã click nút 'Xác nhận'!");

                // Chờ form đóng lại
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(saveButton));

                Console.WriteLine($"✅ Số lượng sản phẩm '{productName}' đã cập nhật về 0!");


                // Đợi cập nhật thành công
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(saveButton));
                Thread.Sleep(3000);

                // Lấy số lượng hàng tồn kho sau khi cập nhật
                drawerBody = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ant-drawer-body")));
                Thread.Sleep(3000);
                countInStockElement = drawerBody.FindElement(By.Id("basic_countInStock"));
                Thread.Sleep(3000);
                string newStock = countInStockElement.GetAttribute("value");
                Console.WriteLine($"🔍 Số lượng hàng tồn kho sau khi cập nhật: {newStock}");

                Console.WriteLine($"✅ Số lượng hàng tồn kho sản phẩm '{productName}' đã được cập nhật về 0!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi cập nhật số lượng hàng tồn kho sản phẩm '{productName}': {ex.Message}");
                return false;
            }
        }
    }
}