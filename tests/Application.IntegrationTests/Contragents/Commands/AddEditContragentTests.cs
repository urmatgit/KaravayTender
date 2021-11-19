// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Features.Contragents.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.Directions.Commands.AddEdit;
using CleanArchitecture.Razor.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.Contragents.Commands
{
    using static Testing;
    [TestFixture]
    public class AddEditContragentTests: TestBase
    {
        [Test]
        public async Task SouldCreateContragent()
        {
            // TODO: Add your test code here
            //var answer = 42;
            //Assert.That(answer, Is.EqualTo(42), "Some useful error message");
            // Arrange
            var direction = new AddEditDirectionCommand
            {
                Name = "Direction 1"
            };
            var resultD = await SendAsync(direction);
            var itemF = await FindAsync<Direction>(resultD.Data);
            
            var contragentcommand = new AddEditContragentCommand
            {
                Name = "Customer1",
                ContactPerson = "Person1",
                ContactPhone = "+7(999) 1111111",
                Email = "test@gmail.com",
                INN="111111",
                FullName="customer one",
                Phone= "+7(999) 1111111",
                TypeOfActivity="adslja;dsf",
                DirectionId=itemF.Id
            };

            //Act
            var result = await SendAsync(contragentcommand);
            var item = await FindAsync<Contragent>(result.Data);
            //Assert
            //Direction
            itemF.Should().NotBeNull();
            itemF.Id.Should().Be(resultD.Data);
            itemF.Name.Should().Be(direction.Name);
            //Contragent
            item.Should().NotBeNull();
            item.Id.Should().Be(result.Data);
            item.Name.Should().Be(contragentcommand.Name);
        }
    }
}
