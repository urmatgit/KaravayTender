// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class CategoryEvent : DomainEvent
    {
        public CategoryEvent(Category item)
        {
            Item = item;
        }

        public Category Item { get; }
    }
}
