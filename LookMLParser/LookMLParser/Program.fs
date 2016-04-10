﻿// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open LookMLParser.BasicParser;

[<EntryPoint>]
let main argv =
    let testString = @" testset:
                        - columnone
                        - columntwo
                        - columnthree
                        - columnfour
                       "


    let result = LookMLParser.BasicParser.run LookMLParser.LookMLParser.set_parser testString

//    
//    let testString = @"    - view: testview
//                                 sql_table_name: schema.table_name
//                                 fields:
//                                  - measure: currency   type: number sql: ${table}.currency
//    
//                       "
//
//
//    let result = LookMLParser.BasicParser.run LookMLParser.LookMLParser.view_parser testString
    printfn "%A" result
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
