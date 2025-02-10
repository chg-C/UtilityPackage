using UnityEditor;

namespace CHG.Utilities.Datas
{
    public static class RecursivoeFolderGenerator
    {
        public static void CreateFolderRecursively(string path)
        {
            // 부모 폴더 경로 추출
            string parentFolder = System.IO.Path.GetDirectoryName(path);

            // 부모 폴더가 존재하지 않으면 재귀적으로 생성
            if (!AssetDatabase.IsValidFolder(parentFolder))
            {
                CreateFolderRecursively(parentFolder);
            }

            // 폴더가 존재하지 않으면 생성
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parentFolder, System.IO.Path.GetFileName(path));
            }

            // 에셋 데이터베이스 저장
            AssetDatabase.SaveAssets();
        }
    }
} 