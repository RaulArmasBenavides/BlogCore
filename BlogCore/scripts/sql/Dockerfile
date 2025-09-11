# We choose exact tag (not 'latest'), to be sure that new version wont break creating image
FROM mcr.microsoft.com/mssql/server:2022-latest

# Set environment variables
ENV ACCEPT_EULA=Y
ENV ACCEPT_EULA=Y \
    MSSQL_SA_PASSWORD="P@ssw0rd?"
# (Opcional: define también SA_PASSWORD para compatibilidad)
# ENV SA_PASSWORD="P@ssw0rd?"
USER root
# Create app directory
RUN mkdir -p /usr/src/app /var/opt/mssql/backup

# Copy the setup script and database backup
COPY CONSULTORIO2.bak /var/opt/mssql/backup/
COPY setup.sql        /usr/src/app/
COPY restore-db.sh    /usr/src/app/
COPY entrypoint.sh    /usr/src/app/


# Permisos y normalizar finales de línea (CRLF -> LF)
RUN find /usr/src/app -maxdepth 1 -type f -name '*.sh' -exec chmod +x {} \; \
 && find /usr/src/app -maxdepth 1 -type f -name '*.sh' -exec sed -i 's/\r$//' {} \; \
 && chown -R mssql:mssql /usr/src/app /var/opt/mssql


USER mssql
# Expose port 1433 in case accesing from other container
EXPOSE 1433

# Run Microsoft SQl Server and initialization script (at the same time)
# Note: If you want to start MsSQL only (without initialization script) you can comment bellow line out, CMD entry from base image will be taken
CMD ["/bin/bash", "-c", "/usr/src/app/entrypoint.sh"]