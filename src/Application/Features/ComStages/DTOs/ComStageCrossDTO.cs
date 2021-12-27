// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Features.ComStages.DTOs
{
    public class ComStageCrossDTO
    {
        public string[] Header { get; set; }
        public object Body { get; set; }

        public int? CurrentStage { get; set; }
        public int? CurrentStageId { get; set; }
        public DateTime DeadlineDate { get; set; }
    }
}
