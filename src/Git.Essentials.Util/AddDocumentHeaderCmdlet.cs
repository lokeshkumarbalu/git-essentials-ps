// Licensed to Lokesh Balu under one or more agreements.
// Lokesh Balu licenses this file to you under the MIT license.

namespace Git.Essentials.Util
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using Git.Essentials.Util.BusinessLogic;

    [Cmdlet(VerbsCommon.Add, "DocumentHeader")]
    [OutputType(typeof(string[]))]
    public class AddDocumentHeaderCmdlet : Cmdlet
    {
        private string[] _supportedExtensions =
        {
            ".cs",
            ".c",
            ".cpp",
            ".html",
        };

        public AddDocumentHeaderCmdlet()
        {
        }

        [Parameter(Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = false,
            HelpMessage = "The item to work on")]
        public FileSystemInfo Item { get; set; }

        [Parameter(Position = 1,
            Mandatory = true,
            HelpMessage = "File path to the header template file")]
        [ValidateNotNullOrEmpty]
        public FileInfo Template { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Begin");
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            WriteVerbose("End");
        }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(Template.Name))
            {
                var exception = new ArgumentNullException(nameof(Template));
                var errorRecord = new ErrorRecord(exception, string.Empty, ErrorCategory.InvalidArgument, Template);
                ThrowTerminatingError(errorRecord);
            }

            if (Path.GetExtension(Template.Extension) != ".template")
            {
                var exception = new ArgumentException("The specified template file is not valid!");
                var errorRecord = new ErrorRecord(exception, string.Empty, ErrorCategory.InvalidArgument, Template);
                ThrowTerminatingError(errorRecord);
            }

            try
            {
                WriteVerbose($"Working on {Item.FullName}");

                if (Item is DirectoryInfo)
                {
                    WriteVerbose($"'{Item.Name}' is a directory, processing skipped.");
                    return;
                }

                if (!_supportedExtensions.Contains(Item.Extension))
                {
                    WriteVerbose($"'{Item.Extension}' is not a supported file extension, processing skipped.");
                    return;
                }

                var header = DocumentHeaderProcessor.GetDocumentHeader(Template, (FileInfo)Item);
                DocumentHeaderProcessor.ReplaceHeader(header, (FileInfo)Item);
                WriteVerbose(header);
            }
            catch (Exception ex)
            {
                var errorRecord = new ErrorRecord(ex, ex.Message, ErrorCategory.InvalidArgument, Item);
                ThrowTerminatingError(errorRecord);
            }
        }
    }
}