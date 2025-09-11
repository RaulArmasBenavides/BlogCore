USE master;
GO

IF DB_ID(N'CONSULTORIO2') IS NULL
BEGIN
    DECLARE @bak NVARCHAR(4000) = N'/usr/src/app/CONSULTORIO2.bak';
    -- Detecta nombres lógicos del .bak (data y log)
    IF OBJECT_ID('tempdb..#fl') IS NOT NULL DROP TABLE #fl;
    CREATE TABLE #fl
    (
        LogicalName sysname,
        PhysicalName NVARCHAR(4000),
        [Type] CHAR(1),
        FileGroupName sysname NULL,
        Size BIGINT,
        MaxSize BIGINT,
        FileId INT
    );

    INSERT INTO #fl (LogicalName, PhysicalName, [Type], FileGroupName, Size, MaxSize, FileId)
    EXEC('RESTORE FILELISTONLY FROM DISK = ''' + @bak + '''');

    DECLARE @ld sysname = (SELECT TOP 1 LogicalName FROM #fl WHERE [Type]='D' ORDER BY FileId);
    DECLARE @ll sysname = (SELECT TOP 1 LogicalName FROM #fl WHERE [Type]='L' ORDER BY FileId);

    RESTORE DATABASE [CONSULTORIO2]
      FROM DISK = @bak
      WITH MOVE @ld TO N'/var/opt/mssql/data/CONSULTORIO2.mdf',
           MOVE @ll TO N'/var/opt/mssql/data/CONSULTORIO2_log.ldf',
           REPLACE, STATS = 5;
END
GO
