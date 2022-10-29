//------------------------------------------------------------------------------
// <copyright file="AddDocumentationHeaderCmdlet.cs" company="Zealag">
//    Copyright © Zealag 2018. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Git.Essentials.Util
{
    using System;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Add, "DocumentHeader")]
    [OutputType(typeof(string[]))]
    public class AddDocumentHeaderCmdlet : Cmdlet
    {
        public AddDocumentHeaderCmdlet()
        {
            FileName = string.Empty;
            Template = string.Empty;
        }

        [Parameter(Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = false,
            HelpMessage = "File name or a regex filter to specify the files to work on.")]
        public string FileName { get; set; }

        [Parameter(Position = 1,
            Mandatory = true,
            HelpMessage = "File path to the header template file")]
        [ValidateNotNullOrEmpty]
        public string Template { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Begin");
        }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                var exception = new ArgumentException($"Argument {nameof(FileName)}, cannot be null or empty");
                var errorRecord = new ErrorRecord(exception, "StringNullOrEmpty", ErrorCategory.InvalidArgument, FileName);
                ThrowTerminatingError(errorRecord);
            }

            if (string.IsNullOrEmpty(Template))
            {
                var exception = new ArgumentException($"Argument {nameof(Template)}, cannot be null or empty");
                var errorRecord = new ErrorRecord(exception, "Invalid header template file name", ErrorCategory.InvalidArgument, Template);
                ThrowTerminatingError(errorRecord);
            }

            WriteVerbose($"Working on {FileName}");
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            WriteVerbose("End");
        }
    }
}