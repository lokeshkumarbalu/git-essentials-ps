// Licensed to Lokesh Balu under one or more agreements.
// Lokesh Balu licenses this file to you under the MIT license.

namespace Git.Essentials.Util.BusinessLogic
{
    using System;
    using System.IO;
    using System.Text;

    public class DocumentHeaderProcessor
    {
        public static void ReplaceHeader(string expectedHeaderText, FileInfo sourceFile)
        {
            var fileName = Path.GetTempFileName();
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            fileName = Path.Combine(folderPath, fileName);

            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (var outFile = new StreamWriter(fileStream, Encoding.UTF8))
            using (var reader = new StreamReader(sourceFile.OpenRead()))
            {
                var isInitialComment = true;
                outFile.Write(expectedHeaderText);
                outFile.Write(Environment.NewLine);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (isInitialComment == true && !line.StartsWith("//"))
                        isInitialComment = false;

                    if (isInitialComment)
                        continue;

                    outFile.WriteLine(line);
                }
            }

            var outFileInfo = new FileInfo(fileName);
            sourceFile.Delete();
            outFileInfo.MoveTo(sourceFile.FullName);
        }

        public static string GetDocumentHeader(FileInfo template, FileInfo sourceFile)
        {
            if (sourceFile is null)
                throw new ArgumentNullException(nameof(sourceFile));

            if (template is null)
                throw new ArgumentNullException(nameof(template));

            var templateText = File.ReadAllText(template.FullName);

            return templateText
                .Replace("{{FileName}}", sourceFile.Name)
                .Replace("{{Year}}", sourceFile.CreationTime.Year.ToString());
        }
    }
}