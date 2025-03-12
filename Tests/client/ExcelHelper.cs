using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

public class ExcelHelper
{
    private string filePath;

    public ExcelHelper(string path)
    {
        filePath = path;
    }

    // Đọc dữ liệu từ sheet Testcase Trân
    public List<Dictionary<string, string>> ReadExcel()
    {
        var testData = new List<Dictionary<string, string>>();
        using (var workbook = new XLWorkbook(filePath))
        {
            var worksheet = workbook.Worksheet("Testcase Trân"); // Đổi tên sheet nếu cần
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Bỏ dòng tiêu đề

            foreach (var row in rows)
            {
                var data = new Dictionary<string, string>
                {
                    { "Test Data", row.Cell(6).GetString() }, // Lấy cột Test Data
                    { "Expected Result", row.Cell(7).GetString() } // Lấy cột Expected Result
                };
                testData.Add(data);
            }
        }
        return testData;
    }

    // Ghi kết quả vào Excel
    public void WriteResult(int row, string actualResult, string status)
    {
        using (var workbook = new XLWorkbook(filePath))
        {
            var worksheet = workbook.Worksheet("Testcase Trân");
            worksheet.Cell(row + 2, 8).Value = actualResult; // Ghi vào cột Actual Result
            worksheet.Cell(row + 2, 9).Value = status; // Ghi vào cột Status
            workbook.Save();
        }
    }
}