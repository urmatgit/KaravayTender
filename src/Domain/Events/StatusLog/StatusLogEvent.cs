// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class StatusLogEvent : DomainEvent
    {
        public StatusLogEvent(StatusLog item)
        {
            Item = item;
        }

        public StatusLog Item { get; }
    }
}
