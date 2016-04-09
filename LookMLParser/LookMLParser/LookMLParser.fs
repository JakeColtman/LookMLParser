namespace LookMLParser

module LookMLParser = 
    open BasicParser;

    let single_whitespace : Parser<char> = anyOf [ ' '  ; '\n' ; '\r' ; '\t' ]
    let whitespace = many single_whitespace

    let dash = parse_character '-'
    let colon = parse_character ':'

    let character : Parser<char> = anyOf [ 'a'.. 'z']
    let string = many character

    let digit : Parser<char> = anyOf ['0' .. '9']
    let number = many digit


    let field_type_parser =
    
        let dimension_parser = "dimension" |> List.ofSeq |> BasicParser.string_parser
        let measure_parser = "measure" |> List.ofSeq |> BasicParser.string_parser 
        let dimension_group_parser = "dimension_group" |> List.ofSeq |> BasicParser.string_parser  

        dimension_parser <|> measure_parser <|> dimension_group_parser

    let field_parser = whitespace .>>. dash .>>. whitespace .>>. field_type_parser .>>. colon .>>. whitespace .>>. string .>>. whitespace