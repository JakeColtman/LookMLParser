from Ecce.json_to_lookml import print_from_json
import yaml
from configurations import configs

def load_from_yaml(file_name):
    with open(file_name, "r") as file_open:
        y = yaml.load(file_open)[0]
        return y

view = load_from_yaml("E:/looker/seller_debtors.view.lookml")

config = configs["only_errors"]
validating_functions, post_processing_functions = config["validation"], config["processing"]

for validating_function in validating_functions:
    if not validating_function(view):
        raise Exception("Validation error")

for processing in post_processing_functions:
    view = processing(view)

print(print_from_json(view))