IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [GroupPolicyObject] (
    [PK_GPO] nvarchar(450) NOT NULL,
    [BsiCertified] bit NOT NULL,
    CONSTRAINT [PK_GroupPolicyObject] PRIMARY KEY ([PK_GPO])
);

GO

CREATE TABLE [Role] (
    [PK_Role] nvarchar(450) NOT NULL,
    [Name] nvarchar(64) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([PK_Role])
);

GO

CREATE TABLE [Systemhouse] (
    [PK_Systemhouse] nvarchar(450) NOT NULL,
    [Name] nvarchar(max),
    CONSTRAINT [PK_Systemhouse] PRIMARY KEY ([PK_Systemhouse])
);

GO

CREATE TABLE [Customer] (
    [PK_Customer] nvarchar(450) NOT NULL,
    [Name] nvarchar(max),
    [FK_Systemhouse] nvarchar(450),
    CONSTRAINT [PK_Customer] PRIMARY KEY ([PK_Customer]),
    CONSTRAINT [FK_Customer_Systemhouse_FK_Systemhouse] FOREIGN KEY ([FK_Systemhouse]) REFERENCES [Systemhouse] ([PK_Systemhouse]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AzureCredentials] (
    [PK_AzureCredentials] nvarchar(450) NOT NULL,
    [ClientId] nvarchar(max),
    [ClientSecret] nvarchar(max),
    [FK_Customer] nvarchar(450),
    [TenantId] nvarchar(max),
    CONSTRAINT [PK_AzureCredentials] PRIMARY KEY ([PK_AzureCredentials]),
    CONSTRAINT [FK_AzureCredentials_Customer_FK_Customer] FOREIGN KEY ([FK_Customer]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Domain] (
    [PK_Domain] nvarchar(450) NOT NULL,
    [FK_Customer] nvarchar(450),
    [GpoId] nvarchar(450),
    [Name] nvarchar(max),
    [Status] nvarchar(max),
    [Tld] nvarchar(max),
    CONSTRAINT [PK_Domain] PRIMARY KEY ([PK_Domain]),
    CONSTRAINT [FK_Domain_Customer_FK_Customer] FOREIGN KEY ([FK_Customer]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Domain_GroupPolicyObject_GpoId] FOREIGN KEY ([GpoId]) REFERENCES [GroupPolicyObject] ([PK_GPO]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Subscription] (
    [PK_Subscription] nvarchar(450) NOT NULL,
    [FK_Customer] nvarchar(450),
    [SubscriptionId] nvarchar(max),
    CONSTRAINT [PK_Subscription] PRIMARY KEY ([PK_Subscription]),
    CONSTRAINT [FK_Subscription_Customer_FK_Customer] FOREIGN KEY ([FK_Customer]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION
);

GO

CREATE TABLE [User] (
    [PK_User] nvarchar(450) NOT NULL,
    [Active] bit NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [FK_Customer] nvarchar(450),
    [DeletedByUserId] nvarchar(450),
    [DeletedDate] datetime2,
    [Email] nvarchar(64) NOT NULL,
    [Login] nvarchar(64) NOT NULL,
    [Password] nvarchar(256) NOT NULL,
    [FK_Systemhouse] nvarchar(450) NOT NULL,
    [UpdatedByUserId] nvarchar(450),
    [UpdatedDate] datetime2 NOT NULL,
    [UserName] nvarchar(64) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([PK_User]),
    CONSTRAINT [FK_User_Customer_FK_Customer] FOREIGN KEY ([FK_Customer]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION,
    CONSTRAINT [FK_User_User_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION,
    CONSTRAINT [FK_User_Systemhouse_FK_Systemhouse] FOREIGN KEY ([FK_Systemhouse]) REFERENCES [Systemhouse] ([PK_Systemhouse]) ON DELETE CASCADE,
    CONSTRAINT [FK_User_User_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION
);

GO

CREATE TABLE [DomainUser] (
    [PK_DomainUser] nvarchar(450) NOT NULL,
    [DomainId] nvarchar(450),
    [Email] nvarchar(max),
    [FirstName] nvarchar(max),
    [LastName] nvarchar(max),
    [Password] nvarchar(max),
    [UserName] nvarchar(max),
    CONSTRAINT [PK_DomainUser] PRIMARY KEY ([PK_DomainUser]),
    CONSTRAINT [FK_DomainUser_Domain_DomainId] FOREIGN KEY ([DomainId]) REFERENCES [Domain] ([PK_Domain]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OrganizationalUnit] (
    [PK_OrganizationalUnit] nvarchar(450) NOT NULL,
    [DomainId] nvarchar(450),
    [Name] nvarchar(max),
    [OrganizationalUnitId] nvarchar(450),
    CONSTRAINT [PK_OrganizationalUnit] PRIMARY KEY ([PK_OrganizationalUnit]),
    CONSTRAINT [FK_OrganizationalUnit_Domain_DomainId] FOREIGN KEY ([DomainId]) REFERENCES [Domain] ([PK_Domain]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrganizationalUnit_OrganizationalUnit_OrganizationalUnitId] FOREIGN KEY ([OrganizationalUnitId]) REFERENCES [OrganizationalUnit] ([PK_OrganizationalUnit]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Attachment] (
    [PK_Attachment] nvarchar(450) NOT NULL,
    [ContentType] nvarchar(256) NOT NULL,
    [CreatedByUserId] nvarchar(450),
    [CreatedDate] datetime2 NOT NULL,
    [FileName] nvarchar(256) NOT NULL,
    [FileSize] bigint NOT NULL,
    [GenFileName] nvarchar(512) NOT NULL,
    CONSTRAINT [PK_Attachment] PRIMARY KEY ([PK_Attachment]),
    CONSTRAINT [FK_Attachment_User_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Scheduler] (
    [PK_Scheduler] nvarchar(450) NOT NULL,
    [CreatedByUserId] nvarchar(450),
    [CreatedDate] datetime2 NOT NULL,
    [EndProcessDate] datetime2,
    [EntityData1] nvarchar(max),
    [EntityData2] nvarchar(max),
    [EntityData3] nvarchar(max),
    [EntityData4] nvarchar(max),
    [EntityId1] nvarchar(max),
    [EntityId2] nvarchar(max),
    [EntityId3] nvarchar(max),
    [EntityId4] nvarchar(max),
    [ErrorMessage] nvarchar(max),
    [IsSynchronous] bit NOT NULL,
    [OnDate] datetime2 NOT NULL,
    [ParentSchedulerId] nvarchar(450),
    [SchedulerActionType] int NOT NULL,
    [StartProcessDate] datetime2,
    CONSTRAINT [PK_Scheduler] PRIMARY KEY ([PK_Scheduler]),
    CONSTRAINT [FK_Scheduler_User_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Scheduler_Scheduler_ParentSchedulerId] FOREIGN KEY ([ParentSchedulerId]) REFERENCES [Scheduler] ([PK_Scheduler]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UserCustomer] (
    [Id] nvarchar(450) NOT NULL,
    [CustomerId] nvarchar(450),
    [UserId] nvarchar(450),
    CONSTRAINT [PK_UserCustomer] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserCustomer_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserCustomer_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UserForgotPassword] (
    [PK_UserForgotPassword] nvarchar(450) NOT NULL,
    [ApprovedDateTime] datetime2,
    [ApproverIpAddress] nvarchar(64),
    [CreatedDate] datetime2 NOT NULL,
    [CreatorIpAddress] nvarchar(64),
    [RequestGuid] uniqueidentifier NOT NULL,
    [FK_User] nvarchar(450),
    CONSTRAINT [PK_UserForgotPassword] PRIMARY KEY ([PK_UserForgotPassword]),
    CONSTRAINT [FK_UserForgotPassword_User_FK_User] FOREIGN KEY ([FK_User]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UserRole] (
    [PK_UserRole] nvarchar(450) NOT NULL,
    [FK_Role] nvarchar(450),
    [FK_User] nvarchar(450),
    CONSTRAINT [PK_UserRole] PRIMARY KEY ([PK_UserRole]),
    CONSTRAINT [FK_UserRole_Role_FK_Role] FOREIGN KEY ([FK_Role]) REFERENCES [Role] ([PK_Role]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRole_User_FK_User] FOREIGN KEY ([FK_User]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UserSubscription] (
    [Id] nvarchar(450) NOT NULL,
    [SubscriptionId] nvarchar(450),
    [UserId] nvarchar(450),
    CONSTRAINT [PK_UserSubscription] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserSubscription_Subscription_SubscriptionId] FOREIGN KEY ([SubscriptionId]) REFERENCES [Subscription] ([PK_Subscription]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserSubscription_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION
);

GO

CREATE TABLE [NotificationEmail] (
    [PK_NotificationEmail] nvarchar(450) NOT NULL,
    [AttemptsCount] int NOT NULL,
    [Body] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [LastAttemptDate] datetime2,
    [LastAttemptError] nvarchar(max),
    [ProcessedDate] datetime2,
    [FK_Scheduler] nvarchar(450),
    [Subject] nvarchar(1024) NOT NULL,
    [ToBccEmailAddresses] nvarchar(max),
    [ToCcEmailAddresses] nvarchar(max),
    [ToEmailAddresses] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_NotificationEmail] PRIMARY KEY ([PK_NotificationEmail]),
    CONSTRAINT [FK_NotificationEmail_Scheduler_FK_Scheduler] FOREIGN KEY ([FK_Scheduler]) REFERENCES [Scheduler] ([PK_Scheduler]) ON DELETE NO ACTION
);

GO

CREATE TABLE [NotificationEmailAttachment] (
    [PK_NotificationEmailAttachment] nvarchar(450) NOT NULL,
    [FK_Attachment] nvarchar(450),
    [FK_Email] nvarchar(450),
    CONSTRAINT [PK_NotificationEmailAttachment] PRIMARY KEY ([PK_NotificationEmailAttachment]),
    CONSTRAINT [FK_NotificationEmailAttachment_Attachment_FK_Attachment] FOREIGN KEY ([FK_Attachment]) REFERENCES [Attachment] ([PK_Attachment]) ON DELETE NO ACTION,
    CONSTRAINT [FK_NotificationEmailAttachment_NotificationEmail_FK_Email] FOREIGN KEY ([FK_Email]) REFERENCES [NotificationEmail] ([PK_NotificationEmail]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Attachment_CreatedByUserId] ON [Attachment] ([CreatedByUserId]);

GO

CREATE UNIQUE INDEX [IX_AzureCredentials_FK_Customer] ON [AzureCredentials] ([FK_Customer]) WHERE [FK_Customer] IS NOT NULL;

GO

CREATE INDEX [IX_Customer_FK_Systemhouse] ON [Customer] ([FK_Systemhouse]);

GO

CREATE INDEX [IX_Domain_FK_Customer] ON [Domain] ([FK_Customer]);

GO

CREATE INDEX [IX_Domain_GpoId] ON [Domain] ([GpoId]);

GO

CREATE INDEX [IX_DomainUser_DomainId] ON [DomainUser] ([DomainId]);

GO

CREATE INDEX [IX_NotificationEmail_FK_Scheduler] ON [NotificationEmail] ([FK_Scheduler]);

GO

CREATE INDEX [IX_NotificationEmailAttachment_FK_Attachment] ON [NotificationEmailAttachment] ([FK_Attachment]);

GO

CREATE INDEX [IX_NotificationEmailAttachment_FK_Email] ON [NotificationEmailAttachment] ([FK_Email]);

GO

CREATE INDEX [IX_OrganizationalUnit_DomainId] ON [OrganizationalUnit] ([DomainId]);

GO

CREATE INDEX [IX_OrganizationalUnit_OrganizationalUnitId] ON [OrganizationalUnit] ([OrganizationalUnitId]);

GO

CREATE INDEX [IX_Scheduler_CreatedByUserId] ON [Scheduler] ([CreatedByUserId]);

GO

CREATE INDEX [IX_Scheduler_ParentSchedulerId] ON [Scheduler] ([ParentSchedulerId]);

GO

CREATE INDEX [IX_Subscription_FK_Customer] ON [Subscription] ([FK_Customer]);

GO

CREATE INDEX [IX_User_FK_Customer] ON [User] ([FK_Customer]);

GO

CREATE INDEX [IX_User_DeletedByUserId] ON [User] ([DeletedByUserId]);

GO

CREATE INDEX [IX_User_FK_Systemhouse] ON [User] ([FK_Systemhouse]);

GO

CREATE INDEX [IX_User_UpdatedByUserId] ON [User] ([UpdatedByUserId]);

GO

CREATE INDEX [IX_UserCustomer_CustomerId] ON [UserCustomer] ([CustomerId]);

GO

CREATE INDEX [IX_UserCustomer_UserId] ON [UserCustomer] ([UserId]);

GO

CREATE INDEX [IX_UserForgotPassword_FK_User] ON [UserForgotPassword] ([FK_User]);

GO

CREATE INDEX [IX_UserRole_FK_Role] ON [UserRole] ([FK_Role]);

GO

CREATE INDEX [IX_UserRole_FK_User] ON [UserRole] ([FK_User]);

GO

CREATE INDEX [IX_UserSubscription_SubscriptionId] ON [UserSubscription] ([SubscriptionId]);

GO

CREATE INDEX [IX_UserSubscription_UserId] ON [UserSubscription] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170712140933_00_Initial', N'1.1.2');

GO

