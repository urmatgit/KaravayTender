using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.Caching;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.AddEdit
{
    public class AddOrDelContragentCategorysCommand : ContragentCategoryDto,IRequest<Result<int>>, IMapFrom<ContragentCategory>
    {
        public string CategoriesJson { get; set; }
         
    }

    public class AddOrDelContragentCategorysCommandHandler : IRequestHandler<AddOrDelContragentCategorysCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddOrDelContragentCategorysCommandHandler> _localizer;
        public AddOrDelContragentCategorysCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddOrDelContragentCategorysCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddOrDelContragentCategorysCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditContragentCategoryCommandHandler method
            if (!string.IsNullOrEmpty(request.CategoriesJson))
            {
                try
                {
                    List<CategoryDto> categories = JsonConvert.DeserializeObject<List<CategoryDto>>(request.CategoriesJson);

                    if (request.ContragentId > 0 && categories?.Count > 0)
                    {
                        foreach (CategoryDto category in categories)
                        {
                            var item = await _context.ContragentCategories.FindAsync(new object[] { request.ContragentId, category.Id }, cancellationToken);
                            if (item != null)
                            {
                                //Finded
                                if (!category.IsCheck)
                                {
                                    //delete
                                    item = new ContragentCategory()
                                    {
                                        ContragentId = request.ContragentId,
                                        CategoryId = category.Id
                                    };
                                    _context.ContragentCategories.Remove(item);
                                }


                            }
                            else
                            {
                                if (category.IsCheck)
                                {
                                    //add
                                    item = new ContragentCategory()
                                    {
                                        ContragentId = request.ContragentId,
                                        CategoryId = category.Id
                                    };
                                    _context.ContragentCategories.Add(item);
                                }
                            }
                        }
                        await _context.SaveChangesAsync(cancellationToken);
                        return Result<int>.Success(request.ContragentId);
                    }
                }catch (Exception er)
                {
                   return Result<int>.Failure(new string[] { er.Message });
                }
            }
            //else
            //{
            //    var item = _mapper.Map<ContragentCategory>(request);
            //    _context.ContragentCategories.Add(item);
            //    await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(request.ContragentId);
            //}
           
        }
    }
}
