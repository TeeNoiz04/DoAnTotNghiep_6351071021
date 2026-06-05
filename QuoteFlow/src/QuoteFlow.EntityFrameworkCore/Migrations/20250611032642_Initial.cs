using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AbpAuditLogs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ApplicationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                TenantName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                ImpersonatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ImpersonatorUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ImpersonatorTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ImpersonatorTenantName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExecutionDuration = table.Column<int>(type: "int", nullable: false),
                ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                ClientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                ClientId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                CorrelationId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                HttpMethod = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                Exceptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Comments = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                HttpStatusCode = table.Column<int>(type: "int", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpAuditLogs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpBackgroundJobs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                JobName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                JobArgs = table.Column<string>(type: "nvarchar(max)", maxLength: 1048576, nullable: false),
                TryCount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                NextTryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastTryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsAbandoned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                Priority = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)15),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpBackgroundJobs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpBlobContainers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpBlobContainers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpClaimTypes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Required = table.Column<bool>(type: "bit", nullable: false),
                IsStatic = table.Column<bool>(type: "bit", nullable: false),
                Regex = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                RegexDescription = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ValueType = table.Column<int>(type: "int", nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpClaimTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpFeatureGroups",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpFeatureGroups", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpFeatures",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GroupName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                ParentName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                DefaultValue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                IsVisibleToClients = table.Column<bool>(type: "bit", nullable: false),
                IsAvailableToHost = table.Column<bool>(type: "bit", nullable: false),
                AllowedProviders = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ValueType = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpFeatures", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpFeatureValues",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Value = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                ProviderName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpFeatureValues", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpLanguages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CultureName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                UiCultureName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpLanguages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpLanguageTexts",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ResourceName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                CultureName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", maxLength: 65536, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpLanguageTexts", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpLinkUsers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SourceUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SourceTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                TargetUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TargetTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpLinkUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpLocalizationResources",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                DefaultCulture = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                BaseResources = table.Column<string>(type: "nvarchar(1280)", maxLength: 1280, nullable: true),
                SupportedCultures = table.Column<string>(type: "nvarchar(640)", maxLength: 640, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpLocalizationResources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpLocalizationTexts",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ResourceName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                CultureName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", maxLength: 1048576, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpLocalizationTexts", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpOrganizationUnits",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                EntityVersion = table.Column<int>(type: "int", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpOrganizationUnits", x => x.Id);
                table.ForeignKey(
                    name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                    column: x => x.ParentId,
                    principalTable: "AbpOrganizationUnits",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "AbpPermissionGrants",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                ProviderName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpPermissionGrants", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpPermissionGroups",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpPermissionGroups", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpPermissions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GroupName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                ParentName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                MultiTenancySide = table.Column<byte>(type: "tinyint", nullable: false),
                Providers = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                StateCheckers = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpPermissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpRoles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                IsDefault = table.Column<bool>(type: "bit", nullable: false),
                IsStatic = table.Column<bool>(type: "bit", nullable: false),
                IsPublic = table.Column<bool>(type: "bit", nullable: false),
                EntityVersion = table.Column<int>(type: "int", nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpSecurityLogs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ApplicationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                Identity = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                Action = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                TenantName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                ClientId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                CorrelationId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpSecurityLogs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpSessions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SessionId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Device = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                DeviceInfo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClientId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                IpAddresses = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                SignedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpSessions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpSettingDefinitions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                DefaultValue = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                IsVisibleToClients = table.Column<bool>(type: "bit", nullable: false),
                Providers = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                IsInherited = table.Column<bool>(type: "bit", nullable: false),
                IsEncrypted = table.Column<bool>(type: "bit", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpSettingDefinitions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpSettings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Value = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                ProviderName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpSettings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpTextTemplateContents",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                CultureName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                Content = table.Column<string>(type: "nvarchar(max)", maxLength: 65535, nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpTextTemplateContents", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpTextTemplateDefinitionRecords",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                IsLayout = table.Column<bool>(type: "bit", nullable: false),
                Layout = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                LocalizationResourceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                IsInlineLocalized = table.Column<bool>(type: "bit", nullable: false),
                DefaultCultureName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                RenderEngine = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpTextTemplateDefinitionRecords", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpUserDelegations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SourceUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TargetUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpUserDelegations", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpUsers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                Surname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                IsExternal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                ShouldChangePasswordOnNextLogin = table.Column<bool>(type: "bit", nullable: false),
                EntityVersion = table.Column<int>(type: "int", nullable: false),
                LastPasswordChangeTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Cargo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cargo", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CargoData",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CargoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PODetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PODetailCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                GoflaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Model = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                PORef = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                SRNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Classification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Product = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Spec1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Spec2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Spec3 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                OrderQty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ExWorkQty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                NonExWorkQty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                InSTCH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Shipped = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                WaitForShip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ShipDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                InSTCHDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                ShipmentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ETA1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                ETA2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                MEVNRequest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                STCReply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EU = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                MEVNAddedRequest = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                NPD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                PlannedShipment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                SODate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CellMarker = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                ShipmentForm = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CargoData", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Customer",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CustomerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Phone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                NationalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DistributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                KeyAccountCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                KeyAccountShortName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                KeyAccountName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                KeyAccountTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                KeyAccountClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                MEVNSalePIC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                IndustryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IndustryCode = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                TargetEU = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                CurrentSaleRoute = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                LastPODate = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                PersonInCharge = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                JobTitle = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                RegisteredKeyAccount = table.Column<bool>(type: "bit", nullable: false),
                RegistrationYear = table.Column<int>(type: "int", nullable: true),
                RegisterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                TargetEndUsers = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                LastRegisteredProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                IsDeactive = table.Column<bool>(type: "bit", nullable: false),
                CurrentApprovalRouteInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CurrentApprovalStepSequence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CurrentApproverRoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customer", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Distributors",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                ContactInfo = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                PriceColumn = table.Column<short>(type: "smallint", nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                IsDeactive = table.Column<bool>(type: "bit", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Distributors", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "FmDirectoryDescriptors",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FmDirectoryDescriptors", x => x.Id);
                table.ForeignKey(
                    name: "FK_FmDirectoryDescriptors_FmDirectoryDescriptors_ParentId",
                    column: x => x.ParentId,
                    principalTable: "FmDirectoryDescriptors",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "GdprRequests",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                ReadyTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GdprRequests", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "KeyAccount",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DistributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                KeyAccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                KeyAccountShortName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                KeyAccountName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                KeyAccountTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                KeyAccountClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                MEVNSalePIC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                NationalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Website = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                TypeOfBusiness = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                TargetEU = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                CurrentSaleRoute = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                LastPODate = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                PersonInCharge = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Phone = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Email = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                RegistrationYear = table.Column<int>(type: "int", nullable: true),
                LastRegisteredProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                JobTitle = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                RegisterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CustomerFullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                TargetEndUsers = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                CustomerLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CurrentApprovalRouteInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CurrentApproverRoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CurrentApproverRoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CurrentApprovalStepSequence = table.Column<int>(type: "int", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KeyAccount", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "KeyAccount_Evaluations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                KeyAccount_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EvaluationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                EvaluationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Distributor_Info1 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Distributor_Info2 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                MEVN_Info1 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                MEVN_Info2 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Competitor_Info1 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Competitor_Info2 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KeyAccount_Evaluations", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MaterialApprovalRequest",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ImportType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                FileName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                RequestNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CurrentApprovalRouteInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CurrentApprovalStepSequence = table.Column<int>(type: "int", nullable: true),
                CurrentApproverRoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CurrentApproverRoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialApprovalRequest", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MaterialHistory",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Note = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialHistory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Materials",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GolfaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                SAP_Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Spec1 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Spec2 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Spec3 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Spec4 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Description_EN = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Description_VN = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Material_SEC_Classification = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Material_Group = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SAPMatGroup = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Product_Hierarchy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ProductHierarchyDescription = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                CountryOfOrigin = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                ReferenceLeadTime = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                WarrantyTime = table.Column<int>(type: "int", nullable: true),
                InventoryCategory = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Maxlot = table.Column<int>(type: "int", nullable: true),
                StockWarning = table.Column<int>(type: "int", nullable: true),
                VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                HS_Code = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                SupplierBUId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SupplierBUCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Factory_Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Input_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                InputCurrency = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                INCOTERMS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                EPA = table.Column<bool>(type: "bit", nullable: false),
                ImportDuty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                AppliedExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                LandedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                MaxSalesOfferPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                MaxMangerOfferPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Standard_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                SellingPrice1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                SellingPrice2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                SellingPrice3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                SellingPrice4 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                SellingPrice5 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                MaterialStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DestinationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                IndeactiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Description_Group = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Origin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Kind = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                Factory = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Vendor = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LeadTime = table.Column<int>(type: "int", nullable: true),
                RefExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Materials", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OpenIddictApplications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ApplicationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ClientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClientType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ConsentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DisplayNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                JsonWebKeySet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PostLogoutRedirectUris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RedirectUris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Requirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Settings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClientUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LogoUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OpenIddictApplications", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OpenIddictScopes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DisplayNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Resources = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OpenIddictScopes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PSI",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PSI_Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                FY = table.Column<int>(type: "int", nullable: true),
                FileName = table.Column<string>(type: "nvarchar(510)", maxLength: 510, nullable: true),
                Note = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: true),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CurrentApprovalRouteInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CurrentApprovalStepSequence = table.Column<int>(type: "int", nullable: true),
                CurrentApproverRoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CurrentApproverRoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleterUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                DeleterName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PSI", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PSI_Detail",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PSI_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MaterialGroupNo = table.Column<int>(type: "int", nullable: true),
                MaterialGroup = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                FY = table.Column<int>(type: "int", nullable: true),
                Month = table.Column<int>(type: "int", nullable: true),
                Plan = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ImportGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PSI_Detail", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "StockCategory",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                StockName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                MainStock = table.Column<bool>(type: "bit", nullable: true),
                DamagedStock = table.Column<bool>(type: "bit", nullable: true),
                SortOrder = table.Column<int>(type: "int", nullable: true),
                IsDeactive = table.Column<bool>(type: "bit", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StockCategory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "StockTracings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                ReportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StockTracings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SystemCategories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                CategoryType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                IsDeactive = table.Column<bool>(type: "bit", nullable: false),
                SortOrder = table.Column<int>(type: "int", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SystemCategories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpAuditLogActions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                AuditLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ServiceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                MethodName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                Parameters = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExecutionDuration = table.Column<int>(type: "int", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpAuditLogActions", x => x.Id);
                table.ForeignKey(
                    name: "FK_AbpAuditLogActions_AbpAuditLogs_AuditLogId",
                    column: x => x.AuditLogId,
                    principalTable: "AbpAuditLogs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpEntityChanges",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AuditLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ChangeTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                ChangeType = table.Column<byte>(type: "tinyint", nullable: false),
                EntityTenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EntityId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                EntityTypeFullName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                table.ForeignKey(
                    name: "FK_AbpEntityChanges_AbpAuditLogs_AuditLogId",
                    column: x => x.AuditLogId,
                    principalTable: "AbpAuditLogs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpBlobs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Content = table.Column<byte[]>(type: "varbinary(max)", maxLength: 2147483647, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpBlobs", x => x.Id);
                table.ForeignKey(
                    name: "FK_AbpBlobs_AbpBlobContainers_ContainerId",
                    column: x => x.ContainerId,
                    principalTable: "AbpBlobContainers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpOrganizationUnitRoles",
            columns: table => new
            {
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OrganizationUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpOrganizationUnitRoles", x => new { x.OrganizationUnitId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AbpOrganizationUnitRoles_AbpOrganizationUnits_OrganizationUnitId",
                    column: x => x.OrganizationUnitId,
                    principalTable: "AbpOrganizationUnits",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AbpOrganizationUnitRoles_AbpRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AbpRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpRoleClaims",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                ClaimValue = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AbpRoleClaims_AbpRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AbpRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpTextTemplateDefinitionContentRecords",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DefinitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                FileContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpTextTemplateDefinitionContentRecords", x => x.Id);
                table.ForeignKey(
                    name: "FK_AbpTextTemplateDefinitionContentRecords_AbpTextTemplateDefinitionRecords_DefinitionId",
                    column: x => x.DefinitionId,
                    principalTable: "AbpTextTemplateDefinitionRecords",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpUserClaims",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                ClaimValue = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AbpUserClaims_AbpUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AbpUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpUserLogins",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ProviderKey = table.Column<string>(type: "nvarchar(196)", maxLength: 196, nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpUserLogins", x => new { x.UserId, x.LoginProvider });
                table.ForeignKey(
                    name: "FK_AbpUserLogins_AbpUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AbpUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpUserOrganizationUnits",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OrganizationUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpUserOrganizationUnits", x => new { x.OrganizationUnitId, x.UserId });
                table.ForeignKey(
                    name: "FK_AbpUserOrganizationUnits_AbpOrganizationUnits_OrganizationUnitId",
                    column: x => x.OrganizationUnitId,
                    principalTable: "AbpOrganizationUnits",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AbpUserOrganizationUnits_AbpUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AbpUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpUserRoles",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AbpUserRoles_AbpRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AbpRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AbpUserRoles_AbpUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AbpUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AbpUserTokens",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AbpUserTokens_AbpUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AbpUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FmFileDescriptors",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DirectoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Size = table.Column<long>(type: "bigint", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FmFileDescriptors", x => x.Id);
                table.ForeignKey(
                    name: "FK_FmFileDescriptors_FmDirectoryDescriptors_DirectoryId",
                    column: x => x.DirectoryId,
                    principalTable: "FmDirectoryDescriptors",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "GdprInfo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Provider = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GdprInfo", x => x.Id);
                table.ForeignKey(
                    name: "FK_GdprInfo_GdprRequests_RequestId",
                    column: x => x.RequestId,
                    principalTable: "GdprRequests",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "OpenIddictAuthorizations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Scopes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OpenIddictAuthorizations", x => x.Id);
                table.ForeignKey(
                    name: "FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId",
                    column: x => x.ApplicationId,
                    principalTable: "OpenIddictApplications",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "MaterialStock",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StockCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GolfaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Qty = table.Column<int>(type: "int", nullable: true),
                Locked = table.Column<int>(type: "int", nullable: true),
                LockStockKeeping = table.Column<int>(type: "int", nullable: true),
                LockStockSO = table.Column<int>(type: "int", nullable: true),
                Available_Qty = table.Column<int>(type: "int", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialStock", x => x.Id);
                table.ForeignKey(
                    name: "FK_MaterialStock_Materials_MaterialId",
                    column: x => x.MaterialId,
                    principalTable: "Materials",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MaterialStock_StockCategory_StockCategoryId",
                    column: x => x.StockCategoryId,
                    principalTable: "StockCategory",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "StockTracingDetails",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StockTracingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ReportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RowNo = table.Column<int>(type: "int", nullable: true),
                PackingListCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CheckListCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                DateEntered = table.Column<DateTime>(type: "datetime2", nullable: true),
                Stock = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                BU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Customer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                Category = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                GIV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Invoice = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                SKUCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                SKUName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                Quality = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                Warranty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Series = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                OriginCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Location = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                GolfaCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StockTracingDetails", x => x.Id);
                table.ForeignKey(
                    name: "FK_StockTracingDetails_StockTracings_StockTracingId",
                    column: x => x.StockTracingId,
                    principalTable: "StockTracings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Buyer",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BuyerTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BuyerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ShortName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                FullName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ContactPhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                PaymentTermCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                PaymentTermDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                CreditLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                CreditExposure = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                AvailableCredit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                AppliedPrice = table.Column<int>(type: "int", nullable: true),
                Deactive = table.Column<bool>(type: "bit", nullable: true),
                Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Buyer", x => x.Id);
                table.ForeignKey(
                    name: "FK_Buyer_SystemCategories_BuyerTypeId",
                    column: x => x.BuyerTypeId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "DistributorSalePIC",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DistributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SaleUserName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                SaleFullName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                MaterialTypes = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DistributorSalePIC", x => x.Id);
                table.ForeignKey(
                    name: "FK_DistributorSalePIC_Distributors_DistributorId",
                    column: x => x.DistributorId,
                    principalTable: "Distributors",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_DistributorSalePIC_SystemCategories_LocationId",
                    column: x => x.LocationId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "MaterialApprovalRequestDetail",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MaterialApprovalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GolfaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                SAP_Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Spec1 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Spec2 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Spec3 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Spec4 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Description_EN = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Description_VN = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Material_SEC_Classification = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Material_Group = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SAPMatGroup = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Product_Hierarchy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ProductHierarchyDescription = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                CountryOfOrigin = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                ReferenceLeadTime = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                WarrantyTime = table.Column<int>(type: "int", nullable: true),
                InventoryCategory = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Maxlot = table.Column<int>(type: "int", nullable: true),
                StockWarning = table.Column<int>(type: "int", nullable: true),
                VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                HS_Code = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                SupplierBUId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SupplierBUCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Factory_Text = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                Input_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                InputCurrency = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                INCOTERMS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                EPA = table.Column<bool>(type: "bit", nullable: false),
                ImportDuty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                AppliedExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                LandedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                MaxSalesOfferPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                MaxMangerOfferPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Standard_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                SellingPrice1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                SellingPrice2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                SellingPrice3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                SellingPrice4 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                SellingPrice5 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                MaterialStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DestinationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                IndeactiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Description_Group = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Origin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Kind = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                Factory = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Vendor = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LeadTime = table.Column<int>(type: "int", nullable: true),
                RefExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialApprovalRequestDetail", x => x.Id);
                table.ForeignKey(
                    name: "FK_MaterialApprovalRequestDetail_MaterialApprovalRequest_MaterialApprovalId",
                    column: x => x.MaterialApprovalId,
                    principalTable: "MaterialApprovalRequest",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MaterialApprovalRequestDetail_SystemCategories_InputCurrency",
                    column: x => x.InputCurrency,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MaterialApprovalRequestDetail_SystemCategories_Material_Group",
                    column: x => x.Material_Group,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "AbpEntityPropertyChanges",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EntityChangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NewValue = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                OriginalValue = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                PropertyName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                PropertyTypeFullName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                table.ForeignKey(
                    name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                    column: x => x.EntityChangeId,
                    principalTable: "AbpEntityChanges",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "OpenIddictTokens",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                AuthorizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RedemptionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                ReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OpenIddictTokens", x => x.Id);
                table.ForeignKey(
                    name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                    column: x => x.ApplicationId,
                    principalTable: "OpenIddictApplications",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                    column: x => x.AuthorizationId,
                    principalTable: "OpenIddictAuthorizations",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "PriceOffer",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PriceOffer_Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DistributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Location_Old = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ProjectName = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ProjectTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EUIndustryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Application = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Province = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                DetailedAddress = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                CompetitorBrand = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                PriceGapWithCompetitor = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                DecisionRight = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                POPlannedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                UpcomingPotentialProjects = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                OtherPJInformation = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                AccountNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                KeyAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                KeyAccountTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                KeyAccountClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CurrentApprovalRouteInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CurrentApprovalStepSequence = table.Column<int>(type: "int", nullable: true),
                CurrentApproverRoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CurrentApproverRoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PriceOffer", x => x.Id);
                table.ForeignKey(
                    name: "FK_PriceOffer_Buyer_DistributorId",
                    column: x => x.DistributorId,
                    principalTable: "Buyer",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PriceOffer_KeyAccount_KeyAccountId",
                    column: x => x.KeyAccountId,
                    principalTable: "KeyAccount",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PriceOffer_SystemCategories_EUIndustryId",
                    column: x => x.EUIndustryId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PriceOffer_SystemCategories_KeyAccountClassId",
                    column: x => x.KeyAccountClassId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PriceOffer_SystemCategories_KeyAccountTypeId",
                    column: x => x.KeyAccountTypeId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PriceOffer_SystemCategories_LocationId",
                    column: x => x.LocationId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PriceOffer_SystemCategories_ProjectTypeId",
                    column: x => x.ProjectTypeId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ApprovalRoute",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                InstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                StepSequence = table.Column<int>(type: "int", nullable: false),
                Approver = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                ApproverRoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ApproverRoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                IsApproved = table.Column<bool>(type: "bit", nullable: false),
                MaterialApprovalRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PriceOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApprovalRoute", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApprovalRoute_MaterialApprovalRequest_MaterialApprovalRequestId",
                    column: x => x.MaterialApprovalRequestId,
                    principalTable: "MaterialApprovalRequest",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ApprovalRoute_PriceOffer_PriceOfferId",
                    column: x => x.PriceOfferId,
                    principalTable: "PriceOffer",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PriceOffer_Customer",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PriceOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SaleChannel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CustomerTaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CustomerName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                CustomerAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                CustomerNationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CustomerType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PriceOffer_Customer", x => x.Id);
                table.ForeignKey(
                    name: "FK_PriceOffer_Customer_Customer_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customer",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PriceOffer_Customer_PriceOffer_PriceOfferId",
                    column: x => x.PriceOfferId,
                    principalTable: "PriceOffer",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "PriceOfferDetail",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PriceOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GolfaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ModelName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                SpecialSpec1 = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                SpecialSpec2 = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                DpoUsed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                Qty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                StandardPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                StandardAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                DistributorPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                RequestedDiscountRatio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                PriceToCustomer = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                MEVNOfferPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CompetitorBrand = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                CompetitorModel = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                CompetitorPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                LandingCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                InputPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                InputCurrency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ManagerMargin = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                PriceOfferMargin = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                AccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ImportGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PriceOfferDetail", x => x.Id);
                table.ForeignKey(
                    name: "FK_PriceOfferDetail_PriceOffer_PriceOfferId",
                    column: x => x.PriceOfferId,
                    principalTable: "PriceOffer",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ApprovalHistories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ApproverRoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ApproverRoleName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                ApproverUsername = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                ApproverFullName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                IsLastApprovalInCurrentWorkflow = table.Column<bool>(type: "bit", nullable: false),
                MaterialApprovalRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PriceOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PriceOfferDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApprovalHistories", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApprovalHistories_MaterialApprovalRequest_MaterialApprovalRequestId",
                    column: x => x.MaterialApprovalRequestId,
                    principalTable: "MaterialApprovalRequest",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ApprovalHistories_PriceOfferDetail_PriceOfferDetailId",
                    column: x => x.PriceOfferDetailId,
                    principalTable: "PriceOfferDetail",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ApprovalHistories_PriceOffer_PriceOfferId",
                    column: x => x.PriceOfferId,
                    principalTable: "PriceOffer",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AbpAuditLogActions_AuditLogId",
            table: "AbpAuditLogActions",
            column: "AuditLogId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpAuditLogActions_TenantId_ServiceName_MethodName_ExecutionTime",
            table: "AbpAuditLogActions",
            columns: new[] { "TenantId", "ServiceName", "MethodName", "ExecutionTime" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpAuditLogs_TenantId_ExecutionTime",
            table: "AbpAuditLogs",
            columns: new[] { "TenantId", "ExecutionTime" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpAuditLogs_TenantId_UserId_ExecutionTime",
            table: "AbpAuditLogs",
            columns: new[] { "TenantId", "UserId", "ExecutionTime" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
            table: "AbpBackgroundJobs",
            columns: new[] { "IsAbandoned", "NextTryTime" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpBlobContainers_TenantId_Name",
            table: "AbpBlobContainers",
            columns: new[] { "TenantId", "Name" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpBlobs_ContainerId",
            table: "AbpBlobs",
            column: "ContainerId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpBlobs_TenantId_ContainerId_Name",
            table: "AbpBlobs",
            columns: new[] { "TenantId", "ContainerId", "Name" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpEntityChanges_AuditLogId",
            table: "AbpEntityChanges",
            column: "AuditLogId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpEntityChanges_TenantId_EntityTypeFullName_EntityId",
            table: "AbpEntityChanges",
            columns: new[] { "TenantId", "EntityTypeFullName", "EntityId" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpEntityPropertyChanges_EntityChangeId",
            table: "AbpEntityPropertyChanges",
            column: "EntityChangeId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpFeatureGroups_Name",
            table: "AbpFeatureGroups",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpFeatures_GroupName",
            table: "AbpFeatures",
            column: "GroupName");

        migrationBuilder.CreateIndex(
            name: "IX_AbpFeatures_Name",
            table: "AbpFeatures",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpFeatureValues_Name_ProviderName_ProviderKey",
            table: "AbpFeatureValues",
            columns: new[] { "Name", "ProviderName", "ProviderKey" },
            unique: true,
            filter: "[ProviderName] IS NOT NULL AND [ProviderKey] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AbpLanguages_CultureName",
            table: "AbpLanguages",
            column: "CultureName");

        migrationBuilder.CreateIndex(
            name: "IX_AbpLanguageTexts_TenantId_ResourceName_CultureName",
            table: "AbpLanguageTexts",
            columns: new[] { "TenantId", "ResourceName", "CultureName" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpLinkUsers_SourceUserId_SourceTenantId_TargetUserId_TargetTenantId",
            table: "AbpLinkUsers",
            columns: new[] { "SourceUserId", "SourceTenantId", "TargetUserId", "TargetTenantId" },
            unique: true,
            filter: "[SourceTenantId] IS NOT NULL AND [TargetTenantId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AbpLocalizationResources_Name",
            table: "AbpLocalizationResources",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpLocalizationTexts_ResourceName_CultureName",
            table: "AbpLocalizationTexts",
            columns: new[] { "ResourceName", "CultureName" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpOrganizationUnitRoles_RoleId_OrganizationUnitId",
            table: "AbpOrganizationUnitRoles",
            columns: new[] { "RoleId", "OrganizationUnitId" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpOrganizationUnits_Code",
            table: "AbpOrganizationUnits",
            column: "Code");

        migrationBuilder.CreateIndex(
            name: "IX_AbpOrganizationUnits_ParentId",
            table: "AbpOrganizationUnits",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpPermissionGrants_TenantId_Name_ProviderName_ProviderKey",
            table: "AbpPermissionGrants",
            columns: new[] { "TenantId", "Name", "ProviderName", "ProviderKey" },
            unique: true,
            filter: "[TenantId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AbpPermissionGroups_Name",
            table: "AbpPermissionGroups",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpPermissions_GroupName",
            table: "AbpPermissions",
            column: "GroupName");

        migrationBuilder.CreateIndex(
            name: "IX_AbpPermissions_Name",
            table: "AbpPermissions",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpRoleClaims_RoleId",
            table: "AbpRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpRoles_NormalizedName",
            table: "AbpRoles",
            column: "NormalizedName");

        migrationBuilder.CreateIndex(
            name: "IX_AbpSecurityLogs_TenantId_Action",
            table: "AbpSecurityLogs",
            columns: new[] { "TenantId", "Action" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpSecurityLogs_TenantId_ApplicationName",
            table: "AbpSecurityLogs",
            columns: new[] { "TenantId", "ApplicationName" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpSecurityLogs_TenantId_Identity",
            table: "AbpSecurityLogs",
            columns: new[] { "TenantId", "Identity" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpSecurityLogs_TenantId_UserId",
            table: "AbpSecurityLogs",
            columns: new[] { "TenantId", "UserId" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpSessions_Device",
            table: "AbpSessions",
            column: "Device");

        migrationBuilder.CreateIndex(
            name: "IX_AbpSessions_SessionId",
            table: "AbpSessions",
            column: "SessionId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpSessions_TenantId_UserId",
            table: "AbpSessions",
            columns: new[] { "TenantId", "UserId" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpSettingDefinitions_Name",
            table: "AbpSettingDefinitions",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpSettings_Name_ProviderName_ProviderKey",
            table: "AbpSettings",
            columns: new[] { "Name", "ProviderName", "ProviderKey" },
            unique: true,
            filter: "[ProviderName] IS NOT NULL AND [ProviderKey] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AbpTextTemplateDefinitionContentRecords_DefinitionId",
            table: "AbpTextTemplateDefinitionContentRecords",
            column: "DefinitionId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpTextTemplateDefinitionRecords_Name",
            table: "AbpTextTemplateDefinitionRecords",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpUserClaims_UserId",
            table: "AbpUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpUserLogins_LoginProvider_ProviderKey",
            table: "AbpUserLogins",
            columns: new[] { "LoginProvider", "ProviderKey" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpUserOrganizationUnits_UserId_OrganizationUnitId",
            table: "AbpUserOrganizationUnits",
            columns: new[] { "UserId", "OrganizationUnitId" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpUserRoles_RoleId_UserId",
            table: "AbpUserRoles",
            columns: new[] { "RoleId", "UserId" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpUsers_Email",
            table: "AbpUsers",
            column: "Email");

        migrationBuilder.CreateIndex(
            name: "IX_AbpUsers_NormalizedEmail",
            table: "AbpUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "IX_AbpUsers_NormalizedUserName",
            table: "AbpUsers",
            column: "NormalizedUserName");

        migrationBuilder.CreateIndex(
            name: "IX_AbpUsers_UserName",
            table: "AbpUsers",
            column: "UserName");

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalHistories_MaterialApprovalRequestId",
            table: "ApprovalHistories",
            column: "MaterialApprovalRequestId");

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalHistories_PriceOfferDetailId",
            table: "ApprovalHistories",
            column: "PriceOfferDetailId");

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalHistories_PriceOfferId",
            table: "ApprovalHistories",
            column: "PriceOfferId");

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalRoute_MaterialApprovalRequestId",
            table: "ApprovalRoute",
            column: "MaterialApprovalRequestId");

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalRoute_PriceOfferId",
            table: "ApprovalRoute",
            column: "PriceOfferId");

        migrationBuilder.CreateIndex(
            name: "IX_Buyer_BuyerTypeId",
            table: "Buyer",
            column: "BuyerTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_DistributorSalePIC_DistributorId",
            table: "DistributorSalePIC",
            column: "DistributorId");

        migrationBuilder.CreateIndex(
            name: "IX_DistributorSalePIC_LocationId",
            table: "DistributorSalePIC",
            column: "LocationId");

        migrationBuilder.CreateIndex(
            name: "IX_FmDirectoryDescriptors_ParentId",
            table: "FmDirectoryDescriptors",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_FmDirectoryDescriptors_TenantId_ParentId_Name",
            table: "FmDirectoryDescriptors",
            columns: new[] { "TenantId", "ParentId", "Name" });

        migrationBuilder.CreateIndex(
            name: "IX_FmFileDescriptors_DirectoryId",
            table: "FmFileDescriptors",
            column: "DirectoryId");

        migrationBuilder.CreateIndex(
            name: "IX_FmFileDescriptors_TenantId_DirectoryId_Name",
            table: "FmFileDescriptors",
            columns: new[] { "TenantId", "DirectoryId", "Name" });

        migrationBuilder.CreateIndex(
            name: "IX_GdprInfo_RequestId",
            table: "GdprInfo",
            column: "RequestId");

        migrationBuilder.CreateIndex(
            name: "IX_GdprRequests_UserId",
            table: "GdprRequests",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialApprovalRequestDetail_InputCurrency",
            table: "MaterialApprovalRequestDetail",
            column: "InputCurrency");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialApprovalRequestDetail_Material_Group",
            table: "MaterialApprovalRequestDetail",
            column: "Material_Group");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialApprovalRequestDetail_MaterialApprovalId",
            table: "MaterialApprovalRequestDetail",
            column: "MaterialApprovalId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialStock_MaterialId",
            table: "MaterialStock",
            column: "MaterialId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialStock_StockCategoryId",
            table: "MaterialStock",
            column: "StockCategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_OpenIddictApplications_ClientId",
            table: "OpenIddictApplications",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type",
            table: "OpenIddictAuthorizations",
            columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

        migrationBuilder.CreateIndex(
            name: "IX_OpenIddictScopes_Name",
            table: "OpenIddictScopes",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type",
            table: "OpenIddictTokens",
            columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

        migrationBuilder.CreateIndex(
            name: "IX_OpenIddictTokens_AuthorizationId",
            table: "OpenIddictTokens",
            column: "AuthorizationId");

        migrationBuilder.CreateIndex(
            name: "IX_OpenIddictTokens_ReferenceId",
            table: "OpenIddictTokens",
            column: "ReferenceId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_DistributorId",
            table: "PriceOffer",
            column: "DistributorId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_EUIndustryId",
            table: "PriceOffer",
            column: "EUIndustryId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_KeyAccountClassId",
            table: "PriceOffer",
            column: "KeyAccountClassId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_KeyAccountId",
            table: "PriceOffer",
            column: "KeyAccountId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_KeyAccountTypeId",
            table: "PriceOffer",
            column: "KeyAccountTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_LocationId",
            table: "PriceOffer",
            column: "LocationId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_ProjectTypeId",
            table: "PriceOffer",
            column: "ProjectTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_Customer_CustomerId",
            table: "PriceOffer_Customer",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_Customer_PriceOfferId",
            table: "PriceOffer_Customer",
            column: "PriceOfferId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOfferDetail_PriceOfferId",
            table: "PriceOfferDetail",
            column: "PriceOfferId");

        migrationBuilder.CreateIndex(
            name: "IX_StockTracingDetails_StockTracingId",
            table: "StockTracingDetails",
            column: "StockTracingId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AbpAuditLogActions");

        migrationBuilder.DropTable(
            name: "AbpBackgroundJobs");

        migrationBuilder.DropTable(
            name: "AbpBlobs");

        migrationBuilder.DropTable(
            name: "AbpClaimTypes");

        migrationBuilder.DropTable(
            name: "AbpEntityPropertyChanges");

        migrationBuilder.DropTable(
            name: "AbpFeatureGroups");

        migrationBuilder.DropTable(
            name: "AbpFeatures");

        migrationBuilder.DropTable(
            name: "AbpFeatureValues");

        migrationBuilder.DropTable(
            name: "AbpLanguages");

        migrationBuilder.DropTable(
            name: "AbpLanguageTexts");

        migrationBuilder.DropTable(
            name: "AbpLinkUsers");

        migrationBuilder.DropTable(
            name: "AbpLocalizationResources");

        migrationBuilder.DropTable(
            name: "AbpLocalizationTexts");

        migrationBuilder.DropTable(
            name: "AbpOrganizationUnitRoles");

        migrationBuilder.DropTable(
            name: "AbpPermissionGrants");

        migrationBuilder.DropTable(
            name: "AbpPermissionGroups");

        migrationBuilder.DropTable(
            name: "AbpPermissions");

        migrationBuilder.DropTable(
            name: "AbpRoleClaims");

        migrationBuilder.DropTable(
            name: "AbpSecurityLogs");

        migrationBuilder.DropTable(
            name: "AbpSessions");

        migrationBuilder.DropTable(
            name: "AbpSettingDefinitions");

        migrationBuilder.DropTable(
            name: "AbpSettings");

        migrationBuilder.DropTable(
            name: "AbpTextTemplateContents");

        migrationBuilder.DropTable(
            name: "AbpTextTemplateDefinitionContentRecords");

        migrationBuilder.DropTable(
            name: "AbpUserClaims");

        migrationBuilder.DropTable(
            name: "AbpUserDelegations");

        migrationBuilder.DropTable(
            name: "AbpUserLogins");

        migrationBuilder.DropTable(
            name: "AbpUserOrganizationUnits");

        migrationBuilder.DropTable(
            name: "AbpUserRoles");

        migrationBuilder.DropTable(
            name: "AbpUserTokens");

        migrationBuilder.DropTable(
            name: "ApprovalHistories");

        migrationBuilder.DropTable(
            name: "ApprovalRoute");

        migrationBuilder.DropTable(
            name: "Cargo");

        migrationBuilder.DropTable(
            name: "CargoData");

        migrationBuilder.DropTable(
            name: "DistributorSalePIC");

        migrationBuilder.DropTable(
            name: "FmFileDescriptors");

        migrationBuilder.DropTable(
            name: "GdprInfo");

        migrationBuilder.DropTable(
            name: "KeyAccount_Evaluations");

        migrationBuilder.DropTable(
            name: "MaterialApprovalRequestDetail");

        migrationBuilder.DropTable(
            name: "MaterialHistory");

        migrationBuilder.DropTable(
            name: "MaterialStock");

        migrationBuilder.DropTable(
            name: "OpenIddictScopes");

        migrationBuilder.DropTable(
            name: "OpenIddictTokens");

        migrationBuilder.DropTable(
            name: "PriceOffer_Customer");

        migrationBuilder.DropTable(
            name: "PSI");

        migrationBuilder.DropTable(
            name: "PSI_Detail");

        migrationBuilder.DropTable(
            name: "StockTracingDetails");

        migrationBuilder.DropTable(
            name: "AbpBlobContainers");

        migrationBuilder.DropTable(
            name: "AbpEntityChanges");

        migrationBuilder.DropTable(
            name: "AbpTextTemplateDefinitionRecords");

        migrationBuilder.DropTable(
            name: "AbpOrganizationUnits");

        migrationBuilder.DropTable(
            name: "AbpRoles");

        migrationBuilder.DropTable(
            name: "AbpUsers");

        migrationBuilder.DropTable(
            name: "PriceOfferDetail");

        migrationBuilder.DropTable(
            name: "Distributors");

        migrationBuilder.DropTable(
            name: "FmDirectoryDescriptors");

        migrationBuilder.DropTable(
            name: "GdprRequests");

        migrationBuilder.DropTable(
            name: "MaterialApprovalRequest");

        migrationBuilder.DropTable(
            name: "Materials");

        migrationBuilder.DropTable(
            name: "StockCategory");

        migrationBuilder.DropTable(
            name: "OpenIddictAuthorizations");

        migrationBuilder.DropTable(
            name: "Customer");

        migrationBuilder.DropTable(
            name: "StockTracings");

        migrationBuilder.DropTable(
            name: "AbpAuditLogs");

        migrationBuilder.DropTable(
            name: "PriceOffer");

        migrationBuilder.DropTable(
            name: "OpenIddictApplications");

        migrationBuilder.DropTable(
            name: "Buyer");

        migrationBuilder.DropTable(
            name: "KeyAccount");

        migrationBuilder.DropTable(
            name: "SystemCategories");
    }
}
