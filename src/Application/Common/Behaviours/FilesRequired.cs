// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Constants;

namespace CleanArchitecture.Razor.Application.Common.Behaviours
{
    public class FilesRequired: ValidationAttribute
    {
        
        public string _subFolder { get; private set; }
        public FilesRequired(
                    string subFolder
            )
        {
            
            _subFolder = subFolder;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errorType;
            var filesFolder = PathConstants.GetSubFolder(_subFolder);
            IBaseEntity baseEntity = (IBaseEntity)validationContext.ObjectInstance;

            var filesExist = Directory.GetFiles(Path.Combine(filesFolder,baseEntity.Id.ToString())).Any();
            if (value == null && !filesExist)
            {
                errorType = "required";
            }
            else
            {
                return ValidationResult.Success;
            }
            ErrorMessage = $"{validationContext.DisplayName}  field is {errorType}.";
            return new ValidationResult(ErrorMessage);
        }
    }
}
