CREATE TABLE [dbo].[SystemSetting] (
    [Name]            NVARCHAR (100)  NOT NULL,
    [Value]           NVARCHAR (1000) NOT NULL,
    [AddedOnDate]     DATETIME2 (0)   NOT NULL,
    [AddedByUserId]   NVARCHAR (256)  NOT NULL,
    [UpdatedOnDate]   DATETIME2 (0)   NULL,
    [UpdatedByUserId] NVARCHAR (256)  NULL,
    [Notes]           NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_SystemSetting] PRIMARY KEY CLUSTERED ([Name] ASC)
);

