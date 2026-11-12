using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task1_3_TextAnalysis
{
    [Flags]
    public enum ReportFields
    {
        Sentences = 1,
        Characters = 2,
        Words = 4,
        Questions = 8,
        Exclamations = 16,
        All = Sentences | Characters | Words | Questions | Exclamations
    }

    public class Program
    {
        public static Task<string> Analyze(string text, ReportFields fields = ReportFields.All,
            string reportPath = null, CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                int sentences = 0;
                int words = 0;
                int questions = 0;
                int exclamations = 0;
                bool inWord = false;
                bool hasText = false;

                for (int i = 0; i < text.Length; i++)
                {
                    token.ThrowIfCancellationRequested();
                    char symbol = text[i];

                    if (char.IsLetterOrDigit(symbol))
                    {
                        if (!inWord)
                            words++;
                        inWord = true;
                        hasText = true;
                    }
                    else
                    {
                        inWord = false;
                    }

                    if (symbol == '.' || symbol == '?' || symbol == '!' || symbol == '…')
                    {
                        bool question = false;
                        bool exclamation = false;
                        while (i < text.Length &&
                            (text[i] == '.' || text[i] == '?' || text[i] == '!' || text[i] == '…'))
                        {
                            token.ThrowIfCancellationRequested();
                            question |= text[i] == '?';
                            exclamation |= text[i] == '!';
                            i++;
                        }
                        i--;

                        if (hasText)
                        {
                            sentences++;
                            if (question)
                                questions++;
                            if (exclamation)
                                exclamations++;
                            hasText = false;
                        }
                    }
                }

                if (hasText)
                    sentences++;

                StringBuilder report = new StringBuilder();
                if ((fields & ReportFields.Sentences) != 0)
                    report.AppendLine("Предложений: " + sentences);
                if ((fields & ReportFields.Characters) != 0)
                    report.AppendLine("Символов: " + text.Length);
                if ((fields & ReportFields.Words) != 0)
                    report.AppendLine("Слов: " + words);
                if ((fields & ReportFields.Questions) != 0)
                    report.AppendLine("Вопросительных предложений: " + questions);
                if ((fields & ReportFields.Exclamations) != 0)
                    report.AppendLine("Восклицательных предложений: " + exclamations);

                token.ThrowIfCancellationRequested();
                string result = report.ToString();
                if (!string.IsNullOrEmpty(reportPath))
                    File.WriteAllText(reportPath, result);
                return result;
            }, token);
        }
    }
}