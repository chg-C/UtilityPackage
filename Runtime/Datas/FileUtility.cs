using System;
using System.Collections.Generic;
using System.IO;

namespace CHG.Utilities.Datas
{
    /// <summary>
    /// 파일 관리 유틸리티 묶음
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// 경로에서 파일 이름만 분리하기
        /// </summary>
        /// <param name="filePath">전체 경로</param>
        /// <param name="includesExtension">확장자 포함 여부</param>
        /// <returns></returns>
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
        public static string ExtractFolderName(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }
    }
}