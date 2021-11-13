// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;

using CleanArchitecture.Razor.Application.Products.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Razor.Application.Features.Products.Commands.AcceptChanges
{
    public class AcceptChangesProductsCommand : IRequest<Result>
    {
        public ProductDto[] Items { get; set; }
    }

    public class AcceptChangesProductsCommandHandler : IRequestHandler<AcceptChangesProductsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AcceptChangesProductsCommandHandler(
            IApplicationDbContext context,
             IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(AcceptChangesProductsCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AcceptChangesProductsCommandHandler method 
            foreach (var item in request.Items)
            {
                switch (item.TrackingState)
                {
                    case TrackingState.Added:
                        var newitem = _mapper.Map<Product>(item);
                        await _context.Products.AddAsync(newitem, cancellationToken);
                        break;
                    case TrackingState.Deleted:
                        var delitem = await _context.Products.FindAsync(new object[] { item.Id }, cancellationToken);
                        _context.Products.Remove(delitem);
                        break;
                    case TrackingState.Modified:
                        var edititem = await _context.Products.FindAsync(new object[] { item.Id }, cancellationToken);
                        //ex. edititem.Name = item.Name;
                        _context.Products.Update(edititem);
                        break;
                    case TrackingState.Unchanged:
                    default:
                        break;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
    }

}
