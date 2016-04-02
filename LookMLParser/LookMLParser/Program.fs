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


type Parser<'T> = Parser of (string -> Result<'T * string>)

let parse_character character = 
    let inner_function str = 
    
        if String.IsNullOrEmpty(str) then
            Failure "No more input"
        else
            let first = str.[0]
            if first = character then
                let tail = str.[1..]
                Success (character, tail)
            else
                Failure "not found"

    Parser inner_function

let parseB = parse_character 'b'

let run parser input = 
    let (Parser innerFn) = parser
    innerFn input

[<EntryPoint>]
let main argv = 
    printfn "%A" (run parseB  "bello world")
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
