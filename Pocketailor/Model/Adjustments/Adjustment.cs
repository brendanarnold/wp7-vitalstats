using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Adjustments
{
     // The class that will get JSONified and sent over the wire to the server
    // Note the names are abbreviated since MongoDB stores fields with each entry
    public class Adjustment
    {
        /// <summary>
        /// Gender of the person the adjustment applies to
        /// </summary>
        public GenderId g;

        /// <summary>
        /// Brand
        /// </summary>
        public RetailId b;

        /// <summary>
        /// Region
        /// </summary>
        public RegionId r;

        /// <summary>
        /// (Index of) the fitted value that the app calculated
        /// </summary>
        public int f;

        /// <summary>
        /// Adjustment to the fitted value e.g. +1, 0, -1
        /// </summary>
        public int a;

        /// <summary>
        /// App ID, a GUID for this installlation
        /// </summary>
        public string i;

        /// <summary>
        /// Conversion ID
        /// </summary>
        public ConversionId c;

        /// <summary>
        /// App version
        /// </summary>
        public string v;

        /// <summary>
        /// Time logged in UNIX time format i.e. seconds since 1/1/1970 UTC
        /// </summary>
        public long t;
    }
    
}
