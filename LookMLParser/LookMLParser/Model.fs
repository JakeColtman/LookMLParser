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
        | PersistFor of PersistFor
        | SQLTriggerValue of SQLTriggerValue

    type DerivedTable = {
        sql : string;
        persistance: Persistance;
        distribution_key : string;
        distribution_style: DistributionStyle;
        sort_method: SortMethod
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
        data: DataSource.DataSource;
        suggestions: bool;
        fields: list<FieldModel.Field>;
        sets: Option<SetModel.Sets>
    }