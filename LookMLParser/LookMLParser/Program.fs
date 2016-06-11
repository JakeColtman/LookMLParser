// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LookMLParser.BasicParser;
open LookMLParser.BlockParser;
open LookMLParser.YAMLParser;


[<EntryPoint>]
let main argv =
    let testString = @"  - dictionary: test
  - testtwo
  hello
  "
  

    let result = run (LookMLParser.BlockParser.parse_line 0) testString

    printfn "%A" result
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
