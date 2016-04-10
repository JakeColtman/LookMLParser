namespace LookMLParser

module LookMLParser = 
    open BasicParser;
    open Integration;

    let single_whitespace : Parser<char> = anyOf [ ' '  ; '\n' ; '\r' ; '\t' ]
    let whitespace = many single_whitespace

    let dash = parse_character '-'
    let colon = parse_character ':'

    let character : Parser<char> = anyOf [ 'a'.. 'z']
    let extended_characters = anyOf (List.append [ 'a'.. 'Z'] [ '$'; '{'; '}'; '_'; '.'] )
    let string = many character
    let extendedString = many (character <|> extended_characters)

    let digit : Parser<char> = anyOf ['0' .. '9']
    let number = many digit

    let sql_table_parser = 
        let p_intro = string_parser "sql_table_name"
        
        whitespace >>. p_intro >>. colon >>. whitespace >>. extendedString |>> ( fun char_list -> BasicParser.charListToString char_list |> convert_into_sql_table)


    let field_parser =

        let p_name = 
            let intro_parser = whitespace >>. dash >>. whitespace
            let middle_spacing = colon >>. whitespace

            let p_dimension = string_parser "dimension" |>> (fun string -> ["looker_type", string])
            let p_measure = string_parser "measure"  |>> (fun string -> ["looker_type", string])
            let p_datatype = p_dimension <|> p_measure
            let p_name = string |>> ( fun char_list -> ["name", BasicParser.charListToString char_list])
            (intro_parser >>. p_datatype .>> middle_spacing .>>. p_name) |>> (fun tuple -> [fst tuple ; snd tuple])

        let p_type = 
            let p_intro = string_parser "type"
            (whitespace >>. p_intro >>. colon >>. whitespace >>. string)
                |>> (fun name -> [["data_type", BasicParser.charListToString name]])

        let p_sql = 
            let p_intro = string_parser "sql"
            let parser = whitespace >>. p_intro .>>. colon .>> whitespace >>. extendedString .>> whitespace
            parser |>> ( fun char_list -> [["sql" , BasicParser.charListToString char_list]])
                   
        let line_parser =  p_name <|> p_sql <|> p_type

        let convert_output_into_map input = 
         input |> List.map List.concat
               |> List.concat
               |> Map.ofList

        (many line_parser)
        |>> convert_output_into_map
           // |>> (fun x -> convert_into_field x)




    let fields_parser = 
        many field_parser

    let view_parser = 
        
        let intro_parser = string_parser "- view:"
        let viewname_parser = extendedString |>> ( fun char_list -> BasicParser.charListToString char_list)
        let field_separator_parser = string_parser "fields:"
        whitespace >>. intro_parser >>. whitespace >>. viewname_parser .>> whitespace .>>. sql_table_parser .>> whitespace .>> field_separator_parser .>> whitespace .>>. fields_parser