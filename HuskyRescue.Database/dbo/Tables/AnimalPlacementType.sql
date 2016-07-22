CREATE TABLE [dbo].[AnimalPlacementType] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AnimalPlacementType_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

