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
                Some(a.AsString())
            | _ -> 
                None


    type DimensionStringy = {
        data_type: string;        
    }

    type DimensionGroupStringy = {
        data_type: string;        
    }

    type MeasureStringy = {
        data_type: string;        
    }

    type FieldStringy = {
        sql: string option
    }

    type Fieldy = 
        | Dimensiony of DimensionStringy * FieldStringy
        | Measurey of MeasureStringy * FieldStringy
        | DimensionGroupy of DimensionGroupStringy * FieldStringy

    type Viewy = {
        name: string;
        data: DerivedTable;
        fields: Fieldy[] option;
        sets: Set[] option
    }

    let parse_dimension json_dimension = 
        
        let ttype = 
            match try_get_property json_dimension "type" with 
                | Some a -> 
                    a
                | None -> "string"     
                
        {DimensionStringy.data_type = ttype}   

    let parse_measure json_measure = 
        
        let ttype = 
            match try_get_property json_measure "type" with 
                | Some a -> 
                    a
                | None -> "string"     
                
        {MeasureStringy.data_type = ttype}   

    let parse_dimension_group json_measure = 
        
        let ttype = 
            match try_get_property json_measure "type" with 
                | Some a -> 
                    a
                | None -> "time"     
                
        {DimensionGroupStringy.data_type = ttype}   
        

    let parse_field (field:JsonValue) =

        let sql = try_get_property field "sql"
        let field_details = {FieldStringy.sql = sql}

        let details =  
            match try_get_property field "dimension" with 
                | Some a -> 
                    Dimensiony ((parse_dimension field) , field_details)
                | _ -> 
                     match try_get_property field "measure" with 
                        | Some a ->  
                             Measurey ((parse_measure field) , field_details)
                        | _ ->  
                            DimensionGroupy ((parse_dimension_group field), field_details)

        details



    let parse_fields json_contents = 
        match json_contents with 
            | JsonValue.Array fields -> 
                Some( Array.map parse_field fields)
            | _ -> None




    let parse_derived_table json_contents = 
        let sql = json_contents?sql.AsString()
        let sql_trigger_value = json_contents?sql_trigger_value.AsString()
        {sql = Some(sql); persistance = None; distribution_key = None; distribution_style = None; sort_method = None}

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