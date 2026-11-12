using System.Text.RegularExpressions;

namespace Task3_Contacts
{
    public static class Program
    {
        public static bool IsFullName(string name)
        {
            return Regex.IsMatch(name, @"\A\p{L}+(?: +\p{L}+)*\z");
        }

        public static bool IsAge(string age)
        {
            return Regex.IsMatch(age, @"\A[0-9]+\z");
        }

        public static bool IsPhone(string phone)
        {
            return Regex.IsMatch(phone, @"\A\+7[0-9]{10}\z");
        }

        public static bool IsEmail(string email)
        {
            return Regex.IsMatch(email, @"\A[A-Za-z0-9_%+-]+(?:\.[A-Za-z0-9_%+-]+)*@[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*(?:\.[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*)*\.[A-Za-z]{2,}\z");
        }
    }
}