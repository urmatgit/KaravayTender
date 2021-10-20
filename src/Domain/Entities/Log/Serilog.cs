// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities.Log
{
    public class Serilog:IEntity
    {
        public int Id {  get; set; }
        public string Message {  get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string UserName { get; set; }
        public string ClientIP { get; set; }
        public string ClientAgent { get; set; }
        public string Properties { get; set; }
        public string LogEvent { get; set; }

    }
}
