CREATE TABLE [dbo].[Organization] (
    [Id]              UNIQUEIDENTIFIER CONSTRAINT [DF_Organization_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [IsActive]        BIT              NOT NULL,
    [IsDeleted]       BIT              NULL,
    [IsBoardingPlace] BIT              NOT NULL,
    [IsAnimalClinic]  BIT              NOT NULL,
    [IsDonor]         BIT              NOT NULL,
    [IsGrantGiver]    BIT              NOT NULL,
    [IsSponsor]       BIT              CONSTRAINT [DF_Organization_IsSponsor] DEFAULT ((0)) NOT NULL,
    [AddedOnDate]     DATETIME2 (0)    NOT NULL,
    [AddedByUserId]   NVARCHAR (256)   NOT NULL,
    [UpdatedOnDate]   DATETIME2 (0)    NULL,
    [UpdatedByUserId] NVARCHAR (256)   NULL,
    [DateActive]      DATETIME2 (0)    NOT NULL,
    [DateInactive]    DATETIME2 (0)    NULL,
    [DateDeleted]     DATETIME2 (0)    NULL,
    [Name]            NVARCHAR (200)   NOT NULL,
    [EIN]             NVARCHAR (10)    NULL,
    [Notes]           NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([Id] ASC)
);





