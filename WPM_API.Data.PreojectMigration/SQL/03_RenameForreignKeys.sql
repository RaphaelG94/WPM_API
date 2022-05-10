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
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Systemhouse] PRIMARY KEY ([PK_Systemhouse])
);

GO

CREATE TABLE [Customer] (
    [PK_Customer] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NULL,
    [FK_Systemhouse] nvarchar(450) NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY ([PK_Customer]),
    CONSTRAINT [FK_Customer_Systemhouse_FK_Systemhouse] FOREIGN KEY ([FK_Systemhouse]) REFERENCES [Systemhouse] ([PK_Systemhouse]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AzureCredentials] (
    [PK_AzureCredentials] nvarchar(450) NOT NULL,
    [ClientId] nvarchar(max) NULL,
    [ClientSecret] nvarchar(max) NULL,
    [FK_Customer] nvarchar(450) NULL,
    [TenantId] nvarchar(max) NULL,
    CONSTRAINT [PK_AzureCredentials] PRIMARY KEY ([PK_AzureCredentials]),
    CONSTRAINT [FK_AzureCredentials_Customer_FK_Customer] FOREIGN KEY ([FK_Customer]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Domain] (
    [PK_Domain] nvarchar(450) NOT NULL,
    [FK_Customer] nvarchar(450) NULL,
    [GpoId] nvarchar(450) NULL,
    [Name] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [Tld] nvarchar(max) NULL,
    CONSTRAINT [PK_Domain] PRIMARY KEY ([PK_Domain]),
    CONSTRAINT [FK_Domain_Customer_FK_Customer] FOREIGN KEY ([FK_Customer]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Domain_GroupPolicyObject_GpoId] FOREIGN KEY ([GpoId]) REFERENCES [GroupPolicyObject] ([PK_GPO]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Subscription] (
    [PK_Subscription] nvarchar(450) NOT NULL,
    [FK_Customer] nvarchar(450) NULL,
    [SubscriptionId] nvarchar(max) NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY ([PK_Subscription]),
    CONSTRAINT [FK_Subscription_Customer_FK_Customer] FOREIGN KEY ([FK_Customer]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION
);

GO

CREATE TABLE [User] (
    [PK_User] nvarchar(450) NOT NULL,
    [Active] bit NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [FK_Customer] nvarchar(450) NULL,
    [DeletedByUserId] nvarchar(450) NULL,
    [DeletedDate] datetime2 NULL,
    [Email] nvarchar(64) NOT NULL,
    [Login] nvarchar(64) NOT NULL,
    [Password] nvarchar(256) NOT NULL,
    [FK_Systemhouse] nvarchar(450) NOT NULL,
    [UpdatedByUserId] nvarchar(450) NULL,
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
    [DomainId] nvarchar(450) NULL,
    [Email] nvarchar(max) NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [Password] nvarchar(max) NULL,
    [UserName] nvarchar(max) NULL,
    CONSTRAINT [PK_DomainUser] PRIMARY KEY ([PK_DomainUser]),
    CONSTRAINT [FK_DomainUser_Domain_DomainId] FOREIGN KEY ([DomainId]) REFERENCES [Domain] ([PK_Domain]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OrganizationalUnit] (
    [PK_OrganizationalUnit] nvarchar(450) NOT NULL,
    [DomainId] nvarchar(450) NULL,
    [Name] nvarchar(max) NULL,
    [OrganizationalUnitId] nvarchar(450) NULL,
    CONSTRAINT [PK_OrganizationalUnit] PRIMARY KEY ([PK_OrganizationalUnit]),
    CONSTRAINT [FK_OrganizationalUnit_Domain_DomainId] FOREIGN KEY ([DomainId]) REFERENCES [Domain] ([PK_Domain]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrganizationalUnit_OrganizationalUnit_OrganizationalUnitId] FOREIGN KEY ([OrganizationalUnitId]) REFERENCES [OrganizationalUnit] ([PK_OrganizationalUnit]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Attachment] (
    [PK_Attachment] nvarchar(450) NOT NULL,
    [ContentType] nvarchar(256) NOT NULL,
    [CreatedByUserId] nvarchar(450) NULL,
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
    [CreatedByUserId] nvarchar(450) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EndProcessDate] datetime2 NULL,
    [EntityData1] nvarchar(max) NULL,
    [EntityData2] nvarchar(max) NULL,
    [EntityData3] nvarchar(max) NULL,
    [EntityData4] nvarchar(max) NULL,
    [EntityId1] nvarchar(max) NULL,
    [EntityId2] nvarchar(max) NULL,
    [EntityId3] nvarchar(max) NULL,
    [EntityId4] nvarchar(max) NULL,
    [ErrorMessage] nvarchar(max) NULL,
    [IsSynchronous] bit NOT NULL,
    [OnDate] datetime2 NOT NULL,
    [ParentSchedulerId] nvarchar(450) NULL,
    [SchedulerActionType] int NOT NULL,
    [StartProcessDate] datetime2 NULL,
    CONSTRAINT [PK_Scheduler] PRIMARY KEY ([PK_Scheduler]),
    CONSTRAINT [FK_Scheduler_User_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Scheduler_Scheduler_ParentSchedulerId] FOREIGN KEY ([ParentSchedulerId]) REFERENCES [Scheduler] ([PK_Scheduler]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UserCustomer] (
    [Id] nvarchar(450) NOT NULL,
    [CustomerId] nvarchar(450) NULL,
    [UserId] nvarchar(450) NULL,
    CONSTRAINT [PK_UserCustomer] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserCustomer_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserCustomer_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UserForgotPassword] (
    [PK_UserForgotPassword] nvarchar(450) NOT NULL,
    [ApprovedDateTime] datetime2 NULL,
    [ApproverIpAddress] nvarchar(64) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatorIpAddress] nvarchar(64) NULL,
    [RequestGuid] uniqueidentifier NOT NULL,
    [FK_User] nvarchar(450) NULL,
    CONSTRAINT [PK_UserForgotPassword] PRIMARY KEY ([PK_UserForgotPassword]),
    CONSTRAINT [FK_UserForgotPassword_User_FK_User] FOREIGN KEY ([FK_User]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UserRole] (
    [PK_UserRole] nvarchar(450) NOT NULL,
    [FK_Role] nvarchar(450) NULL,
    [FK_User] nvarchar(450) NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY ([PK_UserRole]),
    CONSTRAINT [FK_UserRole_Role_FK_Role] FOREIGN KEY ([FK_Role]) REFERENCES [Role] ([PK_Role]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRole_User_FK_User] FOREIGN KEY ([FK_User]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UserSubscription] (
    [Id] nvarchar(450) NOT NULL,
    [SubscriptionId] nvarchar(450) NULL,
    [UserId] nvarchar(450) NULL,
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
    [LastAttemptDate] datetime2 NULL,
    [LastAttemptError] nvarchar(max) NULL,
    [ProcessedDate] datetime2 NULL,
    [FK_Scheduler] nvarchar(450) NULL,
    [Subject] nvarchar(1024) NOT NULL,
    [ToBccEmailAddresses] nvarchar(max) NULL,
    [ToCcEmailAddresses] nvarchar(max) NULL,
    [ToEmailAddresses] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_NotificationEmail] PRIMARY KEY ([PK_NotificationEmail]),
    CONSTRAINT [FK_NotificationEmail_Scheduler_FK_Scheduler] FOREIGN KEY ([FK_Scheduler]) REFERENCES [Scheduler] ([PK_Scheduler]) ON DELETE NO ACTION
);

GO

CREATE TABLE [NotificationEmailAttachment] (
    [PK_NotificationEmailAttachment] nvarchar(450) NOT NULL,
    [FK_Attachment] nvarchar(450) NULL,
    [FK_Email] nvarchar(450) NULL,
    CONSTRAINT [PK_NotificationEmailAttachment] PRIMARY KEY ([PK_NotificationEmailAttachment]),
    CONSTRAINT [FK_NotificationEmailAttachment_Attachment_FK_Attachment] FOREIGN KEY ([FK_Attachment]) REFERENCES [Attachment] ([PK_Attachment]) ON DELETE NO ACTION,
    CONSTRAINT [FK_NotificationEmailAttachment_NotificationEmail_FK_Email] FOREIGN KEY ([FK_Email]) REFERENCES [NotificationEmail] ([PK_NotificationEmail]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Attachment_CreatedByUserId] ON [Attachment] ([CreatedByUserId]);

GO

CREATE UNIQUE INDEX [IX_AzureCredentials_FK_Customer] ON [AzureCredentials] ([FK_Customer]);

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
VALUES (N'20170712140933_00_Initial', N'2.0.0-rtm-26452');

GO

ALTER TABLE [User] DROP CONSTRAINT [FK_User_User_DeletedByUserId];

GO

ALTER TABLE [User] DROP CONSTRAINT [FK_User_User_UpdatedByUserId];

GO

DROP INDEX [IX_User_DeletedByUserId] ON [User];

GO

DROP INDEX [IX_User_UpdatedByUserId] ON [User];

GO

DROP INDEX [IX_AzureCredentials_FK_Customer] ON [AzureCredentials];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'User') AND [c].[name] = N'UpdatedByUserId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [User] ALTER COLUMN [UpdatedByUserId] nvarchar(max) NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'User') AND [c].[name] = N'DeletedByUserId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [User] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [User] ALTER COLUMN [DeletedByUserId] nvarchar(max) NULL;

GO

ALTER TABLE [Domain] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.000';

GO

ALTER TABLE [Domain] ADD [DeletedByUserId] nvarchar(max) NULL;

GO

ALTER TABLE [Domain] ADD [DeletedDate] datetime2 NULL;

GO

ALTER TABLE [Domain] ADD [UpdatedByUserId] nvarchar(max) NULL;

GO

ALTER TABLE [Domain] ADD [UpdatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.000';

GO

CREATE TABLE [ResourceGroup] (
    [PK_ResourceGroup] nvarchar(450) NOT NULL,
    [Location] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_ResourceGroup] PRIMARY KEY ([PK_ResourceGroup])
);

GO

CREATE TABLE [StorageAccount] (
    [PK_StorageAccount] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NULL,
    [Type] nvarchar(max) NULL,
    CONSTRAINT [PK_StorageAccount] PRIMARY KEY ([PK_StorageAccount])
);

GO

CREATE TABLE [VirtualNetwork] (
    [PK_VirtualNetwork] nvarchar(450) NOT NULL,
    [AddressRange] nvarchar(max) NULL,
    [Dns] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_VirtualNetwork] PRIMARY KEY ([PK_VirtualNetwork])
);

GO

CREATE TABLE [Vpn] (
    [PK_Vpn] nvarchar(450) NOT NULL,
    [LocalAddressRange] nvarchar(max) NULL,
    [LocalPublicIp] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [VirtualNetwork] nvarchar(max) NULL,
    [VirtualPublicIp] nvarchar(max) NULL,
    CONSTRAINT [PK_Vpn] PRIMARY KEY ([PK_Vpn])
);

GO

CREATE TABLE [Subnet] (
    [PK_VirtualNetwork] nvarchar(450) NOT NULL,
    [AddressRange] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [VirtualNetworkId] nvarchar(450) NULL,
    CONSTRAINT [PK_Subnet] PRIMARY KEY ([PK_VirtualNetwork]),
    CONSTRAINT [FK_Subnet_VirtualNetwork_VirtualNetworkId] FOREIGN KEY ([VirtualNetworkId]) REFERENCES [VirtualNetwork] ([PK_VirtualNetwork]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Base] (
    [PK_Base] nvarchar(450) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [FK_Credentials] nvarchar(450) NULL,
    [DeletedByUserId] nvarchar(max) NULL,
    [DeletedDate] datetime2 NULL,
    [Name] nvarchar(max) NULL,
    [ResourceGroupId] nvarchar(450) NULL,
    [StorageAccountId] nvarchar(450) NULL,
    [SubscriptionId] nvarchar(450) NULL,
    [UpdatedByUserId] nvarchar(max) NULL,
    [UpdatedDate] datetime2 NOT NULL,
    [VirtualNetworkId] nvarchar(450) NULL,
    [VpnId] nvarchar(450) NULL,
    CONSTRAINT [PK_Base] PRIMARY KEY ([PK_Base]),
    CONSTRAINT [FK_Base_AzureCredentials_FK_Credentials] FOREIGN KEY ([FK_Credentials]) REFERENCES [AzureCredentials] ([PK_AzureCredentials]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Base_ResourceGroup_ResourceGroupId] FOREIGN KEY ([ResourceGroupId]) REFERENCES [ResourceGroup] ([PK_ResourceGroup]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Base_StorageAccount_StorageAccountId] FOREIGN KEY ([StorageAccountId]) REFERENCES [StorageAccount] ([PK_StorageAccount]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Base_Subscription_SubscriptionId] FOREIGN KEY ([SubscriptionId]) REFERENCES [Subscription] ([PK_Subscription]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Base_VirtualNetwork_VirtualNetworkId] FOREIGN KEY ([VirtualNetworkId]) REFERENCES [VirtualNetwork] ([PK_VirtualNetwork]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Base_Vpn_VpnId] FOREIGN KEY ([VpnId]) REFERENCES [Vpn] ([PK_Vpn]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Parameter] (
    [PK_Parameter] nvarchar(450) NOT NULL,
    [BaseId] nvarchar(450) NULL,
    [Key] nvarchar(max) NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_Parameter] PRIMARY KEY ([PK_Parameter]),
    CONSTRAINT [FK_Parameter_Base_BaseId] FOREIGN KEY ([BaseId]) REFERENCES [Base] ([PK_Base]) ON DELETE NO ACTION
);

GO

CREATE TABLE [VirtualMachine] (
    [PK_VirtualMachine] nvarchar(450) NOT NULL,
    [AdminUserName] nvarchar(max) NULL,
    [AdminUserPassword] nvarchar(max) NULL,
    [AzureId] nvarchar(max) NULL,
    [BaseId] nvarchar(450) NULL,
    [Location] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [OperatingSystem] nvarchar(max) NULL,
    [SystemDiskId] nvarchar(450) NULL,
    [Type] nvarchar(max) NULL,
    CONSTRAINT [PK_VirtualMachine] PRIMARY KEY ([PK_VirtualMachine]),
    CONSTRAINT [FK_VirtualMachine_Base_BaseId] FOREIGN KEY ([BaseId]) REFERENCES [Base] ([PK_Base]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Disk] (
    [PK_VirtualMachine] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NULL,
    [SizeInGb] int NOT NULL,
    [VirtualMachineId] nvarchar(450) NULL,
    CONSTRAINT [PK_Disk] PRIMARY KEY ([PK_VirtualMachine]),
    CONSTRAINT [FK_Disk_VirtualMachine_VirtualMachineId] FOREIGN KEY ([VirtualMachineId]) REFERENCES [VirtualMachine] ([PK_VirtualMachine]) ON DELETE NO ACTION
);

GO

CREATE UNIQUE INDEX [IX_AzureCredentials_FK_Customer] ON [AzureCredentials] ([FK_Customer]) WHERE [FK_Customer] IS NOT NULL;

GO

CREATE INDEX [IX_Base_FK_Credentials] ON [Base] ([FK_Credentials]);

GO

CREATE INDEX [IX_Base_ResourceGroupId] ON [Base] ([ResourceGroupId]);

GO

CREATE INDEX [IX_Base_StorageAccountId] ON [Base] ([StorageAccountId]);

GO

CREATE INDEX [IX_Base_SubscriptionId] ON [Base] ([SubscriptionId]);

GO

CREATE INDEX [IX_Base_VirtualNetworkId] ON [Base] ([VirtualNetworkId]);

GO

CREATE INDEX [IX_Base_VpnId] ON [Base] ([VpnId]);

GO

CREATE INDEX [IX_Disk_VirtualMachineId] ON [Disk] ([VirtualMachineId]);

GO

CREATE INDEX [IX_Parameter_BaseId] ON [Parameter] ([BaseId]);

GO

CREATE INDEX [IX_Subnet_VirtualNetworkId] ON [Subnet] ([VirtualNetworkId]);

GO

CREATE INDEX [IX_VirtualMachine_BaseId] ON [VirtualMachine] ([BaseId]);

GO

CREATE INDEX [IX_VirtualMachine_SystemDiskId] ON [VirtualMachine] ([SystemDiskId]);

GO

ALTER TABLE [VirtualMachine] ADD CONSTRAINT [FK_VirtualMachine_Disk_SystemDiskId] FOREIGN KEY ([SystemDiskId]) REFERENCES [Disk] ([PK_VirtualMachine]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170825111231_01_BaseDomainAzureModels', N'2.0.0-rtm-26452');

GO

ALTER TABLE [AzureCredentials] DROP CONSTRAINT [FK_AzureCredentials_Customer_FK_Customer];

GO

ALTER TABLE [Base] DROP CONSTRAINT [FK_Base_AzureCredentials_FK_Credentials];

GO

ALTER TABLE [Customer] DROP CONSTRAINT [FK_Customer_Systemhouse_FK_Systemhouse];

GO

ALTER TABLE [Domain] DROP CONSTRAINT [FK_Domain_Customer_FK_Customer];

GO

ALTER TABLE [Subscription] DROP CONSTRAINT [FK_Subscription_Customer_FK_Customer];

GO

DROP INDEX [IX_Subscription_FK_Customer] ON [Subscription];

GO

DROP INDEX [IX_Domain_FK_Customer] ON [Domain];

GO

DROP INDEX [IX_Customer_FK_Systemhouse] ON [Customer];

GO

DROP INDEX [IX_Base_FK_Credentials] ON [Base];

GO

DROP INDEX [IX_AzureCredentials_FK_Customer] ON [AzureCredentials];

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Subscription') AND [c].[name] = N'FK_Customer');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Subscription] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Subscription] DROP COLUMN [FK_Customer];

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Domain') AND [c].[name] = N'FK_Customer');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Domain] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Domain] DROP COLUMN [FK_Customer];

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Customer') AND [c].[name] = N'FK_Systemhouse');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Customer] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Customer] DROP COLUMN [FK_Systemhouse];

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Base') AND [c].[name] = N'FK_Credentials');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Base] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Base] DROP COLUMN [FK_Credentials];

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'AzureCredentials') AND [c].[name] = N'FK_Customer');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [AzureCredentials] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [AzureCredentials] DROP COLUMN [FK_Customer];

GO

EXEC sp_rename N'AzureCredentials.PK_AzureCredentials', N'Id', N'COLUMN';

GO

ALTER TABLE [Subscription] ADD [CustomerId] nvarchar(450) NULL;

GO

ALTER TABLE [Subscription] ADD [Name] nvarchar(max) NULL;

GO

ALTER TABLE [Domain] ADD [CustomerId] nvarchar(450) NULL;

GO

ALTER TABLE [Customer] ADD [SystemhouseId] nvarchar(450) NULL;

GO

ALTER TABLE [Base] ADD [CredentialsId] nvarchar(450) NULL;

GO

ALTER TABLE [AzureCredentials] ADD [CustomerId] nvarchar(450) NULL;

GO

CREATE INDEX [IX_Subscription_CustomerId] ON [Subscription] ([CustomerId]);

GO

CREATE INDEX [IX_Domain_CustomerId] ON [Domain] ([CustomerId]);

GO

CREATE INDEX [IX_Customer_SystemhouseId] ON [Customer] ([SystemhouseId]);

GO

CREATE INDEX [IX_Base_CredentialsId] ON [Base] ([CredentialsId]);

GO

CREATE UNIQUE INDEX [IX_AzureCredentials_CustomerId] ON [AzureCredentials] ([CustomerId]) WHERE [CustomerId] IS NOT NULL;

GO

ALTER TABLE [AzureCredentials] ADD CONSTRAINT [FK_AzureCredentials_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION;

GO

ALTER TABLE [Base] ADD CONSTRAINT [FK_Base_AzureCredentials_CredentialsId] FOREIGN KEY ([CredentialsId]) REFERENCES [AzureCredentials] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Customer] ADD CONSTRAINT [FK_Customer_Systemhouse_SystemhouseId] FOREIGN KEY ([SystemhouseId]) REFERENCES [Systemhouse] ([PK_Systemhouse]) ON DELETE NO ACTION;

GO

ALTER TABLE [Domain] ADD CONSTRAINT [FK_Domain_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION;

GO

ALTER TABLE [Subscription] ADD CONSTRAINT [FK_Subscription_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170825143950_02_RenameTablesAndForreignKeys', N'2.0.0-rtm-26452');

GO

ALTER TABLE [NotificationEmail] DROP CONSTRAINT [FK_NotificationEmail_Scheduler_FK_Scheduler];

GO

ALTER TABLE [NotificationEmailAttachment] DROP CONSTRAINT [FK_NotificationEmailAttachment_Attachment_FK_Attachment];

GO

ALTER TABLE [NotificationEmailAttachment] DROP CONSTRAINT [FK_NotificationEmailAttachment_NotificationEmail_FK_Email];

GO

ALTER TABLE [User] DROP CONSTRAINT [FK_User_Customer_FK_Customer];

GO

ALTER TABLE [User] DROP CONSTRAINT [FK_User_Systemhouse_FK_Systemhouse];

GO

ALTER TABLE [UserForgotPassword] DROP CONSTRAINT [FK_UserForgotPassword_User_FK_User];

GO

ALTER TABLE [UserRole] DROP CONSTRAINT [FK_UserRole_Role_FK_Role];

GO

ALTER TABLE [UserRole] DROP CONSTRAINT [FK_UserRole_User_FK_User];

GO

EXEC sp_rename N'UserRole.FK_User', N'UserId', N'COLUMN';

GO

EXEC sp_rename N'UserRole.FK_Role', N'RoleId', N'COLUMN';

GO

EXEC sp_rename N'UserRole.IX_UserRole_FK_User', N'IX_UserRole_UserId', N'INDEX';

GO

EXEC sp_rename N'UserRole.IX_UserRole_FK_Role', N'IX_UserRole_RoleId', N'INDEX';

GO

EXEC sp_rename N'UserForgotPassword.FK_User', N'UserId', N'COLUMN';

GO

EXEC sp_rename N'UserForgotPassword.IX_UserForgotPassword_FK_User', N'IX_UserForgotPassword_UserId', N'INDEX';

GO

EXEC sp_rename N'User.FK_Systemhouse', N'SystemhouseId', N'COLUMN';

GO

EXEC sp_rename N'User.FK_Customer', N'CustomerId', N'COLUMN';

GO

EXEC sp_rename N'User.IX_User_FK_Systemhouse', N'IX_User_SystemhouseId', N'INDEX';

GO

EXEC sp_rename N'User.IX_User_FK_Customer', N'IX_User_CustomerId', N'INDEX';

GO

EXEC sp_rename N'NotificationEmailAttachment.FK_Email', N'NotificationEmailId', N'COLUMN';

GO

EXEC sp_rename N'NotificationEmailAttachment.FK_Attachment', N'AttachmentId', N'COLUMN';

GO

EXEC sp_rename N'NotificationEmailAttachment.IX_NotificationEmailAttachment_FK_Email', N'IX_NotificationEmailAttachment_NotificationEmailId', N'INDEX';

GO

EXEC sp_rename N'NotificationEmailAttachment.IX_NotificationEmailAttachment_FK_Attachment', N'IX_NotificationEmailAttachment_AttachmentId', N'INDEX';

GO

EXEC sp_rename N'NotificationEmail.FK_Scheduler', N'SchedulerId', N'COLUMN';

GO

EXEC sp_rename N'NotificationEmail.IX_NotificationEmail_FK_Scheduler', N'IX_NotificationEmail_SchedulerId', N'INDEX';

GO

ALTER TABLE [NotificationEmail] ADD CONSTRAINT [FK_NotificationEmail_Scheduler_SchedulerId] FOREIGN KEY ([SchedulerId]) REFERENCES [Scheduler] ([PK_Scheduler]) ON DELETE NO ACTION;

GO

ALTER TABLE [NotificationEmailAttachment] ADD CONSTRAINT [FK_NotificationEmailAttachment_Attachment_AttachmentId] FOREIGN KEY ([AttachmentId]) REFERENCES [Attachment] ([PK_Attachment]) ON DELETE NO ACTION;

GO

ALTER TABLE [NotificationEmailAttachment] ADD CONSTRAINT [FK_NotificationEmailAttachment_NotificationEmail_NotificationEmailId] FOREIGN KEY ([NotificationEmailId]) REFERENCES [NotificationEmail] ([PK_NotificationEmail]) ON DELETE NO ACTION;

GO

ALTER TABLE [User] ADD CONSTRAINT [FK_User_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customer] ([PK_Customer]) ON DELETE NO ACTION;

GO

ALTER TABLE [User] ADD CONSTRAINT [FK_User_Systemhouse_SystemhouseId] FOREIGN KEY ([SystemhouseId]) REFERENCES [Systemhouse] ([PK_Systemhouse]) ON DELETE CASCADE;

GO

ALTER TABLE [UserForgotPassword] ADD CONSTRAINT [FK_UserForgotPassword_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION;

GO

ALTER TABLE [UserRole] ADD CONSTRAINT [FK_UserRole_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([PK_Role]) ON DELETE NO ACTION;

GO

ALTER TABLE [UserRole] ADD CONSTRAINT [FK_UserRole_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([PK_User]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170825153757_03_RenameForreignKeys', N'2.0.0-rtm-26452');

GO

