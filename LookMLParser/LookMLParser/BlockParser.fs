namespace LookMLParser 

module BlockParser = 
    open BasicParser;
    open LookMLParser;

    ///Parse into blocks of the same level of indentation, with subblocks when the indendation goes in

    type Line = string
    type Block = Line list

    let endOfLineParser = string_parser "\r\n";
    let single_whitespace : Parser<char> = anyOf [ ' ' ; '\t' ]

    let parse_line previous_indentation = 
        let whitespace_count = 
            (many single_whitespace) |>> (fun x -> x.Length)
        let rest_of_string = string;
        let p_block_white_space = manyN single_whitespace  
        let line_contents = manyN single_whitespace whitespace_count 
        starting_whitespace
     //  let parse_first_whitespace = many single_whitespace |>> (fun x -> x.Length)
     //  parseNOf single_whitespace (fst blockIndentation)

    
    let testString = @"""  t
  t
  t
    r
    r
  e    
";

    
        


