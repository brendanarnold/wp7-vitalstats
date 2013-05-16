using Microsoft.Phone.Shell;
using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.View
{
    public static class SecondaryTileHelpers
    {
        public static void CreateSecondaryTile(Profile p)
        {
            StandardTileData td = new StandardTileData()
            {
                BackgroundImage = null,
                Title = p.Name,
                BackTitle = p.Name,
                BackBackgroundImage = null,
                BackContent = String.Empty,
                Count = 0,
            };
            string uriStr = String.Format("/View/MeasurementsPage.xaml?Id={0}", p.Id);
            // Remove any existing tile with the same URI
            DeleteSecondaryTile(p);
            ShellTile.Create(new Uri(uriStr, UriKind.Relative), td);
        }

        public static bool DeleteSecondaryTile(Profile p)
        {
            ShellTile delTile = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().EndsWith(String.Format("Id={0}", p.Id)));
            if (delTile != null)
            {
                delTile.Delete();
                return true;
            }
            return false;
        }

    }
}
