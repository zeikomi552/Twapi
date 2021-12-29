using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Utilities
{
    public class StrUtil
    {
        #region バイト数で文字列を切り取る
        /// <summary>
        /// バイト数で文字列を切り取る
        /// 最後の文字が2バイト文字で半分に切れてしまう場合は一文字丸ごと取り除く
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="size_max">切り取るサイズ byte数</param>
        /// <returns>切り取ったサイズの文字列</returns>
        public static string SubstringByte(string s, int size_max)
        {
            int len = 0, size = 0;

            foreach (var tmp in s)
            {
                int delta = 2;

                // 文字サイズを確認
                if (tmp < 256)
                {
                    // 256未満なら1byte文字として扱う
                    delta = 1;
                }

                // 文字を足してサイズを超えるかどうかを確認
                if (size + delta > size_max)
                {
                    break;
                }
                else
                {
                    // 文字列を足しこむ
                    size = size + delta;
                    len++;
                }
            }

            // 指定されたサイズで切り取って返却する
            return s.Substring(0, len);
        }
        #endregion
    }
}

