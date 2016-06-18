import re
from ..Helper.SQL import SQL
import logging

def columns_needed_in_pdt_sql(fields):
    columns_needed = []
    for field in fields:
        try:
            field_sql = field["sql"]
            columns = re.findall(r"\${TABLE}\.([a-zA-Z_0-9]*)", field_sql)
            columns_needed += columns
        except KeyError:
            pass
    return columns_needed


def columns_in_sql(sql_text):
    sql_parsed = SQL(sql_text)
    if sql_parsed.columns is None or len(sql_parsed.columns) == 0:
        return True
    print(sql_parsed.columns)
    return [x.lower() for x in sql_parsed.columns if x is not None]


def all_fields_exist_in_sql(view):
    try:
        sql = view["derived_table"]["sql"]
    except KeyError:
        return True

    if "fields" not in view or view["fields"] is None:
        return True

    fields = view["fields"]

    needed_columns = columns_needed_in_pdt_sql(fields)
    needed_columns = [x.lower() for x in needed_columns]
    actual_columns = columns_in_sql(sql)

    problems = [x for x in needed_columns if x not in actual_columns]

    logs = logging.getLogger("BuildLog")
    if len(problems) != 0:
        logs.warning("Found fields that don't exist in SQL")
        logs.warning(problems)
        return False
    logs.info("All fields found in SQL")
    return len(problems) == 0
