﻿open System.IO
open FSharp.Data
open FSharp.Data.JsonExtensions
open LookMLParser.IntegrationLayer
open LookMLParser.SetModel;
open FsYaml.Yaml
type lookml = Map<string, obj> list

[<EntryPoint>]
let main argv =


    let looker_location = @"E:\looker\testJson.view.lookml"
    
    let string_yaml = File.ReadAllText looker_location
    let view_json = JsonValue.Parse(string_yaml)
    
    let view = view_json?view
    let fields = view_json?fields
    let derived_table = view_json?derived_table
    let sets = view_json?sets

    printfn "%A" (parse_fields fields)

//
//    let output = parse_derived_table derived_table
//
//    FsYaml.Yaml.dump output
//        |> printfn "%A"
//    let parsed_sets = parse_sets sets
//    match parsed_sets with 
//        | Some sets -> 
//           Array.iter (fun (s:Set) -> printfn "%A" s.name) sets
//        | None -> printfn "%A" "Failure"


    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
