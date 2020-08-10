using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface ISpeechPartPropertyService : IService<SpeechPartProperty>
    {
        public SpeechPartProperty GetByID(int id);

    }
}
