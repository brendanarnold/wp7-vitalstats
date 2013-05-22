# vim: set fileencoding=utf-8 :
# Above is necessary for the half substitution


from python_constants import *
import os

IN_DIR = r'MarksSpencer'
IN_FN = r'Female_Dress.txt'


def main():
    # Read in the formats indexed by MeasurementId
    format_dict = dict()
    with open(FILE_FORMATS_FN, 'r') as fh:
        for line in fh:
            els = [el.strip().lower() for el in line.split('\t')]
            format_dict[els[0]] = els[1:]
    # Create or wipe temporary build files, insert headerline and create
    # handles to the files
    out_fns = {}
    for conversion_id in CONVERSION_IDS:
        conversion_id = conversion_id.lower()
        out_fns[conversion_id] = TMP_FILE_DIR + conversion_id + TMP_FILE_SUFFIX
        with open(out_fns[conversion_id], 'w') as fh:
            fh.write('\t'.join(format_dict[conversion_id]) + '\n')

    # TODO iterate directories
    for path, dirs, fns in os.walk(IN_DIR):
        pass
    in_dir = IN_DIR
    in_fns = [IN_FN]

    # Sanity check the dir name
    brand = os.path.basename(in_dir).lower()
    if brand not in [b.lower() for b in BRAND_IDS]:
        raise Exception("No brand in BrandId called: " + brand)
    # Create a list of filenames and auxialliary filenames
    fns = []
    aux_fns = []
    for fn in in_fns:
        # Parse and sanity check filename
        gender, conversion, aux = parse_fn(fn)
        if (aux == None) or aux not in ['insideleg', 'height']:
            fns.append(fn)
        else:
            fn.append(aux_fn)
    # Read each file into the output CSV file
    for fn in fns:
        # Read in the data
        data = read_csv_input(os.path.join(in_dir, fn))
        gender, conversion, aux = parse_fn(fn)
        # Read in auxilliary data if any and use to augment the ordinary data
        aux_data = get_aux_file_data(fn, aux_fns)
        if aux_data != None:
            data = combine_aux_data(data, aux_data)
        # Write the data for each region in the file  n.b. files with a
        # region in the filename also have the region in the file in the
        # usual place so no need to extract from the filename
        regions = [r.lower() for r in data.keys() if r.lower() in [i.lower() for i in REGION_IDS]]
        for region in regions:
            write_csv_output(data, format_dict[conversion], out_fns[conversion], gender, region, brand)


def combine_aux_data(data, aux_data):
    # Need to replicate the main data set for each data point in
    # the auxilliary data
    for dk in data.keys():
        if dk == 'generalsize':
            all_sizes = []
            for aux_sz in aux_data['generalsize']:
                all_sizes = all_sizes + [ sz + ' (%s)' % aux_sz for sz in data['generalsize']]
            data['generalsize'] = all_sizes
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
            data[dk] = len(aux_data['generalsize']) * data[dk]
    # Finally add the axilliary measuremnts
    for aux_dk in aux_data.keys():
        if aux_dk not in  ['generalsize', 'sizeid']:
            data[aux_dk] = aux_data[aux_dx]
    return data


def get_aux_file_data(fn, aux_fns):
    '''Obtains the data in the auxialliary file if there is any, otherwise returns None'''
    if fn in aux_fns:
        return None
    stem = fn.split('.')[0]
    aux_data = None
    for aux_fn in aux_fns:
        if stem == aux_fn[0:len(stem)]:
            aux_data = read_csv_input(os.path.join(in_dir, aux_fn))
            processed_aux.append(aux_fn)
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
    if (aux != None) and (aux not in ['', 'InternationalConversions', 'InsideLeg', 'Height']):
        raise Exception("No extra filefeature called: " + str(aux))
    return (gender, conversion, aux)


def write_csv_output(data, fmt, fn, gender, region, brand):
    # Process the normal data according to the format dictionary
    with open(fn, 'a') as out_fh:
        for i,sz in enumerate(data['sizeid']):
            row = []
            for header in fmt:
                if header == 'gender':
                    row.append(gender)
                elif header == 'region':
                    row.append(region)
                elif header == 'brand':
                    row.append(brand)
                elif header == 'generalsize':
                    if 'generalsize' in data.keys():
                        row.append(data['generalsize'][i])
                    else:
                        row.append('')
                elif header == 'regionalsize':
                    if region in data.keys():
                        row.append(data[region][i])
                    else:
                        row.append('')
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


def read_csv_input(fn):
    '''Read the data into a dictionary'''
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
                    if line.split(':')[1].lower().strip() in ['inches', 'inch']:
                        is_metric = False
                    else:
                        print "WARNING: Maybe a missed directive?: " + line
                continue
            line = line.strip()
            if line == '':
                continue
            els = line.split('\t')
            header = els[0].lower().strip()
            # Common header replacements
            if header == 'hip':
                header = 'hips'
            if header == 'eu':
                header = 'europe'
            if header == 'aus':
                header = 'australia'
            header = header.replace('sizeletter', 'generalsize')
            # Common element replacements
            els = [el.strip().lower() for el in els[1:]]
            for el in els:
                if '"' in el:
                    is_metric = False
            els = [el.replace('"', '') for el in els]
            els = [el.replace('"', '') for el in els]
            els = [el.replace("½", ".5") for el in els]
            if (not is_metric) and header in [x.lower() for x in MEASUREMENT_IDS]:
                els = ["%.2f" % (float(el) * 2.54) for el in els]
            if header in OK_HEADERS:
                data[header] = els
            else:
                raise Exception("Unrecognised header: " + header)
    return data


if __name__ == '__main__':
    main()
