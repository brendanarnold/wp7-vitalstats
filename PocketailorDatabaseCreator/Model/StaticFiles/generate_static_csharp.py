IN_FN = r'Regions.csv'

OUT_FN = r'RegionTree.cs'
LOOKUP_OUT_FN = r'RegionLookup.cs'

with open(IN_FN, 'r') as fh:
    line_num = 0
    data = []
    for line in fh:
        line_num += 1
        # Skip header
        if line_num == 1:
            continue
        # Skip empty lines
        if line.strip() == '':
            continue
        # Skip commented lines
        if line.startswith('#'):
            continue
        els = [el.strip() for el in line.split('\t')]
        # Otherwise load the data
        data.append(els)

with open(OUT_FN, 'w') as fh:
    for row in data:
        fh.write('{ "%s", "%s" },\n' % (row[1], row[0]))

with open(LOOKUP_OUT_FN, 'w') as fh:
    for row in data:
        fh.write('{ "%s", "%s" },\n' % (row[1], row[2]))
