namespace LookMLParser 

module DataSource = 
    
    type DistributionStyle = 
        | All
        | Even

    type PersistanceTimeUnit = 
        | Seconds
        | Minutes
        | Hours

    type SqlTable = {
        name : string
    }

    type DerivedTable = {
        sql : string;
        persist_for : int * PersistanceTimeUnit;
        distribution_key : string;
        distribution_style: DistributionStyle;
        sort_kets : list<string>;
        indexes : list<string>
    }

    type DataSource = 
        | SqlTable of SqlTable
        | DerivedTables of DerivedTable

module SetModel = 

    type Set = {
        name: string
        fields : list<string>
    }

    type Sets = {
        sets : list<Set>
    }

module View = 

    type View = {
        name: string;
        data: DataSource;
        suggestions: bool;
        fields: list<FieldModel.Field>;
        sets: Option<SetModel.Sets>
    }