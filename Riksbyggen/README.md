This project is a .Net API for tenant management.

Dependencies:
 - .Net 9 sdk
 - .Net runtime

Nutget Packages:

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.SqlServer

Migrations:

- dotnet ef migrations add <migrationName>
- dotnet ef database update

TODO:
1. Make connection string safer!!!!!!!!!!1
2. Guid.NewGuid() for model


###################################3333

export DOTNET_ROOT=/usr/local/dotnet
export PATH=$DOTNET_ROOT:$HOME/.dotnet/tools:$PATH


source ~/.bashrc


curl -k -X POST https://localhost:7237/api/webhooks/apartments/status -H "Content-Type: application/json" -H "X-Webhook-Secret: RandoMSecret-Webhook-Key" -d '{
  "apartmentId": 6,
  "status": "Busy"
}'

