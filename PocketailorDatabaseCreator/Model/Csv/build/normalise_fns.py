import shutil
import os
from csharp_utils import *
from python_constants import *

BASE_DIR = '.'

region_ids = get_csharp_enums(REGIONID_FN, 'RegionId')

for (path, dirs, fns) in os.walk(BASE_DIR):
    if path in [BASE_DIR, '.\\test']:
        continue
    for fn in fns:
        if fn.endswith('.swp'):
            continue
        old_fn = fn
        fn = fn.replace('Shirts', 'Shirt')
        fn = fn.replace('Bras', 'Bra')
        fn = fn.replace('DressSizes', 'Dress')
        fn = fn.replace('DressSize', 'Dress')
        fn = fn.replace('Trousers', 'Trouser')
        fn = fn.replace('Suits', 'Suit')
        fn = fn.replace('Hats', 'Hat')
        fn = fn.replace('Hats', 'Hat')
        fn = fn.replace('Shoes', 'Shoe')

        if old_fn != fn:
            print '%s -> %s' % (old_fn, fn)
            shutil.move(os.path.join(path, old_fn), os.path.join(path, fn))

        els = fn.split('.')[0].split('_')
        gender = els[0]
        conversion = els[1]
        if conversion not in ['Shirt', 'Bra', 'Dress', 'Trouser',  
            'Suit', 'Hat', 'Hosiery', 'Shoe']:
            print conversion
        if gender not in ['Male', 'Female']:
            print gender
        if len(els) == 3:
            aux = els[2]
            if aux in region_ids:
                # TODO: Separate for each region
                pass
            elif aux in ['InsideLeg', 'Height', 'InternationalConversions']:
                # TODO compile these special cases
                pass
