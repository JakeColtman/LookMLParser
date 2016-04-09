// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LookMLParser.BasicParser;

[<EntryPoint>]
let main argv =
    
    let testString = @"     - dimension: currency   "

    let find = "dimension"
    let white_space_parser = LookMLParser.BasicParser.parse_character ' '
    let manyWhitespace = LookMLParser.BasicParser.many white_space_parser
    let dashParser = LookMLParser.BasicParser.parse_character '-'
    let colonParser = LookMLParser.BasicParser.parse_character ':'
    let row_parser = manyWhitespace .>>. dashParser .>>. manyWhitespace

    let result = LookMLParser.BasicParser.run row_parser
    printfn "%A" (result testString)
    System.Console.ReadKey() |> ignore`
    0 // return an integer exit code
