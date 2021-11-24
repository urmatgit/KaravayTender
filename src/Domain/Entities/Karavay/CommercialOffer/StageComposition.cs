// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
    public class StageComposition : AuditableSoftDeleteEntity, IAuditTrial
    {
        public int ComStageId { get; set; }
        public virtual ComStage ComStage {get;set;}
        public int ContragentId { get; set; }
        public virtual Contragent Contragent { get; set; }
        public int ComPositionId { get; set; }
        public virtual ComPosition ComPosition { get; set; }

    }
}
