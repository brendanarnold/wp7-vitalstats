using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PocketailorDatabaseCreator.Model;

namespace PocketailorDatabaseCreator.Model.CsvReaders
{
    public interface ICsvReader
    {
        ConversionId ConversionId { get; }
        ConversionsDataContext Db { get; set; }
        void QueueWriteObj(CsvLine csvLine);
    }
}
