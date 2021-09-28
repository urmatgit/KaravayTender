// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.UnitTests.Common.Cache
{
    public class CashingKeyTests
    {
        public CashingKeyTests()
        {

        }
        [Test]
        public void GetCachKeys()
        {

            // Arrange
            string key = "CashingTests-test1";
            //Act
            string keyname = this.AllCacheKey("test1");
            //Assert
            keyname.Should().Be(key);
        }
        [Test]
        public void GetCachKeyWithParameter()
        {

            // Arrange
            string key = "CashingTests,test1";
            //Act
            string keyname = this.GetPagtionCacheKey("test1");
            //Assert
            keyname.Should().Be(key);
        }

    }
}
