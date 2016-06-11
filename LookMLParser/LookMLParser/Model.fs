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

    type SortKeys = {
        keys : string
    }

    type Indexes = {
        keys : string
    }

    type SortMethod = 
        | SortKeys of SortKeys
        | Indexes of Indexes

    type PersistFor = int * PersistanceTimeUnit

    type SQLTriggerValue = string 

    type Persistance = 
        | PersistFor of string
        | SQLTriggerValue of SQLTriggerValue

    type DerivedTable = {
        sql : Option<string>;
        persistance: Option<Persistance>;
        distribution_key : Option<string>;
        distribution_style: DistributionStyle;
        sort_method: Option<SortMethod>
    }

    type DataSource = 
        | SqlTable of SqlTable
        | DerivedTable of DerivedTable

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
        data: DataSource.DataSource;
        suggestions: bool;
        fields: FieldModel.Field[] option;
        sets: SetModel.Set[] option
    }