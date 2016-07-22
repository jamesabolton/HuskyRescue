CREATE TABLE [dbo].[StoreCart] (
    [Id]            UNIQUEIDENTIFIER CONSTRAINT [DF_StoreCart_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [CreatedOnDate] DATETIME2 (0)    NOT NULL,
    [UpdatedOnDate] DATETIME2 (0)    NULL,
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_StoreCart] PRIMARY KEY CLUSTERED ([Id] ASC)
);

