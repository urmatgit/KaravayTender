// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Application.KeyValues.DTOs
{
    public partial class KeyValueDto : IMapFrom<KeyValue>
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public TrackingState TrackingState { get; set; }
    }
}
