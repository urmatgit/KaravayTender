// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Domain.Enums
{

    /// <summary>
    /// Статус коммерческого предложения (лот)
    /// </summary>
    public enum LotStatus : short
    {
        [Description(@"Подготовка")]
        Preparation = 0,
        [Description(@"Ожидание КП")]
        Waiting = 1,
        [Description(@"Оценка")]
        Evaluation = 2,
        [Description(@"Определение победителя")]
        WinnerDetermining = 3,
        [Description(@"Победитель определён")]
        WinnerDetermined = 4,
        [Description(@"Отменено")]
        Cancelled = -1

    }
}
