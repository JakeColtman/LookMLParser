// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System

let Parser str = 
    if String.IsNullOrEmpty(str) then
        (false , "")
    else if str.[0] = 'A' then 
        (true, str.[1..])
    else
        (false, str)

[<EntryPoint>]
let main argv = 
    printfn "%A" (Parser "AJake")
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
