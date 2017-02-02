CREATE PROCEDURE [dbo].[uspTestOutCommand]
    (
      @param1 VARCHAR(100) ,
      @result VARCHAR(100) OUT
    )
AS
    BEGIN
        SET @result = @param1;
    END;
