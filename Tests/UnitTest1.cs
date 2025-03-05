namespace test_salephone;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[TestFixture]
public class Tests
{
    private IWebDriver? driver; 
    private readonly string url = "https://example.com"; 

    [OneTimeSetUp]
    public void SetupReport()
    {
        ExtentReportHelper.StartReport();
        ExtentReportHelper.LogTest("Setup Report", "Info", "Khởi tạo báo cáo test");
    }

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Navigate().GoToUrl(url);
        ExtentReportHelper.LogTest("Setup", "Info", $"Điều hướng đến {url}");
    }

    [Test]
    public void Test1()
    {
        if (driver != null)
        {
            ExtentReportHelper.LogTest("Test1", "Pass", "Driver hợp lệ, tiếp tục test...");
        }
        else
        {
            ExtentReportHelper.LogTest("Test1", "Fail", "Driver chưa được khởi tạo!");
        }
        Assert.NotNull(driver, "Driver chưa được khởi tạo!");
    }

    [TearDown]
    public void TearDown()
    {
        driver?.Quit();
        driver?.Dispose();
        ExtentReportHelper.LogTest("TearDown", "Info", "Trình duyệt đã đóng.");
    }

    [OneTimeTearDown]
    public void EndReport()
    {
        ExtentReportHelper.LogTest("EndReport", "Info", "Hoàn tất test, kết thúc báo cáo.");
        ExtentReportHelper.EndReport();
    }
}
