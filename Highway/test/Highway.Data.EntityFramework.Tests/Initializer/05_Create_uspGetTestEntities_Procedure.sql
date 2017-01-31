CREATE PROCEDURE [dbo].[uspGetTestEntities]
AS
    BEGIN
        SELECT TOP 10
                Id ,
                Name ,
                Email
        FROM    [dbo].[TestEntity];
    END;
