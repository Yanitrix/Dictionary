using Api.Dto;
using Api.Service;
using AutoMapper;
using Dictionary_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/languages")]
    public class LanguageController : Controller

    {
        private readonly IService<Language> service;
        private readonly IMapper mapper;

        public LanguageController(IService<Language> service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;

            Language german = new Language
            {
                Name = "german",
            };
            Language english = new Language
            {
                Name = "english",
            };

            languages = new List<Language>
            {
                german, english
            };
        }

        private IEnumerable<Language> languages;

        public IEnumerable<LanguageDto> Index()
        {
            return languages.Select(x => mapper.Map<LanguageDto>(x));
        }
    }
}
