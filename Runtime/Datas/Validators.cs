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
            string pattern = @"(?!-)[A-Za-z0-9-]{1,63}(?<!-)(\.[A-Za-z]{2,})+$";

            return Regex.IsMatch(text, pattern) && text.StartsWith("com.");
        }
        /// <summary>
        /// 시맨틱 버전(x.x.x) 체크
        /// </summary>
        public static bool IsValidSemanticVersion(string text)
        {
            //버전 관계없음 표시, 즉시 통과
            if(text == "*")
                return true;
            //UnityPackage는 하위 버전에 x 사용 가능
            string pattern = @"^(\d+)\.(x|\d+)\.(x|\d+)(-([\da-zA-Z-]+(\.[\da-zA-Z-]+)*))?(\+([\da-zA-Z0-9-]+(\.[\da-zA-Z0-9-]+)*))?$";

            return Regex.IsMatch(text, pattern);
        }
    }
}
