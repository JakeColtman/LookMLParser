def extract_set_entries(sets):
    totalEntries = []
    for sety in sets:
        for entry in sets[sety]:
            if "*" in entry or "." in entry:
                continue
            totalEntries.append(entry)
    return list(set(totalEntries))

def trim_set_name(name):
    ranges = ["date", "time", "month", "week", "year"]
    for modifier in ranges:
        if modifier in name:
            return name.replace("_" + modifier, "")