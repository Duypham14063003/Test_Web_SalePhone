//using NUnit.Framework;
//using OpenQA.Selenium;
//using System;
//using test_salephone.Utilities; // Import đúng namespace
//using OpenQA.Selenium;

//namespace test_salephone.client
//{
//    public class Test_TKSP : TestBase
//    {
//        [Test, TestCaseSource(typeof(ExcelReportHelper), nameof(ExcelReportHelper.GetTestCasesForNUnit))]
//        [Category("TKSP")]

//        public void TiemKiemSanPham(string id, string data)
//        {
//            string status = "fail";
//            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[2]/div/input")).SendKeys(data);
//            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[2]/div/button")).Click();
//            Thread.Sleep(2000);
//            // check kết quả
//            IWebElement banner = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div[2]/div/div/div/div[3]/div/div/img"));
//            if(!banner.Displayed)
//            {
//                status = "pass";
//                ExcelReportHelper.WriteToExcel("testCase_Duy", id, status);
//            }
//            else
//            {
//                ExcelReportHelper.WriteToExcel("testCase_Duy", id, status);
//            }
//            Console.WriteLine($"📌 ID: {id}, Data: {data}");
//        }
//    }
//}
