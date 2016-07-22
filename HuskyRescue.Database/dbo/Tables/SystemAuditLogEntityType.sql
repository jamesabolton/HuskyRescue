CREATE TABLE [dbo].[SystemAuditLogEntityType] (
    [Id]    INT            NOT NULL,
    [Name]  NVARCHAR (200) NOT NULL,
    [Notes] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SystemAuditLogEntityType_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

