using System;
using System.IO;
using ClosedXML.Excel;
using System.Linq;

namespace test_salephone.Helpers
{
    public static class ExcelReportHelper_Phuc
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Report", "BDCLPM.xlsx");
        public static void WriteToExcel(string Worksheets, string numberTest, string status, string actual = null, string intergration = null)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(Worksheets);
                Console.WriteLine($"Đang xử lý worksheet: {worksheet}");

                var row = worksheet.RowsUsed()
                                .Skip(8) // Bỏ qua 8 dòng header
                                .FirstOrDefault(r => r.Cell(2).GetValue<string>() == numberTest);

                if (row != null)
                {
                    string productName = row.Cell(2).GetValue<string>();
                    int rowIndex = row.RowNumber();

                    // Ghi giá trị actual và intergration vào cùng một ô
                    string cellValue = "";
                    if (actual != null)
                    {
                        cellValue += $"Hệ thống hiển thị thông báo: '{actual}'";
                    }
                    if (intergration != null)
                    {
                        cellValue += (cellValue != "" ? "\n" : "") + $"Thông tin trên web:\n {intergration}";
                    }
                    else if (intergration == null)
                    {
                        cellValue += (cellValue != "" ? "\n" : "") + $"Hệ thống không cập nhật lại thông tin";
                    }
                    worksheet.Cell(rowIndex, 9).Value = cellValue;
                    worksheet.Cell(rowIndex, 9).Style.Alignment.SetWrapText(true); // Bật chế độ xuống dòng
                    Console.WriteLine($"✅ Đã cập nhật Actual và Intergration cho {numberTest}: {cellValue}");

                    // Ghi giá trị status
                    worksheet.Cell(rowIndex, 10).Value = status;
                    Console.WriteLine($"✅ Đã cập nhật Status cho {numberTest}: {status}");

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
