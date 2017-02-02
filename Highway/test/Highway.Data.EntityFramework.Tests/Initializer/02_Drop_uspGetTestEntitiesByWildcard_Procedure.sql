IF ( EXISTS ( SELECT    *
                  FROM      sys.objects
                  WHERE     type = 'P'
                            AND name = 'uspGetTestEntitiesByWildcard' )
   )
    BEGIN
        DROP PROCEDURE [dbo].[uspGetTestEntitiesByWildcard];
    END;
