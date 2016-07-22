CREATE TABLE [dbo].[AddressState] (
    [Id]           INT            NOT NULL,
    [Name]         NVARCHAR (200) NOT NULL,
    [Abbreviation] NVARCHAR (2)   NOT NULL,
    CONSTRAINT [PK_AddressState] PRIMARY KEY CLUSTERED ([Id] ASC)
);

