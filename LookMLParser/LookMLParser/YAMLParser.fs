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
    let line_break : Parser<char> = anyOf [ ' '  ; '\n' ; '\r' ; '\t' ]
    let whitespace = many single_whitespace
    let parser_to_output_map input = 
        input |> Map.ofList

    type Node = 
        | Scalar of string
        | Sequence of string list
        | Mapping of Map<string, string>

    let p_keyValuePair = 
        extendedString .>> colon .>>. extendedString

    let p_mapping = 
        (many p_keyValuePair) |>> (fun x -> Mapping (parser_to_output_map x))

    let p_sequenceEntry = 
        dash .>> whitespace >>. extendedString

    let p_sequence = 
        (many p_sequenceEntry) |>> (fun x -> Sequence(x))

    let p_scalar = extendedString |>> (fun x -> Scalar(x))

    let p_node = p_scalar <|> p_mapping <|> p_sequence
        
        
