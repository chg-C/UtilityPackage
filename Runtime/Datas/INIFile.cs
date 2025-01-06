
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace CHG.Utilities.Datas
{
    public class IniFile
    {
        private readonly string filePath;
        private Dictionary<string, Dictionary<string, string>> data;

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

        public string GetValue(string section, string key, string defaultValue = "")
        {
            if (data.ContainsKey(section) && data[section].ContainsKey(key))
            {
                return data[section][key];
            }
            return defaultValue;
        }

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
        