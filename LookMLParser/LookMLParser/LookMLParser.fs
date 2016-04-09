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
    
        let dimension_parser = "dimension" |> List.ofSeq |> BasicParser.string_parser
        let measure_parser = "measure" |> List.ofSeq |> BasicParser.string_parser 
        let dimension_group_parser = "dimension_group" |> List.ofSeq |> BasicParser.string_parser  

        let intro_parser = whitespace .>>. dash .>>. whitespace
        let middle_spacing = whitespace .>>. colon .>>. whitespace

        let field_type_parser = dimension_parser <|> measure_parser <|> dimension_group_parser
        let field_name_parser = string |>> ( fun char_list -> BasicParser.charListToString char_list)

        let mapped_field_type_parser  = 
                                field_type_parser |>> ( fun name -> 
                                    match name with 
                                        | "dimension" -> LookMLParser.FieldModel.DimensionType
                                        | "measure" -> LookMLParser.FieldModel.MetricType
                                        | "dimension_group" -> LookMLParser.FieldModel.DimensionGroupType
                                        | _  -> LookMLParser.FieldModel.DimensionGroupType
                                )

        ((intro_parser >>. mapped_field_type_parser ) .>> middle_spacing) .>>. field_name_parser

   