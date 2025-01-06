using System.IO;

namespace CHG.Utilities.Datas
{
    public static class FileUtility
    {
        public static string ExtractFileName(string filePath, bool includesExtension)
        {
            if(includesExtension)
            {
                return Path.GetFileName(filePath);
            }
            else
            {
                return Path.GetFileNameWithoutExtension(filePath);
            }
        }
    }
}