using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        public static string ConvertToRelativePath(string absolutePath)
        {
            if (absolutePath.StartsWith(Application.dataPath))
            {
                return "Assets" + absolutePath.Substring(Application.dataPath.Length);
            }

            return absolutePath; // "Assets" 폴더 외부의 경우 원래 경로 반환
        }
        public static string ConvertToAbsolutePath(string relativePath)
        {
            ///Root 폴더로 시작하는 경우 이미 절대경로
            if(Path.IsPathRooted(relativePath))
            {
                return relativePath;
            }
            //Assets는 DataPath에 포함되므로 삭제
            if(relativePath.StartsWith("Assets/"))
                relativePath = relativePath.Remove(0, 7);
            

            return Path.GetFullPath(Path.Combine(Application.dataPath, relativePath));
        }
    }
}