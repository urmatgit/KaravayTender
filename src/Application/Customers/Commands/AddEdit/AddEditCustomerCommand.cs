// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;

namespace CleanArchitecture.Razor.Application.Customers.Commands.AddEdit
{
    public class AddEditCustomerCommand:IRequest<Result>, IMapFrom<Customer>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Customer, AddEditCustomerCommand>().ReverseMap();
    
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameOfEnglish { get; set; }
        public string GroupName { get; set; }
        public PartnerType PartnerType { get; set; }

        public string Region { get; set; }
        public string Sales { get; set; }
        public string RegionSalesDirector { get; set; }

        public string Address { get; set; }

        public string AddressOfEnglish { get; set; }

        public string Contract { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Fax { get; set; }
        public string Comments { get; set; }
    }

    public class AddEditCustomerCommandHandler : IRequestHandler<AddEditCustomerCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddEditCustomerCommandHandler(
            IApplicationDbContext context,
             IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(AddEditCustomerCommand request, CancellationToken cancellationToken)
        {

           
            if (request.Id > 0)
            {
                var customer = await _context.Customers.FindAsync(request.Id);
                customer=_mapper.Map(request, customer);
            }
            else
            {
                var customer = _mapper.Map<Customer>(request);
                var createevent = new CustomerCreatedEvent(customer);
                customer.DomainEvents.Add(createevent);
                _context.Customers.Add(customer);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
    }
}