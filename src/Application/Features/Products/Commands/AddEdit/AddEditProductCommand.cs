// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Products.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Products.Commands.AddEdit
{
    public class AddEditProductCommand : ProductDto, IRequest<Result>, IMapFrom<Product>
    {

    }

    public class AddEditProductCommandHandler : IRequestHandler<AddEditProductCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditProductCommandHandler> _localizer;
        public AddEditProductCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditProductCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(AddEditProductCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditProductCommandHandler method 
            if (request.Id > 0)
            {
                var customer = await _context.Products.FindAsync(request.Id);
                customer = _mapper.Map(request, customer);
                _context.Products.Update(customer);
            }
            else
            {
                var customer = _mapper.Map<Product>(request);
                var createevent = new ProductCreatedEvent(customer);
                customer.DomainEvents.Add(createevent);
                _context.Products.Add(customer);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
