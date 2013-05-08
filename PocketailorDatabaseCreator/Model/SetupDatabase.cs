using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketailorDatabaseCreator.Model
{
    public class SetupDatabase
    {

        public static void LoadConversions(ConversionsDataContext db)
        {
            CsvFileReader.ReadFileIntoDb(db, ConversionId.SuitSize);
            CsvFileReader.ReadFileIntoDb(db, ConversionId.ShirtSize);
            CsvFileReader.ReadFileIntoDb(db, ConversionId.TrouserSize);
            CsvFileReader.ReadFileIntoDb(db, ConversionId.HatSize);
            CsvFileReader.ReadFileIntoDb(db, ConversionId.DressSize);
            CsvFileReader.ReadFileIntoDb(db, ConversionId.BraSize);
            CsvFileReader.ReadFileIntoDb(db, ConversionId.HosierySize);
            CsvFileReader.ReadFileIntoDb(db, ConversionId.ShoeSize);
            CsvFileReader.ReadFileIntoDb(db, ConversionId.SkiBootSize);
            CsvFileReader.ReadFileIntoDb(db, ConversionId.WetsuitSize);

        }

 
    }
}
