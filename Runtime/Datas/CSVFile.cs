using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CHG.Utilities.Datas
{
    public class CSVFile
    {
        private readonly string filePath;
        private List<string> headerList;
        private List<string> csvDataList;


        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="path">INI 파일이 위치한 경로</param>
        public CSVFile(string path)
        {
            filePath = path;
            
            Load();
        }
        
        private void Load()
        {
            if (!filePath.EndsWith(".csv") || !File.Exists(filePath))
                return;

            string[] lines = File.ReadAllLines(filePath);
            
        }
    }
}