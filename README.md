# tbsg

Online Platform to play turn based strategy games

## Games

### Reversi

## Dev

### Database

The develpoment database uses secret manager to store database auth.
Following entries are needed for the api secret manager
```
dotnet user-secrets set "database:user" "<user>"
dotnet user-secrets set "database:password" "<password>"
```
