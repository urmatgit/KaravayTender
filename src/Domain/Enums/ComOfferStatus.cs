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
        /// <summary>
        /// Подготовка
        /// </summary>
        [Description(@"Подготовка")]
        [Display(Name= "Подготовка")]
        Preparation = 0,
        /// <summary>
        /// Ожидание КП
        /// </summary>
        [Display(Name = "Ожидание КП")]
        [Description(@"Ожидание КП")]
        Waiting = 1,
        /// <summary>
        /// Оценка КП
        /// </summary>
        [Description(@"Оценка КП")]
        [Display(Name = "Оценка КП")]
        Evaluation = 2,
        /// <summary>
        /// Определение победителя
        /// </summary>
        [Description(@"Определение победителя")]
        [Display(Name ="Определение победителя")]
        WinnerDetermining = 3,
        /// <summary>
        /// Победитель определён
        /// </summary>
        [Description(@"Победитель определён")]
        [Display(Name = "Победитель определён")]
        WinnerDetermined = 4,
        /// <summary>
        /// Отменено
        /// </summary>
        [Description(@"Отменено")]
        [Display(Name = "Отменено")]
        Cancelled = -1

    }
}
