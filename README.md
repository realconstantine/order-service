# Order Service

This is a web API project which implements a simple Create Order API. This project is created based on .NET 8, and uses Sqlite as the database. EF Core Code First is approach used for data persistence. xUnit is used as the unit tests framework.

## Setup Instruction
Once this project is pulled to local, simply open it with Microsoft Visual Studio or JetBrain Rider. No extra steps needed.

Alternatively, use Visual Studio Code with C# Dev Kit installed.

## Run Web API
In commond line tool, navigate to folder ```\src\OrderService```, then run following commond:
 > dotnet run --launch-profile https

In browser, open the URL shown on the commond line output, and append ```\swagger``` to URL. This will open swagger page of the API.

## Run Unit Tests
In commond line tool, navigate to folder ```\src\Tests```, then run following commond:
 > dotnet test

Unit tests will be executed and results will be shown on the command output.

## API Spec
API Route
```
POST /api/orders
``` 
Request Body
```json
{
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "Jane Doe",
  "items": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantity": 1
    }
  ],
  "createdAt": "2025-10-20T11:12:12.293Z"
}
```
Possible Response
* When creation is successful, ```HTTP 201 Created``` is returned, with following response body:
```json
{
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```
* When request body is in a bad format, or an order with same OrderId is already existing, ```HTTP 404 Bad Request``` is returned, with detailed failure reason present.
* When unexpected error occurs, ```HTTP 500 Internal Server Error``` is returned.