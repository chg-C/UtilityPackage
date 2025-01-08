
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace CHG.Utilities.Datas
{
    /// <summary>
    /// INI 파일을 사용하는 Wrapper 클래스
    /// </summary>
    public class IniFile
    {
        private readonly string filePath;
        private Dictionary<string, Dictionary<string, string>> data;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="path">INI 파일이 위치한 경로</param>
        public IniFile(string path)
        {
            filePath = path;
            data = new Dictionary<string, Dictionary<string, string>>();
            Load();
        }

        private void Load()
        {
            if (!File.Exists(filePath))
                return;

            string[] lines = File.ReadAllLines(filePath);
            string currentSection = null;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                // 섹션 구분
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine[1..^1];
                    data[currentSection] = new Dictionary<string, string>();
                }
                // 키-값 쌍
                else if (currentSection != null && trimmedLine.Contains("="))
                {
                    string[] parts = trimmedLine.Split(new[] { '=' }, 2);
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    data[currentSection][key] = value;
                }
            }
        }
        /// <summary>
        /// 값 얻어오기
        /// </summary>
        /// <param name="section">섹션</param>
        /// <param name="key">키 값</param>
        /// <param name="defaultValue">Fallback 값</param>
        /// <returns>설정 값</returns>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            if (data.ContainsKey(section) && data[section].ContainsKey(key))
            {
                return data[section][key];
            }
            return defaultValue;
        }
        /// <summary>
        /// 값 설정하기
        /// </summary>
        /// <param name="section">섹션</param>
        /// <param name="key">키 값</param>
        /// <param name="value">설정 값</param>
        public void SetValue(string section, string key, string value)
        {
            if (!data.ContainsKey(section))
            {
                data[section] = new Dictionary<string, string>();
            }
            data[section][key] = value;
            Save();
        }

        private void Save()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var section in data)
            {
                sb.AppendLine($"[{section.Key}]");
                foreach (var kvp in section.Value)
                {
                    sb.AppendLine($"{kvp.Key} = {kvp.Value}");
                }
                sb.AppendLine();
            }

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
        