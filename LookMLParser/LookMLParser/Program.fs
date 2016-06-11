// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open FsYaml;
open System.IO;


type sets = {sets: string}
type fields = {fields: string}
type derived_table = {derived_table: string}
type view = {view: string}

type highest_level = 
    | Set of sets
    | Field of fields
    | Derived_Table of derived_table
    | View of view


type lookml = Map<string, string> list

[<EntryPoint>]
let main argv =


    let looker_location = @"E:\looker\test.view.lookml"
    
    let string_yaml = File.ReadAllText looker_location
    let x = Yaml.load<lookml> string_yaml

    printfn "%A" x
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
