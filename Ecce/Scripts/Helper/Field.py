def extract_type(field):
    if "dimension" in field:
        return "dimension"
    elif "measure" in field:
        return "measure"
    elif "dimension_group" in field:
        return "dimension_group"
    raise KeyError("One of dimension, measure or dimension group needs to be in the field")

def fields_exposed_to_sets(view):
    if "field" not in view or view["field"] is None:
        return []

    output = []

    for field in view["fields"]:
        field_type = extract_type(field)
        if field_type != "dimension_group":
            output.append(field[field_type])
        else:
            for modifier in field["timeframes"]:
                output.append(field[field_type] + "_" + modifier)

    return output