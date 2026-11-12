using System;
using Library = Task3_Contacts.Program;

namespace Task3_Contacts_App
{
    public static class Program
    {
        public static string Check()
        {
            if (!IsFullName("Иван Петров") || IsFullName("Иван1") ||
                !IsAge("25") || IsAge("-1") ||
                !IsPhone("+79991234567") || IsPhone("123") ||
                !IsEmail("test@example.com") || IsEmail("test@@example.com"))
                throw new Exception("Ошибка проверки контактов.");

            return "Проверены ФИО, возраст, телефон и email.";
        }

        public static bool IsFullName(string name)
        {
            return Library.IsFullName(name);
        }

        public static bool IsAge(string age)
        {
            return Library.IsAge(age);
        }

        public static bool IsPhone(string phone)
        {
            return Library.IsPhone(phone);
        }

        public static bool IsEmail(string email)
        {
            return Library.IsEmail(email);
        }
    }
}
