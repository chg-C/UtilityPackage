using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CHG.Utilities.Datas
{
    public class CSVFile
    {
        private readonly string filePath;

        private List<string> headerList = new List<string>();
        private List<string> csvDataList = new List<string>();

        string rawText;

        char seperator;


        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="path">INI 파일이 위치한 경로</param>
        public CSVFile(string path, char seperator)
        {
            filePath = path;
            this.seperator = seperator;
            
            LoadFromFile();
        }
        private void SaveFile()
        {

        }
        private void LoadFromFile()
        {
            if (!filePath.EndsWith(".csv") || !File.Exists(filePath))
                return;
            
            rawText = File.ReadAllText(filePath);
            SeperateValue();
        }
        private void SeperateValue()
        {
            if(string.IsNullOrEmpty(rawText))
                return;

            headerList.Clear();

            string[] lines = rawText.Split('\n');
            string[] seperatedText = lines[0].Split(seperator);
            for(int i = 0; i < seperatedText.Length; ++i)
            {
                headerList.Add(seperatedText[i]);
            }
           
            csvDataList.Clear();

            if(lines.Length > 1)
            {
                csvDataList.Add("\n");
                for(int i = 1; i < lines.Length; ++i)
                {
                    seperatedText = lines[i].Split(seperator);
                }
            }
        }

        public override string ToString()
        {
            return rawText;
        }
    }
}