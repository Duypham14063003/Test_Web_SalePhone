using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace test_salephone.PageObjects
{
    public class CheckCart
    {
        private readonly IWebDriver driver;
        private readonly By cartButton = By.CssSelector(".sc-bLmarx.kcYPxN");
        private readonly By IsCartEmpty = By.CssSelector(".sc-bLmarx.kcYPxN");
        private readonly By productInCart = By.CssSelector(".sc-hJRrWL.RErTn");
        private readonly By productCheckbox = By.XPath("//input[@type='checkbox']");
        private readonly By firstBuyButton = By.XPath("(//button[contains(@class, 'ant-btn-primary') and span[text()='Mua Hàng']])[1]");

        private readonly By lastBuyButton = By.XPath("(//button[contains(@class, 'ant-btn-primary') and span[text()='Mua Hàng']])[last()]");
        private readonly By productToSelect = By.XPath("//div[contains(@class, 'sc-cEzcPc') and contains(text(), 'iPhone 15')]");
        private readonly By addToCartButton = By.XPath("//button[contains(@class, 'ant-btn-primary') and contains(., 'Thêm vào giỏ hàng')]");
        private readonly By clickaddToCartButton = By.XPath("//button[contains(@class, 'ant-btn-primary')]//span[contains(text(), 'Thêm vào giỏ hàng')]");
        public CheckCart(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void OpenCart()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement cartBtn = wait.Until(ExpectedConditions.ElementToBeClickable(cartButton));
            cartBtn.Click();
        }

        public bool IsProductInCart()
        {
            var products = driver.FindElements(productInCart);
            return products.Count > 0;
        }

        public void SelectLatestProduct()
        {
            var checkboxes = driver.FindElements(productCheckbox);
            if (checkboxes.Count > 0)
            {
                checkboxes[checkboxes.Count - 1].Click();
            }
        }

        public void BuyProduct()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            int beforeBuyCount = GetCartItemCount();
            Console.WriteLine($"📦 Trước khi mua: {beforeBuyCount} sản phẩm.");

            if (driver.FindElements(firstBuyButton).Count > 0)
            {
                driver.FindElement(firstBuyButton).Click();
                Console.WriteLine("✅ Đã click vào nút Mua Hàng đầu tiên.");

                wait.Until(ExpectedConditions.ElementExists(lastBuyButton));

                if (driver.FindElements(lastBuyButton).Count > 0)
                {
                    driver.FindElement(lastBuyButton).Click();
                    Console.WriteLine("✅ Đã click vào nút Mua Hàng cuối cùng.");

                    Thread.Sleep(3000);

                    int afterBuyCount = GetCartItemCount();
                    Console.WriteLine($"📦 Sau khi mua: {afterBuyCount} sản phẩm.");

                    if (afterBuyCount == beforeBuyCount - 1)
                    {
                        Console.WriteLine("✅ Sản phẩm đã được mua thành công! Giỏ hàng giảm 1 sản phẩm.");
                    }
                    else
                    {
                        Console.WriteLine("❌ Sản phẩm không giảm trong giỏ hàng sau khi mua.");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Không tìm thấy nút Mua Hàng cuối cùng.");
                }
            }
            else
            {
                Console.WriteLine("❌ Không tìm thấy nút Mua Hàng đầu tiên.");
            }
        }




        public void ClickLastBuyButtonOnly()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                if (driver.FindElements(lastBuyButton).Count > 0)
                {
                    IWebElement lastBtn = wait.Until(ExpectedConditions.ElementToBeClickable(lastBuyButton));
                    lastBtn.Click();
                    Console.WriteLine("✅ Đã click vào nút Mua Hàng cuối cùng.");
                    Thread.Sleep(3000);
                }
                else
                {
                    Console.WriteLine("❌ Không tìm thấy nút Mua Hàng cuối cùng.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Lỗi khi click vào nút Mua Hàng cuối cùng: {e.Message}");
            }
        }


        public int GetCartItemCount()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement cartBtn = wait.Until(ExpectedConditions.ElementToBeClickable(cartButton));
            cartBtn.Click();
            Console.WriteLine("🛒 Đã mở giỏ hàng.");

            Thread.Sleep(2000);

            int productCount = driver.FindElements(productInCart).Count;
            Console.WriteLine($"🔍 Giỏ hàng có {productCount} sản phẩm.");

            return productCount;
        }



        public void AddProductToCart()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/");

            IWebElement product = wait.Until(ExpectedConditions.ElementExists(productToSelect));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", product);
            Thread.Sleep(1500); // Đợi một chút để đảm bảo phần tử đã cuộn vào tầm nhìn

            product.Click();
            Thread.Sleep(2500);


            var buttons = driver.FindElements(clickaddToCartButton);
            Console.WriteLine($"🔍 Tìm thấy {buttons.Count} nút clickaddToCartButton.");

            if (buttons.Count > 0)
            {
                buttons[0].Click();
                Console.WriteLine("✅ Đã click vào nút thêm vào giỏ hàng.");
            }
            else
            {
                Console.WriteLine("❌ Không tìm thấy nút clickaddToCartButton.");
            }




            Thread.Sleep(1500);
        }




        public void FindAndClickProductByName(string productName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            for (int i = 0; i < 5; i++)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 500);");
                Thread.Sleep(1000);
            }

            By productLocator = By.XPath($"//div[contains(@class, 'sc-cEzcPc') and contains(text(), '{productName}')]");

            try
            {
                IWebElement product = wait.Until(ExpectedConditions.ElementToBeClickable(productLocator));

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", product);
                Thread.Sleep(1000);


                product.Click();
                Console.WriteLine($"✅ Đã click vào sản phẩm: {productName}");

                AddProductToCartAndBuy();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Không tìm thấy sản phẩm: {productName}");
            }
        }




        public void AddProductToCartAndBuy()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            By addToCartButton = By.XPath("//button[contains(@class, 'ant-btn-primary') and contains(., 'Thêm vào giỏ hàng')]");

            try
            {
                IWebElement addToCartBtn = wait.Until(ExpectedConditions.ElementToBeClickable(addToCartButton));
                addToCartBtn.Click();
                Console.WriteLine("✅ Đã thêm sản phẩm vào giỏ hàng.");
                Thread.Sleep(2000);
            }
            catch (Exception e)
            {
                Console.WriteLine("❌ Không tìm thấy nút 'Thêm vào giỏ hàng'.");
                return;
            }

            OpenCart();
            Thread.Sleep(2000);


            SelectLatestProduct();

            ClickFirstBuyButtonOnly();
        }


        public void ClickFirstBuyButtonOnly()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                if (driver.FindElements(firstBuyButton).Count > 0)
                {
                    IWebElement firstBtn = wait.Until(ExpectedConditions.ElementToBeClickable(firstBuyButton));
                    firstBtn.Click();
                    Console.WriteLine("✅ Đã click vào nút Mua Hàng đầu tiên.");
                    Thread.Sleep(3000);
                }
                else
                {
                    Console.WriteLine("❌ Không tìm thấy nút Mua Hàng đầu tiên.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Lỗi khi click vào nút Mua Hàng đầu tiên: {e.Message}");
            }
        }



        public void ClickLogoToGoToHomePage()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement logo = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//img[@src='/assets/logo-HOh0M7tK.png']")));
            logo.Click();
            wait.Until(ExpectedConditions.UrlToBe("https://frontend-salephones.vercel.app/"));
        }

    }
}
