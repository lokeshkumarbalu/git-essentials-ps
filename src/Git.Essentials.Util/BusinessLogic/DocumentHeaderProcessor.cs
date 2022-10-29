namespace Git.Essentials.Util.BusinessLogic
{
    using System;
    using System.IO;

    internal class DocumentHeaderProcessor
    {
        internal static bool CheckIfHeaderExists()
        {
            return true;
        }

        internal static string GetDocumentHeader(string templatePath, string filePath, string companyName)
        {
            if (string.IsNullOrEmpty(templatePath))
                throw new ArgumentException($"'{nameof(templatePath)}' cannot be null or empty.", nameof(templatePath));

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or empty.", nameof(filePath));

            if (string.IsNullOrEmpty(companyName))
                throw new ArgumentException($"'{nameof(companyName)}' cannot be null or empty.", nameof(companyName));

            var template = File.ReadAllText(templatePath);
            var filename = Path.GetFileName(filePath);

            template.Replace("{{FileName}}", filename)
                .Replace("{{Company}}", companyName)
                .Replace("{{Year}}", "");


            return filename;
        }
    }
}