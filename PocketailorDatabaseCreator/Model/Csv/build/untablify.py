from pylab import *

IN_FILE = 'tmp.txt'
OUT_FN = 'tmp_out.txt'

data = loadtxt(IN_FILE, delimiter='\t', dtype=str)

inner_data = data[1:,1:]
col_headers = data[0,1:]
row_headers = data[1:,0]

# aux = ['53-57', '53-57', '68-72', '73-77', '78-82', '83-87', '88-92', '93-97' ]

out = []
for i in xrange(len(col_headers)):
    for j in xrange(len(row_headers)):
        c = col_headers[i]
        r = row_headers[j]
        inner = inner_data[j,i]
        if not inner:
            continue
        out.append([inner, r, c])
        # out.append([r+c, aux[j], inner])
out = array(out)
out = out.T
with open(OUT_FN, 'w') as fh:
    for els in out:
        fh.write('\t'.join(els) + '\n')
