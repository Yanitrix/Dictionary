using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service
{
    public interface IMeaningService
    {
        public Meaning GetByID(int id);
    }
}
