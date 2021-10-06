docker build -t webapi WebApi\bin\Release\net5.0\publish
docker build -t nservicebusservice NServiceBusService\bin\Release\net5.0\publish
docker run -dp 5000:5000 webapi  
docker run nservicebusservice