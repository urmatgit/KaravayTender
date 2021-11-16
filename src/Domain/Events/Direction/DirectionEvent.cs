// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class DirectionEvent : DomainEvent
    {
        public DirectionEvent(Direction item)
        {
            Item = item;
        }

        public Direction Item { get; }
    }
}