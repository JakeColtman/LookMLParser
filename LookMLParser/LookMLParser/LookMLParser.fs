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
                                        | "number" -> LookMLParser.FieldModel.Number
                                        | "yesno" -> LookMLParser.FieldModel.YesNo
                                        | "string" -> LookMLParser.FieldModel.String
                                        | _ ->  LookMLParser.FieldModel.String
                                    )

        ((((intro_parser >>. p_datatype ) .>> middle_spacing) .>>. p_name) .>> whitespace) .>>. field_type_parser

   