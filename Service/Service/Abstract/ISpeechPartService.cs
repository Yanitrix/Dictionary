using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface ISpeechPartService : IService<SpeechPart>
    {
        SpeechPart GetByNameAndLanguage(String languageName, String name);

        SpeechPart GetByIndex(int index);
    }
}
