namespace LookMLParser 

module YAMLParser = 
    open BasicParser;

    let dash = parse_character '-'
    let colon = parse_character ':'
    let character : Parser<char> = anyOf [ 'a'.. 'z']
    let extended_characters = anyOf (List.append [ 'a'.. 'Z'] [ '$'; '{'; '}'; '_'; '.'] )
    let string = many character
    let extendedString = many (character <|> extended_characters)
    let single_whitespace : Parser<char> = anyOf [ ' '  ; '\t' ]
    let line_break : Parser<char> = anyOf [ ' '  ; '\n' ; '\r' ; '\t' ]
    let whitespace = many single_whitespace

    type Scalar = string

    type Sequence = string list
    
    type Mapping = string list
    
    type Node = 
        | Scalar of Scalar
        | Sequence of Sequence
        | Mapping of Mapping

    let p_keyValuePair = 
        extendedString .>> colon .>>. extendedString

    let sequenceEntry = 
        dash >>. whitespace >>. extendedString

        
        
