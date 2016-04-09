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
        | COlumn

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

    type MetricDataType = 
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

    type DimensionGroupDetails = {
        data_type : DimensionGroupDataType;
        convert_tz : bool;
        primary_key: bool;
        timeframes : list<string>
    }

    type DimensionDetails = {
        data_type : DimensionDataType;
        hidden : bool;
        primary_key: bool;
        sql: string;
        aplha_sort: bool;
        tiers : list<int>;
        style: DimensionStyle;
        suggestable: bool
    }

    type MeasureDetails = {
        data_type : MetricDataType;
        direction: MeasureDirection;
        approximate: bool;
        approximate_threshold : int;
        sql_distinct_key : string;
        list_field: string;
        filters: string
    }

    type FieldDetails = {
        label : string;
        view_label: string;
        description: string;
        hidden: bool;
        alias: list<string>;
        required_fields: list<string>;
        drill_fields: list<string>
    }

    type FieldType = 
        | DimensionType
        | MetricType
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