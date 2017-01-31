IF ( EXISTS ( SELECT    *
              FROM      sys.objects
              WHERE     type = 'P'
                        AND name = 'uspTestCommand' ) )
    BEGIN
        DROP PROCEDURE [dbo].[uspTestCommand];
    END;
