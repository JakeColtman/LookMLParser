open System.IO
open FSharp.Data
open FSharp.Data.JsonExtensions
open LookMLParser.IntegrationLayer
open LookMLParser.SetModel;
open LookMLParser.View;
open LookMLParser.DataSource;
open FsYaml.Yaml
type lookml = Map<string, obj> list

[<EntryPoint>]
let main argv =


    let looker_location = @"E:\looker\testJson.view.lookml"
    
    let string_yaml = File.ReadAllText looker_location
    let view_json = JsonValue.Parse(string_yaml)
    
    let view_name = view_json?view.AsString()

    let fields = view_json?fields
    let sets = view_json?sets


    let parsed_dervied_table = 
        match  view_json.TryGetProperty("sql_table_name") with 
            | Some a -> SqlTable {SqlTable.name  = a.ToString() }
            | None -> 
                let table_element = view_json?derived_table
                DerivedTable (parse_derived_table table_element)
    let parsed_fields = parse_fields fields
    let parsed_sets = parse_sets sets

    let viewy = {
        View.name = view_name;
        sets = parsed_sets;
        data =  parsed_dervied_table;
        fields = parsed_fields;  
        suggestions= true
    }

    let output = FsYaml.Yaml.dump viewy
    File.WriteAllText( @"E:\looker\testJson_output.view.lookml",output)

    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
