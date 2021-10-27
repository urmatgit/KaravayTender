// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Models;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Razor.Application.Common.Interfaces
{
    public interface IUploadService
    {
        Task<string> UploadAsync(UploadRequest request);
        Task<IResult> UploadContragentFileAsync(int Id, List<IFormFile> files);
        Task<IResult<Dictionary<string, long>>> LoadContragentFilesAsync(int Id);
    }
}
