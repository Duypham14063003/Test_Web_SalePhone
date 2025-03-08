using NUnit.Framework;
using test_salephone.Helpers;

namespace test_salephone.Tests
{
    public class UnitTest1
    {
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
