// Licensed to Lokesh Balu under one or more agreements.
// Lokesh Balu licenses this file to you under the MIT license.

namespace Git.Essentials.Util.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Git.Essentials.Util.BusinessLogic;
    using NUnit.Framework;
    using FluentAssertions;

    [TestFixture]
    public class DocumentHeaderProcessorTests
    {
        private FileInfo _sourceFile;

        private FileInfo _templateFile;

        [SetUp]
        public async Task SetUp()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var templateFileName = Path.ChangeExtension(Path.GetRandomFileName(), "template");
            templateFileName = Path.Combine(folderPath, templateFileName);

            using var templateFileStream = new FileStream(templateFileName, FileMode.Create, FileAccess.ReadWrite);
            using var templateStreamWriter = new StreamWriter(templateFileStream, Encoding.UTF8);
            var headerTemplate = """
                // Copyright (c) Action Corp. All rights reserved.
                // {{FileName}} created in {{Year}}.
                // Licensed under the MIT license.
                """;

            await templateStreamWriter.WriteAsync(headerTemplate);

            var sourceFileName = Path.ChangeExtension(Path.GetRandomFileName(), "cs");
            sourceFileName = Path.Combine(folderPath, sourceFileName);
            using var sourceFileStream = new FileStream(sourceFileName, FileMode.Create, FileAccess.ReadWrite);
            using var sourceStreamWriter = new StreamWriter(sourceFileStream, Encoding.UTF8);
            var sourceText = """
                // unformatted document header.
                using System;

                Console.WriteLine("Hello World!");
                """;

            await sourceStreamWriter.WriteAsync(sourceText);

            _templateFile = new FileInfo(templateFileName);
            _sourceFile = new FileInfo(sourceFileName);
        }

        [Test]
        public void GivenATemplateAndSourceFile_GetDocumentHeaderShouldGenerateATemplateStringWithFileNameInIt()
        {
            var documentHeader = DocumentHeaderProcessor.GetDocumentHeader(_templateFile, _sourceFile);
            Console.Write(documentHeader);

            documentHeader.Should().Contain(_sourceFile.Name);
            documentHeader.Should().Contain(_sourceFile.CreationTime.Year.ToString());
        }

        [Test]
        public void GivenADocumentHederAndASourceFile_ReplaceHeaderShouldAddTheHeaderToTheSourceFile()
        {
            var documentHeader = DocumentHeaderProcessor.GetDocumentHeader(_templateFile, _sourceFile);
            DocumentHeaderProcessor.ReplaceHeader(documentHeader, _sourceFile);
            _sourceFile.Refresh();

            var updatedSourceFile = File.ReadAllText(_sourceFile.FullName);
            Console.Write(updatedSourceFile);

            updatedSourceFile.Should().Contain(documentHeader);
        }

        [TearDown]
        public void TearDown()
        {
            _sourceFile.Delete();
            _templateFile.Delete();
        }
    }
}