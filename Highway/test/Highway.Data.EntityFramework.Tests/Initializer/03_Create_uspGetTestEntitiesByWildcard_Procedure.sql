CREATE PROCEDURE [dbo].[uspGetTestEntitiesByWildcard] (@wildcard VARCHAR(100))
AS
  BEGIN
      SELECT TOP 10 Id,
                    NAME,
                    Email
      FROM   [dbo].[TestEntity]
      WHERE  ( NAME LIKE '%' + @wildcard + '%' );
  END; 
