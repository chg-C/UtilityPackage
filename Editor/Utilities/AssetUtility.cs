using UnityEditor;
using UnityEngine;

namespace CHG.Editor
{
    public static class AssetUtility
    {
        /// <summary>
        /// Asset Object에서 확장자 string 추출
        /// </summary>
        /// <param name="assetObject">Asset</param>
        /// <returns>.을 포함한 확장자(유효하지 않은 값일 시 Empty string)</returns>
        public static string GetAssetExtension(Object assetObject)
        {
            try
            {
                string assetPath = AssetDatabase.GetAssetPath(assetObject);
                
                return System.IO.Path.GetExtension(assetPath);
            }
            catch(System.Exception ex)
            {
                return string.Empty;
            }
        }
    }
}