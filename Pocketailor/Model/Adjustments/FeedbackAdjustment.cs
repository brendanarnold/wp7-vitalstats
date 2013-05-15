using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Adjustments
{
    // The class that will get JSONified and sent over the wire to the server
    // Note the names are abbreviated since MongoDB stores fields with each entry
    public class FeedbackAdjustment
    {
        /// <summary>
        /// Gender of the person the adjustment applies to
        /// </summary>
        public GenderId g;

        /// <summary>
        /// Brand
        /// </summary>
        public BrandId b;

        /// <summary>
        /// (SizeId of) the fitted value that the app calculated
        /// </summary>
        public int f;

        /// <summary>
        /// SizeId of the corrected value
        /// </summary>
        public int s;

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
