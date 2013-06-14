# vim: set fileencoding=utf-8 :
# Above is necessary for the half substitution


from iso3166 import countries
from python_constants import *
import os



COPY_TO_DEPLOY = True

IN_DIR = r'Raw'


def main():
    # Read in the formats indexed by MeasurementId
    format_dict = dict()
    with open(FILE_FORMATS_FN, 'r') as fh:
        for line in fh:
            els = [el.strip().lower() for el in line.split('\t')]
            format_dict[els[0]] = els[1:]
    # Create or wipe temporary build files, insert headerline and create
    # handles to the files
    build_fns = {}
    deploy_fns = {}
    for conversion_id in CONVERSION_IDS:
        conversion_id = conversion_id.lower()
        build_fns[conversion_id] = BUILD_DIR + conversion_id + BUILD_FILE_SUFFIX
        deploy_fns[conversion_id] = DEPLOY_DIR + conversion_id + DEPLOY_FILE_SUFFIX
        with open(build_fns[conversion_id], 'w') as fh:
            fh.write('\t'.join(format_dict[conversion_id]) + '\n')

    # TODO iterate directories
    for in_dir, dirs, in_fns in os.walk(IN_DIR):
        if 'test' in in_dir:
            continue
        # Sanity check the dir name
        brand = os.path.basename(in_dir).lower()
        if brand not in [b.lower() for b in BRAND_IDS]:
            print "WARNING: No brand in BrandId called: " + brand
            continue
        # Create a list of filenames and auxialliary filenames
        fns = []
        aux_fns = []
        for fn in in_fns:
            if not fn.endswith('.txt'):
                continue
            # Parse and sanity check filename
            gender, conversion, aux = parse_fn(fn)
            if (aux == None) or aux.lower() not in ['sleeve', 'insideleg', 'height']:
                fns.append(fn)
            else:
                aux_fns.append(fn)
        # Read each file into the output CSV file
        for fn in fns:
            # Read in the data
            data = read_csv_input(os.path.join(in_dir, fn), is_aux=False)
            gender, conversion, aux = parse_fn(fn)
            # Read in auxilliary data if any and use to augment the ordinary data
            aux_data = get_aux_file_data(fn, aux_fns, in_dir)
            if aux_data != None:
                data = combine_aux_data(data, aux_data)
            # Write the data for each region in the file  n.b. files with a
            # region in the filename also have the region in the file in the
            # usual place so no need to extract from the filename
            # regions = [r.lower() for r in data.keys() if r.lower() in [i.lower() for i in REGION_IDS]]
            regions = [r for r in data.keys() if (get_two_letter_iso_code(r) is not None)]
            if not len(regions):
                print "WARNING: No region data for: " + in_dir + fn
                continue
            measurements = [r.lower() for r in data.keys() if r.lower() in [i.lower() for i in MEASUREMENT_IDS]]
            if not len(measurements):
                print "WARNING: No measurement data for: " + os.path.join(in_dir,fn)
                continue
            for region in regions:
                write_csv_output(data, format_dict[conversion], build_fns[conversion], gender, region, brand)
    # Finally copy the temporary files to the deployment area
    if COPY_TO_DEPLOY:
        import shutil
        for conversion in [c.lower() for c in CONVERSION_IDS]:
            shutil.move(build_fns[conversion], deploy_fns[conversion])
            print "%s -> %s" % (build_fns[conversion], deploy_fns[conversion])

def combine_aux_data(data, aux_data):
    # Need to replicate the main data set for each data point in
    # the auxilliary data
    num_data_els = len(data['sizeid'])
    num_aux_data_els = len(aux_data['sizeid'])
    for dk in data.keys():
        if get_two_letter_iso_code(dk) is not None:
            all_sizes = []
            for aux_sz in aux_data['aux_size']:
                all_sizes = all_sizes + [ sz + ' (%s)' % aux_sz for sz in data[dk]]
            data[dk] = all_sizes
        # Need to combine the sizeid of the auxilliary
        # measurements with the main measurements
        # aux sizeids are usually -2,0,2 ... whereas sizeids ae
        # usually -20, 0, 20, ...
        elif dk == 'sizeid':
            all_sizeids = []
            for aux_szid in aux_data['sizeid']:
                all_sizeids = all_sizeids + [ str(int(aux_szid) + int(szid)) for szid in data['sizeid']]
            data['sizeid'] = all_sizeids
        else:
            data[dk] = num_aux_data_els * data[dk]
    # Finally add the axilliary measuremnts
    for aux_dk in aux_data.keys():
        if aux_dk not in  ['aux_size', 'sizeid']:
            data[aux_dk] = aux_data[aux_dk] * num_data_els
    return data


def get_aux_file_data(fn, aux_fns, in_dir):
    '''Obtains the data in the auxialliary file if there is any, otherwise returns None'''
    if fn in aux_fns:
        return None
    stem = fn.split('.')[0]
    aux_data = None
    for aux_fn in aux_fns:
        if stem == aux_fn[0:len(stem)]:
            aux_data = read_csv_input(os.path.join(in_dir, aux_fn), is_aux=True)
            break
    return aux_data

                
def parse_fn(fn):
    els = fn.split('.')[0].split('_')
    gender, conversion = [el.lower() for el in els[0:2]]
    conversion += 'size'
    if len(els) == 3:
        aux = els[2]
    else:
        aux = None
    if gender not in [g.lower() for g in GENDERS]:
        raise Exception("Gender not recognised: " + str(gender))
    if conversion not in [c.lower() for c in CONVERSION_IDS]:
        raise Exception("No conversion in ConversionIds called: " + str(conversion))
    if (aux != None) \
        and (get_two_letter_iso_code(correct_common_header_typos(aux.lower())) is None) \
        and (aux not in ['', 'InternationalConversions', 'InsideLeg', 'Height', 'Sleeve']):
        raise Exception("No extra filefeature called: " + str(aux))
    return (gender, conversion, aux)


def write_csv_output(data, fmt, fn, gender, region, brand):
    # Process the normal data according to the format dictionary
    with open(fn, 'a') as out_fh:
        for i,sz in enumerate(data['sizeid']):
            # Don't write data for which there are no sizes
            if data[region][i] == '':
                continue
            row = []
            for header in fmt:
                if header == 'gender':
                    row.append(gender)
                elif header == 'region':
                    row.append(region)
                elif header == 'brand':
                    row.append(brand)
                elif header == 'size':
                    row.append(data[region][i])
                elif header in [m.lower() for m in MEASUREMENT_IDS]:
                    if header in data.keys():
                        row.append(data[header][i])
                    else:
                        row.append('')
                elif header == 'sizeid':
                    row.append(data[header][i])
                else:
                    print header
            out_fh.write('\t'.join(row) + '\n')


def read_csv_input(fn, is_aux):
    '''Read the data into a dictionary'''
    # print fn
    data = {}
    is_metric = True
    with open(fn, 'r') as fh:
        line_num = 0
        for line in fh:
            line_num += 1
            # Skip if is commented
            if line.startswith('#'):
                # Check if there is a directive e.g. #Unit:Inches
                if line_num == 1:
                    directives = [el.lower().strip() for el in line.split(':')]
                    if (len(directives) > 1) and (directives[1] in ['inches', 'inch']):
                        is_metric = False
                    else:
                        print "WARNING: Maybe a missed directive?: " + line
                continue
            els = line.split('\t')
            if len(els) == 1:
                continue                        
            header = els[0].lower().strip()
            # Common header replacements
            header = correct_common_header_typos(header)
            # Common element replacements
            els = [el.strip() for el in els[1:]]
            els = [el.replace("cm", "").strip() for el in els]
            if header in [x.lower() for x in MEASUREMENT_IDS]:
                # Some sizes use these weird unicddes for space reasons e.g. M&S Male Hats
                els = [el.replace("½", ".5") for el in els]
                if '"' in el:
                    is_metric = False
            els = [el.replace('"', '').strip() for el in els]
            # Process the measurements
            if header in [x.lower() for x in MEASUREMENT_IDS]:
                # Deal with ranges
                buff = []
                for el in els:
                    try:
                        rng = [float(r.strip()) for r in el.split('-')]
                    except:
                        buff.append(el)
                        continue
                    if len(rng) == 1:
                        buff.append(el)
                        continue
                    buff.append("%.2f" % ((rng[0] + rng[1]) / 2.0))
                els = buff
                # Convert to metres
                buff = []
                for el in els:
                    if el.strip() == '':
                        buff.append(el.strip())
                    elif is_metric:
                        if header != 'weight':
                            buff.append("%.4f" % (float(el) * 0.01))
                        else:
                            buff.append(el)
                    else:
                        if header != 'weight':
                            buff.append("%.4f" % (float(el) * 2.54 * 0.01))
                        else:
                            buff.append("%.4f" % (float(el) * 0.45))
                els = buff
            # Finally raise a warning if a weird header is found 
            if header in OK_HEADERS:
                data[header] = els
            elif get_two_letter_iso_code(header) is not None:
                header = get_two_letter_iso_code(header)
                data[header] = els
            else:
                raise Exception("WARNING: Unrecognised header: '" + header + "' in file: " + fn)
    # Raise a warning if there is a mismatch in the size of the
    # measurements
    num_els = None
    for key in data.keys():
        if num_els == None:
            num_els = len(data[key])
        elif num_els != len(data[key]):
            raise Exception("WARNING: Uneven data table")
    # Combine the regional size and the size letter into a single size of format 
    # '#/XX', 'X' or '#'. e.g. '10/M' or 'M' n.b. will add the auxialliary 
    # measurements later e.g (short)
    if is_aux:
        data['aux_size'] = data['sizeletter']
    else:
        regions = [r for r in data.keys() if (get_two_letter_iso_code(r) is not None)]
        if not regions:
            raise Exception('WARNING: No region data for file: ' + fn)
        region_data = {}
        for region in regions:
            region_data[region] = []
        for i in range(len(data['sizeid'])):
            if 'sizeletter' in data.keys():
                sizeletter = data['sizeletter'][i]
            else:
                sizeletter = ''
            for region in regions:
                regionalsize = data[region][i]
                if regionalsize and sizeletter:
                    data[region][i] = '%s/%s' % (regionalsize, sizeletter)
                elif regionalsize:
                    data[region][i] = regionalsize
                elif sizeletter:
                    data[region][i] = sizeletter
                else:
                    data[region][i] = ''
        if 'sizeletter' in data.keys():
            del data['sizeletter']
    # All done
    return data

def get_two_letter_iso_code(s):
    s = s.lower()
    try:
        tmp = countries.get(s)
        return tmp.alpha2
    except:
        pass
    if s == 'global':
        return "AA"
    if s in ['europe', 'eu']:
        return "XA"
    if s == 'northamerica':
        return 'XB'
    if s == 'southamerica':
        return 'XC'
    if s == 'asia':
        return 'XD'
    if s == 'australasia':
        return 'XE'
    if s == 'africa':
        return 'XF'
    if s.upper() in ['AA', 'XA', 'XB', 'XC', 'XD', 'XE', 'XF']:
        return s
    return None
    
    
def correct_common_header_typos(s):
    if s == 'sizeletters':
        return 'sizeletter'
    if s == 'hip':
        return 'hips'
    if s == 'bust':
        return 'chest'
    if s == 'uk':
        return 'gb'
    if s == 'korea':
        return 'kr'
    if s == 'hongkong':
        return 'hk'
    if s == 'isreal':
        return 'israel'
    if s == 'newzealand':
        return 'nz'
    if s == 'southafrica':
        return 'za'
    if s == 'russia':
        return 'ru'
    return s
            
    

if __name__ == '__main__':
    main()
