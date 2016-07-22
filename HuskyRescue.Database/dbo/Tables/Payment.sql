CREATE TABLE [dbo].[Payment] (
    [Id]                   UNIQUEIDENTIFIER CONSTRAINT [DF_Donation_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [PersonId]             UNIQUEIDENTIFIER NULL,
    [OrganizationId]       UNIQUEIDENTIFIER NULL,
    [PaymentTransactionId] NVARCHAR (50)    NOT NULL,
    [PayeeName]            NVARCHAR (200)   NOT NULL,
    [DateSubmitted]        DATETIME2 (0)    NOT NULL,
    [IsCash]               BIT              NOT NULL,
    [IsCheck]              BIT              NOT NULL,
    [IsOnline]             BIT              NOT NULL,
    [Amount]               MONEY            NOT NULL,
    [AddedOnDate]          DATETIME2 (0)    NULL,
    [AddedByUserId]        NVARCHAR (256)   NULL,
    [UpdatedOnDate]        DATETIME2 (0)    NULL,
    [UpdatedByUserId]      NVARCHAR (256)   NULL,
    [Notes]                NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Donation] PRIMARY KEY CLUSTERED ([Id] ASC)
);

