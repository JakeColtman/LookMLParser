namespace LookMLParser 

module ModelModel = 

    type Relationship = 
        | OneToOne
        | ManyToOne
        | OneToMany
        | ManyToMany
    
    type JoinType = 
        | InnerJoin
        | FullOuterJoin
        | CrossJoin
        | LeftJoin

    type Join = {
        join: string;
        relationship: Relationship;
        sql_on : string;
        join_type: JoinType;
        view_label: string
    }

    type Explore = {
        explore: string;
        label: string;
        view: string;
        fields: string list; 
        joins: Join list    
    }

    type Model =  {
        includes: string list;
        connection: string;
        explores: Explore list    
    }