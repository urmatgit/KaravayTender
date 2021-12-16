// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Domain.Enums
{
    /// <summary>
    /// Статус участника торгов
    /// </summary>
    public enum ParticipantStatus:short
    {
        [Description(@"Не участвует")]
        [Display(Name = "Не участвует")]
        NotParticipate = 0,
        
        [Description(@"Участвует")]
        [Display(Name = "Участвует")]
        Participates = 1,
        [Description(@"В ожидании")]
        [Display(Name = "В ожидании")]
        Waiting = 2
    }
}
