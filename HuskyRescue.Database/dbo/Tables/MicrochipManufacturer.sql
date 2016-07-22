CREATE TABLE [dbo].[MicrochipManufacturer] (
    [Id]              UNIQUEIDENTIFIER CONSTRAINT [DF_MicrochipManufacturer_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [OrganizationId]  UNIQUEIDENTIFIER NOT NULL,
    [IsActive]        BIT              NOT NULL,
    [CostPerChip]     MONEY            NULL,
    [AddedOnDate]     DATETIME2 (0)    NOT NULL,
    [AddedByUserId]   UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOnDate]   DATETIME2 (0)    NULL,
    [UpdatedByUserId] UNIQUEIDENTIFIER NULL,
    [Notes]           NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_MicrochipManufacturer_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Organization_Id___MicrochipManufacturer_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id])
);

