﻿using Pocketailor.Model;
using Pocketailor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expression.Blend.SampleData.SampleDataSource
{
    public class LongListSampleData : SampleDataSource
    {
        public LongListSampleData() : base()
        {

            List<MockConversionData> conversions = new List<MockConversionData>() {
                    new MockConversionData() {
                        FormattedValue = "10 (L)",
                        RegionString = "Europe",
                        BrandName = "ASOS",
                    },
                    new MockConversionData() {
                        FormattedValue = "10 (M)",
                        BrandName = "Abercrombie & Fitch",
                        RegionString = "US",
                    },
                    new MockConversionData() {
                        FormattedValue = "8",
                        BrandName = "Abcdef",
                        RegionString = "Europe",
                    },
                    new MockConversionData() {
                        FormattedValue = "18 (XXL)",
                        BrandName = "Some long named retailer",
                        RegionString = "Europe",
                    },
                    new MockConversionData() {
                        FormattedValue = "12",
                        BrandName = "Marks & Spencers",
                        RegionString = "Europe",
                    },
                    new MockConversionData() {
                        FormattedValue = "12",
                        BrandName = "Jaeger",
                        RegionString = "Europe",
                    },
                };

            this.groupedConversions = from c in conversions
                                      group c by c.BrandName[0].ToString().ToLower() into g
                                      orderby g.Key
                                      select new LongListSelectorGroup<MockConversionData>(g.Key, g);
        }

        private IEnumerable<LongListSelectorGroup<MockConversionData>> groupedConversions;
        public IEnumerable<LongListSelectorGroup<MockConversionData>> GroupedConversions
        {
            get
            {
                return this.groupedConversions;
            }
        }
    }

    public class MockConversionData
    {
        public string FormattedValue { get; set; }
        public string BrandName { get; set; }
        public string RegionString { get; set; }

    }


}
