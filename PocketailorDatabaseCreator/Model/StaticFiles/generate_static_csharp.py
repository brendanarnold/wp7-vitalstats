IN_FN = r'Regions.csv'

REGIONID_FN = r'RegionIds.cs'
REGIONLOOKUP_FN = r'RegionIdLookup.cs'
REGIONTREE_FN = r'RegionTree.cs'

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
        # Skip lines which are not to be included
        if els[1] != 'y':
            continue
        # Otherwise load the data
        data.append(els)

with open(REGIONLOOKUP_FN, 'w') as fh:
    for row in data:
        fh.write('{ RegionId.%s, "%s" },\n' % (row[3], row[4]))

with open(REGIONID_FN, 'w') as fh:
    for row in data:
        fh.write('%s = %s,\n' % (row[3], row[0]))

with open(REGIONTREE_FN, 'w') as fh:
    for row in data:
        fh.write('{ RegionId.%s, RegionId.%s },\n' % (row[3], row[2]))
