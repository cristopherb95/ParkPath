# ParkPath
Application was done while learning .NET Core 3.1 Web API. It contains CRUD operations for national parks and it's trails, uses authentication and is consumed by .NET Core 3.1 MVC project. App is using Code-First approach, Entity Framework Core, MS Sql Server and repository pattern.
## API
API was created with .NET Core 3.1. You can perform CRUD operations on Trails and National Parks and register user or login. API is using Bearer token authentication as well as Admin and User roles. As default, user is registered as admin (just for sample purpose). Versioning was added for potential future releases. Also, sample XML documentation for some parts is included (visible in Swagger). Only GetNationalPark and GetTrail routes and authorized, for checking the correctness of authorization.

![image](https://user-images.githubusercontent.com/48388060/78028994-2dc28980-7360-11ea-9d78-677d237bb30f.png)

## MVC
Client side application was created with .NET Core MVC 3.1. It uses session storage to store token from API. Data about trails and national parks is placed in jQuery datatables plug-in. Toastr notifications are provided for delete operations and SweetAlert was used for confirmation boxes. You can perform CRUD operations on Trails and National Parks. Client-side is using cookie authentication with username and role coming from API request.

![image](https://user-images.githubusercontent.com/48388060/78031614-14234100-7364-11ea-8fa1-9be8d565a4b5.png)
