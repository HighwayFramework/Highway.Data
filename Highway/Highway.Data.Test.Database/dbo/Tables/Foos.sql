CREATE TABLE [dbo].[Foos] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (12) NULL,
    CONSTRAINT [PK_Foos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

