CREATE TABLE [dbo].[Person] (
    [Id]                  UNIQUEIDENTIFIER CONSTRAINT [DF_Person_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [UserId]              UNIQUEIDENTIFIER NULL,
    [IsActive]            BIT              NOT NULL,
    [IsDeleted]           BIT              NOT NULL,
    [IsVolunteer]         BIT              NOT NULL,
    [IsFoster]            BIT              NOT NULL,
    [IsAvailableFoster]   BIT              NOT NULL,
    [IsAdopter]           BIT              NOT NULL,
    [IsDonor]             BIT              NOT NULL,
    [IsSponsor]           BIT              CONSTRAINT [DF_Person_IsSponsor] DEFAULT ((0)) NOT NULL,
    [IsBoardMember]       BIT              NOT NULL,
    [IsSystemUser]        BIT              NOT NULL,
    [IsDoNotAdopt]        BIT              NOT NULL,
    [AddedOnDate]         DATETIME2 (0)    NULL,
    [AddedByUserId]       NVARCHAR (256)   NULL,
    [UpdatedOnDate]       DATETIME2 (0)    NULL,
    [UpdatedByUserId]     NVARCHAR (256)   NULL,
    [DateActive]          DATETIME2 (0)    NOT NULL,
    [DateInactive]        DATETIME2 (0)    NULL,
    [DateDeleted]         DATETIME2 (0)    NULL,
    [FirstName]           NVARCHAR (200)   NOT NULL,
    [LastName]            NVARCHAR (200)   NULL,
    [Sex]                 NCHAR (1)        NULL,
    [DriverLicenseNumber] NVARCHAR (50)    NULL,
    [Notes]               NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id] ASC)
);



