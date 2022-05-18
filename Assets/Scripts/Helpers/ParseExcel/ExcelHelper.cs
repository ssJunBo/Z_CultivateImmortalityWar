using System.IO;
using UnityEngine;

namespace Helpers.ParseExcel
{
    public class ExcelHelper
    {
        private void ParseExcel(string name)
        {
            string path = $"{Application.dataPath}/{name}.xls";
            FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
//            IExcelDataReader
        }
    }
}