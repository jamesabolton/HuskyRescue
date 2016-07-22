CREATE TABLE [dbo].[AnimalIntakeType] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AnimalIntakeType_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

