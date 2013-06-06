'''A script to include a SizeId line in the data files since didn't do this to begin with'''

import os
import shutil

START_DIR = 'Raw\\Gul'

for path, dirs, fns in os.walk(START_DIR):
    if os.path.basename(path) in ['.', 'test', 'Backup']:
        continue
    for fn in fns:
        if fn.startswith('Countries.txt'):
            continue
        if not fn.endswith('.txt'):
            continue
        if fn.split('.')[0].split('_')[-1] in ['Height', 'InsideLeg', 'InternationalConversions']:  
            continue
        orig_fn = os.path.join(path, fn)
        tmp_fn = os.path.join(path, fn + '.tmp')

        is_seeded = False
        with open(orig_fn, 'r') as r_fh:
            for line in r_fh:
                if line.startswith("SizeId"):
                    is_seeded = True
        if is_seeded:
            continue
        with open(orig_fn, 'r') as r_fh:
            with open(tmp_fn, 'w') as w_fh:
                is_inserted = False
                for line in r_fh:
                    w_fh.write(line)
                    if not line.startswith('#') and not is_inserted:
                        num_els = len(line.split('\t')) - 1
                        size_id_line = 'SizeId\t' + '\t'.join([str( (x - int(num_els / 2)) * 20) for x in range(num_els)]) + '\n'
                        w_fh.write(size_id_line)
                        is_inserted = True
        shutil.move(tmp_fn, orig_fn)


