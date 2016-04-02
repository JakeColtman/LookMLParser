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

let andThen parser1 parser2 = 
    let innerFn input = 
        let result1 = run parser1 input

        match result1 with
            | Failure err -> 
                Failure err

            | Success (value1, remaining1) ->
                let result2 = run parser2 remaining1
                match result2 with 
                    | Failure err ->
                        Failure err
                    | Success (value2, remaining2) ->
                        let newValue = (value1, value2)
                        Success (newValue, remaining2)

    Parser innerFn

let orElse parser1 parser2 = 
    let innerFn input = 
        let result1 = run parser1 input

        match result1 with 
            | Failure err ->
                let result2 = run parser2 input
                match result2 with 
                | Failure err -> 
                    Failure err
                | Success (value2, remaining2) ->
                    Success (value2, remaining2)

            | Success (value1, remaining1) -> 
                Success (value1, remaining1)

    Parser innerFn

let ( .>>. ) = andThen
let ( .>>>. ) = orElse

let choice listOfParsers = 
    List.reduce ( .>>>. ) listOfParsers

let parseD = parse_character 'D'
let parseE = parse_character 'E'

let parseDthenE = parseD .>>. parseE
let parseDorE = parseD .>>>. parseE

let parserDthenEthenDorE = parseDthenE .>>. parseDorE

[<EntryPoint>]
let main argv = 
    printfn "%A" (run parserDthenEthenDorE  "DEDEllo world")
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
