namespace Learning

module Learning = 
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

    let letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    let anyOf listOfChars = 
        listOfChars |> List.map parse_character |> choice

    let parseAnyUpperCaseLetter = anyOf (letters |> Array.toList)

    let parseDigit = anyOf ['0'..'9']

    let mapP f parser = 
        let innerFn input = 
            let result = run parser input
            match result with 
            | Success (value, remaining) ->
                let newValue = f value
                Success (newValue, remaining)
            | Failure err -> 
                Failure err

        Parser innerFn

    let ( <!> ) = mapP
    let ( |>> ) x f = mapP f x

    let parseThreeDigits = 
        let tupleParser =
            parseDigit .>>. parseDigit .>>. parseDigit

        let transformTuple ( (c1, c2), c3) = 
            String [| c1 ; c2; c3 |]

        mapP transformTuple tupleParser

    let returnP x = 
        let innerFn input = 
            Success (x, input)
        Parser innerFn

    let applyP fP xP = 
        (fP .>>. xP)
        |> mapP (fun (f,x) -> f x )

    let ( <*> ) = applyP

    let lift2 f xP yP = 
        returnP f <*> xP <*> yP

    let addP = 
        lift2 (+)

    let rec sequence parserList = 
        let cons head tail = head::tail 
        let consP = lift2 cons

        match parserList with 
            | [] -> returnP []
            | head::tail ->
                consP head (sequence tail)

    let parseDigits = sequence [parseDigit ; parseDigit ; parseDigit]

    let charsToString chars = 
        String(List.toArray chars)

    let pstring str = 
        str
            |> List.ofSeq
            |> List.map parse_character
            |> sequence
            |> mapP charsToString

    let helloParser = pstring "hello"       

    let rec parseZeroOrMore parser input = 
        let firstResult = run parser input

        match firstResult with 
        | Failure err -> 
            ([], input)
        | Success (firstValue, inputAfterFirstParse) ->
            let (subsequentValues, remainingInput) = 
                parseZeroOrMore parser inputAfterFirstParse
            let values = firstValue::subsequentValues
            (values, remainingInput)

    let many parser = 

        let rec innerFn input = 
            Success (parseZeroOrMore parser input)
        Parser innerFn
