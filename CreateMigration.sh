cd ./IdentityWebApi

echo "Please, enter migration name:"
read migrationName

dotnet ef migrations add "${migrationName}" -c DatabaseContext -o ./DAL/Migrations
dotnet ef database update