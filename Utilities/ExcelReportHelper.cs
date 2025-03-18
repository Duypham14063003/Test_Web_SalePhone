using System;
using System.IO;
using ClosedXML.Excel;
using System.Linq;

namespace test_salephone.Utilities
{

    // Định nghĩa lớp TestCase chứa các thuộc tính cần thiết
  

    public class ExcelReportHelper
    {
        public class TestCase
        {
            public string Id { get; set; }
            public string data { get; set; }
            // Bạn có thể bổ sung thêm các thuộc tính khác nếu cần
        }
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

         // Hàm đọc dữ liệu từ Excel và trả về danh sách các TestCase
        public static List<TestCase> GetTestCases(string worksheetName)
        {
            var testCases = new List<TestCase>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(worksheetName);
                Console.WriteLine($"📂 Đọc dữ liệu từ sheet: {worksheetName}");

                // Giả sử 8 dòng đầu là header, bỏ qua chúng
                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    // Giả sử cột 2 chứa ID và cột 10 chứa trạng thái
                    string id = row.Cell(1).GetValue<string>();
                    string data = row.Cell(2).GetValue<string>();

                    var testCase = new TestCase
                    {
                        Id = id,
                        data = data
                    };

                    testCases.Add(testCase);
                }
            }

            return testCases;
        }

        public static IEnumerable<object[]> GetTestCasesForNUnit()
        {
            // Sử dụng tên sheet cố định "testCase_Duy"
            var testCases = GetTestCases("TestData_TKSP");
            foreach (var testCase in testCases)
            {
                yield return new object[] { testCase.Id, testCase.data };
            }
        }
    }
}
