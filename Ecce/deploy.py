from Ecce.json_to_lookml import print_from_json
import yaml
from configurations import configs
import logging

def load_from_yaml(file_name):
    with open(file_name, "r") as file_open:
        y = yaml.load(file_open)[0]
        return y

CONFIGNAME = "only_errors"
logging.basicConfig(level=logging.DEBUG,
                    format='%(asctime)s %(name)-12s %(levelname)-8s %(message)s',
                    datefmt='%m-%d %H:%M',
                    filename="build_log.txt",
                    filemode='w')

logs = logging.getLogger("BuildLog")

view = load_from_yaml("E:/looker/seller_debtors.view.lookml")


logs.info("Starting Checks")
config = configs["only_errors"]
logs.info("Using {0} config setup".format(CONFIGNAME))

validating_functions, post_processing_functions = config["validation"], config["processing"]

for strict_validation in validating_functions["error"]:
    if not strict_validation(view):
        logs.error("Failed build")
        raise Exception("Validation error")

for warning_validation in validating_functions["error"]:
    if not warning_validation(view):
        logs.warning("Build warning")

logs.info("Validation Finished")

for processing in post_processing_functions:
    view = processing(view)

logs.info("Processing Finished")

print(print_from_json(view))