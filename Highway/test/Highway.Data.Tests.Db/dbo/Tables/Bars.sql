CREATE TABLE [dbo].[Bars] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (MAX) NULL,
    [Qux_Id] INT            NULL,
    [Foo_Id] INT            NULL,
    CONSTRAINT [PK_dbo.Bars] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Bars_dbo.Foos_Foo_Id] FOREIGN KEY ([Foo_Id]) REFERENCES [dbo].[Foos] ([Id]),
    CONSTRAINT [FK_dbo.Bars_dbo.Quxes_Qux_Id] FOREIGN KEY ([Qux_Id]) REFERENCES [dbo].[Quxes] ([Id])
);



GO
CREATE NONCLUSTERED INDEX [IX_Qux_Id]
    ON [dbo].[Bars]([Qux_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Foo_Id]
    ON [dbo].[Bars]([Foo_Id] ASC);

