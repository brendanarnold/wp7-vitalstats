
IN_FN1 = r'FF\\Female_Bra.txt'
IN_FN2 = r'FF\\Female_Bra_InternationalConversions.txt'



lookup = {}
with open(IN_FN2, 'r') as fh:
    for line in fh:
        els = line.split('\t')
        lookup[els[0]] = [x.strip() for x in els[1:]]

with open(IN_FN1, 'r') as in_fh:
    with open(IN_FN1 + '.tmp', 'w') as out_fh:
        for line in in_fh:
            out_fh.write(line)
            if line.startswith('SizeLetter'):
                uk_sizes = line.split('\t')[1:]                
                for key in lookup.keys():
                    if key == 'UK':
                        continue
                    out_line = [key]
                    for uk_sz in uk_sizes:
                        if uk_sz in lookup['UK']:
                            out_line.append(lookup[key][lookup['UK'].index(uk_sz)])
                        else:
                            out_line.append('')
                    out_fh.write('\t'.join(out_line) + '\n')

