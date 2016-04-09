namespace LookMLParser 

module BasicParser = 
    open System

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
    let ( <|> ) = orElse

    let choice listOfParsers = 
        List.reduce ( <|> ) listOfParsers

    let anyOf characterList = 
        characterList 
        |> List.map parse_character
        |> choice

    let bindP f p = 
        let innerFn input = 
            let result1 = run p input
            match result1 with 
            | Failure err -> Failure err
            | Success (value1, remainingInput) -> 
                let p2 = f value1
                run p2 remainingInput

        Parser innerFn

    let ( >>= ) p f = bindP f p 

    let returnP x = 
        let innerFn input = 
            Success(x, input)
        Parser innerFn

    let mapP f = 
        bindP ( f >> returnP)

    let ( <!> ) = mapP
    let ( |>> ) x f = mapP f x

    let applyP fP xP = 
        fP >>= (fun f -> xP >>= (fun x -> returnP (f x)))

    let ( <*> ) = applyP

    let lift2 f xP yP = 
        returnP f <*> xP <*> yP

    let rec sequence parserList = 
        
        let cons head tail = head::tail

        let consP = lift2 cons

        match parserList with 
        | [] -> 
            returnP []
        | head::tail -> 
            consP head (sequence tail)

    let charListToString  character_list = 
        String(List.toArray character_list)

    let string_parser str = 
        str
            |> List.ofSeq
            |> List.map parse_character
            |> sequence
            |> mapP charListToString

    let rec parseZeroOrMore parser input = 

        let result1 = run parser input

        match result1 with 
        | Failure err
                -> ([], input)
        | Success (firstValue, inputAfterFirstValue) -> 
            let (subsequentValues, remainingInput) = parseZeroOrMore parser inputAfterFirstValue
            let values = firstValue::subsequentValues
            (values, remainingInput)

    let many parser = 
        let rec innerFn input = 
            Success(parseZeroOrMore parser input)

        Parser innerFn

    let many1 parser = 
        parser  >>= (fun head -> many parser >>= fun tail -> returnP (head::tail))

    let opt p = 
        let some = p |>> Some
        let none = returnP None
        some <|> none

    let (.>>) p1 p2 = 
        p1 .>>. p2 |> mapP (fun (a,b) -> a)

    let (>>.) p1 p2 = 
        p1 .>>. p2 |> mapP (fun (a,b) -> b)

    let between p1 p2 p3 = 
        p1 >>. p2 .>> p3

    let sepBy1 p sep = 
        let sepThenP = sep >>. p
        p .>>. many sepThenP
        |>> fun (p, pList) -> p::pList

    let sepBy p sep = 
        sepBy1 p sep <|> returnP []

    let (>>%) p x = p |>> (fun _ -> x)

    

