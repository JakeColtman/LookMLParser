namespace LookMLParser 

module YAMLParser = 

    type Scalar = string

    type Sequence = string list
    
    type Mapping = string list
    
    type Node = 
        | Scalar of Scalar
        | Sequence of Sequence
        | Mapping of Mapping
        
        
