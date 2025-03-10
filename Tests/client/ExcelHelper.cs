using System;
using System.IO;
using ClosedXML.Excel;

public static class ExcelHelper
{
    private static string filePath = @"C:\Users\ngotr\Downloads\BDCLPM.xlsx"; 

    public static void WriteResult(string testCaseID, string actualResult, string status)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("⚠️ File Excel không tồn tại!");
                return;
            }

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet("Testcase Trân"); 
                var lastRow = worksheet.LastRowUsed().RowNumber();

                for (int row = 2; row <= lastRow; row++)
                {
                    if (worksheet.Cell(row, 1).GetString() == testCaseID)
                    {
                        worksheet.Cell(row, 8).Value = actualResult; 
                        worksheet.Cell(row, 9).Value = status; 
                        break;
                    }
                }

                workbook.Save();
                Console.WriteLine($"✅ Kết quả {testCaseID} đã được ghi vào Excel.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi ghi file Excel: {ex.Message}");
        }
    }
}
