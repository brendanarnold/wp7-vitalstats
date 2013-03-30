using System;
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
        public Table<Stat> Stats;
        public Table<Conversions.Trousers> Trousers;
        public Table<Conversions.Shirt> Shirts;
        public Table<Conversions.Hat> Hats;
        public Table<Conversions.Suit> Suits;
        public Table<Conversions.DressSize> DressSizes;
        public Table<Conversions.Bra> Bras;
        public Table<Conversions.Hosiery> Hosiery;
        public Table<Conversions.Shoes> Shoes;
        public Table<Conversions.SkiBoots> SkiBoots;
        public Table<Conversions.TennisRaquetSize> TennisRaquetSizes;
        public Table<Conversions.Wetsuit> Wetsuits;
    
    
    }
}
