CREATE TABLE [dbo].[AnimalPlacement] (
    [Id]                    UNIQUEIDENTIFIER CONSTRAINT [DF_AnimalPlacement_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [AnimalId]              UNIQUEIDENTIFIER NOT NULL,
    [AnimalPlacementTypeId] INT              NOT NULL,
    [DateIn]                DATETIME2 (0)    NOT NULL,
    [DateOut]               DATETIME2 (0)    NULL,
    [AddedOnDate]           DATETIME2 (7)    NOT NULL,
    [AddedByUserId]         UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOnDate]         DATETIME2 (0)    NULL,
    [UpdatedByUserId]       UNIQUEIDENTIFIER NULL,
    [PersonId]              UNIQUEIDENTIFIER NULL,
    [OrganizationId]        UNIQUEIDENTIFIER NULL,
    [Notes]                 NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_AnimalPlacement] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Animal_Id___AnimalPlacement_AnimalId] FOREIGN KEY ([AnimalId]) REFERENCES [dbo].[Animal] ([Id]),
    CONSTRAINT [FK_AnimalPlacementType_Id___AnimalPlacement_AnimalPlacementTypeId] FOREIGN KEY ([AnimalPlacementTypeId]) REFERENCES [dbo].[AnimalPlacementType] ([Id]),
    CONSTRAINT [FK_Organization_Id___AnimalPlacement_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [FK_Person_Id___AnimalPlacement_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);

