//using NUnit.Framework;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System;
//using System.Collections.Generic;
//using ClosedXML.Excel;
//using System.IO;
//using OpenQA.Selenium.Support.UI;
//using SeleniumExtras.WaitHelpers;

//[TestFixture]
//public class EditUserTest
//{
//    private IWebDriver driver;
//    private ExcelHelper excelHelper;
//    private string excelPath = @"D:\Book1.xlsx"; // Đường dẫn file Excel

//    [SetUp]
//    public void Setup()
//    {
//        driver = new ChromeDriver();
//        driver.Manage().Window.Maximize();
//        excelHelper = new ExcelHelper(excelPath);
//    }

//    [Test]
//    public void ChinhSuaThongTinNguoiDung()
//    {
//        string testCaseID = "ID_ChinhSuaTTND_1";
//        string testData = excelHelper.ReadTestDataByID(testCaseID);

//        if (string.IsNullOrEmpty(testData))
//        {
//            Assert.Fail($"⚠️ Không tìm thấy Test Data cho {testCaseID}");
//        }

//        // Nhập tên mới từ Test Data
//        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
//        var nameField = driver.FindElement(By.CssSelector("input[id='name']"));
//        nameField.Clear();
//        nameField.SendKeys(testData);

//        // Click nút "Cập nhật"
//        var updateButton = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div/div/div[5]/button"));
//        updateButton.Click();

//        // Kiểm tra kết quả
//        string actualName = driver.FindElement(By.Id("name")).GetAttribute("value");

//        if (actualName.Trim().Equals(testData, StringComparison.OrdinalIgnoreCase))
//        {
//            excelHelper.WriteResult(testCaseID, "Pass");
//        }
//        else
//        {
//            excelHelper.WriteResult(testCaseID, "Fail");
//        }

//        Assert.That(actualName.Trim(), Is.EqualTo(testData).IgnoreCase, "Tên không được cập nhật!");
//    }

//    [TearDown]
//    public void TearDown()
//    {
//        driver.Quit();
//    }
//}