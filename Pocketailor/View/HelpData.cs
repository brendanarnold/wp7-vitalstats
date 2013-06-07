using Pocketailor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.View
{
    public class HelpData
    {

        public Dictionary<MeasurementId, string> HelpText;
        public Dictionary<MeasurementId, string> HelpImgMale;
        public Dictionary<MeasurementId, string> HelpImgFemale;

        public HelpData()
        {
        

            this.HelpText = new Dictionary<MeasurementId, string>
            {
                { MeasurementId.Sleeve,  @"Place arm on hip.

Measure from the centre of the neck, over the top of the shoulder, around the outside of the arm to the wrist." },
                { MeasurementId.Height, @"Stand upright next to a wall.

Measure from floor to the top of the head along the wall." },
                { MeasurementId.Chest, @"Tuck tape measure under the arms. Measure around the upper torso at the widest part.

For women this will likely be across the top of the breasts, for men this will likely be the highest the tape measure will go under the arms." },
                { MeasurementId.Hips, @"Measure around the hips at the widest part." },
                { MeasurementId.Waist, @"Wrap tape measure around the mid/lower torso.

For women, take the measurement just above the belly button at the narrowest part. For men, take the measurement just below the belly button."},
                { MeasurementId.InsideLeg, @"Stand upright and measure from the crotch to where you expect the trouser seam to stop."},
                { MeasurementId.Neck, @"Wrap the tape around the neck, over the Adam's apple.

Tuck in four fingers as you make the measurement to allow breathing space." },
                { MeasurementId.UnderBust, @"Measure around the upper torso so that crosses just below the breasts." },
                { MeasurementId.Head, @"Measure around the head at the level where you expect a hat to sit."},
                { MeasurementId.FootWidth, @"Draw around the foot on a piece of paper.

Measure the widest part at the ball of the foot."},
                { MeasurementId.FootLength, @"Draw around the foot on a piece of paper.

Measure the longest distance from the heel to the big toe." },
                { MeasurementId.TorsoLength, @"Stand upright.

Measure from the base of the back of the collar down to where you expect the bottom of the shirt to reach." },
                { MeasurementId.Shoulder, @"Measure from the seam on one should, across the front of the body to the seam on the other shoulder.

The seam should lie just past the corner of the shoulder." },
                { MeasurementId.OutsideLeg, @"Stand upright and measure from where you typically wear the top of your trousers down to where you expect the trouser seam to stop." },
                { MeasurementId.Weight, @"Stand evenly on the scales and read off the value." },
            };

            this.HelpImgMale = new Dictionary<MeasurementId, string>()
            {
                { MeasurementId.Sleeve, "SleeveMan.png" },
                { MeasurementId.Height, "HeightMan.png" },
                { MeasurementId.Chest, "ChestMan.png" },
                { MeasurementId.Hips, "HipsMan.png" },
                { MeasurementId.Waist, "WaistMan.png" },
                { MeasurementId.InsideLeg, "InsideLegMan.png"},
                { MeasurementId.Neck, "NeckMan.png"},
                { MeasurementId.UnderBust, "UnderBustMan.png" },
                { MeasurementId.Head, "HeadMan.png" },
                { MeasurementId.FootWidth, "FootWidthMan.png" },
                { MeasurementId.FootLength, "FootLengthMan.png" },
                { MeasurementId.TorsoLength, "TorsoLengthMan.png" },
                { MeasurementId.Shoulder, "ShoulderMan.png" },
                { MeasurementId.OutsideLeg, "OutsideLegMan.png" },
                { MeasurementId.Weight, "WeightMan.png" },
            };

            this.HelpImgFemale = new Dictionary<MeasurementId, string>
            {
                { MeasurementId.Sleeve, "SleeveWoman.png" },
                { MeasurementId.Height, "HeightWoman.png" },
                { MeasurementId.Chest, "ChestWoman.png" },
                { MeasurementId.Hips, "HipsWoman.png" },
                { MeasurementId.Waist, "WaistWoman.png" },
                { MeasurementId.InsideLeg, "InsideLegWoman.png"},
                { MeasurementId.Neck, "NeckWoman.png"},
                { MeasurementId.UnderBust, "UnderBustWoman.png" },
                { MeasurementId.Head, "HeadWoman.png" },
                { MeasurementId.FootWidth, "FootWidthWoman.png" },
                { MeasurementId.FootLength, "FootLengthWoman.png" },
                { MeasurementId.TorsoLength, "TorsoLengthWoman.png" },
                { MeasurementId.Shoulder, "ShoulderWoman.png" },
                { MeasurementId.OutsideLeg, "OutsideLegWoman.png" },
                { MeasurementId.Weight, "WeightWoman.png" },
            };

        }

    }
}
