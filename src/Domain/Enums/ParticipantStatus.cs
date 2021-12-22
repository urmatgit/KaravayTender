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
        
        [Description(@"Запрос цены")]
        [Display(Name = "Запрос цены")]
        PriceRequest = 1,

        [Description(@"Цена предоставлена")]
        [Display(Name = "Цена предоставлена")]
        PriceConfirmed = 2,
        [Description(@"Отказ поставщика")]
        [Display(Name = "Отказ поставщика")]
        FailureParitipate = 3,
        [Description(@"Исключен менеджером")]
        [Display(Name = "Исключен менеджером")]
        Excluded = -1

    }
}
