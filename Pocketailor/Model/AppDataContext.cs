﻿using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Pocketailor.Model
{
    public class AppDataContext : DataContext
    {
        public AppDataContext(string connectionString) : base(connectionString) 
        {
        }

        public Table<Profile> Profiles;
        public Table<Measurement> Measurements;
    }


}
