using NUnit.Framework;
using OpenQA.Selenium;
// using test_salephone.Helpers;
using test_salephone.Utilities;

namespace test_salephone.Tests
{
    public class UnitTest1
    {
        private IWebDriver driver;
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMethod1()
        {
            string status = "Fail";
            try
            {
                Assert.AreEqual(1, 1);  // Test Pass
                // status = "Pass";
            }
            catch(Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");

            }
            ExcelReportHelper.WriteToExcel("Testcase Duy", "ID_Dangky_1", status);
            
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}
