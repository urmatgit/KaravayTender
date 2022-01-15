// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs
{
    public class ParticipantCrossDto
    {
        public int ComOfferId { get; set; }
        public int ContragentId { get; set; }
        public int ComStageId { get; set; }
        public ParticipantStatus Status { get; set; }
        public string Description { get; set; }
        //current Stage number
        public int Number { get; set; }
    }
}
