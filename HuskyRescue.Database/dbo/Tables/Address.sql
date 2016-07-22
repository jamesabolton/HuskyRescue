CREATE TABLE [dbo].[Address] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_Address_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [AddressTypeId]     INT              NOT NULL,
    [Address1]          NVARCHAR (200)   NOT NULL,
    [Address2]          NVARCHAR (200)   NULL,
    [Address3]          NVARCHAR (200)   NULL,
    [City]              NVARCHAR (200)   NULL,
    [AddressStateId]    INT              NULL,
    [ZipCode]           NVARCHAR (10)    NULL,
    [CountryId]         NVARCHAR (3)     NULL,
    [IsBillingAddress]  BIT              NOT NULL,
    [IsShippingAddress] BIT              NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AddressState_Id___Address_AddressStateId] FOREIGN KEY ([AddressStateId]) REFERENCES [dbo].[AddressState] ([Id]),
    CONSTRAINT [FK_AddressType_Id___Address_AddressTypeId] FOREIGN KEY ([AddressTypeId]) REFERENCES [dbo].[AddressType] ([Id])
);

