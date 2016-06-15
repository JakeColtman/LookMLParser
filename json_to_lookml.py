import os
import json
from copy import copy
import sqlparse

os.chdir("E:\looker")
with open("testJson.view.lookml", "r") as json_file:
    view = json.load(json_file)

def field_type(field):
    if "dimension" in field:
        return "dimension"
    if "measure" in field:
        return "measure"
    else:
        return "dimension_group"

def cleanup_field(field):
    field.pop("measure", None)
    field.pop("dimension", None)
    field.pop("dimension_group", None)
    return field

def print_fields(fields_json):

    def print_sql_case(case_json):
        output = ""
        base = "\t\t  \t{0}: {1}\n"
        for entry in case_json:
            output += base.format(entry, case_json[entry])

        return output

    def print_field(field_json):
        base_field = """
\t\t- {0}: {1}
"""
        field = copy(field_json)
        ttype = field_type(field)
        name = field_json[ttype]
        field = cleanup_field(field)
        base = base_field.format(ttype, name)
        for key in field:
            if key == "value_format":
                field[key] = '"' + field[key].replace('"', '\\"') + '"'
            if key in ["sql_case", "filters"]:
                base += "\t\t  {0}: \n".format(key)
                base += print_sql_case(field[key])
            else:
                if isinstance(field[key],str) and len(field[key]) > 30:
                    content = field[key].replace("\n", "\n\t\t  \t")
                    base += "\t\t  {0}: | \n".format(key)

                    base += "\t\t  \t{0}\n".format(content)
                else:
                    base += "\t\t  {0}: {1}\n".format(key, field[key])

        return base

    base = ""
    for field in fields_json:
        base += print_field(field)
    return base

def print_derived_table(table_json):
    table = copy(table_json)
    base = "\t\t{0}: {1}\n"
    output = ""
    for entry in table:
        if entry == "sql":
            continue
        output += base.format(entry, table[entry])

    if "sql" in table:
        sql_raw = table["sql"]
        sql = sqlparse.format(sql_raw, reindent=True, keyword_case='upper')
        sql = sql.replace("\n", "\n\t\t\t")

        output += "\t\tsql: |\n\t\t\t{0}".format(sql)

    return output

def print_sets(sets_json):

    def print_set(set_name, set_fields):
        base = "\t\t{0}:\n".format(set_name)
        for field in set_fields:
            base += "\t\t  - {0}\n".format(field)
        return base
    full_base = ""
    for name in sets_json:
        full_base += print_set(name, sets_json[name])

    return full_base

def print_from_json(json):
    base_view = """- view: {0}

  derived_table:
{1}
  fields:
    {2}
  sets:
{3}"""
    view = json["view"]
    derived_table = print_derived_table(json["derived_table"])
    fields = print_fields(json["fields"])
    sets = print_sets(json["sets"])
    return base_view.format(view, derived_table, fields, sets)

with open("E:/looker/aatest.yaml", "w") as output_file:
    output_file.write(print_from_json(view))