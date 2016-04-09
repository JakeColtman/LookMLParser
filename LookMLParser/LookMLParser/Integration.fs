namespace LookMLParser

module Integration = 
    open LookMLParser.FieldModel;
    open LookMLParser.View;

    let convert_into_sql_table name_string = 
        SqlTable {name = name_string}

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
            label = Some name_string;
            sql = sql_string;
            view_label = Some "Test";
            description = Some "Test";
            hidden = false;
            alias = None;
            required_fields = None;
            drill_fields = None
        }

        match (dimension_type, data_type) with 
            | (DimensionType, DimensionDataType parsed_data_type) -> 
                let details = {
                    data_type = parsed_data_type;
                    primary_key = true;
                    alpha_sort = true;
                    tiers = None;
                    style = None ;
                    suggestable = true
                }
                let output = Dimension (DimensionType , details , field_details)
                Some output
                 
            | (MeasureType, MeasureDataType parsed_data_type) ->
                let details = {
                    data_type = parsed_data_type;
                    direction = Some Row;
                    approximate = Some false;
                    approximate_threshold = Some 0;
                    sql_distinct_key = None;
                    list_field = None;
                    filters = None
                }
                Some (Measure (MeasureType , details , field_details))

            | ( _ , _ ) -> None
