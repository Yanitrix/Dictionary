﻿using Domain.Dto;
using FluentValidation;

namespace Domain.Validation
{
    public class WordPropertyDtoValidator : AbstractValidator<WordPropertyDto>
    {
        public WordPropertyDtoValidator()
        {
            RuleFor(wp => wp.Name).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
            RuleFor(wp => wp.Values).NotEmpty();
            RuleForEach(wp => wp.Values).Cascade(CascadeMode.Stop).NotEmpty().NoDigits();
        }
    }
}
