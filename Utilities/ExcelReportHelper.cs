using System;
using System.IO;
using ClosedXML.Excel;
using System.Linq;

namespace test_salephone.Helpers
{
    public static class ExcelReportHelper
    {
        private static string filePath = "C:\\Users\\ngotr\\Downloads\\BDCLPM.xlsx";

        public static void WriteToExcel(string sheetName, string numberTest, string status, string actualResult)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"❌ Không tìm thấy file: {filePath}");
                return;
            }

            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == sheetName);
                    if (worksheet == null)
                    {
                        Console.WriteLine($"❌ Sheet '{sheetName}' không tồn tại.");
                        return;
                    }

                    var row = worksheet.RowsUsed().Skip(8)
                                       .FirstOrDefault(r => r.Cell(1).GetValue<string>().Trim() == numberTest.Trim());

                    if (row != null)
                    {
                        row.Cell(8).Value = actualResult; // Cập nhật Actual Result vào cột H
                        row.Cell(9).Value = status;       // Cập nhật Status vào cột I

                        workbook.SaveAs(filePath);
                        Console.WriteLine($"✅ Đã cập nhật Test Case {numberTest}:\n   - Status: {status}\n   - Actual Result: {actualResult}");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Không tìm thấy Test Case có ID '{numberTest}' trong sheet '{sheetName}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
            }
        }
    }
}
