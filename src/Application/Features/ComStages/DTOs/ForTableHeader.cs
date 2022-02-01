// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;

namespace CleanArchitecture.Razor.Application.Features.ComStages.DTOs
{
   public class ForTableHeader
    {
        public ContragentDto Contragent { get; set; }
        public string ParticipantStatus { get; set; }
    }
}
