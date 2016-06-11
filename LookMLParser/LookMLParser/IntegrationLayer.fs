namespace LookMLParser

module IntegrationLayer = 
    open LookMLParser.FieldModel;
    open LookMLParser.SetModel;
    open LookMLParser.View;
    open LookMLParser.DataSource;
    open FSharp.Data.JsonExtensions;
    open FSharp.Data;
   
    let try_get_property (json_object:JsonValue) (property:string) = 
        match (json_object.TryGetProperty(property)) with 
            | Some a -> 
                try
                    Some (a.AsString())
                with 
                    | _ -> None
            | _ -> 
                None

    let parse_dimension json_dimension = 
        
        let ttype = 
            match try_get_property json_dimension "type" with 
                | Some a -> 
                    match a with
                        | "string" -> DimensionDataType.String
                        | "number" -> DimensionDataType.Number
                        | "int" -> DimensionDataType.Number
                        | "yesno" -> YesNo
                        | "time" -> Time
                        | "tier" -> Tier
                        | "location" -> Location
                        | "zipcode" -> ZipCode
                        | _ -> 
                            printfn "%A" a
                            DimensionDataType.String
                | None -> DimensionDataType.String
                
        let hidden = 
            match try_get_property json_dimension "hidden" with 
                | Some a -> true
                | None -> false

        let primary_key = 
            match try_get_property json_dimension "primary_key" with 
                | Some a -> true
                | None -> false

        let suggestable = 
            match try_get_property json_dimension "suggestable" with 
                | Some a -> true
                | None -> false

        {
            DimensionDetails.data_type = ttype;
            primary_key= primary_key;
            alpha_sort= false;
            tiers =None;
            style = None;
            suggestable= suggestable
        }



    let parse_measure json_measure = 
        
        let ttype = 
            match try_get_property json_measure "type" with 
                | Some a -> 
                    match a with
                        | "string" -> MeasureDataType.String 
                        | "date"  -> MeasureDataType.Date
                        | "number" -> MeasureDataType.Number
                        | "count" -> MeasureDataType.Count
                        | "count_distinct"  -> MeasureDataType.CountDistinct
                        | "sum"  -> MeasureDataType.Sum
                        | "sum_distinct"  -> MeasureDataType.SumDistinct
                        | "avg"  -> MeasureDataType.Avg
                        | "average"  -> MeasureDataType.Avg
                        | "avg_distinct" -> MeasureDataType.AvgDistinct
                        | "average_distinct"  -> MeasureDataType.AvgDistinct
                        | "min"  -> MeasureDataType.Min
                        | "max" -> MeasureDataType.Max
                        | "list" -> MeasureDataType.List
                        | "percent_of_previous" -> MeasureDataType.PercentOfPrevious
                        | "percent_of_total" -> MeasureDataType.PercentOfTotal
                        | "running_total" -> MeasureDataType.RunningTotal
                        | _ -> 
                            printfn "%A" a
                            MeasureDataType.Sum
                | None -> MeasureDataType.Sum
                 
        {
            MeasureDetails.data_type = ttype;
            direction= None;
            approximate= Some false;
            approximate_threshold =  None;
            sql_distinct_key =  try_get_property json_measure "sql_distinct_key";
            list_field=  try_get_property json_measure "list_field";
            filters =  try_get_property json_measure "filters"
        }
         

    let parse_dimension_group json_measure = 
        
        let ttype = 
            match try_get_property json_measure "type" with 
                | Some a ->
                    match a with
                        | "epoch" -> DimensionGroupDataType.Epoch 
                        | "timestamp"  -> DimensionGroupDataType.TimeStamp
                        | "datetime" -> DimensionGroupDataType.DateTime
                        | "date" -> DimensionGroupDataType.Date
                        | "yyyymmdd"  -> DimensionGroupDataType.YYYYMMDD
                        | "time"  -> DimensionGroupDataType.DateTime
                        | _ -> 
                            printfn "%A" a
                            DimensionGroupDataType.DateTime
                | None -> DimensionGroupDataType.DateTime
                
        {
            DimensionGroupDetails.data_type = ttype;
            convert_tz = false;
            primary_key = false;
            timeframes = None
        }   
        

    let parse_field (field:JsonValue) =

        let field_details = {
            FieldDetails.sql = try_get_property field "sql";
            label =  try_get_property field "label";
            view_label =  try_get_property field "view_label";
            description =  try_get_property field "description";
            alias =  try_get_property field "alias";
            required_fields =  try_get_property field "required_fields";
            drill_fields =  try_get_property field "drill_fields";
            hidden =  try_get_property field "hidden";
        }

        let details =  
            match try_get_property field "dimension" with 
                | Some a -> 
                    Dimension ((parse_dimension field) , field_details)
                | _ -> 
                     match try_get_property field "measure" with 
                        | Some a ->  
                             Measure ((parse_measure field) , field_details)
                        | _ ->  
                             DimensionGroup ((parse_dimension_group field), field_details)

        details



    let parse_fields json_contents = 
        match json_contents with 
            | JsonValue.Array fields -> 
                Some( Array.map parse_field fields)
            | _ -> None




    let parse_derived_table json_contents = 

        let persistance = match try_get_property json_contents "persist_for" with 
            | Some a -> Some(PersistFor a)
            | None -> 
                match try_get_property json_contents "sql_trigger_value" with 
                    | Some a -> Some(SQLTriggerValue a)
                    | None -> None
            
        let distributionStyle = match try_get_property json_contents "distribution_style" with
            | Some "ALL" -> All
            | Some "all" -> All
            | Some "Even" -> Even
            | Some "EVEN" -> Even
            | _ -> All
            
        let sql = json_contents?sql.AsString()
        let sql_trigger_value = json_contents?sql_trigger_value.AsString()
        {
            sql = Some(sql); 
            persistance = persistance; 
            distribution_key = try_get_property json_contents "distribution_key"; 
            distribution_style = distributionStyle; 
            sort_method = None
        }

    let process_set json_set = 
        let name = fst json_set
        let entries = snd json_set
        let mutable columns = []
        match entries with 
            | JsonValue.Array a ->
                for entry in a do
                    columns <- (entry.AsString())::columns
            | _ -> printfn "%A" "nothing"
        {Set.name = name; fields = columns}

    let parse_sets json_contents = 
        match json_contents with 
            | JsonValue.Record x -> 
                Some (Array.map process_set x)
            | _ -> None