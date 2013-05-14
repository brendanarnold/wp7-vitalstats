using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{
    public class ConversionsDataContext : DataContext
    {
        public ConversionsDataContext(string connectionString) : base(connectionString) 
        {
        }

        public Table<ConversionData> ConversionData;

    }
}
