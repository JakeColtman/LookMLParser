def no_select_all(view):
    try:
        sql = view["derived_table"]["sql"]
    except KeyError:
        return True

    return "*" not in sql