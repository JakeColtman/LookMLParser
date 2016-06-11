open System.IO
open FSharp.Data

open FSharp.Data.JsonExtensions

type sets = Map<string, string>
type fields = {fields: string}
type derived_table = {derived_table: string}
type view = {view: string}

type highest_level = 
    | Set of sets
    | String

type lookml = Map<string, obj> list

[<EntryPoint>]
let main argv =


    let looker_location = @"E:\looker\testJson.view.lookml"
    
    let string_yaml = File.ReadAllText looker_location
    let x = JsonValue.Parse(string_yaml)
    
    printfn "%A" (x?view.AsString())
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
