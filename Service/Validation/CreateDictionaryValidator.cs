﻿using Commons;
using Data.Dto;
using FluentValidation;
using System;

namespace Service.Validation
{
    public class CreateDictionaryValidator : AbstractValidator<CreateDictionary>
    {
        public CreateDictionaryValidator()
        {
            RuleFor(d => d.LanguageIn).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(d => d.LanguageOut).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(MessageConstants.NOT_EMPTY);
            RuleFor(d => d.LanguageIn).NotEqual(d => d.LanguageOut, StringComparer.OrdinalIgnoreCase).WithMessage("LanguageIn and LanguageOut cannot be same");
        }
    }
}
