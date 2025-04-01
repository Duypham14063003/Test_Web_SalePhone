
using System;
using System.IO;
using ClosedXML.Excel;
using System.Linq;

namespace test_salephone.Utilities
{

    public class ExcelReportHelper_Tran
    {
        public class TestCase
        {
            public string Id { get; set; }
            public string data { get; set; }
        }


        private static string[] possiblePaths =
        {
            //cái file excel chỗ report tui xài không được nene tui đung đuongfw dẫn cứng
            "D:\\BDCLPM.xlsx",
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Report", "BDCLPM.xlsx")
        };

        // Chọn đường dẫn đầu tiên tồn tại, nếu không có, dùng đường dẫn cuối cùng
        private static string filePath = possiblePaths.FirstOrDefault(File.Exists) ?? possiblePaths.Last();


        public static void WriteToExcel(string sheetName, string numberTest, string status, string actualResult = "")
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
                                    .FirstOrDefault(r => r.Cell(2).GetValue<string>().Trim() == numberTest.Trim());

                    if (row != null)
                    {
                        row.Cell(9).Value = actualResult; // Cập nhật Actual Result vào cột H
                        row.Cell(10).Value = status;       // Cập nhật Status vào cột I

                        workbook.SaveAs(filePath);
                        Console.WriteLine($"✅ Đã lưu file Excel");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Không tìm thấy TestCase có ID {numberTest}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Lỗi: {ex.Message}");
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

        //    public static IEnumerable<object[]> GetTestCasesForNUnit()
        //    {
        //        // Sử dụng tên sheet cố định "testCase_Duy"
        //        var testCases = GetTestCases("TestData_TKSP");
        //        foreach (var testCase in testCases)
        //        {
        //            yield return new object[] { testCase.Id, testCase.data };
        //        }
        //    }
        //}
    }
}
