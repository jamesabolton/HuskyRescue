CREATE TABLE [dbo].[EmailAddress] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_EmailAddress_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [EmailAddressTypeId] INT              NOT NULL,
    [Address]            NVARCHAR (200)   NOT NULL,
    CONSTRAINT [PK_EmailAddress] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmailAddressType_Id___EmailAddress_EmailAddressTypeId] FOREIGN KEY ([EmailAddressTypeId]) REFERENCES [dbo].[EmailAddressType] ([Id])
);

