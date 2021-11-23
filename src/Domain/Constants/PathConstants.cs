// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Domain.Constants
{
   public static class PathConstants
    {
        public static string CurrentDirectory { get; set; }
        public const string FilesPath = "Files";
        public const string DocumentsPath= "Documents";
        public const string SpecificationsPath = "Specifications";
        public static string GetSpecificationsPath => Path.Combine(CurrentDirectory, FilesPath, SpecificationsPath);
        public static string GetDocumentsPath => Path.Combine(CurrentDirectory, FilesPath, DocumentsPath);
        public static string GetSubFolder(string subFolder)=> Path.Combine(CurrentDirectory, FilesPath, subFolder);
        public static char FilesStringSeperator = ',';
    }
}
