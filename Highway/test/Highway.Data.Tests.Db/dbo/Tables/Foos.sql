CREATE TABLE [dbo].[Foos] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (MAX) NULL,
    [Address] NVARCHAR (MAX) NULL,
    [Bar_Id]  INT            NULL,
    CONSTRAINT [PK_dbo.Foos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Foos_dbo.Bars_Bar_Id] FOREIGN KEY ([Bar_Id]) REFERENCES [dbo].[Bars] ([Id])
);



GO
CREATE NONCLUSTERED INDEX [IX_Bar_Id]
    ON [dbo].[Foos]([Bar_Id] ASC);

