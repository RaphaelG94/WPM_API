﻿using WPM_API.Data.DataContext.Entities;
using WPM_API.Models;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Code.Mappers.CSV_Mapper
{
    public sealed class DefaultMap : ClassMap<Default>
    {
        public DefaultMap()
        {
            Map(m => m.Name).Index(0);
            Map(m => m.Value).Index(1);
            Map(m => m.Category).Index(2);
            Map(m => m.CustomerId).Ignore();
            Map(m => m.Id).Ignore();
            Map(m => m.Customer).Ignore();
        }
    }
}
