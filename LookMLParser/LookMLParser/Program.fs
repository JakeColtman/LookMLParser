// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LookMLParser.BasicParser;

[<EntryPoint>]
let main argv =
    
    let testString = @"
          - dimension: currency
    "

    let find = "dimension"
    let findParser = testString
                        |> List.ofSeq
                        |> List.map LookMLParser.BasicParser.parse_character
                        |> LookMLParser.BasicParser.sequence

    let result = LookMLParser.BasicParser.run findParser
    printfn "%A" (result testString)
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
