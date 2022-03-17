// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Razor.Infrastructure.Services
{
    public class UploadService : IUploadService
    {
        private readonly ILogger<IUploadService> _logger;
        private readonly ICurrentUserService _currentUserService;
        public UploadService(ILogger<IUploadService> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }
        public async Task<string> UploadAsync(UploadRequest request, string subFolder)
        {
            if (request.Data == null) return string.Empty;
            var streamData = new MemoryStream(request.Data);
            if (streamData.Length > 0)
            {
                //var folder = request.UploadType.ToDescriptionString();
                var folderName = Path.Combine("Files", subFolder);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                bool exists = Directory.Exists(pathToSave);
                if (!exists) Directory.CreateDirectory(pathToSave);
                
                var fileName = request.FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                if (File.Exists(dbPath))
                {
                    dbPath = NextAvailableFilename(dbPath);
                    fullPath = NextAvailableFilename(fullPath);
                }
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await streamData.CopyToAsync(stream);
                }
                return dbPath;
            }
            else
            {
                return string.Empty;
            }
        }
        public async Task<string> UploadAsync(UploadRequest request)
        {
            if (request.Data == null) return string.Empty;
            var streamData = new MemoryStream(request.Data);
            if (streamData.Length > 0)
            {
                var folder = request.UploadType.ToDescriptionString();
                var folderName = Path.Combine("Files", folder);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                bool exists = Directory.Exists(pathToSave);
                if (!exists) Directory.CreateDirectory(pathToSave);
                var fileName = request.FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                if (File.Exists(dbPath))
                {
                    dbPath = NextAvailableFilename(dbPath);
                    fullPath = NextAvailableFilename(fullPath);
                }
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await streamData.CopyToAsync(stream);
                }
                return dbPath;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string _numberPattern = " ({0})";

        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), _numberPattern));

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFilename(path + _numberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            //if (tmp == pattern)
            //throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }

        public async Task<IResult> UploadFileAsync(int Id, string subfolder, List<IFormFile> files)
        {

            try
            {
                var folder = Id.ToString();// request.UploadType.ToDescriptionString();
                var folderName = Path.Combine("Files", subfolder, folder);
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                foreach (IFormFile file in files)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(folderName, fileName), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
            catch (Exception er)
            {
                return await Result.FailureAsync(new string[] { er.Message });
            }
            return await Result.SuccessAsync();

        }
        public async Task<IResult<List<string>>> LoadFilesAsync(int Id, string subfolder)
        {
            List<string> Result = new List<string>();
            try
            {
                var folder = Id.ToString();// request.UploadType.ToDescriptionString();
                var folderName = Path.Combine("Files", subfolder, folder);
                if (Directory.Exists(folderName))
                {
                    foreach (string file in Directory.GetFiles(folderName))
                    {

                        Result.Add(Path.Combine(folderName, Path.GetFileName(file)));
                    }
                }
            }
            catch (Exception er)
            {
                return await Result<List<string>>.FailureAsync(new string[] { er.Message });
            }
            return await Result<List<string>>.SuccessAsync(Result);
        }

        public async Task<IResult> RemoveFileAsync(int Id, string name, string subfolder)
        {
            var folder = Id.ToString();// request.UploadType.ToDescriptionString();
            var folderName = Path.Combine("Files", subfolder, folder, name);
            if (File.Exists(folderName))
            {
                try
                {
                    File.Delete(folderName);
                    _logger.LogInformation($"File deleted ({folderName})");

                }
                catch (Exception er)
                {
                    _logger.LogError($"File delete error ({folderName})",er);
                    return await Result.FailureAsync(new string[] { er.Message });
                }

            }
            return await Result.SuccessAsync();
        }

    }
}
