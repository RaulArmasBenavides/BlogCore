#!/bin/bash
set -euo pipefail

# Detecta sqlcmd (tools18 > tools)
SQLCMD="/opt/mssql-tools18/bin/sqlcmd"
if ! command -v "$SQLCMD" >/dev/null 2>&1; then
  SQLCMD="/opt/mssql-tools/bin/sqlcmd"
fi

# Espera a que SQL Server esté listo (mejor que sleep fijo)
echo "Esperando a que SQL Server acepte conexiones..."
for i in {1..60}; do
  if "$SQLCMD" -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" >/dev/null 2>&1; then
    echo "SQL Server listo."
    break
  fi
  sleep 1
done

# echo "Ejecutando setup.sql..."
# NOTA: SIEMPRE entrecomilla la contraseña
# "$SQLCMD" -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -C -d master -i /usr/src/app/setup.sql

# --- OPCIONAL: Restaurar el .BAK (si lo necesitas) ---
# Ajusta las rutas lógicas/físicas según tu .bak
# "$SQLCMD" -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -C -d master -Q "
# DECLARE @db sysname = N'CONSULTORIO2';
# IF DB_ID(@db) IS NULL
# BEGIN
#   RESTORE DATABASE [CONSULTORIO2]
#   FROM DISK = N'/usr/src/app/CONSULTORIO2.bak'
#   WITH MOVE N'CONSULTORIO2' TO N'/var/opt/mssql/data/CONSULTORIO2.mdf',
#        MOVE N'CONSULTORIO2_log' TO N'/var/opt/mssql/data/CONSULTORIO2_log.ldf',
#        REPLACE, STATS = 10;
# END
# "
