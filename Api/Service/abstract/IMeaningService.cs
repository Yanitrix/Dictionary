using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Abstract
{
    public interface IMeaningService
    {
        public Meaning GetByID(int id);
    }
}
