// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LookMLParser.BasicParser;

[<EntryPoint>]
let main argv =
    let testString = @" derived_table:
  sql_trigger_value: imastring
  sortkeys: imakey"

    let parser = LookMLParser.LookMLParser.single_whitespace
    let NWhiteSpace = LookMLParser.BasicParser.manyN parser
    let parserWidth = NWhiteSpace 2
    let result = LookMLParser.BasicParser.run parserWidth testString

    printfn "%A" result
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
