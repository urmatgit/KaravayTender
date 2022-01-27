using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.Caching;
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.DTOs;
using CleanArchitecture.Razor.Domain.Constants;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Commands.AddEdit
{
    public class AddEditQualityDocCommand: QualityDocDto,IRequest<Result<int>>, IMapFrom<QualityDoc>
    {
        public string CacheKey => QualityDocCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => QualityDocCacheTokenSource.ResetCacheToken;
        public UploadRequest UploadRequest { get; set; }
    }

    public class AddEditQualityDocCommandHandler : IRequestHandler<AddEditQualityDocCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditQualityDocCommandHandler> _localizer;
        private readonly IUploadService _uploadService;
        public AddEditQualityDocCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditQualityDocCommandHandler> localizer,
            IMapper mapper,
            IUploadService uploadService
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _uploadService = uploadService;
        }
        public async Task<Result<int>> Handle(AddEditQualityDocCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditQualityDocCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.QualityDocs.FindAsync(new object[] { request.Id }, cancellationToken);
                
                item = _mapper.Map(request, item);
                if (item.URL != request.UploadRequest.FileName)
                {
                    var result = await _uploadService.UploadAsync(request.UploadRequest, PathConstants.QualityDocPath);
                    item.URL = result;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var result = await _uploadService.UploadAsync(request.UploadRequest, PathConstants.QualityDocPath);
                var document = _mapper.Map<Document>(request);
                document.URL = result;
                
                
                var item = _mapper.Map<QualityDoc>(request);
                item.URL = result;
                var createdevent = new QualityDocEvent(item);
                document.DomainEvents.Add(createdevent);
                _context.QualityDocs.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}
