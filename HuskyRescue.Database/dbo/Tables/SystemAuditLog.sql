CREATE TABLE [dbo].[SystemAuditLog] (
    [Id]                         INT              IDENTITY (1, 1) NOT NULL,
    [DateAdded]                  DATETIME2 (7)    NOT NULL,
    [UserId]                     UNIQUEIDENTIFIER NOT NULL,
    [SystemAuditLogEntityTypeId] INT              NOT NULL,
    [EntitySurrogateId]          NVARCHAR (1000)  NOT NULL,
    [Deleted]                    BIT              NOT NULL,
    [Added]                      BIT              NOT NULL,
    [Updated]                    BIT              NOT NULL,
    CONSTRAINT [PK_SystemAuditLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SystemAuditLogEntityType_Id___SystemAuditLog_SystemAuditLogEntityTypeId] FOREIGN KEY ([SystemAuditLogEntityTypeId]) REFERENCES [dbo].[SystemAuditLogEntityType] ([Id])
);

