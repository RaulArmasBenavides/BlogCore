#start SQL Server, start the script to create the DB and import the data, start the app
#/opt/mssql/bin/sqlserver & /usr/src/app/import-data.sh
#!/bin/bash
set -euo pipefail

# Arranca SQL Server en background
/opt/mssql/bin/sqlservr &

# Corre la inicialización cuando ya esté listo
/usr/src/app/restore-db.sh

# Mantén el proceso principal
wait
