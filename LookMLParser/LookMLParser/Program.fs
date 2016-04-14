// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LookMLParser.BasicParser;

[<EntryPoint>]
let main argv =
    let testString = @" 
    derived_table:
    sql_trigger_value: imastring
    sortkeys: imakey"


    let result = LookMLParser.BasicParser.run LookMLParser.BlockParser.parser_block testString

    printfn "%A" result
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
