/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Hometown] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[Exercise]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exercise](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](255) NOT NULL,
	[Target] [float] NOT NULL,
	[SetId] [bigint] NOT NULL,
 CONSTRAINT [PK_Exercise] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[ExerciseAttributes]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExerciseAttributes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AttributeID] [smallint] NOT NULL,
	[Name] [nchar](512) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[ExerciseId] [bigint] NOT NULL,
 CONSTRAINT [PK_ExerciseAttributes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[ExerciseRecord]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExerciseRecord](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Record] [float] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[ExerciseId] [bigint] NOT NULL,
 CONSTRAINT [PK_ExerciseRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[ExerciseRecordAttributes]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExerciseRecordAttributes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AttributeID] [smallint] NOT NULL,
	[Name] [nchar](512) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[ExerciseRecordID] [bigint] NOT NULL,
 CONSTRAINT [PK_ExerciseRecordAttributes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[Set]    Script Date: 4/27/2015 1:01:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Set](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](255) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_Set] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'201412221431036_InitialCreate', N'MyFitnessTrackerVS.Models.ApplicationDbContext', 0x1F8B0800000000000400DD5C5B6FE3B6127E3FC0F90F829E7A0E522B97B38B3D81B38BD4897B826E2E5867177D5BD012ED102B51AA44A5098AFEB23EF427F52F9CA144C9122FBAD88AED140B2C2272F8CD70382487C3A1FFFAE3CFF187A7C0B71E719C90909ED947A343DBC2D40D3D42976776CA16DFBFB33FBCFFE73FC6975EF0647D29E84E381DB4A4C999FDC05874EA3889FB8003948C02E2C661122ED8C80D030779A1737C78F85FE7E8C8C100610396658D3FA5949100671FF03909A98B239622FF3AF4B09F8872A89965A8D60D0A701221179FD9D7CF53C2284E92FB18B9DF70FC6536CA1BD9D6B94F100834C3FEC2B610A521430CC43DFD9CE0198B43BA9C455080FCFBE70803DD02F90916DD385D9177EDD1E131EF91B36A5840B969C2C2A027E0D189509123375F4BD176A94250E225289B3DF35E678A3CB3AF3C9C157D0A7D5080CCF074E2C79C18745DB2384FA21BCC4645C3510E398D01EED730FE36AA221E589DDB1D9426753C3AE4FF0EAC49EAB334C66714A72C46FE817597CE7DE2FE849FEFC36F989E9D1CCD1727EFDEBC45DEC9DBFFE09337D59E425F81AE560045777118E11864C38BB2FFB6E5D4DB3972C3B259A54DAE15B025981DB6758D9E3E62BA640F306F8EDFD9D6943C61AF2811C6F59912984CD088C5297CDEA4BE8FE63E2EEB9D469EFCFF06AEC76FDE0EC2F5063D926536F4127F983831CCAB4FD8CF6A930712E5D3AB36DE5F05D9340E03FE5DB7AFBCF6EB2C4C639777263492DCA37889595DBAB1B332DE4E26CDA18637EB0275FF4D9B4BAA9AB7969477689D9950B0D8F66C28E47D59BE9D2DEE3C8A60F032D3E21A693438D39E359240C0BE14D295211D7535240A1DFC3BAF8B970122FE000B63072EE09A2C481CE0B2973F84608688F696F90E2509AC0BDEFF50F2D0203AFC3980E833ECA63198EB8CA1207A716E770F21C5376930E7B3607BBC061B9AFB5FC3297259185F52DE6A63BC8FA1FB2D4CD925F52E10C39F995B00F2CF7B12740718449C73D785F5640AC68CBD49089E77017845D9C9716F38BE4EEDDA3199F888047ACF445A51BF16A42BEF444FA1782806329D97D224EAC770496837510B52B3A83945ABA882ACAFA81CAC9BA482D22C6846D02A674E3598DF978DD0F08E5F06BBFF9EDF669BB7692DA8A871062B24FE11531CC332E6DD21C6704C5723D065DDD885B3900D1F67FAE27B53C6E90BF2D3A159AD351BB24560F8D990C1EEFF6CC8C484E247E271AFA4C371A82006F84EF4FA9356FB9C9324DBF674A87573DBCCB7B30698A6CB7992842EC96681261026C21875F9C187B3DA631A796FE4B808740C0C9DF02D0F4AA06FB66C54B7F402FB9861EBDCCD03851394B8C853D5081DF27A0856ECA81AC156F191BA70FF567882A5E3983742FC1094C04C2594A9D382509744C86FD592D4B2E316C6FB5EF2906B2E70842967D8AA892ECCF5E1102E40C9471A94360D8D9D8AC5351BA2C16B358D799B0BBB1A77254AB1159B6CF19D0D7629FCB71731CC668D6DC1389B55D2450063686F17062ACE2A5D0D403EB8EC9B814A272683810A976A2B065AD7D80E0CB4AE925767A0F911B5EBF84BE7D57D33CFFA4179FBDB7AA3BA76609B357DEC9969E6BE27B461D002C7AA795ECC79257E629AC319C829CE678970756513E1E033CCEA219B95BFABF5439D6610D9889A005786D6022A2E0515206542F510AE88E5354A27BC881EB045DCAD1156ACFD126CC50654ECEAE56885D07C852A1B67A7D347D9B3D21A1423EF7458A8E0680C425EBCEA1DEFA014535C56554C175FB88F375CE998188C0605B578AE0625159D195C4B8569B66B49E790F571C936D292E43E19B4547466702D091B6D5792C629E8E1166CA4A2FA163ED0642B221DE56E53D68D9D3C754A148C1D438ED5F81A4511A1CB4ACE9528B16679C2D5E4FB59FF14A420C770DC449389544A5B7262618C9658AA05D620E994C409BB400CCD118FF34CBC4021D3EEAD86E5BF6059DD3ED5412CF681829AFF9DB7305FE5D7B65CD527115053E868C01D9B2C9AAE31037D738BA7C2211FC59A00FE24F4D3809AFD2C73EBFC1AAFDA3E2F5111C68E24BFE247294A53BCDDFA08741A1F756E0C3B56A537B3FE7899214C5A2F7CD1AADE4DFEA919A5085755514C21AC9D8D9FC9AD5967CC64C7B1FF90B522BCCC2C13D92A550051D413A392F0A08055EABAA3D67352AA98F59AEE8852E2491552AAEA216535BDA42664B5622D3C8346F514DD39A809255574B5B63BB226B5A40AADA95E035B23B35CD71D55937D5205D65477C75EA5A2C86BE91EEF63C6E3CCA61B597EF0DD6C273360BCCCC238CC4658B9DFAF02558A7B62891B7C054C94EFA551194F7F9B1A551EF6D8CCA80C18E675A876415E5F861A6FF5CD98B55BEFDA52DF74EB6FC6EB67BA2F6A20CA19502629B9976741E9CC3716E7AFF6C737CA812C27B1AD428DB0CD3F270C07234E309AFDE24F7C82F9A25E105C234A16386179A6877D7C78742C3DDCD99F47344E9278BEE6FC6A7A49531FB32D246DD14714BB0F28565328367868B20255A2D357D4C34F67F66F59ABD32CD0C1FFCA8A0FACABE43325BFA450711FA7D8FA5D4D091D26F1BEF9E4B5A7CF24BA6BF5EAE7AF79D303EB368619736A1D4ABA5C6784EB8F277A499337DD409AB59F54BCDE09557BA1A0459526C4FA0F12E6840DF218A190F2BB003DFDABAF68DA07071B216A1E150C8537880A4D8F06D6C1323E18F0E093650F06FA7556FF80601DD18C8F0708ED0F263F1DE8BE0C152D77B8D5688E46DB5892323DB7A65E6F9487B9EBBD49C9D0DE68A2AB59D83DE036C8B45EC3325E5992F260BBA326077930EC5D9AF68B271EEF4BAEF12A0B64B729C6DBCC2A6EB82BFA5B2513EF41FA9B269D67F729C3DBB635533877CFF32EFB2506EF99B18924AFDDA7FF6EDBD84C61DE3D37B65E49BE7B666BBBDA3F776C699DB7D09DA7ECAAD947866B195D2CB82D25370F9CC3097F1E8211E41E65FE92529F03D694BFDAC2704562666A4E3E93192B1347E1AB5034B3EDD757B1E1377656D034B335A46C36F116EB7F236F41D3CCDB9008B98B64626D2AA22EC1BB651D6BCA8C7A4DC9C3B59EB4E4AAB7F9AC8D77ECAF29577810A5D4668FE18EF8F5A4060FA29221A74E8F5460F5BA17F6CECA2F32C2FE9D90E50A82FF3E23C56E6DD72C69AEE8222C366F49A282448AD05C63863CD852CF634616C86550CD63CCD953F02C6EC76F3AE6D8BBA2B7298B52065DC6C1DCAF05BCB813D0C43FCB77AECB3CBE8DB25F3519A20B2026E1B1F95BFA434A7CAF947BAA89091920B8772122BA7C2C198FEC2E9F4BA49B90760412EA2B9DA27B1C443E8025B774861EF13AB281F97DC44BE43EAF22802690F681A8AB7D7C41D0324641223056EDE1136CD80B9EDEFF1F414793AF98540000, N'6.1.1-30610')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'Admin')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'279eb757-4f0f-42b5-a535-408e427a775d', N'1')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Hometown]) VALUES (N'279eb757-4f0f-42b5-a535-408e427a775d', N'lionheartadi@hotmail.com', 0, N'ADNM7EVR07TYKsH2LIulHlypo5alFYEv7DEp/01vBQpDSoq5WmhSV1ZVzmi0B/HsUQ==', N'9ada21c3-3e06-40bc-a689-43b3f2ab2b48', NULL, 0, 0, NULL, 1, 0, N'lionheartadi@hotmail.com', NULL)
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Hometown]) VALUES (N'dc197d89-cb6a-4274-8428-b0e354309c93', N'demo@account.com', 0, N'AIPsXJtAkY0wbRhHu0MaRaOVyvTNbT+UG3D+qdHJjsumjcpXwj2+u6prsrHd4lVnIA==', N'db2fc3c4-292d-49e9-b4aa-61ff5031a928', NULL, 0, 0, NULL, 1, 0, N'demo@account.com', NULL)
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Hometown]) VALUES (N'e1145529-f399-42b7-8a3b-dff91ff2583b', N'test@account.com', 0, N'AOlDZXI1X7NCTPzRwHVe/FqNnfohsY2TWE8R4+kP7i+9tUbxoIciMibIXFsew+vJlQ==', N'379d453d-ce68-4e9c-9957-47a8c7aaa22c', NULL, 0, 0, NULL, 1, 0, N'test@account.com', NULL)
SET IDENTITY_INSERT [dbo].[Exercise] ON 

INSERT [dbo].[Exercise] ([Id], [Name], [Target], [SetId]) VALUES (1, N'e1                                                                                                                                                                                                                                                             ', 123, 2)
INSERT [dbo].[Exercise] ([Id], [Name], [Target], [SetId]) VALUES (2, N'e2                                                                                                                                                                                                                                                             ', 23, 2)
INSERT [dbo].[Exercise] ([Id], [Name], [Target], [SetId]) VALUES (3, N'e3                                                                                                                                                                                                                                                             ', 123, 3)
INSERT [dbo].[Exercise] ([Id], [Name], [Target], [SetId]) VALUES (4, N'e1e1                                                                                                                                                                                                                                                           ', 12, 4)
INSERT [dbo].[Exercise] ([Id], [Name], [Target], [SetId]) VALUES (5, N'etest321                                                                                                                                                                                                                                                       ', 123, 5)
INSERT [dbo].[Exercise] ([Id], [Name], [Target], [SetId]) VALUES (6, N'New Excer                                                                                                                                                                                                                                                      ', 24, 4)
INSERT [dbo].[Exercise] ([Id], [Name], [Target], [SetId]) VALUES (7, N'Pushups                                                                                                                                                                                                                                                        ', 23, 6)
INSERT [dbo].[Exercise] ([Id], [Name], [Target], [SetId]) VALUES (8, N'Squats                                                                                                                                                                                                                                                         ', 12, 6)
SET IDENTITY_INSERT [dbo].[Exercise] OFF
SET IDENTITY_INSERT [dbo].[ExerciseAttributes] ON 

INSERT [dbo].[ExerciseAttributes] ([Id], [AttributeID], [Name], [Data], [ExerciseId]) VALUES (1, 0, N'GEOLocation                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ', N'[{"Latitude":60.37100166666667,"LocationTime":0.0,"Longitude":24.746016666666666}]', 4)
SET IDENTITY_INSERT [dbo].[ExerciseAttributes] OFF
SET IDENTITY_INSERT [dbo].[ExerciseRecord] ON 

INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (4, 12, CAST(N'2014-12-16 00:00:00.0000000' AS DateTime2), CAST(N'2014-12-15 00:00:00.0000000' AS DateTime2), CAST(N'2014-12-17 00:00:00.0000000' AS DateTime2), 1)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (5, 23, CAST(N'2014-12-12 00:00:00.0000000' AS DateTime2), CAST(N'2014-12-23 11:23:53.0000000' AS DateTime2), CAST(N'2014-12-09 00:00:00.0000000' AS DateTime2), 2)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (6, 23, CAST(N'2014-12-30 15:12:04.0000000' AS DateTime2), CAST(N'2014-12-30 15:12:04.0000000' AS DateTime2), CAST(N'2014-12-30 15:12:04.0000000' AS DateTime2), 2)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (7, 54, CAST(N'2014-12-31 00:00:00.0000000' AS DateTime2), CAST(N'2014-12-23 00:00:00.0000000' AS DateTime2), CAST(N'2014-12-31 00:00:00.0000000' AS DateTime2), 1)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (8, 78, CAST(N'2014-12-30 15:28:11.0000000' AS DateTime2), CAST(N'2014-12-10 00:00:00.0000000' AS DateTime2), CAST(N'2014-12-30 15:28:11.0000000' AS DateTime2), 1)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (9, 123, CAST(N'2015-01-05 12:21:46.0000000' AS DateTime2), CAST(N'2015-01-05 12:21:46.0000000' AS DateTime2), CAST(N'2015-01-05 12:21:46.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (10, 23, CAST(N'2015-01-05 15:57:20.0000000' AS DateTime2), CAST(N'2015-01-05 15:57:20.0000000' AS DateTime2), CAST(N'2015-01-05 15:57:20.0000000' AS DateTime2), 5)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (11, 12, CAST(N'2015-01-08 16:05:12.0000000' AS DateTime2), CAST(N'2015-01-08 16:05:12.0000000' AS DateTime2), CAST(N'2015-01-08 16:05:12.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (12, 24, CAST(N'2015-01-08 16:05:21.0000000' AS DateTime2), CAST(N'2015-01-08 16:05:21.0000000' AS DateTime2), CAST(N'2015-01-08 16:05:21.0000000' AS DateTime2), 5)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (13, 43, CAST(N'2015-01-08 16:05:27.0000000' AS DateTime2), CAST(N'2015-01-08 16:05:27.0000000' AS DateTime2), CAST(N'2015-01-08 16:05:27.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (14, 7, CAST(N'2015-01-06 00:00:00.0000000' AS DateTime2), CAST(N'2015-01-08 16:06:34.0000000' AS DateTime2), CAST(N'2015-01-08 16:06:34.0000000' AS DateTime2), 8)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (15, 16, CAST(N'2015-01-08 16:06:58.0000000' AS DateTime2), CAST(N'2015-01-08 16:06:58.0000000' AS DateTime2), CAST(N'2015-01-08 16:06:58.0000000' AS DateTime2), 8)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (16, 13, CAST(N'2015-01-08 16:07:11.0000000' AS DateTime2), CAST(N'2015-01-08 16:07:11.0000000' AS DateTime2), CAST(N'2015-01-08 16:07:11.0000000' AS DateTime2), 7)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (17, 34, CAST(N'2015-01-05 00:00:00.0000000' AS DateTime2), CAST(N'2015-01-08 16:07:28.0000000' AS DateTime2), CAST(N'2015-01-08 16:07:28.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (18, 21, CAST(N'2015-01-01 00:00:00.0000000' AS DateTime2), CAST(N'2015-01-08 16:07:36.0000000' AS DateTime2), CAST(N'2015-01-08 16:07:36.0000000' AS DateTime2), 7)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (19, 23, CAST(N'2015-02-02 00:00:00.0000000' AS DateTime2), CAST(N'2015-01-09 08:49:48.0000000' AS DateTime2), CAST(N'2015-01-09 08:49:48.0000000' AS DateTime2), 7)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (20, 34, CAST(N'2015-02-04 00:00:00.0000000' AS DateTime2), CAST(N'2015-01-09 08:50:18.0000000' AS DateTime2), CAST(N'2015-01-09 08:50:18.0000000' AS DateTime2), 7)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (21, 23, CAST(N'2015-01-12 15:46:05.0000000' AS DateTime2), CAST(N'2015-01-12 15:46:05.0000000' AS DateTime2), CAST(N'2015-01-12 15:46:05.0000000' AS DateTime2), 8)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (22, 26, CAST(N'2015-02-20 00:00:00.0000000' AS DateTime2), CAST(N'2015-01-12 15:46:37.0000000' AS DateTime2), CAST(N'2015-01-12 15:46:37.0000000' AS DateTime2), 7)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (23, 45, CAST(N'2015-01-12 15:47:04.0000000' AS DateTime2), CAST(N'2015-01-12 15:47:04.0000000' AS DateTime2), CAST(N'2015-01-12 15:47:04.0000000' AS DateTime2), 8)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (24, 43, CAST(N'2015-02-11 00:00:00.0000000' AS DateTime2), CAST(N'2015-01-12 15:49:40.0000000' AS DateTime2), CAST(N'2015-01-12 15:49:40.0000000' AS DateTime2), 8)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (25, 209150, CAST(N'2015-04-20 07:23:31.0000000' AS DateTime2), CAST(N'2015-04-20 07:23:31.0000000' AS DateTime2), CAST(N'2015-04-20 07:23:31.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (26, 22907, CAST(N'2015-04-20 07:30:43.0000000' AS DateTime2), CAST(N'2015-04-20 07:30:43.0000000' AS DateTime2), CAST(N'2015-04-20 07:30:43.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (27, 22907, CAST(N'2015-04-20 07:30:47.0000000' AS DateTime2), CAST(N'2015-04-20 07:30:47.0000000' AS DateTime2), CAST(N'2015-04-20 07:30:47.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (28, 56304, CAST(N'2015-04-20 07:49:41.0000000' AS DateTime2), CAST(N'2015-04-20 07:49:41.0000000' AS DateTime2), CAST(N'2015-04-20 07:49:41.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (29, 19404, CAST(N'2015-04-20 07:55:13.0000000' AS DateTime2), CAST(N'2015-04-20 07:55:13.0000000' AS DateTime2), CAST(N'2015-04-20 07:55:13.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (30, 3149, CAST(N'2015-04-20 08:01:59.0000000' AS DateTime2), CAST(N'2015-04-20 08:01:59.0000000' AS DateTime2), CAST(N'2015-04-20 08:01:59.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (31, 3224, CAST(N'2015-04-20 08:03:56.0000000' AS DateTime2), CAST(N'2015-04-20 08:03:56.0000000' AS DateTime2), CAST(N'2015-04-20 08:03:56.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (32, 9134, CAST(N'2015-04-20 08:08:03.0000000' AS DateTime2), CAST(N'2015-04-20 08:08:03.0000000' AS DateTime2), CAST(N'2015-04-20 08:08:03.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (33, 3058, CAST(N'2015-04-20 08:17:10.0000000' AS DateTime2), CAST(N'2015-04-20 08:17:10.0000000' AS DateTime2), CAST(N'2015-04-20 08:17:10.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (34, 36841, CAST(N'2015-04-24 04:57:28.0000000' AS DateTime2), CAST(N'2015-04-24 04:57:28.0000000' AS DateTime2), CAST(N'2015-04-24 04:57:28.0000000' AS DateTime2), 4)
INSERT [dbo].[ExerciseRecord] ([Id], [Record], [Date], [StartDate], [EndDate], [ExerciseId]) VALUES (35, 25521, CAST(N'2015-04-24 05:22:44.0000000' AS DateTime2), CAST(N'2015-04-24 05:22:44.0000000' AS DateTime2), CAST(N'2015-04-24 05:22:44.0000000' AS DateTime2), 4)
SET IDENTITY_INSERT [dbo].[ExerciseRecord] OFF
SET IDENTITY_INSERT [dbo].[ExerciseRecordAttributes] ON 

INSERT [dbo].[ExerciseRecordAttributes] ([Id], [AttributeID], [Name], [Data], [ExerciseRecordID]) VALUES (1, 0, N'GEOLocation                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ', N'[{"Latitude":60.37100166666667,"LocationTime":0.0,"Longitude":24.746016666666666},{"Latitude":60.62864833333333,"LocationTime":8127.0,"Longitude":24.85176},{"Latitude":60.37100166666667,"LocationTime":0.0,"Longitude":24.746016666666666}]', 35)
SET IDENTITY_INSERT [dbo].[ExerciseRecordAttributes] OFF
SET IDENTITY_INSERT [dbo].[Set] ON 

INSERT [dbo].[Set] ([Id], [Name], [UserId]) VALUES (2, N'test                                                                                                                                                                                                                                                           ', N'279eb757-4f0f-42b5-a535-408e427a775d')
INSERT [dbo].[Set] ([Id], [Name], [UserId]) VALUES (3, N'test2                                                                                                                                                                                                                                                          ', N'279eb757-4f0f-42b5-a535-408e427a775d')
INSERT [dbo].[Set] ([Id], [Name], [UserId]) VALUES (4, N'Set num 1                                                                                                                                                                                                                                                      ', N'dc197d89-cb6a-4274-8428-b0e354309c93')
INSERT [dbo].[Set] ([Id], [Name], [UserId]) VALUES (5, N'test321                                                                                                                                                                                                                                                        ', N'dc197d89-cb6a-4274-8428-b0e354309c93')
INSERT [dbo].[Set] ([Id], [Name], [UserId]) VALUES (6, N'Set 2015                                                                                                                                                                                                                                                       ', N'dc197d89-cb6a-4274-8428-b0e354309c93')
SET IDENTITY_INSERT [dbo].[Set] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [RoleNameIndex]    Script Date: 4/27/2015 1:01:05 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 4/27/2015 1:01:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 4/27/2015 1:01:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_RoleId]    Script Date: 4/27/2015 1:01:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserId]    Script Date: 4/27/2015 1:01:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UserNameIndex]    Script Date: 4/27/2015 1:01:05 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Exercise]  WITH CHECK ADD  CONSTRAINT [FK_Exercise_Set1] FOREIGN KEY([SetId])
REFERENCES [dbo].[Set] ([Id])
GO
ALTER TABLE [dbo].[Exercise] CHECK CONSTRAINT [FK_Exercise_Set1]
GO
ALTER TABLE [dbo].[ExerciseAttributes]  WITH CHECK ADD  CONSTRAINT [FK_ExerciseAttributes_Exercise] FOREIGN KEY([ExerciseId])
REFERENCES [dbo].[Exercise] ([Id])
GO
ALTER TABLE [dbo].[ExerciseAttributes] CHECK CONSTRAINT [FK_ExerciseAttributes_Exercise]
GO
ALTER TABLE [dbo].[ExerciseRecord]  WITH CHECK ADD  CONSTRAINT [FK_ExerciseRecord_Exercise] FOREIGN KEY([ExerciseId])
REFERENCES [dbo].[Exercise] ([Id])
GO
ALTER TABLE [dbo].[ExerciseRecord] CHECK CONSTRAINT [FK_ExerciseRecord_Exercise]
GO
ALTER TABLE [dbo].[ExerciseRecordAttributes]  WITH CHECK ADD  CONSTRAINT [FK_ExerciseRecordAttributes_ExerciseRecord] FOREIGN KEY([ExerciseRecordID])
REFERENCES [dbo].[ExerciseRecord] ([Id])
GO
ALTER TABLE [dbo].[ExerciseRecordAttributes] CHECK CONSTRAINT [FK_ExerciseRecordAttributes_ExerciseRecord]
GO
ALTER TABLE [dbo].[Set]  WITH CHECK ADD  CONSTRAINT [FK_Set_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Set] CHECK CONSTRAINT [FK_Set_AspNetUsers]
GO
