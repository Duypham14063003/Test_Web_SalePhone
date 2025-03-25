using System;
using System.IO;
using ClosedXML.Excel;
using System.Linq;
using System.Collections.Generic;

namespace test_salephone.Helpers
{
    public static class ReadTestDataToExcel
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
                Console.WriteLine($"***************************ReadDataToExcel - {numberTest}***************************");

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

                    Console.WriteLine($"✅ Parsed Data test của {numberTest} ở cột 7: \n" + parsedData);
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
        /// Chỉ lấy dữ liệu trong "" đầu tiên nếu có, với trường image chỉ lấy đường dẫn (không lấy base64).
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
                    // Lấy phần bên phải và loại bỏ khoảng trắng đầu cuối
                    string value = parts[1].Trim();

                    // Kiểm tra nếu là dòng image
                    if (line.Trim().StartsWith("image:"))
                    {
                        // Tìm vị trí dấu " đầu tiên và dấu " thứ hai
                        int firstQuote = value.IndexOf('"');
                        int secondQuote = value.IndexOf('"', firstQuote + 1);
                        if (firstQuote >= 0 && secondQuote > firstQuote)
                        {
                            // Chỉ lấy phần đường dẫn trong dấu ""
                            value = value.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
                        }
                        else
                        {
                            value = value.Trim(); // Giữ nguyên nếu không có dấu ""
                        }
                    }
                    else
                    {
                        // Xử lý các dòng khác: chỉ lấy phần trong "" nếu có
                        int firstQuote = value.IndexOf('"');
                        if (firstQuote >= 0)
                        {
                            int secondQuote = value.IndexOf('"', firstQuote + 1);
                            if (secondQuote > firstQuote)
                            {
                                value = value.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
                            }
                            else
                            {
                                value = value.Substring(firstQuote + 1).Trim();
                            }
                        }
                        else
                        {
                            value = value.Trim(); // Giữ nguyên nếu không có dấu ""
                        }
                    }
                    values.Add(value);
                }
            }
            // Nối các giá trị với newline
            return string.Join(Environment.NewLine, values);
        }

    }
}
