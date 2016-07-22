CREATE TABLE [dbo].[OrganizationAddresses] (
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [AddressId]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_OrganizationAddresses] PRIMARY KEY CLUSTERED ([OrganizationId] ASC, [AddressId] ASC),
    CONSTRAINT [FK_Address_Id___OrganizationAddresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address] ([Id]),
    CONSTRAINT [FK_Organization_Id___OrganizationAddresses_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id])
);

