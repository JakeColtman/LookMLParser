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

        let row_string_parser row_name key_name = 
            let p_intro = string_parser row_name
            (whitespace >>. p_intro >>. colon >>. whitespace >>. string)
                |>> (fun name -> [[key_name, BasicParser.charListToString name]])

        let p_name = 
            let intro_parser = whitespace >>. dash >>. whitespace
            let middle_spacing = colon >>. whitespace

            let p_dimension = string_parser "dimension" |>> (fun string -> ["looker_type", string])
            let p_measure = string_parser "measure"  |>> (fun string -> ["looker_type", string])
            let p_datatype = p_dimension <|> p_measure
            let p_name = string |>> ( fun char_list -> ["name", BasicParser.charListToString char_list])
            (intro_parser >>. p_datatype .>> middle_spacing .>>. p_name) |>> (fun tuple -> [fst tuple ; snd tuple])

        let p_type = row_string_parser "type" "data_type"
        let p_alias = row_string_parser "alias" "alias"            
        let p_label = row_string_parser "label" "label"
        let p_required_fields = row_string_parser "required_fields" "required_fields"
        let p_drill_fields = row_string_parser "drill_fields" "drill_fields"
        let p_hidden = row_string_parser "hidden" "hidden"
        let p_primary_key = row_string_parser "primary_key" "pk"

        let p_sql = 
            let p_intro = string_parser "sql"
            let parser = whitespace >>. p_intro .>>. colon .>> whitespace >>. extendedString .>> whitespace
            parser |>> ( fun char_list -> [["sql" , BasicParser.charListToString char_list]])
                   
        let line_parser =  p_name <|> p_sql <|> p_type <|> p_alias <|> p_label <|> p_required_fields <|> p_drill_fields <|> p_hidden <|> p_primary_key

        let convert_output_into_map input = 
         input |> List.map List.concat
               |> List.concat
               |> Map.ofList

        (many line_parser)
        |>> convert_output_into_map
        |>> (fun x -> convert_into_field x)




    let fields_parser = 
        many field_parser

    let set_parser = 
        let set_name_parser = whitespace >>. string .>> colon |>> ( fun char_list -> [BasicParser.charListToString char_list])
        let row_parser = whitespace >>. dash >>. whitespace >>. string |>> ( fun char_list -> BasicParser.charListToString char_list)
        set_name_parser .>>. (many row_parser)


    let view_parser = 
        
        let intro_parser = string_parser "- view:"
        let viewname_parser = extendedString |>> ( fun char_list -> BasicParser.charListToString char_list)
        let field_separator_parser = string_parser "fields:"
        whitespace >>. intro_parser >>. whitespace >>. viewname_parser .>> whitespace .>>. sql_table_parser .>> whitespace .>> field_separator_parser .>> whitespace .>>. fields_parser