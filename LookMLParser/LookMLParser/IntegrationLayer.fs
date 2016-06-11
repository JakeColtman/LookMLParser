namespace LookMLParser

module IntegrationLayer = 
    open LookMLParser.FieldModel;
    open LookMLParser.SetModel;
    open LookMLParser.View;
    open LookMLParser.DataSource;
    open FSharp.Data.JsonExtensions;
    open FSharp.Data;
   


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