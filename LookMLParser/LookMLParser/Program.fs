// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LookMLParser.BasicParser;
open LookMLParser.BlockParser;

[<EntryPoint>]
let main argv =
    let testString = @"  derived_table: name
  sql_trigger_value: imastring
  sortkeys: imakey"

    let result = run (parser_block 2) testString

    printfn "%A" result
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
