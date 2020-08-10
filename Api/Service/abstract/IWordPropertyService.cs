﻿using Dictionary_MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Service.Abstract
{
    public interface IWordPropertyService : IService<WordProperty>
    {
        public WordProperty GetByID(int id);
    }
}
