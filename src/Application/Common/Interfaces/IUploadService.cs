// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Razor.Application.Common.Interfaces
{
    public interface IUploadService
    {
        Task<string> UploadAsync(UploadRequest request);
        Task<string> UploadAsync(UploadRequest request,string subFolder);
        Task<IResult> UploadFileAsync(int Id,string subfolder, List<IFormFile> files);
        Task<IResult<List<string>>> LoadFilesAsync(int Id, string subfolder);
        Task<IResult> RemoveFileAsync(int Id, string name, string subfolder);

    }
}
