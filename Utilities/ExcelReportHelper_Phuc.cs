using System;
using System.IO;
using ClosedXML.Excel;
using System.Linq;

namespace test_salephone.Helpers
{
    public static class ExcelReportHelper_Phuc
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Report", "BDCLPM.xlsx");
        public static void WriteToExcel(string Worksheets, string numberTest, string status, string actual = null)
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
                    // Console.WriteLine($"✅ Tên sản phẩm có ID {numberTest}: {productName}");

                    int rowIndex = row.RowNumber();

                    // Ghi giá trị actual (nếu có, nếu không thì để trống hoặc giá trị mặc định)
                    worksheet.Cell(rowIndex, 9).Value = actual != null ? $"Hệ thống hiển thị thông báo: '{actual}'" : "N/A";
                    Console.WriteLine($"✅ Đã cập nhật Actual cho {numberTest}: {actual ?? "N/A"}");

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
