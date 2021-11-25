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
    /// Статус коммерческого предложения (лот)
    /// </summary>
    public enum ComOfferStatus : short
    {
        [Description(@"Подготовка")]
        [Display(Name= "Подготовка")]
        Preparation = 0,
        [Display(Name = "Ожидание КП")]
        [Description(@"Ожидание КП")]
        Waiting = 1,
        [Description(@"Оценка")]
        [Display(Name = "Оценка")]
        Evaluation = 2,
        [Description(@"Определение победителя")]
        [Display(Name ="Определение победителя")]
        WinnerDetermining = 3,
        [Description(@"Победитель определён")]
        [Display(Name = "Победитель определён")]
        WinnerDetermined = 4,
        [Description(@"Отменено")]
        [Display(Name = "Отменено")]
        Cancelled = -1

    }
}
