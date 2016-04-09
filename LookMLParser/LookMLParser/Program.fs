// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LookMLParser.BasicParser;

[<EntryPoint>]
let main argv =
    
    let testString = @"     - dimension: currency   type: number sql: {table}.hello"


    let result = LookMLParser.BasicParser.run LookMLParser.LookMLParser.field_parser testString
    printfn "%A" result
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
