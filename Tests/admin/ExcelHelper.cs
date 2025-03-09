using System;
using System.IO;
using ClosedXML.Excel;
using System.Linq;
using System.Collections.Generic;

namespace test_salephone.Helpers
{
    public static class ExcelHelper
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Report", "BDCLPM.xlsx");

        /// <summary>
        /// Đọc data test ở cột 7 (theo row khớp numberTest ở cột 2),
        /// ghi status vào cột 10, parse dữ liệu test thành các giá trị riêng biệt và trả về chuỗi data test đã parse.
        /// </summary>
        /// <param name="Worksheets">Tên sheet</param>
        /// <param name="numberTest">Giá trị ở cột 2 để tìm row</param>
        /// <param name="status">Giá trị ghi vào cột 10</param>
        /// <returns>Dữ liệu test đã được parse (các giá trị, mỗi giá trị nằm trên 1 dòng)</returns>
        public static string ReadDataToExcel(string Worksheets, string numberTest)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                Console.WriteLine($"PHUCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC");

                var worksheet = workbook.Worksheet(Worksheets);
                Console.WriteLine($"▶ Sheet hiện tại: {worksheet.Name}");

                // Tìm hàng khớp với numberTest ở cột 2, bỏ qua 8 dòng tiêu đề
                var row = worksheet.RowsUsed()
                                   .Skip(8)
                                   .FirstOrDefault(r => r.Cell(2).GetValue<string>() == numberTest);

                if (row != null)
                {
                    // Lấy dữ liệu test từ cột 7
                    string dataTest = row.Cell(7).GetValue<string>().Trim();

                    // Parse dữ liệu test để chỉ lấy giá trị sau dấu ':' trên mỗi dòng
                    string parsedData = ParseTestDataValues(dataTest);

                    Console.WriteLine("✅ Parsed Data test ở cột 7: \n" + parsedData);
                    Console.WriteLine($"PHUCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC");

                    // Trả về data test đã được parse
                    return parsedData;
                }
                else
                {
                    Console.WriteLine($"❌ Không tìm thấy TestCase có ID {numberTest}");
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Parse chuỗi dữ liệu test theo định dạng "Key: Value" trên mỗi dòng,
        /// trả về chuỗi chỉ chứa các giá trị, mỗi giá trị nằm trên một dòng.
        /// </summary>
        /// <param name="dataTest">Chuỗi dữ liệu test gốc</param>
        /// <returns>Chuỗi chứa các giá trị sau dấu ':'</returns>
        private static string ParseTestDataValues(string dataTest)
        {
            // Tách các dòng, hỗ trợ các kiểu newline khác nhau
            string[] lines = dataTest.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var values = new List<string>();

            foreach (var line in lines)
            {
                // Tách theo dấu ':' chỉ 1 lần
                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    // Lấy phần bên phải và loại bỏ khoảng trắng
                    string value = parts[1].Trim();
                    // Nếu value bắt đầu và kết thúc bằng dấu " thì loại bỏ chúng
                    if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length >= 2)
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                    values.Add(value);
                }
            }
            // Nối các giá trị với newline
            return string.Join(Environment.NewLine, values);
        }
    }
}
