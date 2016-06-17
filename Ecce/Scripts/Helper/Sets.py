def extract_set_entries(sets):
    totalEntries = []
    for sety in sets:
        for entry in sets[sety]:
            if "*" in entry or "." in entry:
                continue
            totalEntries.append(entry)
