using System;
using System.IO;
using ClosedXML.Excel;
using System.Linq;
using System.Collections.Generic;

namespace test_salephone.Helpers
{
    public static class ReadTestDataFromExcel
    {
        // Đường dẫn file Excel (bạn có thể cấu hình lại nếu cần)
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Report", "BDCLPM.xlsx");

        /// <summary>
        /// Đọc dữ liệu từ một phạm vi (hàng từ startRow đến endRow) của cột được chỉ định (columnIndex)
        /// trong sheet được chỉ định. Dữ liệu được trim và nối với nhau theo dòng mới.
        /// </summary>
        /// <param name="sheetName">Tên sheet</param>
        /// <param name="startRow">Dòng bắt đầu (Excel đánh số từ 1)</param>
        /// <param name="endRow">Dòng kết thúc</param>
        /// <param name="columnIndex">Chỉ số cột (Excel đánh số từ 1; với cột F thì columnIndex = 6)</param>
        /// <returns>Chuỗi dữ liệu được nối với newline</returns>
        public static string ReadDataRangeFromExcel(string sheetName, int startRow, int endRow, int columnIndex)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Không tìm thấy file Excel tại: {filePath}");
            }
            
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(sheetName);
                if (worksheet == null)
                {
                    throw new Exception($"Sheet '{sheetName}' không tồn tại trong file Excel.");
                }
                
                List<string> values = new List<string>();

                // Duyệt từ startRow đến endRow
                for (int rowNumber = startRow; rowNumber <= endRow; rowNumber++)
                {
                    // Lấy dữ liệu từ ô, trim khoảng trắng
                    string cellData = worksheet.Cell(rowNumber, columnIndex).GetValue<string>().Trim();
                    // Nếu dữ liệu có định dạng "Key: Value", parse để chỉ lấy phần Value.
                    string parsedData = ParseTestDataValues(cellData);
                    values.Add(parsedData);
                }

                string result = string.Join(Environment.NewLine, values);
                Console.WriteLine($"✅ Dữ liệu từ sheet '{sheetName}' (dòng {startRow}-{endRow}, cột {columnIndex}):\n{result}");
                return result;
            }
        }

        /// <summary>
        /// Parse chuỗi dữ liệu theo định dạng "Key: Value" trên mỗi dòng.
        /// Nếu chuỗi chứa dấu ':' thì chỉ lấy phần sau dấu ':' (loại bỏ khoảng trắng và dấu nháy nếu có),
        /// nếu không thì trả về chuỗi gốc.
        /// </summary>
        /// <param name="dataTest">Chuỗi dữ liệu gốc</param>
        /// <returns>Chuỗi sau khi parse</returns>
        private static string ParseTestDataValues(string dataTest)
        {
            if (string.IsNullOrEmpty(dataTest))
                return dataTest;

            string[] lines = dataTest.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var values = new List<string>();

            foreach (var line in lines)
            {
                if (line.Contains(":"))
                {
                    var parts = line.Split(new[] { ':' }, 2);
                    if (parts.Length == 2)
                    {
                        string value = parts[1].Trim();
                        if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length >= 2)
                        {
                            value = value.Substring(1, value.Length - 2);
                        }
                        values.Add(value);
                    }
                }
                else
                {
                    values.Add(line.Trim());
                }
            }
            return string.Join(Environment.NewLine, values);
        }
    }
}
