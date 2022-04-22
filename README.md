## About The Project

**Note**: This project is a non-commercial application based on authors personal interests of technologies.

This project represents application that is responsible for handling users and related to them entities.

### Built With

* [C# 10](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10);
* [ASP.Net 6.0 WebAPI](https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-6.0?view=aspnetcore-6.0);
* [Entity Framework](https://entityframeworkcore.com);
* [Identity Server Core](https://docs.identityserver.io/en/latest/quickstarts/6_aspnet_identity.html);
* [Handlebars](https://handlebarsjs.com/);
* [Automapper](https://automapper.org/);
* [MailKit](https://github.com/jstedfast/MailKit);
* [HealthChecks UI](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health);
* [NUnit](https://nunit.org/);

### Prerequisites

Before launching this application make sure you have prepared the following components:

* Windows | macOS | Linux;
* [MSSQL](https://www.microsoft.com/en-us/sql-server/sql-server-2019?rtc=1);
* [.Net 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0);
* [Visual Studio](https://visualstudio.microsoft.com/) | [Visual Studio Code](https://code.visualstudio.com/) | [Rider](https://www.jetbrains.com/rider/);
* [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) | [DBeaver](https://dbeaver.io/) | [DataGrip](https://www.jetbrains.com/datagrip/);
* [Docker](https://www.docker.com) - Optional;

### Installation and launch

1. Clone repository:
   ```sh
   git clone https://github.com/dotnet-fizzyy/ASP.Net-Identity.git
   ```
2. Run **IdentityWebApi.sln** in the root directory;
3. Depending on your OS, choose IIS or Kestrel as hosting webservices;
4. Make sure your MSSQL database instance is running. Otherwise you will not be able to launch app;
5. Start the application;
6. Visit the following URL: https://localhost:{port}/swagger. You should be able to see Swagger description;

### Docker launch

Docker is not necessary to launch application if you have prepared prerequisites for your physical OS. If you want to launch it without and modifications and would like to see the working result, you can refer to the next steps:

1. Follow the first step from previous article;
2. Switch open project root directory via terminal;
3. Run the following command: 
    ```
    docker-compose up --build
    ```
4. Wait until corresponding images will be downloaded, all steps from Dockerfile will pass and application will start;
5. Visit the following URL: http://localhost:13501/swagger. You should be able to see Swagger description;
6. If you need to stop Docker containers, you can just press `ctrl + C` keyboard combination in your terminal and wait until containers will be stopped. To terminate containers, enter (or stop via Docker dashboard):
    ```
    docker-compose down
    ```
7. To remove application images, enter the following command (or remove via Docker dashboard):
    ```
    docker rmi mcr.microsoft.com/mssql/server identitywebapi
    ```

## Logging
Application doesn't writes logs to files, it display logs in terminal in the following ways:
1. Local run via IDE;
2. Docker;

## Usage

For more examples, please, refer to the following options:
* [Swagger](https://swagger.io/)

## Contact

dotnet-fizzyy | [GitHub](https://github.com/dotnet-fizzyy) | ezzyfizzy27@gmail.com