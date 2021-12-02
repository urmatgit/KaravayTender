// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Constants;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.Update
{
    public class UpdateSpecificationsNomenclatureCommand : IRequest<Result>
    {

        public int Id { get; set; }
        public string Description { get; set; }

        
    }
    public class UpdateSpecificationsNomenclatureCommandHandler : IRequestHandler<UpdateSpecificationsNomenclatureCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateSpecificationsNomenclatureCommandHandler> _localizer;
        private readonly IUploadService _uploadService;
        public UpdateSpecificationsNomenclatureCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateSpecificationsNomenclatureCommandHandler> localizer,
            IUploadService uploadService,
             IMapper mapper
            
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _uploadService = uploadService;
        }
        public async Task<Result> Handle(UpdateSpecificationsNomenclatureCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UpdateContragentCommandHandler method 
            var item = await _context.Nomenclatures.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item != null   )
            {

                var files = await _uploadService.LoadFilesAsync(request.Id, PathConstants.SpecificationsPath);
                if (files.Succeeded)
                {
                    var newfiles = string.Join(PathConstants.FilesStringSeperator, files.Data.Select(f => Path.GetFileName(f)));
                    if (item.Specifications != newfiles)
                    {
                        item.Specifications = newfiles;
                        var createevent = new NomenclatureEvent(item);
                        item.DomainEvents.Add(createevent);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
               
            }
            return Result.Success();
        }
    }
}