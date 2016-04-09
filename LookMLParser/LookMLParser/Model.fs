namespace LookMLParser 

module Model = 

    type DimensionDataType = 
        | String
        | Number
        | YesNo
        | Time
        | Tier
        | Location
        | ZipCode

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
        ttype : DimensionDataType;
        hidden : bool;
        primary_key: bool;
        sql: string
    }

    type DimensionDetails = {
        ttype : DimensionDataType;
        hidden : bool;
        primary_key: bool;
        sql: string
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

    type FieldTypes = 
        | Dimension
        | Metric
        | DimensionGroup