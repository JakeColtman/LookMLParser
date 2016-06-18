from Ecce.Validation.all_fields_exist_in_sql import all_fields_exist_in_sql
from Ecce.Validation.meets_naming_convention import all_fields_obey_naming_convention
from Ecce.Validation.no_select_alls_in_sql_query import no_select_all
from Ecce.Processing.remove_bad_set_points import remove_set_elements_that_have_no_views

configs = {

    "strictest": {
        "validation":
            {
                "error":
                    [
                        all_fields_exist_in_sql,
                        all_fields_obey_naming_convention,
                        no_select_all
                    ],
                "warning": []
            },
        "processing":
            [
                remove_set_elements_that_have_no_views
            ]
    },
    "only_errors": {
        "validation":
            {
                "error":
                    [
                        all_fields_exist_in_sql
                    ],
                "warning":
                    [
                        all_fields_obey_naming_convention,
                        no_select_all
                    ],
            },
        "processing":
            [
                remove_set_elements_that_have_no_views
            ]
    }
}
