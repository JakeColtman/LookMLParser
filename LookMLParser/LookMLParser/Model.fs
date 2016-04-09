namespace LookMLParser 

module Model = 

    type DimensionType = 
        | String
        | Number
        | YesNo
        | Time
        | Tier
        | Location
        | ZipCode

    type MetricType = 
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

    type DimensionGroup = {
        ttype : DimensionType;
        hidden : bool;
        primary_key: bool;
        sql: string
    }

    type Dimension = {
        ttype : DimensionType;
        hidden : bool;
        primary_key: bool;
        sql: string
    }

    type Field = 
        | Dimension
        | Metric
        | DimensionGroup