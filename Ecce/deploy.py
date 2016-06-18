from Ecce.json_to_lookml import print_from_json
from Ecce.Validation.all_fields_exist_in_sql import all_fields_exist_in_sql
from Ecce.Validation.meets_naming_convention import all_fields_obey_naming_convention
from Ecce.Validation.no_select_alls_in_sql_query import no_select_all
import yaml

def load_from_yaml(file_name):
    with open(file_name, "r") as file_open:
        y = yaml.load(file_open)[0]
        return y

view = load_from_yaml("E:/looker/seller_debtors.view.lookml")

validating_functions = [
    all_fields_exist_in_sql,
    all_fields_obey_naming_convention
    #no_select_all
]

post_processing_functions = [

]

for validating_function in validating_functions:
    if not validating_function(view):
        raise Exception("Validation error")

for processing in post_processing_functions:
    view = processing(view)

print(print_from_json(view))