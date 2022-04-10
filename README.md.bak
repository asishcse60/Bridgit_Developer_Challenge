# Bridgit_Developer_Challenge
This is a demo api, which serves basic requirements from developer challenge guide.

#### Time Estimation
     - Coding 4 hours
     - Testing 1 hours
     - Documentation 30 min

#### API Prerequisites
    - C# .NET 6.0
    - Visual Studio 2022
    - MS SQL Server
    - Docker

##### Dependencies
    - Swagger
    - Entity Framework core
    - Fluent Validation
    - Xunit
## Project Structure
   - The Project follows Contracts, Database, Business, Security, Application, Test layer accordingly.

#### Validation Rules
     - Validation rules properly handled input level and database level.
     - Different types input model validator injected in startup.cs file.

## Database:
   - Must need set up MS SQL Server(if you want to run local)

##### REST Endpoints: Version v1
  ## Screech
     - http://localhost:8000/api/v1/Screech/add [Add Screech] 
         - HTTP METHOD : POST, Status Code: 201
     - http://localhost:8000/api/v1/Screech/get [Get a Screech by key]
         - HTTP METHOD: GET, Status Code: 200, 400  
     - http://localhost:8000/api/v1/Screech/update-text [Update a Screech by key]
         - HTTP METHOD: PATCH(partial update), Status Code: 204, 400
     - http://localhost:8000/api/v1/Screech/all-screech [Get all Screech]
         - HTTP METHOD: GET, Status Code: 200, 400
         - Pagination Implemented
             - if user id set, then it will return user id specific all screech
             - Othewise, it will return all screech
         - Default Filter: PageNo: 1, PageSize: 50, Sort: Dsc

 ## UserProfile
    - http://localhost:8000/api/v1/UserProfile/add [Add User]
        - HTTP METHOD : POST, Status Code: 201
    - http://localhost:8000/api/v1/UserProfile/get [Get a User by key]
        - HTTP METHOD: GET, Status Code: 200, 400
    - http://localhost:8000/api/v1/UserProfile/update [Update a User entity by key]
        - HTTP METHOD: PUT, Status Code: 204, 400
    - http://localhost:8000/api/v1/UserProfile/update-image [Update a User picture by key]
        - HTTP METHOD: PATCH(partial update), Status Code: 204, 400
    - http://localhost:8000/api/v1/Screech/all-profile [Get all profiles]
        - HTTP METHOD: GET, Status Code: 200
        - Pagination Implemented
            - if user id set, then it will return user id specific all screech
            - Othewise, it will return all screech
        - Default Filter: PageNo: 1, PageSize: 50, Sort: Dsc

## Security:
   - I wanted to implement Authentication and Authorization, Written some implementation, but it's incomplete. Although, I have knowledge different type of mechanism.
   - Like JWT Validation, Basic Auth and Identity Server.
   - For Simplicity, I have choosen Basic Auth with simple base64 encoding secret token with {username}:{password}
   - For this project, demo token(asish:secret123!) : YXNpc2g6c2VjcmV0MTIzIQ==

## TEST
   - Test written in Test Projects

## Docker
   - The api is docker buildable.
   - provided docker-compose file.
   - But I didn't docker env test properly.

## How to run Api ?
     - Step 1: Set MS SQL Server Connection String in appsettings.json file
          - For me,  "ScreechDB": "Data Source=ASISH-PC;Initial Catalog=ScreechDB;Integrated Security=True;Pooling=False"
     - Step 2:  Migration DB
            - Go to "ScreechDemo.Api" directory and run a command prompt.
            - Write and enter "dotnet ef migrations add InitialCreate5 --context UserProfileRepositoryContext" 
     - Step 3: Go to solution file directory and run "StartApi.bat" file
     - step 4: wow! Api is running and automatically launch with swagger. You will see all endpoints and could do testing.

## How to run Test?
   - Go to  "ScreechrDemo.Test" directory and open a command prompt.
   - write "dotnet test" in the command prompt and give enter.
   - your test running successful!

## Further Improvement
   - Authentication and Authorization
   - Docker container deployment like Kubernetes cluster.
   - Test Improvement.

#### Have a great day! :)
  
   
