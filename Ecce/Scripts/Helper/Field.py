def extract_name(field):
    if "dimension" in field:
        return field["dimension"]
    elif "measure" in field:
        return field["measure"]
    elif "dimension_group" in field:
        return field["dimension_group"]
    raise KeyError("One of dimension, measure or dimension group needs to be in the field")