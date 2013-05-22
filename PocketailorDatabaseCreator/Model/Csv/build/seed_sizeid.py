'''A script to include a SizeId line in the data files since didn't do this to begin with'''

import os
import shutil

START_DIR = '.'

for path, dirs, fns in os.walk(START_DIR):
    if os.path.basename(path) in ['.', 'test', 'Backup']:
        continue
    for fn in fns:
        if fn.split('.')[0].split('_')[-1] in ['Height', 'InsideLeg', 'InternationalConversions']:  
            continue
        # if fn.endswith('.tmp'):
        #     os.remove(os.path.join(path, fn))
        if fn.endswith('.swp') or fn.endswith('.tmp') or not fn.endswith('.txt'):
            continue
        orig_fn = os.path.join(path, fn)
        tmp_fn = os.path.join(path, fn + '.tmp')
        fix = False
        with open(orig_fn, 'r') as r_fh:
            for line in r_fh:
                if line.strip() == 'SizeId':            
                    fix = True
                    break
        if fix:
            with open(orig_fn, 'r') as r_fh:
                with open(tmp_fn, 'w') as w_fh:
                    line_num = 0
                    fixed = False
                    for line in r_fh:
                        line_num += 1
                        if line.strip() == 'SizeId':
                            continue
                        w_fh.write(line)
                        if not line.startswith('#') and not fixed:
                            num_els = len(line.split('\t')) - 1
                            size_id_line = 'SizeId\t' + '\t'.join([str( (x - int(num_els / 2)) * 20) for x in range(num_els)]) + '\n'
                            w_fh.write(size_id_line)
                            fixed = True
            shutil.move(tmp_fn, orig_fn)


