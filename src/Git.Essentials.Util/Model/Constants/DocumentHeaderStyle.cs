// Licensed to Lokesh Balu under one or more agreements.
// Lokesh Balu licenses this file to you under the MIT license.

namespace Git.Essentials.Util.Model.Constants
{
    public static class DocumentHeaderStyle
    {
        public static readonly string pattern = @"^[\/]{2}[-]{78}[\n][\/]{2}\s?<copyright(.*?)<\/copyright>[\n][\/]{2}[-]{78}";

        public static readonly string[] BorderLineRegex = new string[]
        {
            @"^[\/]{2}[-]*[\n]$",
            @""
        };

        public static readonly string CopyrightStartTagRegex = @"^[\/]{2}\s?<copyright\sfile="".*""\scompany="".*"">$";
    }
}