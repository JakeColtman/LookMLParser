namespace LookMLParser

module Integration = 
    open LookMLParser.FieldModel;

    let convert_into_field (((dimension_type_string , name_string), type_string), sql_string) = 

        let dimension_type = 
            match dimension_type_string with 
            | "dimension" -> DimensionType
            | "measure" -> MeasureType
            | _ -> DimensionType

        let data_type = 
            match dimension_type with
            | DimensionType -> 
                match type_string with 
                | "number" -> DimensionDataType DimensionDataType.Number
                | "location" -> DimensionDataType DimensionDataType.Location
                | "string" -> DimensionDataType DimensionDataType.String
                | "tier" -> DimensionDataType DimensionDataType.Tier
                | "time" -> DimensionDataType DimensionDataType.Time
                | "yesno" -> DimensionDataType DimensionDataType.YesNo
                | "zipcode" -> DimensionDataType DimensionDataType.ZipCode
                | _ -> DimensionDataType DimensionDataType.String

            | MeasureType -> 
                match type_string with 
                | "sum" -> MeasureDataType MeasureDataType.Number
                | _ -> MeasureDataType MeasureDataType.Sum

            | _ -> DimensionDataType DimensionDataType.String


        let field_details = {
            label = name_string;
            view_label = "Test";
            description = "Test";
            hidden = true;
            alias = [];
            required_fields = [];
            drill_fields = []
        }

        match (dimension_type, data_type) with 
            | (DimensionType, DimensionDataType parsed_data_type) -> 
                let details = {
                    data_type = parsed_data_type;
                    hidden = true;
                    primary_key = true;
                    sql = sql_string;
                    aplha_sort = true;
                    tiers = [];
                    style = Integer ;
                    suggestable = true
                }
                let output = Dimension (DimensionType , details , field_details)
                Some output
                 
            | (MeasureType, MeasureDataType parsed_data_type) ->
                let details = {
                    data_type = parsed_data_type;
                    direction = Row;
                    approximate = false;
                    approximate_threshold = 0;
                    sql_distinct_key = "";
                    list_field = "";
                    filters = ""
                }
                Some (Measure (MeasureType , details , field_details))

            | ( _ , _ ) -> None
