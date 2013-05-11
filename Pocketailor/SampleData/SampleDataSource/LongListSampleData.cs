using Pocketailor.Model.Conversions;
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

            IEnumerable<MockConversionData> conversions = new List<MockConversionData>() {
                    new MockConversionData() {
                        FormattedValue = "10 (L)",
                        BrandName = "ASOS",
                    },
                    new MockConversionData() {
                        FormattedValue = "10 (M)",
                        BrandName = "Abercrombie & Fitch",
                    },
                    new MockConversionData() {
                        FormattedValue = "8",
                        BrandName = "Abcdef",
                    },
                    new MockConversionData() {
                        FormattedValue = "18 (XXL)",
                        BrandName = "Some long named retailer",
                    },
                    new MockConversionData() {
                        FormattedValue = "12",
                        BrandName = "Marks & Spencers",
                    },
                };

            this.groupedConversions = new List<LongListSelectorGroup<MockConversionData>>()
            {
                new LongListSelectorGroup<MockConversionData>("a", conversions),
                new LongListSelectorGroup<MockConversionData>("b", conversions),
                new LongListSelectorGroup<MockConversionData>("c", conversions),
                new LongListSelectorGroup<MockConversionData>("d", conversions),
                new LongListSelectorGroup<MockConversionData>("e", conversions),
            };
        }

        private List<LongListSelectorGroup<MockConversionData>> groupedConversions;
        public List<LongListSelectorGroup<MockConversionData>> GroupedConversions
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

    }


}
