using Pocketailor.Model;
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
            // Conversions
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

            // Regions
            List<MockRegionContainer> regions = new List<MockRegionContainer>()
            {
                new MockRegionContainer() { Name="Albania"},
                new MockRegionContainer() { Name="Africa"},
                new MockRegionContainer() { Name="Antarctica"},
                new MockRegionContainer() { Name="Andes"},
                new MockRegionContainer() { Name="Belgium"},
                new MockRegionContainer() { Name="Belize"},
                new MockRegionContainer() { Name="Bosnia"},
                new MockRegionContainer() { Name="Brussels"},
                new MockRegionContainer() { Name="Basra"},
                new MockRegionContainer() { Name="Barcelona"},
                new MockRegionContainer() { Name="Belgium Again"},
            };

            this.groupedRegions = from r in regions
                                  group r by r.Name[0].ToString().ToLower() into g
                                  orderby g.Key
                                  select new LongListSelectorGroup<MockRegionContainer>(g.Key, g);

            List<MockBrandContainer> brands = new List<MockBrandContainer>()
            {
                new MockBrandContainer() { Name="Abercrombie & Fitch", Selected=false },
                new MockBrandContainer() { Name="ASDA", Selected=false },
                new MockBrandContainer() { Name="Angle Clothing", Selected=true },
                new MockBrandContainer() { Name="Boodlums", Selected=false },
                new MockBrandContainer() { Name="Bakelitewear", Selected=true },
                new MockBrandContainer() { Name="Bosch it off", Selected=false},
            };

            this.groupedBrands = from b in brands
                                 group b by b.Name[0].ToString().ToLower() into g
                                 orderby g.Key
                                 select new LongListSelectorGroup<MockBrandContainer>(g.Key, g);

        }

        private IEnumerable<LongListSelectorGroup<MockConversionData>> groupedConversions;
        public IEnumerable<LongListSelectorGroup<MockConversionData>> GroupedConversions
        {
            get
            {
                return this.groupedConversions;
            }
        }

        private IEnumerable<LongListSelectorGroup<MockRegionContainer>> groupedRegions;
        public IEnumerable<LongListSelectorGroup<MockRegionContainer>> GroupedRegions
        {
            get
            {
                return this.groupedRegions;
            }
        }

        private IEnumerable<LongListSelectorGroup<MockBrandContainer>> groupedBrands;
        public IEnumerable<LongListSelectorGroup<MockBrandContainer>> GroupedBrands
        {
            get
            {
                return this.groupedBrands;
            }
        }

        



    }

    public class MockConversionData
    {
        public string FormattedValue { get; set; }
        public string BrandName { get; set; }
        public string RegionString { get; set; }

    }

    public class MockRegionContainer
    {
        public string Name { get; set; }
    }

    public class MockBrandContainer
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
    }


}
