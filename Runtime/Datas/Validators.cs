using System.Text.RegularExpressions;
using UnityEngine;

namespace CHG.Utilities.Datas
{
    public static class Validators
    {
        /// <summary>
        /// 역-도메인 표기법(com.example) 체크
        /// </summary>
        public static bool IsValidReverseDomain(string text)
        {
            string pattern = @"^(?!-)[A-Za-z0-9-]{1,63}(?<!-)(\.[A-Za-z]{2,})+$";

            return Regex.IsMatch(text, pattern) && text.StartsWith("com.");
        }
        /// <summary>
        /// 시맨틱 버전(x.x.x) 체크
        /// </summary>
        public static bool IsValidSemanticVersion(string text)
        {
            //버전 관계없음 표시, 확인할 것 없이 통과
            if(text == "*")
                return true;

            string pattern = @"(\d+)\.(\d+)\.(\d+)(?:-(\w+(\.\w+)*))?(?:\+(\w+(\.\w+)*))?$";

            return Regex.IsMatch(text, pattern);
        }
    }
}
