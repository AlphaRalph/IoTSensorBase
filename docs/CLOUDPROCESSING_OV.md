<div id="top"></div>

<br />

There are some ways to access data form DynamoDB. For this see [https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/AccessingDynamoDB.html]. 
For example, you could use Amazon Quicksight to create a dashboard and visalise data. 

In this case we used Grafana in connection with a plugin, since Grafana has no native support for AWS DynamoDB. 
As a suitable plugin already existed, we just had to figure out how to connect the datasource and Grafana. 

Accessing data is done by querying it. To do this, you need to define a partition key attribute and a single vlaue attribute. 
See [https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Query.html] and [https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_Query.html] for more information. 

