CREATE TABLE [dbo].[PhoneNumber] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_Phone_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [PhoneNumberTypeId] INT              NOT NULL,
    [PhoneNumber]       NVARCHAR (15)    NOT NULL,
    CONSTRAINT [PK_Phone] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PhoneNumberType_Id___PhoneNumber_PhoneNumberTypeId] FOREIGN KEY ([PhoneNumberTypeId]) REFERENCES [dbo].[PhoneNumberType] ([Id])
);

