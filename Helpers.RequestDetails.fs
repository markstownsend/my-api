namespace my_api.Helpers

open System

[<StructuredFormatDisplay("RequestDetails :: MessageId:{MessageId};
                          StartedAt:{Started};EndedAt:{Ended};
                          Method:{RequestMethod};V:{Version};P:{Product};
                          Status:{ResponseStatus}.")>]
type RequestDetails =
 {
    MessageId : string
    Started : DateTime
    Ended : DateTime 
    RequestMethod : string 
    Version : string 
    Product : string 
    ResponseStatus : int 
 }
 override r.ToString() = $"RequestDetails :: MessageId:{r.MessageId};
                          StartedAt:{r.Started};EndedAt:{r.Ended};
                          Method:{r.RequestMethod};V:{Version};P:{r.Product};
                          Status:{r.ResponseStatus}."