def try_extract(item, entry):
    if entry in item:
        return item[entry]
    else:
        return []