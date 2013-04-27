from pylab import *

IN_FILE = 'tmp.txt'
OUT_FN = 'tmp_out.txt'

data = loadtxt(IN_FILE, delimiter='\t', dtype=str)

inner_data = data[1:,1:]
col_headers = data[0,1:]
row_headers = data[1:,0]

# aux = ['32', '34', '36', '38']

out = []
for j in xrange(len(col_headers)):
    for i in xrange(len(row_headers)):
        c = col_headers[j]
        r = row_headers[i]
        inner = inner_data[i,j]
        if not inner:
            continue
        out.append([inner, r, c])
        # out.append([r+c, aux[j], inner])
out = array(out)
out = out.T
with open(OUT_FN, 'w') as fh:
    for els in out:
        fh.write('\t'.join(els) + '\n')
