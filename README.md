# MusicCollection
 
## Add Migration
dotnet ef migrations add <> -p MusicCollection.DataAccess -s MusicCollection.API -o ./Migrations -c MusicCollectionDbContext

## Reset Database
dotnet ef database update 0 -p .\MusicCollection.DataAccess\ -s .\MusicCollection.API\ -c MusicCollectionDbContext