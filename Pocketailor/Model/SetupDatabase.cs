using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.IO;
using Pocketailor.Model.Conversions;

namespace Pocketailor.Model
{
    public class SetupDatabase
    {

        public static void InitialiseDB(AppDataContext db)
        {
            db.CreateDatabase();
        }

        public static void EmptyDB(AppDataContext db)
        {
            // The order here is important so as to not leave hanging references
            db.Stats.DeleteAllOnSubmit(db.Stats);
            db.Profiles.DeleteAllOnSubmit(db.Profiles);
            db.SubmitChanges();
        }

        
 
    }
}
