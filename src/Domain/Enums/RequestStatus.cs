// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace CleanArchitecture.Razor.Domain.Enums
{
    public enum RequestStatus : short
    {
        [Description(@"Отмена")]
        Cancel = -1,
        [Description(@"Создана")]
        Created = 0,
        [Description(@"Зарегистрировано")]
        Registered = 1


    }
}
