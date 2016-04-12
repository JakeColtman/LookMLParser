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

    let row_string_parser row_name key_name = 
        let p_intro = string_parser row_name
        (whitespace >>. p_intro >>. colon >>. whitespace >>. string)
            |>> (fun name -> [[key_name, BasicParser.charListToString name]])

    let row_strings_parser row_names = 
        List.map (fun x -> row_string_parser x x) row_names |> choice

    let parser_to_output_map input = 
         input |> List.map List.concat
               |> List.concat
               |> Map.ofList


    let derived_table_parser = 

        let rowKeys = ["sortkeys" ; "persist_for"; "sql_trigger_value"; "distribution"; "distribution_style"; "indexes"]
        let p_string_rows = row_strings_parser rowKeys |> many

        let p_intro = string_parser "derived_table"
        
        whitespace >>. p_intro >>. colon >>. p_string_rows |>> parser_to_output_map |>> convert_into_derived_table

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

        let row_parsers = 
            ["type" ; "alias"; "label"; "required_fields" ; "drill_fields"; "hidden" ; "primary_key" ]
            |> row_strings_parser

        let p_sql = 
            let p_intro = string_parser "sql"
            let parser = whitespace >>. p_intro .>>. colon .>> whitespace >>. extendedString .>> whitespace
            parser |>> ( fun char_list -> [["sql" , BasicParser.charListToString char_list]])
                   
        let line_parser =  p_name <|> p_sql <|> row_parsers

        (many line_parser)
        |>> parser_to_output_map
        |>> (fun x -> convert_into_field x)




    let fields_parser = 
        many field_parser

    let set_parser = 
        let set_name_parser = whitespace >>. string .>> colon |>> ( fun char_list -> BasicParser.charListToString char_list)
        let row_parser = whitespace >>. dash >>. whitespace >>. string |>> ( fun char_list -> BasicParser.charListToString char_list)
        set_name_parser .>>. (many row_parser)

    let sets_parser = 
        let intro = whitespace >>. (string_parser "sets:")
        intro >>. many set_parser |>> (fun x -> convert_into_sets x)

    let view_parser = 
        
        let intro_parser = string_parser "- view:"
        let viewname_parser = extendedString |>> ( fun char_list -> BasicParser.charListToString char_list)
        let field_separator_parser = string_parser "fields:"
        let data_source_parser = sql_table_parser // <|> //derived_table_parser
        whitespace >>. intro_parser >>. whitespace >>. viewname_parser .>> whitespace .>>. sql_table_parser .>> whitespace .>> field_separator_parser .>> whitespace .>>. fields_parser .>>. sets_parser