namespace LookMLParser

module Integration = 
    open LookMLParser.FieldModel;
    open LookMLParser.SetModel;
    open LookMLParser.View;
    open LookMLParser.DataSource;

    let (|Found|_|) key map =
      map
      |> Map.tryFind key
      |> Option.map (fun x -> x, Map.remove key map)

    let convert_into_sql_table name_string = 
        SqlTable {name = name_string}

    let convert_into_sets input = 
        let sets = 
            input
                |> List.map (fun (x, y) -> {Set.name = x; fields = y})
        {Sets.sets = sets}

    let convert_into_derived_table (input: Map<string,string>) = 
        
        let persistance = 
            match input.TryFind "persist_for" with 
            | Some x -> PersistFor (1, Hours)
            | None -> match input.TryFind "sql_trigger_value" with 
                      | Some x -> SQLTriggerValue x
                      | None -> SQLTriggerValue "None"

        let sortmethod = 
            match input.TryFind "sortkeys" with 
            | Some x -> SortKeys {keys = x} 
            | None -> match input.TryFind "sql_trigger_value" with 
                      | Some x -> Indexes {keys = x} 
                      | None -> Indexes {keys = "None"} 

        DerivedTable {
            sql = None;
            persistance = Some persistance;
            distribution_key = None;
            distribution_style = None;
            sort_method = Some sortmethod
        }

    let convert_into_field input_map = 

        let dimension_type = 
            match input_map with 
            | Found "dimension" input_map -> DimensionType
            | Found "measure" input_map  -> MeasureType
            | _ -> DimensionType

        let data_type = 
            match dimension_type with
            | DimensionType -> 
                match input_map with 
                | Found "number" input_map -> DimensionDataType DimensionDataType.Number
                | Found "location" input_map -> DimensionDataType DimensionDataType.Location
                | Found "string" input_map -> DimensionDataType DimensionDataType.String
                | Found "tier" input_map -> DimensionDataType DimensionDataType.Tier
                | Found "time" input_map -> DimensionDataType DimensionDataType.Time
                | Found "yesno" input_map -> DimensionDataType DimensionDataType.YesNo
                | Found "zipcode" input_map -> DimensionDataType DimensionDataType.ZipCode
                | _ -> DimensionDataType DimensionDataType.String

            | MeasureType -> 
                match input_map with 
                | Found "string"  input_map -> MeasureDataType MeasureDataType.String 
                | Found "date"  input_map -> MeasureDataType MeasureDataType.Date
                | Found "number"  input_map -> MeasureDataType MeasureDataType.Number
                | Found "count"  input_map -> MeasureDataType MeasureDataType.Count
                | Found "count_distinct"  input_map -> MeasureDataType MeasureDataType.CountDistinct
                | Found "sum"  input_map -> MeasureDataType MeasureDataType.Sum
                | Found "sum_distinct"  input_map -> MeasureDataType MeasureDataType.SumDistinct
                | Found "avg"  input_map -> MeasureDataType MeasureDataType.Avg
                | Found "avg_distinct"  input_map -> MeasureDataType MeasureDataType.AvgDistinct
                | Found "min"  input_map -> MeasureDataType MeasureDataType.Min
                | Found "max"  input_map -> MeasureDataType MeasureDataType.Max
                | Found "list"  input_map -> MeasureDataType MeasureDataType.List
                | Found "percent_of_previous"  input_map -> MeasureDataType MeasureDataType.PercentOfPrevious
                | Found "percent_of_total"  input_map -> MeasureDataType MeasureDataType.PercentOfTotal
                | Found "running_total"  input_map -> MeasureDataType MeasureDataType.RunningTotal
                | _ -> MeasureDataType MeasureDataType.Sum

            | _ -> DimensionDataType DimensionDataType.String

        let sql_text = 
            match input_map.TryFind "name" with 
                | Some text -> text
                | None -> "no sql given!!"

        let default_false lookup = 
            match input_map.TryFind lookup with 
                | Some "true" -> true
                | Some "false" -> false
                | Some _ -> true
                | None -> false

        let hidden = default_false "hidden"

        let pk = default_false "hidden"

        let field_details = {
            label = input_map.TryFind "label";
            sql = sql_text;
            view_label = Some "Test";
            description = Some "Test";
            hidden = hidden;
            alias = input_map.TryFind "alias";
            required_fields = input_map.TryFind "required_fields";
            drill_fields = input_map.TryFind "drill_fields"
        }

        match (dimension_type, data_type) with 
            | (DimensionType, DimensionDataType parsed_data_type) -> 
                let details = {
                    data_type = parsed_data_type;
                    primary_key = pk;
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
