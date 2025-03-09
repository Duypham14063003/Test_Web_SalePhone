using System.ComponentModel.DataAnnotations;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using test_salephone.Helpers;
using SeleniumExtras.WaitHelpers;

namespace test_salephone.client{
    [TestFixture]   
    public class Register: TestBase{

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[1]/div[1]/div[4]")).Click();
            driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[1]/div/div/div/label[2]")).Click();
        }
        [Test]
        [Description("Test đăng ký thành người dùng")]
        [Category("Register")]
        [TestCase("ID_Dangky_1","ngocduy140635@gmail.com", "Duy123","Duy123","Đăng nhập",TestName ="ID_Dangky_1")]
        [TestCase("ID_Dangky_2","ngocduy140623@gmail.com", "Duy123","Duy123","Email này đã được đăng ký, vui lòng chọn email khác",TestName ="ID_Dangky_2")]
        [TestCase("ID_Dangky_3","ngocduy140623gmail.com","Duy123","Duy123","Hãy nhập email hợp lệ và có đuôi @gmail.com", TestName ="ID_Dangky_3")]
        [TestCase("ID_Dangky_4","ngocduy123@gmail.com","Duy123","Duy123","Email không tồn tại",TestName = "ID_Dangky_4")]
        [TestCase("ID_Dangky_5","","Duy123","Duy123","Hãy nhập đầy đủ thông tin",TestName = "ID_Dangky_5")]
        [TestCase("ID_Dangky_6","ngocduy140635@gmail.com","Duy123","duy123","Hãy nhập mật khẩu và nhập lại mật khẩu trùng khớp",TestName = "ID_Dangky_6")]
        [TestCase("ID_Dangky_7","ngocduy140635@gmail.com","Duy123aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdsdsdsdsdsdsdsddds","Duy123aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdsdsdsdsdsdsdsddds","Mật khẩu quá dài!",TestName = "ID_Dangky_7")]


        public void Test_Register(string id,string email, string passWord,string confirmPassword, string text_notification){
            string status = "Fail";
            
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[1]/input")).SendKeys(email);
                driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[2]/span/input")).SendKeys(passWord);
                driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[3]/span/input")).SendKeys(confirmPassword);
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[4]/div/div/button")).Click();
                Thread.Sleep(5000);
                if(id != "ID_Dangky_1"){
                    IWebElement text_success = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/span")));
                    if (text_success.Displayed)
                    {
                        status = text_success.Text.Contains(text_notification) ? "Pass" : "Fail";

                    }
                }else if(id == "ID_Dangky_1"){
                    IWebElement text_login = driver.FindElement(By.XPath("//*[@id='root']/div/div/div/div/div[2]/div/div[2]/div/div/div[3]/div/div/button/span"));
                    Assert.That(text_login.Text == text_notification);
                    status = "Pass";
                }
               
                Console.WriteLine($"🔥 Testcase: {id} - {status}"); 
                ExcelReportHelper.WriteToExcel("Testcase Duy", id, status);
        }
    }
}