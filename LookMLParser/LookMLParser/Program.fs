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

type Result<'a> = 
    | Success of 'a
    | Failure of string 

let parse_character character str = 
    if String.IsNullOrEmpty(str) then
        Failure "No more input"
    else
        let first = str.[0]
        if first = character then
            let tail = str.[1..]
            Success (character, tail)
        else
            Failure "not found"
        

[<EntryPoint>]
let main argv = 
    printfn "%A" (Parser "AJake")
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
