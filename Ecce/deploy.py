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

for strict_validation in validating_functions["error"]:
    if not strict_validation(view):
        raise Exception("Validation error")

for warning_validation in validating_functions["error"]:
    if not warning_validation(view):
        print("WARNING!!")

for processing in post_processing_functions:
    view = processing(view)

print(print_from_json(view))