using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    /// <summary>
    /// Вспомогательный класс чтения клавиш
    /// </summary>
    public static class LetterMaster
    {
        private static string ruKeyMap = @"йцукенгшщзхъфывапролджэ\ячсмитьбю.";
        private static string enKeyMap = @"qwertyuiop[]asdfghjkl;'\zxcvbnm,./";

        /// <summary>
        /// Определяет равенство(эквивалентность) переданных букв
        /// </summary>
        /// <param name="a">Первый символ для сравнения</param>
        /// <param name="b">Второй символ для сравнения</param>
        /// <returns>Вернёт true для й,й q,q и  q,й </returns>
        public static bool isKey(char a, char b)
        {
            if (b == a || b == GetBrother(a))
                return true;

            return false;
        }

        /// <summary>
        /// Определяет с что начинается ли переданная строка с переданной буквы либо м эквивалента
        /// </summary>
        /// <param name="target">Целевая строка</param>
        /// <param name="f">Буква для сравнения</param>
        /// <returns>Вернёт true для йод,q </returns>
        public static bool isFirstKey(this string target, char f)
        {
            if (string.IsNullOrEmpty(target))
                return false;

            target = target.ToLower();

            return isKey(target[0], f);
        }

        private static char GetBrother(char a)
        {
            if (ruKeyMap.Contains(a))
            {
                return enKeyMap[Position(a, ruKeyMap)];
            }
            else
            {
                return ruKeyMap[Position(a, enKeyMap)];
            }
        }

        private static int Position(char target, string map)
        {
            int result = -1;

            for (int i = 0; i <= map.Length - 1; i++)
            {
                if (map[i] == target)
                {
                    return i;
                }
            }

            return result;

        }
    }
}
