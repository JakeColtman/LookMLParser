import re
from ..Helper.Field import extract_label, is_hidden
import logging

patterns = {
    "yesno" : r"^(is|has|was) ",
    "count_distinct" : r"^unique.* count$",
    "count" : r".*count$",
    "avg" : r"^(avg |average ).*",
    "avg_distinct" : r"^(avg |average ).*",
    "sum" : r"^total.*",
    "sum_distinct" : r"^total.*",
    "percent": r"\(\%\)$"
}

def is_percentage(field):
    if field["type"] in ["number", "avg", "avg_distinct"] and "value_format" in field and "%" in field["value_format"]:
        return

def all_fields_obey_naming_convention(view):

    if "fields" not in view or view["fields"] is None:
        return

    fields = view["fields"]
    problems = []

    exposed_fields = [x for x in fields if not is_hidden(x)]

    for field in exposed_fields:
        if "type" in field and field["type"] in patterns:

            if is_percentage(field):
                pattern = patterns["percent"]
            else:
                pattern = patterns[field["type"]]

            name = extract_label(field)
            if "id" in name:
                continue

            results = re.findall(pattern, name)
            if len(results) == 0:
                problems.append(name)

    logs = logging.getLogger("BuildLog")

    if problems != []:
        logs.warning("Found naming convention errors")
        logs.warning(problems)
        return False
    logs.info("No naming convention errors found")
    return True