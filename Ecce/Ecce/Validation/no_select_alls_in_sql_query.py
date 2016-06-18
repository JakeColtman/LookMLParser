import logging

def no_select_all(view):
    try:
        sql = view["derived_table"]["sql"]
    except KeyError:
        return True
    logs = logging.getLogger("BuildLog")
    if "*" in sql:
        logs.warning("Found use of select all in SQL")
        return True
    logs.info("No select all found in SQL")
    return False