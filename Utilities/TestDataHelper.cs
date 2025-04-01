using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;

public class TestDataHelper
{
    private static readonly string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Report", "BDCLPM.xlsx"));
    private static readonly string sheetName = "TestData_Trân";

    public static Dictionary<string, string>? GetTestData(string testCaseId)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"❌ Không tìm thấy file: {filePath}");
            return null;
        }

        using (var workbook = new XLWorkbook(filePath))
        {
            if (!workbook.Worksheets.Contains(sheetName))
            {
                Console.WriteLine($"❌ Không tìm thấy sheet '{sheetName}' trong Excel!");
                return null;
            }

            var sheet = workbook.Worksheet(sheetName);

            foreach (var row in sheet.RowsUsed())
            {
                string currentTestCaseId = row.Cell(1).GetValue<string>() ?? "";
                Console.WriteLine($"🔍 Kiểm tra TestCaseID: {currentTestCaseId}");

                if (currentTestCaseId == testCaseId)
                {
                    Console.WriteLine($"✅ Đã tìm thấy testcase {testCaseId}!");

                    return new Dictionary<string, string>
                    {
                        { "TestCaseID", currentTestCaseId },
                        { "ProductName", row.Cell(2).GetValue<string>() ?? "" },
                        { "Brand", row.Cell(3).GetValue<string>() ?? "" },
                        { "Quantity", row.Cell(4).GetValue<int?>()?.ToString() ?? "1" },
                        { "Price", row.Cell(5).GetValue<decimal?>()?.ToString() ?? "0" },
                        { "Description", row.Cell(6).GetValue<string>() ?? "" },
                        { "ExtraField", row.Cell(7).GetValue<string>() ?? "" } // Giá mới
                    };
                }
            }
        }

        Console.WriteLine($"⚠️ Không tìm thấy dữ liệu testcase {testCaseId}!");
        return null;
    }
}
