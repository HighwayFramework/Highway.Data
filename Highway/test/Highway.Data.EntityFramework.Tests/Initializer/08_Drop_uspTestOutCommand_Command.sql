IF ( EXISTS ( SELECT    *
              FROM      sys.objects
              WHERE     type = 'P'
                        AND name = 'uspTestOutCommand' ) )
    BEGIN
        DROP PROCEDURE [dbo].[uspTestOutCommand];
    END;
