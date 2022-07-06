# TheoremOne Practical Exercise - Proof of Concept for SmartAC

Please read these documents to begin the project:

* [Introduction](docs/project/smartac-intro.md)
* [SmartAC specification](docs/project/smartac-spec.md)
* Prioritization:
    * for [Backend-only variation](docs/project/smartac-priorities-backend-only.md)
    * for [Fullstack variation](docs/project/smartac-priorities-fullstack.md)
* [Wireframes](https://www.figma.com/file/uYhDDrxN5wq7r837wO8A7Q/Wireframes) for reference

Add any additional project documentation here as you work on the project and complete the assigned tasks.

## Backend built With

- [NetCore](https://docs.microsoft.com/en-us/dotnet/) - The webapi framework used
- [Hangfire](https://www.hangfire.io/) - Background job scheduler
- [Swagger](https://swagger.io/) - Documentation generator
- [ProblemDetails](https://github.com/khellang/Middleware) - Error handling
- [In-Memory Database](https://github.com/aspnet/EntityFrameworkCore) - In-memory database
- [Autofac](https://autofac.org/) - Dependency injection
- [Serilog](https://serilog.net/) - Logging
- [AutoMapper](https://automapper.org/) - Object mapping
- [FluentValidation](https://fluentvalidation.net/) - Object validation

### Documentation

For testing purposes, you can use the following url: [Swagger-UI](https://localhost:5001/swagger/index.html)

There is a default user account located in the appsettings.json file for retrieve information from the admin, the
credentials are:

```json
{
  "userName": "admin",
  "password": "admin"
}
````

By default the application has the following smart devices registered:

```json
[
  {
    "serialNumber": "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
    "secretKey": "fff754c711b34ccd9bf1547f2ea96049"
  },
  {
    "serialNumber": "02488664b3dd433ba0ab64ba84b9539c",
    "secretKey": "d7c8b47a619c442da8e918b875ea3e5c"
  },
  {
    "serialNumber": "ea9c98ed90df4d2686d1b57264e8159e",
    "secretKey": "ad5f8a5dffc542ef8417230f090a4b05"
  },
  {
    "serialNumber": "e662a95e4a3245df8f13f6ab09384575",
    "secretKey": "3a2e80528b2244d08cb7b4b0f2335b5d"
  },
  {
    "serialNumber": "3f70d60576d64fba88716d382556e3d0",
    "secretKey": "7e5958c36a084b06bcf4fc16a0e5ca3a"
  }
]  
``` 

### Installing App

Get git source code:

```sh
$ git clone https://github.com/TheoremOne-Assessments/fs-32b5eedf-f33a-4b52-a593-847bab276d5a-smartac.git
```

Restore DotNET and npm packages:

```sh
$ cd fs-32b5eedf-f33a-4b52-a593-847bab276d5a-smartac
$ dotnet build source/SmartAC.Web/SmartAC.Web.csproj /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary
```

Create and run docker image:

```sh
$ docker build -t smartacweb -f source/SmartAC.Web/Dockerfile .
$ docker run --rm -d  -p 5000:5000/tcp smartacweb
```

Go to the following url: `http://localhost:5000` and that's it.
