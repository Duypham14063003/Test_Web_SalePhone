using System;
using System.IO;
using ClosedXML.Excel;
using System.Linq;

namespace test_salephone.Helpers
{
    public static class ExcelReportHelper
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Report", "BDCLPM.xlsx");
        public static void WriteToExcel(string Worksheets, string numberTest ,string status)
        {
            
            using (var workbook = new XLWorkbook(filePath))
            {
                Console.WriteLine($"📂 Đường dẫn file: {filePath}");

                var worksheet = workbook.Worksheet(Worksheets);
                Console.WriteLine($"dang o word: {worksheet}");
                var row = worksheet.RowsUsed()
                                .Skip(8) // Skip header
                                    .FirstOrDefault(r => r.Cell(2).GetValue<string>() == numberTest);

                if (row != null)
                {
                    string productName = row.Cell(2).GetValue<string>();
                    Console.WriteLine($"✅ Tên sản phẩm có ID {numberTest}: {productName}");

                    int rowIndex = row.RowNumber();
                    worksheet.Cell(rowIndex, 10).Value = status;
                    Console.WriteLine($"✅ Đã cập nhật trạng thái cho Test Case {numberTest}: {status}");

                    workbook.SaveAs(filePath);
                    Console.WriteLine($"✅ Đã lưu file Excel");
                }
                else
                {
                    Console.WriteLine($"❌ Không tìm thấy TestCase có ID {numberTest}");
                }
            }  
        }
    }
}
