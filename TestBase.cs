using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[TestFixture]
public class TestBase
{
    protected IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://frontend-salephones.vercel.app/");
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose(); // Giải phóng tài nguyên
    }
}
