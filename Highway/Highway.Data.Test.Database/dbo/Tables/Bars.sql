CREATE TABLE [dbo].[Bars] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Bars] PRIMARY KEY CLUSTERED ([Id] ASC)
);

