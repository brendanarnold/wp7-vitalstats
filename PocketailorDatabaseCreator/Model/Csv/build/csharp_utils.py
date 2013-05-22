
def get_csharp_enums(fn, enum_name):
    '''Code to read enums from c# source'''
    start_record = False
    ids = []
    with open(fn, 'r') as fh:
        for line in fh:
            if ('enum ' + enum_name) in line:
                start_record = True
                continue
            if start_record:
                line = line.strip()
                if (line in ['{', '']) or line.startswith("//"):
                    continue
                if line == '}':
                    break
                else:
                    ids.append(line.split('=')[0].replace(',','').strip())
    return ids
