// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open FsYaml;
open System.IO;
open LookMLParser.View;

type view = {view: string;  derived_table: string; fields: string; sets: string}

[<EntryPoint>]
let main argv =


    let looker_location = @"E:\looker\test.view.lookml"
    
    let string_yaml = File.ReadAllText looker_location
    let x = Yaml.load<view> string_yaml

    printfn "%A" x
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
