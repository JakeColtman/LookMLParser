namespace LookMLParser

module LookMLParser = 
    open BasicParser;
    open FieldModel;

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


    let field_parser =
    
        let intro_parser = whitespace .>>. dash .>>. whitespace
        let middle_spacing = whitespace .>>. colon .>>. whitespace

        let p_dimension = string_parser "dimension" >>% LookMLParser.FieldModel.DimensionType
        let p_measure = string_parser "measure" >>% LookMLParser.FieldModel.MetricType
        let p_datatype = p_dimension <|> p_measure

        let p_name = string |>> ( fun char_list -> BasicParser.charListToString char_list)

        let field_type_parser = 
            let p_intro = string_parser "type"
            (p_intro >>. colon >>. whitespace >>. string)
                |>> (fun name ->
                                    let string_name = BasicParser.charListToString name
                                    match string_name with 
                                        | "number" -> DimensionDataType LookMLParser.FieldModel.Number
                                        | "yesno" -> DimensionDataType LookMLParser.FieldModel.YesNo
                                        | "string" -> DimensionDataType LookMLParser.FieldModel.String
                                        | _ ->  DimensionDataType LookMLParser.FieldModel.String
                                    )

        let p_sql = 
            let p_intro = string_parser "sql"
            let parser = p_intro .>>. colon .>> whitespace >>. extendedString
            parser |>> ( fun char_list -> BasicParser.charListToString char_list)
                   

        intro_parser 
            >>. p_datatype 
            .>> middle_spacing 
            .>>. p_name 
            .>> whitespace 
            .>>. field_type_parser 
            .>> whitespace 
            .>>. p_sql
            |>> (fun x -> convert_into_field x)
            |>> printfn "%A"

