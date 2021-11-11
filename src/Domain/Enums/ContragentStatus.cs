// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace CleanArchitecture.Razor.Domain.Enums
{
    public enum ContragentStatus : short
    {
        [Description(@"Удален")]
        Deleted = -1,
        [Description(@"Не активен")]
        NotActive = 0,
        [Description(@"На регистрации")]
        OnRegistration = 1,
        [Description(@"Зарегистрирован")]
        Registered = 2,
        [Description(@"Отклонена")]
        Reject = 3

    }
}
