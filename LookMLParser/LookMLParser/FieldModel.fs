namespace LookMLParser 


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
        timeframes : list<string> option
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
        sql: string option;
        label : Option<string>;
        view_label: Option<string>;
        description: Option<string>;
        hidden: string option;
        alias: Option<string>;
        required_fields: Option<string>;
        drill_fields: Option<string>
    }

    type Field = 
        | Dimension of  DimensionDetails * FieldDetails
        | Measure of MeasureDetails * FieldDetails
        | DimensionGroup of DimensionGroupDetails * FieldDetails