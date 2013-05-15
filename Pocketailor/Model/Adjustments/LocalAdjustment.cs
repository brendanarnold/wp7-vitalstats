using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model.Adjustments
{
    // An adjustment that will be attached to the Profile
    public class LocalAdjustment
    {
        public Profile Profile { get; set; }
        public GenderId Gender { get; set; }
        public BrandId Brand { get; set; }
        public ConversionId Conversion { get; set; }
        public int SizeId { get; set; }

    }
}
