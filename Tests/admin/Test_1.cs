//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Support.UI;
//using OpenQA.Selenium;

//[TestFixture]
//public class CartTest
//{
//    private IWebDriver driver;
//    private string baseUrl = "https://frontend-salephones.vercel.app";  // Thay đổi URL của bạn

//    [SetUp]
//    public void SetUp()
//    {
//        // Khởi tạo WebDriver với Chrome
//        driver = new ChromeDriver();
//        driver.Manage().Window.Maximize();
//        driver.Navigate().GoToUrl(baseUrl);  // Truy cập vào URL trang chủ
//    }

//    // ID_CapNhatTTGH_2
//    [Test]
//    public void Test_GoToCart()
//    {
//        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

//        // 1. Đăng nhập vào tài khoản người dùng
//        var loginButton = wait.Until(d => d.FindElement(By.XPath("//div[contains(text(),'Đăng Nhập')]")));
//        loginButton.Click();

//        var emailField = wait.Until(d => d.FindElement(By.XPath("//input[@placeholder='Email']")));
//        emailField.SendKeys("user@gmail.com");

//        var passwordField = driver.FindElement(By.XPath("//input[@type='password']"));
//        passwordField.SendKeys("123123A");

//        var signInButton = wait.Until(d => d.FindElement(By.XPath("//div[@class='ant-spin-container']//button//span[text()='Đăng nhập']")));
//        signInButton.Click();

//        // Chờ một chút để đăng nhập hoàn tất
//        System.Threading.Thread.Sleep(4000);

//        // 2. Trở về trang chủ và chọn sản phẩm
//        var product = wait.Until(d => d.FindElement(By.XPath("//div[contains(text(),'TECNO SPARK | 4GB | 128GB | Vàng')]"))); // Cập nhật với sản phẩm phù hợp
//        product.Click();

//        // 3. Thêm sản phẩm vào giỏ hàng
//        var addToCartButton = wait.Until(d => d.FindElement(By.XPath("//button[@class='ant-btn css-qnu6hi ant-btn-primary sc-hylbpc gsqsH']//span[text()='Thêm vào giỏ hàng']")));
//        addToCartButton.Click();        // 4. Chờ giỏ hàng cập nhật
//        System.Threading.Thread.Sleep(2000);

//        // 5. Kiểm tra giỏ hàng có sản phẩm
//        var cartButton = wait.Until(d => d.FindElement(By.XPath("//div[@class='sc-bLmarx kcYPxN']//span[@class='anticon anticon-shopping-cart']")));
//        cartButton.Click();

//        // 6. Chờ trang giỏ hàng mở lên
//        System.Threading.Thread.Sleep(2000);

//        // 7. Chọn sản phẩm được thêm vào giỏ hàng (checkbox cuối cùng)
//        var lastCheckbox = wait.Until(d => d.FindElement(By.XPath("(//input[@class='ant-checkbox-input'])[last()]")));
//        lastCheckbox.Click();

//        // 8. Kiểm tra xem checkbox đã được chọn
//        Assert.That(lastCheckbox.Selected, Is.True, "Checkbox không được chọn.");

//        // 9. Nhấn nút "Mua Hàng" (nút trước đó)
//        var muaHangButtonStep9 = driver.FindElement(By.XPath("//button[contains(@class, 'ant-btn') and contains(@class, 'ant-btn-primary') and span[text()='Mua Hàng']]"));
//        muaHangButtonStep9.Click();

//        // 10. Kiểm tra hành động sau khi nhấn "Mua Hàng" (ví dụ: chuyển hướng đến trang thanh toán hoặc hiển thị thông báo)
//        System.Threading.Thread.Sleep(2000); // Tùy chỉnh thời gian chờ theo tình huống thực tế

//        //// Bước 10: Click vào nút "Mua Hàng" khác sau khi đã thực hiện bước 9
//        //var muaHangButtonStep10 = wait.Until(d => d.FindElement(By.XPath("//button[@class='ant-btn css-qnu6hi ant-btn-primary' and contains(span, 'Mua Hàng')]")));

//        //// Cuộn đến phần tử nếu cần thiết
//        //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
//        //js.ExecuteScript("arguments[0].scrollIntoView(true);", muaHangButtonStep10);

//        //// Nhấn nút "Mua Hàng"
//        //muaHangButtonStep10.Click();


//    }
//    [Test]
//    public void Test_UpdateShippingInfo()
//    {
//        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

//        // 1. Đăng nhập vào tài khoản người dùng
//        var loginButton = wait.Until(d => d.FindElement(By.XPath("//div[contains(text(),'Đăng Nhập')]")));
//        loginButton.Click();

//        var emailField = wait.Until(d => d.FindElement(By.XPath("//input[@placeholder='Email']")));
//        emailField.SendKeys("user@gmail.com");

//        var passwordField = driver.FindElement(By.XPath("//input[@type='password']"));
//        passwordField.SendKeys("123123A");

//        var signInButton = wait.Until(d => d.FindElement(By.XPath("//div[@class='ant-spin-container']//button//span[text()='Đăng nhập']")));
//        signInButton.Click();

//        // Chờ một chút để đăng nhập hoàn tất
//        System.Threading.Thread.Sleep(4000);

//        // 2. Trở về trang chủ và chọn sản phẩm
//        var product = wait.Until(d => d.FindElement(By.XPath("//div[contains(text(),'TECNO SPARK | 4GB | 128GB | Vàng')]"))); // Cập nhật với sản phẩm phù hợp
//        product.Click();

//        // 3. Thêm sản phẩm vào giỏ hàng
//        var addToCartButton = wait.Until(d => d.FindElement(By.XPath("//button[@class='ant-btn css-qnu6hi ant-btn-primary sc-hylbpc gsqsH']//span[text()='Thêm vào giỏ hàng']")));
//        addToCartButton.Click();

//        // 4. Chờ giỏ hàng cập nhật
//        System.Threading.Thread.Sleep(2000);

//        // 5. Kiểm tra giỏ hàng có sản phẩm
//        var cartButton = wait.Until(d => d.FindElement(By.XPath("//div[@class='sc-bLmarx kcYPxN']//span[@class='anticon anticon-shopping-cart']")));
//        cartButton.Click();

//        // 6. Chờ trang giỏ hàng mở lên
//        System.Threading.Thread.Sleep(2000);
//        // 7. Chọn sản phẩm được thêm vào giỏ hàng (checkbox cuối cùng)
//        var lastCheckbox = wait.Until(d => d.FindElement(By.XPath("(//input[@class='ant-checkbox-input'])[last()]")));
//        lastCheckbox.Click();

//        // 8. Kiểm tra xem checkbox đã được chọn
//        Assert.That(lastCheckbox.Selected, Is.True, "Checkbox không được chọn.");

//        // 7. Chọn "Mua Hàng"
//        var muaHangButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(@class, 'ant-btn') and contains(@class, 'ant-btn-primary') and span[text()='Mua Hàng']]")));
//        muaHangButton.Click();
//        // 8. Chọn nút "OK" sau khi cửa sổ popup hiển thị
//        var okButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(@class, 'ant-btn') and span[text()='OK']]")));
//        okButton.Click();
//        System.Threading.Thread.Sleep(2000);

//    }
//    [Test]
//    public void ID_poopup()
//    {
//        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

//        // 1. Đăng nhập vào tài khoản người dùng
//        var loginButton = wait.Until(d => d.FindElement(By.XPath("//div[contains(text(),'Đăng Nhập')]")));
//        loginButton.Click();

//        var emailField = wait.Until(d => d.FindElement(By.XPath("//input[@placeholder='Email']")));
//        emailField.SendKeys("user@gmail.com");

//        var passwordField = driver.FindElement(By.XPath("//input[@type='password']"));
//        passwordField.SendKeys("123123A");

//        var signInButton = wait.Until(d => d.FindElement(By.XPath("//div[@class='ant-spin-container']//button//span[text()='Đăng nhập']")));
//        signInButton.Click();

//        // Chờ một chút để đăng nhập hoàn tất
//        System.Threading.Thread.Sleep(4000);

//        // 2. Trở về trang chủ và chọn sản phẩm
//        var product = wait.Until(d => d.FindElement(By.XPath("//div[contains(text(),'TECNO SPARK | 4GB | 128GB | Vàng')]"))); // Cập nhật với sản phẩm phù hợp
//        product.Click();

//        // 3. Thêm sản phẩm vào giỏ hàng
//        var addToCartButton = wait.Until(d => d.FindElement(By.XPath("//button[@class='ant-btn css-qnu6hi ant-btn-primary sc-hylbpc gsqsH']//span[text()='Thêm vào giỏ hàng']")));
//        addToCartButton.Click();

//        // 4. Chờ giỏ hàng cập nhật
//        System.Threading.Thread.Sleep(2000);

//        // 5. Kiểm tra giỏ hàng có sản phẩm
//        var cartButton = wait.Until(d => d.FindElement(By.XPath("//div[@class='sc-bLmarx kcYPxN']//span[@class='anticon anticon-shopping-cart']")));
//        cartButton.Click();

//        // 6. Chờ trang giỏ hàng mở lên
//        System.Threading.Thread.Sleep(2000);
//        // 7. Chọn sản phẩm được thêm vào giỏ hàng (checkbox cuối cùng)
//        var lastCheckbox = wait.Until(d => d.FindElement(By.XPath("(//input[@class='ant-checkbox-input'])[last()]")));
//        lastCheckbox.Click();

//        // 8. Kiểm tra xem checkbox đã được chọn
//        Assert.That(lastCheckbox.Selected, Is.True, "Checkbox không được chọn.");

//        // 7. Chọn "Mua Hàng"
//        var muaHangButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(@class, 'ant-btn') and contains(@class, 'ant-btn-primary') and span[text()='Mua Hàng']]")));
//        muaHangButton.Click();


//    }
//    [TearDown]
//    public void TearDown()
//    {
//        driver.Quit();
//        driver.Dispose();
//        driver = null;
//    }

//}