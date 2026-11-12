using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Task2_Text
{
    public static class Program
    {
        public static bool IsPalindrome(string text)
        {
            string letters = new string(text.Where(char.IsLetterOrDigit)
                .Select(char.ToLowerInvariant).ToArray());
            return letters.SequenceEqual(letters.Reverse());
        }

        public static int CountSentences(string text)
        {
            return Regex.Split(text, @"[.!?…]+")
                .Count(part => !string.IsNullOrWhiteSpace(part));
        }

        public static string Reverse(string text)
        {
            return new string(text.Reverse().ToArray());
        }
    }
}