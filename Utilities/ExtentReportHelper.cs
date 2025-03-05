using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

public static class ExtentReportHelper
{
    private static ExtentReports? extent;
    private static ExtentTest? test;
    private static ExtentHtmlReporter? htmlReporter;
    private static string reportPath = "Reports/TestReport.html";

    public static void StartReport()
    {
        try
        {
            if (!Directory.Exists("Reports"))
            {
                Directory.CreateDirectory("Reports"); // Tạo thư mục nếu chưa có
                Console.WriteLine("📁 Thư mục Reports đã được tạo.");
            }

            htmlReporter = new ExtentHtmlReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            Console.WriteLine($"📄 Báo cáo sẽ được lưu tại: {reportPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi khi khởi tạo báo cáo: {ex.Message}");
        }
    }

    public static void LogTest(string testName, string status, string message)
    {
        try
        {
            test = extent?.CreateTest(testName);
            if (test != null)
            {
                if (status == "Pass")
                    test.Pass(message);
                else if (status == "Fail")
                    test.Fail(message);
                else
                    test.Info(message);
                Console.WriteLine($"📝 Log: {testName} - {status} - {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Lỗi khi ghi log: {ex.Message}");
        }
    }

    public static void EndReport()
    {
        try
        {
            if (extent != null)
            {
                Console.WriteLine("📄 Đang xuất báo cáo...");
                extent.Flush(); // Xuất báo cáo ra file
                Console.WriteLine("✅ Báo cáo đã được tạo thành công tại: " + reportPath);
            }
            else
            {
                Console.WriteLine("⚠️ Không tìm thấy instance của ExtentReports.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi khi kết thúc báo cáo: {ex.Message}");
        }
    }

}
