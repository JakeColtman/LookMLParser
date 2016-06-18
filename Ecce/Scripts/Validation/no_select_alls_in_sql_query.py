def all_fields_exist_in_sql(view):
    try:
        sql = view["derived_table"]["sql"]
    except KeyError:
        return True

    return "*" not in sql