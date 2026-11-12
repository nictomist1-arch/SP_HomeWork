using System;
using Library = Task2_Text.Program;

namespace Task2_Text_App
{
    public static class Program
    {
        public static string Check()
        {
            if (!IsPalindrome("А роза упала на лапу Азора") ||
                IsPalindrome("Привет") || CountSentences("Привет! Как дела? Хорошо.") != 3 ||
                Reverse("абв") != "вба")
                throw new Exception("Ошибка обработки текста.");

            return "Палиндром: да" + Environment.NewLine +
                "Предложений: 3" + Environment.NewLine +
                "Переворот строки: вба";
        }

        public static bool IsPalindrome(string text)
        {
            return Library.IsPalindrome(text);
        }

        public static int CountSentences(string text)
        {
            return Library.CountSentences(text);
        }

        public static string Reverse(string text)
        {
            return Library.Reverse(text);
        }
    }
}
