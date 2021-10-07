docker build -t webapi WebApi\bin\Release\net5.0\publish
docker build -t nservicebusservice NServiceBusService\bin\Release\net5.0\publish
docker-compose up -d
REM docker run -dp 5000:5000 -e ASPNETCORE_URLS=http://+:5000 -e rabbitmqhost=172.17.0.2 webapi  
REm docker run -e rabbitmqhost=172.17.0.2 nservicebusservice