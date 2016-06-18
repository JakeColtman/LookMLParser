from ..Helper.Field import fields_exposed_to_sets
import logging


def remove_set_elements_that_have_no_views(view):
    if "sets" not in view:
        return []

    sets = view["sets"]
    exposed_field_names = fields_exposed_to_sets(view)

    logs = logging.getLogger("BuildLog")

    for set_name in sets:
        for set_entry in sets[set_name]:
            if set_entry not in exposed_field_names and "*" not in set_entry and "." not in set_entry:
                sets[set_name].remove(set_entry)
                logs.info("Removing {0} from {1}".format(set_entry, set_name))
    return view
