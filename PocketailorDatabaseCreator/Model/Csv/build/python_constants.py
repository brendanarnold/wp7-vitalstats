from csharp_utils import *

BUILD_FILE_SUFFIX = '.tmp'
DEPLOY_FILE_SUFFIX = '.txt'
REGIONID_FN = r"..\..\..\..\Pocketailor\Model\Static\RegionIds.cs"
MEASUREMENTID_FN = r"..\..\..\..\Pocketailor\Model\Static\MeasurementIds.cs"
BRANDID_FN = r"..\..\..\..\Pocketailor\Model\Static\BrandIds.cs"
CONVERSIONID_FN = r"..\..\..\..\Pocketailor\Model\Static\ConversionIds.cs"
ID_FN = r"..\..\..\..\Pocketailor\Model\Static\RegionId.cs"
FILE_FORMATS_FN = r'CsvFileFormats.txt'
GENDERS = ['Male', 'Female', 'Unspecified']
BUILD_DIR = "test\\"
DEPLOY_DIR = "..\\"


# Read in the region ids
REGION_IDS = get_csharp_enums(REGIONID_FN, 'RegionId')
# Get measurement ids
MEASUREMENT_IDS = get_csharp_enums(MEASUREMENTID_FN, 'MeasurementId')
# Get retail ids
BRAND_IDS = get_csharp_enums(BRANDID_FN, 'BrandId')
# Get conversion ids
CONVERSION_IDS = get_csharp_enums(CONVERSIONID_FN, 'ConversionId')

# Compile a list of acceptable headers 
OK_HEADERS = [x.lower() for x in MEASUREMENT_IDS] + [x.lower() for x in REGION_IDS] + ['sizeletter', 'sizeid']
