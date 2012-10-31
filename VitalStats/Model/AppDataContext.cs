﻿using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace VitalStats.Model
{
    public class AppDataContext : DataContext
    {
        public AppDataContext(string connectionString) : base(connectionString) 
        {
        }

        public Table<Profile> Profiles;

        public Table<Stat> Stats;

        public Table<Stat> StatTemplates;

        public Table<MeasurementType> MeasurementTypes;

        public Table<Unit> Units;

    
    
    
    }
}