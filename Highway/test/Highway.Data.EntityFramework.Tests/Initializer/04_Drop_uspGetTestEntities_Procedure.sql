IF ( EXISTS ( SELECT    *
                  FROM      sys.objects
                  WHERE     type = 'P'
                            AND name = 'uspGetTestEntities' )
   )
    BEGIN
        DROP PROCEDURE [dbo].[uspGetTestEntities];
    END;
