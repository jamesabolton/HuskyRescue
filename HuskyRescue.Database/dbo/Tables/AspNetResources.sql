CREATE TABLE [dbo].[AspNetResources] (
    [Id]         NVARCHAR (128) NOT NULL,
    [Name]       NVARCHAR (256) NOT NULL,
    [Operations] INT            NOT NULL,
    CONSTRAINT [PK_AspNetResources] PRIMARY KEY CLUSTERED ([Id] ASC)
);

