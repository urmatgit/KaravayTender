// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Constants
{
    public  static class ErrorMessages
    {
        public   const string NotFoundComStage = "Не найден этап";
        public  const string StatusDontMatchForSentRequest = "Статус КП не соответствует для отправки запроса!";

        public const string ParcipantsNotFound= "Участники не добавлены";
        public const string PositionsNotFound = "Позиции не добавлены";
    }
}
