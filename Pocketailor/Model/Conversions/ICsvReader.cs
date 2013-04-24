using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Conversions
{
    public interface ICsvReader
    {
        ConversionId ConversionId { get; }
        AppDataContext Db { get; set; }
        void QueueWriteObj(Model.SetupDatabase.CsvLine csvLine);
    }
}
