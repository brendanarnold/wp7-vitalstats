# vim: set fileencoding=utf-8 :
# Above is necessary for the half substitution


TMP_FILE_SUFFIX = '.tmp'
REGIONID_FN = r"..\..\Static\RegionIds.cs"
MEASUREMENTID_FN = r"..\..\Static\MeasurementIds.cs"
RETAILID_FN = r"..\..\Static\RetailIds.cs"
CONVERSIONID_FN = r"..\..\Static\ConversionIds.cs"
ID_FN = r"..\..\Static\RegionId.cs"
FILE_FORMATS_FN = r'CsvFileFormats.txt'
IN_FN = r'Gap_Female_ShirtSize.txt'
GENDERS = ['Male', 'Female', 'Unspecified']
TMP_FILE_DIR = "test\\"
OUT_FILE_DIR = "..\\"

def get_csharp_enums(fn, enum_name):
    '''Code to read enums from c# source'''
    start_record = False
    ids = []
    with open(fn, 'r') as fh:
        for line in fh:
            if ('enum ' + enum_name) in line:
                start_record = True
                continue
            if start_record:
                line = line.strip()
                if (line in ['{', '']) or line.startswith("//"):
                    continue
                if line == '}':
                    break
                else:
                    ids.append(line.split('=')[0].replace(',','').strip())
    return ids

# Read in the formats indexed by MeasurementId
format_dict = dict()
with open(FILE_FORMATS_FN, 'r') as fh:
    for line in fh:
        els = [el.strip() for el in line.split('\t')]
        format_dict[els[0]] = els[1:]
# Read in the region ids
region_ids = get_csharp_enums(REGIONID_FN, 'RegionIds')
# Get measurement ids
measurement_ids = get_csharp_enums(MEASUREMENTID_FN, 'MeasurementId')
# Get retail ids
retail_ids = get_csharp_enums(RETAILID_FN, 'RetailId')
# Get conversion ids
conversion_ids = get_csharp_enums(CONVERSIONID_FN, 'ConversionId')

# Create or wipe temporary build files and insert headerline
for conversion_id in conversion_ids:
    fn = TMP_FILE_DIR + conversion_id + TMP_FILE_SUFFIX
    with open(fn, 'w') as fh:
        fh.write('\t'.join(format_dict[conversion_id]) + '\n')

# TODO iterate files
in_fn = IN_FN

# Parse and sanity check filename
retailer, gender, conversion = in_fn.split('.')[0].split('_')
if retailer not in retail_ids:
    raise Exception("No retailer in RetailIds called: " + retailer)
if gender not in GENDERS:
    raise Exception("Gender not recognised: " + gender)
if conversion not in conversion_ids:
    raise Exception("No conversion in ConversionIds called: " + conversion)

# Read in data
with open(in_fn, 'r') as fh:
    size_letters = []
    size_numbers = dict() # By region
    measurements = dict() # By measurement
    for line in fh:
        line = line.strip()
        if not line:
            continue
        if line.startswith("#"):
            continue
        els = [el.strip() for el in line.split("\t")]
        if els[0] == "SizeLetter":
            size_letters = els[1:]
        elif els[0] in region_ids:
            size_numbers[els[0]] = els[1:]
        elif els[0] in measurement_ids:
            measurements[els[0]] = [el.replace("½", ".5") for el in els[1:]]
        else:
            raise Exception("Unparseable line:\n" + line)
# Append data to the temporary file
out_fn = TMP_FILE_DIR + conversion + TMP_FILE_SUFFIX
with open(out_fn, 'a') as fh:
    fmt = format_dict[conversion]
    for region in size_numbers.keys():
        for i, size_number in enumerate(size_numbers[region]):
            line = []
            for header in fmt:
                if header == "SizeLetter":
                    line.append(size_letters[i])
                elif header == "SizeNumber":
                    line.append(size_number)
                elif header == "Gender":
                    line.append(gender)
                elif header == "Region":
                    line.append(region)
                elif header == "Retailer":
                    line.append(retailer)
                elif header in measurements.keys():
                    line.append(measurements[header][i])
                else:
                    line.append("")
            fh.write("\t".join(line) + "\n")
    
    
