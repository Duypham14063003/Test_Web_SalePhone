namespace test_salephone;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[TestFixture]
public class Tests
{
    private IWebDriver? driver; 
    private readonly string url = "https://example.com"; // Gán URL mặc định

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Navigate().GoToUrl(url); // Sử dụng `url` để tránh warning
    }

    [Test]
    public void Test1()
    {
        Assert.NotNull(driver, "Driver chưa được khởi tạo!");
        Console.WriteLine("hello world");
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose(); // Giải phóng bộ nhớ
    }
}
