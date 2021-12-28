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
    public enum ComOfferFilterForParticipant: short
    {
        [Description(@"Все")]
        [Display(Name = "Все")]
        All = 0,
        /// <summary>
        /// Действующие
        /// </summary>
        [Description(@"Действующие")]
        [Display(Name = "Действующие")]
        Actials = 1,
        /// <summary>
        /// Архивные
        /// </summary>
        [Description(@"Архивные")]
        [Display(Name = "Архивные")]
        Archives = 2,
        /// <summary>
        /// В работе
        /// </summary>
        [Description(@"В работе")]
        [Display(Name = "В работе")]
        Waitings = 3,
    }
}
