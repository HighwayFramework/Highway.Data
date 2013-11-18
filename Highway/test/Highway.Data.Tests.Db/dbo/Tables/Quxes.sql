CREATE TABLE [dbo].[Quxes] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (MAX) NULL,
    [Bar_Id] INT            NULL,
    CONSTRAINT [PK_dbo.Quxes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Quxes_dbo.Bars_Bar_Id] FOREIGN KEY ([Bar_Id]) REFERENCES [dbo].[Bars] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Bar_Id]
    ON [dbo].[Quxes]([Bar_Id] ASC);

