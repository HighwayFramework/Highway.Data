IF ( NOT EXISTS ( SELECT    NULL AS Nothing
                  FROM      INFORMATION_SCHEMA.TABLES
                  WHERE     TABLE_SCHEMA = 'dbo'
                            AND TABLE_NAME = 'TestEntity' )
   )
    BEGIN

        CREATE TABLE [dbo].[TestEntity]
            (
              [Id] [BIGINT] IDENTITY(1, 1)
                            NOT NULL ,
              [Email] [VARCHAR](100) NOT NULL ,
              [Name] [VARCHAR](100) NOT NULL ,
              CONSTRAINT [PK_TestEntity] PRIMARY KEY CLUSTERED ( [Id] ASC )
                WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
                       IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
                       ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
            )
        ON  [PRIMARY];

        ALTER TABLE [dbo].[TestEntity] ADD  CONSTRAINT [DF_TestEntity_Email]  DEFAULT ('') FOR [Email];

        ALTER TABLE [dbo].[TestEntity] ADD  CONSTRAINT [DF_TestEntity_Name]  DEFAULT ('') FOR [Name];
    END;