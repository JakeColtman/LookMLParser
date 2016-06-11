// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open YamlDotNet.Serialization;
open YamlDotNet.Serialization.NamingConventions;
open System.IO;

[<EntryPoint>]
let main argv =
    let looker_location = @"E:\looker\test.view.lookml"
    
    let string_yaml = File.ReadAllText looker_location

    printfn "%A" string_yaml
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
