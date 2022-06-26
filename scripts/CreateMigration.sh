cd ../src/IdentityWebApi/ || exit

echo "Please, enter migration name:"
read migrationName

dotnet ef migrations add "${migrationName}" --context DatabaseContext -o ./Infrastructure/Database/Migrations
dotnet ef database update