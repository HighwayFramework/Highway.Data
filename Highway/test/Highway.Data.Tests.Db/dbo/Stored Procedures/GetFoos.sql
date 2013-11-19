Create PROCEDURE [dbo].[GetFoos]
	@Name varchar(20) 
AS
BEGIN
	SET NOCOUNT ON;

	Select x.Id as testID, x.Name as testName, x.Address as testAddress 
	from dbo.Foos as x
	where x.Name = @Name
END