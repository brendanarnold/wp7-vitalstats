from pylab import *

OUT_FN = r'tmp_out.txt'

UNDERBUST = ['63-65', '68-70', '73-75', '78-80', '83-85', '88-90']
SIZE =      ['30', '32', '34', '36', '38', '40']
CUP = [10, 13, 15, 18, 20, 23, 25]
UK = ['A', 'B', 'C', 'D', 'DD', 'E', 'F']

out = []
for j,ub in enumerate(UNDERBUST):
    l, u = [int(n) for n in ub.split('-')]
    for i,cup in enumerate(CUP):
        lc = l + cup
        lu = u + cup 
        letter = UK[i]
        sz = SIZE[j]
        out.append([str(lu) + '-' + str(lc), str(l) + '-' + str(u), sz+letter])
out = array(out)
out = out.T
with open(OUT_FN, 'w') as fh:
    for line in out:
        fh.write("\t".join(line) + "\n")
