namespace LookMLParser 

module SqlTable = 
    
    type SqlTable = {
        name : string
    }

module DerivedTable = 

    type DistributionStyle = 
        | All
        | Even

    type PersistanceTimeUnit = 
        | Seconds
        | Minutes
        | Hours

    type DerivedTable = {
        sql : string;
        persist_for : int * PersistanceTimeUnit;
        distribution_key : string;
        distribution_style: DistributionStyle;
        sort_kets : list<string>;
        indexes : list<string>
    }

module SetModel = 

    type Set = {
        name: string
        fields : list<string>
    }

    type Sets = {
        sets : list<Set>
    }

module FieldModel = 

    type DimensionStyle = 
        | Interval
        | Classic 
        | Integer
        | Relational

    type MeasureDirection = 
        | Row
        | Column

    type DimensionDataType = 
        | String
        | Number
        | YesNo
        | Time
        | Tier
        | Location
        | ZipCode

    type DimensionGroupDataType = 
        | Epoch
        | TimeStamp
        | DateTime
        | Date
        | YYYYMMDD

    type MeasureDataType = 
        | String 
        | Date
        | Number
        | Count
        | CountDistinct
        | Sum
        | SumDistinct
        | Avg
        | AvgDistinct
        | Min
        | Max
        | List
        | PercentOfPrevious
        | PercentOfTotal
        | RunningTotal

    type LookerDataType = 
        | MeasureDataType of MeasureDataType
        | DimensionDataType of DimensionDataType

    type DimensionGroupDetails = {
        data_type : DimensionGroupDataType;
        convert_tz : bool;
        primary_key: bool;
        timeframes : list<string>
    }

    type DimensionDetails = {
        data_type : DimensionDataType;
        primary_key: bool;
        alpha_sort: bool;
        tiers : Option<list<int>>;
        style: Option<DimensionStyle>;
        suggestable: bool
    }

    type MeasureDetails = {
        data_type : MeasureDataType;
        direction: Option<MeasureDirection>;
        approximate: Option<bool>;
        approximate_threshold : Option<int>;
        sql_distinct_key : Option<string>;
        list_field: Option<string>;
        filters: Option<string>
    }

    type FieldDetails = {
        sql: string;
        label : Option<string>;
        view_label: Option<string>;
        description: Option<string>;
        hidden: bool;
        alias: Option<list<string>>;
        required_fields: Option<list<string>>;
        drill_fields: Option<list<string>>
    }

    type FieldType = 
        | DimensionType
        | MeasureType
        | DimensionGroupType

    type Field = 
        | Dimension of FieldType * DimensionDetails * FieldDetails
        | Measure of FieldType * MeasureDetails * FieldDetails
        | DimensionGroup of FieldType * DimensionGroupDetails * FieldDetails




module View = 

    type ViewData = 
        | DerivedTable
        | SqlTable

    type View = {
        name: string;
        data: ViewData;
        suggestions: bool;
        fields: list<FieldModel.Field>;
        sets: SetModel.Sets
    }