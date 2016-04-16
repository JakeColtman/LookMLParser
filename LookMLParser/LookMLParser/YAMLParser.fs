namespace LookMLParser 

module YAMLParser = 
    open BasicParser;

    let dash = parse_character '-'
    let colon = parse_character ':'
    let character : Parser<char> = anyOf [ 'a'.. 'z']
    let extended_characters = anyOf (List.append [ 'a'.. 'Z'] [ '$'; '{'; '}'; '_'; '.'] )
    let string = many character
    let extendedString = many (character <|> extended_characters) |>>  ( fun char_list -> charListToString char_list)
    let single_whitespace : Parser<char> = anyOf [ ' '  ; '\t' ]
    let endOfLineParser = string_parser "\r\n"
    let whitespace = many single_whitespace
    let parser_to_output_map input = 
        input |> Map.ofList

    type Node = 
        | Scalar of string
        | Sequence of string list
        | Mapping of Map<string, string>

    let p_keyValuePair indentation = 
        indentation >>. extendedString .>> colon .>> whitespace .>>. extendedString .>> endOfLineParser

    let p_mapping indentation = 
        (many1 (p_keyValuePair indentation)) |>> (fun x -> Mapping (parser_to_output_map x))

    let p_sequenceEntry indentation = 
        indentation >>. dash .>> whitespace >>. extendedString .>> endOfLineParser

    let p_sequence indentation = 
        (many1 (p_sequenceEntry indentation)) |>> (fun x -> Sequence(x))

    let p_scalar indentation = indentation >>. extendedString .>> endOfLineParser |>> (fun x -> Scalar(x))

    let p_node indentation_level = 
        let indentation = manyNWithFailure single_whitespace indentation_level 
        (p_sequence indentation) <|> (p_mapping indentation) <|> (p_scalar indentation)
        
    let p_nodes = many1 (p_node 2)
