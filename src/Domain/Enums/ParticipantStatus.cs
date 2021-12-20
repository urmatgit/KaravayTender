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
        //[Description(@"Не участвует")]
        //[Display(Name = "Не участвует")]
        //NotParticipate = 0,
        
        [Description(@"Запрос цены")]
        [Display(Name = "Запрос цены")]
        PriceRequest = 1,

        [Description(@"Подтвержден цена")]
        [Display(Name = "Подтвержден цена")]
        PriceConfirmed = 2,
        [Description(@"Отказ от участие")]
        [Display(Name = "Отказ от участие")]
        FailureParitipate = 3,
        [Description(@"Исключен")]
        [Display(Name = "Исключен")]
        Excluded = -1

    }
}
