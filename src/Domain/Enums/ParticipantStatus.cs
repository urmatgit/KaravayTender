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
        [Description(@"Победитель")]
        [Display(Name = "Победитель")]
        Winner = 0,
        
        [Description(@"Ожидание")]
        [Display(Name = "Ожидание")]
        Request = 1,

        [Description(@"Подтверждение")]
        [Display(Name = "Подтверждение")]
        Confirmed = 2,
        [Description(@"Отказ")]
        [Display(Name = "Отказ")]
        FailureParitipate = 3,
        [Description(@"Отмена")]
        [Display(Name = "Отмена")]
        Cancel = 4

    }
}
