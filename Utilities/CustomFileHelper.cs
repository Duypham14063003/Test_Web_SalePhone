using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_salephone.Utilities
{
    public static class CustomFileHelper
    {
        private static string downloadPath = @"C:\Users\ngotr\Downloads";

        public static FileInfo GetLatestExcelFile()
        {
            var directory = new DirectoryInfo(downloadPath);
            return directory.GetFiles("*.xlsx")
                            .OrderByDescending(f => f.LastWriteTime)
                            .FirstOrDefault();
        }

        public static void OpenFile(FileInfo file)
        {
            if (file != null)
            {
                Process.Start(new ProcessStartInfo(file.FullName) { UseShellExecute = true });
            }
            else
            {
                Console.WriteLine("❌ Không tìm thấy file Excel.");
            }
        }
    }
}

