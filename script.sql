USE [master]
GO
/****** Object:  Database [OnlineAppointmentSystemDB]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'OnlineAppointmentSystemDB')
BEGIN
CREATE DATABASE [OnlineAppointmentSystemDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OnlineAppointmentSystemDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\OnlineAppointmentSystemDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OnlineAppointmentSystemDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\OnlineAppointmentSystemDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
END
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OnlineAppointmentSystemDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET RECOVERY FULL 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET  MULTI_USER 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'OnlineAppointmentSystemDB', N'ON'
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [OnlineAppointmentSystemDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Appointments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Appointments](
	[AppointmentId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
	[AppointmentDate] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[ReminderSent] [bit] NOT NULL,
 CONSTRAINT [PK_Appointments] PRIMARY KEY CLUSTERED 
(
	[AppointmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customers](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[Gender] [nvarchar](10) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employees]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Employees](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Department] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[EmployeeServices]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmployeeServices]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EmployeeServices](
	[EmployeeId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
 CONSTRAINT [PK_EmployeeServices] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC,
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notifications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Notifications](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[AppointmentId] [int] NOT NULL,
	[NotificationType] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[IsSent] [bit] NOT NULL,
	[SentDate] [datetime2](7) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Services]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Services]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Services](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[Duration] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[WorkingHours]    Script Date: 13.05.2025 17:03:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkingHours]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WorkingHours](
	[WorkingHoursId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[DayOfWeek] [int] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_WorkingHours] PRIMARY KEY CLUSTERED 
(
	[WorkingHoursId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250512213217_InitialCreate', N'8.0.15')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250512220643_AddIsActiveToCustomer', N'8.0.15')
GO
SET IDENTITY_INSERT [dbo].[Appointments] ON 

INSERT [dbo].[Appointments] ([AppointmentId], [CustomerId], [EmployeeId], [ServiceId], [AppointmentDate], [Status], [Notes], [CreatedDate], [UpdatedDate], [ReminderSent]) VALUES (1, 2, 1, 3, CAST(N'2025-05-14T10:30:00.0000000' AS DateTime2), 2, NULL, CAST(N'2025-05-13T01:54:24.7404052' AS DateTime2), CAST(N'2025-05-13T01:54:55.9167255' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Appointments] OFF
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'03112A28-192F-4AE3-A08C-C3D29BB5E671', N'Admin', N'ADMIN', N'463063EA-7D44-4A76-96EE-B0CF3C8A6B1B')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'7849D1AC-A061-44D7-AF0D-AF1B8A12BF09', N'Employee', N'EMPLOYEE', N'9AA9E89B-6061-4E70-851A-054B808A2D5E')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'F4278966-9D6D-4912-858C-C2D501A564FE', N'Customer', N'CUSTOMER', N'3EFB32FF-6C15-4933-916A-5C6427047555')
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 

INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1, N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'FirstName', N'Bahadır Alp')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2, N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'LastName', N'Er')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (5, N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'FirstName', N'Bahadır Alp')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (6, N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'LastName', N'Er')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (9, N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'FirstName', N'Mehmet Emin ')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (10, N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'LastName', N'Kök')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (11, N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'FirstName', N'Bahadır Alp')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (12, N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'LastName', N'Er')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (13, N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'FirstName', N'Mehmet Emin ')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (14, N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'LastName', N'Kök')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (15, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'FirstName', N'Hüseyin Cenk')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (16, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'LastName', N'Karatopcu')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (17, N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'FirstName', N'Bahadır Alp')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (18, N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'LastName', N'Er')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (19, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'FirstName', N'Hüseyin Cenk')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (20, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'LastName', N'Karatopcu')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (21, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'FirstName', N'Hüseyin Cenk')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (22, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'LastName', N'Karatopcu')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (23, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'FirstName', N'Hüseyin Cenk')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (24, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'LastName', N'Karatopcu')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (25, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'FirstName', N'Hüseyin Cenk')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (26, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'LastName', N'Karatopcu')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'03112A28-192F-4AE3-A08C-C3D29BB5E671')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'7849D1AC-A061-44D7-AF0D-AF1B8A12BF09')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'F4278966-9D6D-4912-858C-C2D501A564FE')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Address], [CreatedDate], [IsActive], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'Hüseyin Cenk', N'Karatopcu', N'Beylikdüzü', CAST(N'2025-05-13T01:53:00.3784454' AS DateTime2), 1, N'cenkkaratopcu@hastane.com', N'CENKKARATOPCU@HASTANE.COM', N'cenkkaratopcu@hastane.com', N'CENKKARATOPCU@HASTANE.COM', 0, N'AQAAAAIAAYagAAAAEMB2ObdQbAqzNj7aQ/YsbvCACxd0j4ho9ArVws8wOm1LHj5XBQQIFg4JlLxRJDnF4g==', N'T22UBB3TZTWNB6D6ILNQ367KK64TJBBX', N'caa69190-ec2b-4d2b-987d-064ee4fc24bd', N'+905384777725', 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Address], [CreatedDate], [IsActive], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'Mehmet Emin ', N'Kök', N'Ümraniye', CAST(N'2025-05-13T01:26:09.4371843' AS DateTime2), 1, N'bahadir.ermon@outlook.com', N'BAHADIR.ERMON@OUTLOOK.COM', N'bahadir.ermon@outlook.com', N'BAHADIR.ERMON@OUTLOOK.COM', 1, N'AQAAAAIAAYagAAAAEEurqC1yN95J4t5gW/wpZCSIlDq9bXR34+UFfAQKLiARbZE3KbWxctzs+IL50YVXDw==', N'B4ZL5JV757XT6HSBLJDZSXIOI5L5RLAI', N'9743ffb0-a69e-4a5d-96a4-55e34ee5b8db', N'+905522920355', 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Address], [CreatedDate], [IsActive], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'Bahadır Alp', N'Er', N'Piripaşa Mahallesi / Beyoğlu', CAST(N'2025-05-07T17:33:17.2130000' AS DateTime2), 1, N'bahadiralper03@gmail.com', N'BAHADIRALPER03@GMAIL.COM', N'bahadiralper03@gmail.com', N'BAHADIRALPER03@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAELSU4F9Y+cx+d6dFnx1Ikxtap8bs9bG/DUHUeDxRrLMROMN1RBSDUEuQpNB+pgbmYA==', N'ZKPJ5PSVDPWXI5KJY4YNZHKZ5XKPBT6Y', N'37285a73-07ea-4899-84f9-54e82ee3cfc1', N'+905342822401', 0, 0, CAST(N'2025-05-09T14:15:39.6571945+00:00' AS DateTimeOffset), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 

INSERT [dbo].[Customers] ([CustomerId], [UserId], [DateOfBirth], [Gender], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (2, N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', NULL, NULL, CAST(N'2025-05-13T01:26:09.8282393' AS DateTime2), NULL, 1)
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Employees] ON 

INSERT [dbo].[Employees] ([EmployeeId], [UserId], [Title], [Department], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'Op.Dr', N'Beyin ve Sinir Cerrahisi', 1, CAST(N'2025-05-13T01:53:00.7546403' AS DateTime2), CAST(N'2025-05-13T01:53:13.6867605' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Employees] OFF
GO
INSERT [dbo].[EmployeeServices] ([EmployeeId], [ServiceId]) VALUES (1, 3)
GO
SET IDENTITY_INSERT [dbo].[Notifications] ON 

INSERT [dbo].[Notifications] ([NotificationId], [AppointmentId], [NotificationType], [Content], [IsSent], [SentDate], [CreatedDate]) VALUES (1, 1, N'Email', N'
<html>
<body style=''font-family: Arial, sans-serif; line-height: 1.6; color: #333;''>
    <div style=''max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;''>
        <h2 style=''color: #4a86e8;''>Merhaba Mehmet Emin  Kök,</h2>
        
        <p>Randevunuz başarıyla oluşturuldu. Aşağıda randevu detaylarınızı bulabilirsiniz:</p>
        
        <div style=''background-color: #f9f9f9; padding: 15px; border-radius: 5px; margin: 20px 0;''>
            <p><strong>📅 Tarih:</strong> 14.05.2025</p>
            <p><strong>⏰ Saat:</strong> 10:30</p>
            <p><strong>🏥 Branş:</strong> Nöroloji</p>
            <p><strong>👨‍⚕️ Doktor:</strong> Op.Dr Hüseyin Doğu</p>
            
        </div>
        
        <p><strong>Not:</strong> Randevunuz şu anda <span style=''color: #ff9800; font-weight: bold;''>beklemede</span> durumundadır ve doktor tarafından onaylanması gerekmektedir.</p>
        <p>Randevunuz onaylandığında size tekrar bir bildirim e-postası gönderilecektir.</p>
        <p>Sorularınız için bizimle iletişime geçebilirsiniz.</p>
        <p>Sağlıklı günler dileriz.</p>
        
        <div style=''margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 12px; color: #777;''>
            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
        </div>
    </div>
</body>
</html>', 1, CAST(N'2025-05-13T01:57:38.4546360' AS DateTime2), CAST(N'2025-05-13T01:54:24.7797149' AS DateTime2))
INSERT [dbo].[Notifications] ([NotificationId], [AppointmentId], [NotificationType], [Content], [IsSent], [SentDate], [CreatedDate]) VALUES (2, 1, N'Email', N'
<html>
<body style=''font-family: Arial, sans-serif; line-height: 1.6; color: #333;''>
    <div style=''max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;''>
        <h2 style=''color: #4a86e8;''>Merhaba Mehmet Emin  Kök,</h2>
        
        <p>Randevunuz <strong>onaylandı</strong>. Aşağıda randevu detaylarınızı bulabilirsiniz:</p>
        
        <div style=''background-color: #f9f9f9; padding: 15px; border-radius: 5px; margin: 20px 0;''>
            <p><strong>📅 Tarih:</strong> 14.05.2025</p>
            <p><strong>⏰ Saat:</strong> 10:30</p>
            <p><strong>🏥 Branş:</strong> Nöroloji</p>
            <p><strong>👨‍⚕️ Doktor:</strong> Op.Dr Hüseyin Doğu</p>
            
        </div>
        
        <p>Sorularınız için bizimle iletişime geçebilirsiniz.</p>
        <p>Sağlıklı günler dileriz.</p>
        
        <div style=''margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-size: 12px; color: #777;''>
            <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
        </div>
    </div>
</body>
</html>', 1, CAST(N'2025-05-13T01:57:38.5167981' AS DateTime2), CAST(N'2025-05-13T01:54:55.9233264' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Notifications] OFF
GO
SET IDENTITY_INSERT [dbo].[Services] ON 

INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (1, N'Kardiyoloji', N'Kalp ve damar hastalıklarının tanı ve tedavisi.', 15, CAST(500.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-12T00:46:58.2470000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (2, N'Dermatoloji', N'Cilt hastalıklarının teşhis ve tedavisi.', 15, CAST(400.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (3, N'Nöroloji', N'Beyin ve sinir sistemi rahatsızlıkları.', 15, CAST(550.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (4, N'Ortopedi', N'Kemik, eklem ve kas sistemi hastalıkları.', 15, CAST(450.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (5, N'Göz Hastalıkları', N'Görme ve göz hastalıklarının tedavisi.', 15, CAST(350.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (6, N'Kulak Burun Boğaz', N'KBB rahatsızlıklarının teşhis ve tedavisi.', 15, CAST(400.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (7, N'Üroloji', N'İdrar yolları ve üreme sistemi hastalıkları.', 15, CAST(500.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (8, N'Kadın Doğum', N'Kadın sağlığı ve gebelik takibi.', 15, CAST(500.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (9, N'Dahiliye', N'İç hastalıklarının genel kontrolü.', 15, CAST(400.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (10, N'Psikiyatri', N'Ruh sağlığı ve hastalıkları tedavisi.', 15, CAST(600.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (11, N'Fizik Tedavi', N'Kas-iskelet sistemi rehabilitasyonu.', 15, CAST(550.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (12, N'Enfeksiyon Hastalıkları', N'Bulaşıcı hastalıkların tanısı ve tedavisi.', 15, CAST(400.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (13, N'Genel Cerrahi', N'Cerrahi müdahale gerektiren genel hastalıklar.', 15, CAST(700.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (14, N'Endokrinoloji', N'Hormon sistemi ve şeker hastalığı tedavisi.', 15, CAST(500.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
INSERT [dbo].[Services] ([ServiceId], [ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate]) VALUES (15, N'Gastroenteroloji', N'Mide, bağırsak ve sindirim sistemi hastalıkları.', 15, CAST(500.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Services] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkingHours] ON 

INSERT [dbo].[WorkingHours] ([WorkingHoursId], [EmployeeId], [DayOfWeek], [StartTime], [EndTime], [IsActive]) VALUES (1, 1, 1, CAST(N'08:00:00' AS Time), CAST(N'17:30:00' AS Time), 1)
INSERT [dbo].[WorkingHours] ([WorkingHoursId], [EmployeeId], [DayOfWeek], [StartTime], [EndTime], [IsActive]) VALUES (2, 1, 3, CAST(N'08:00:00' AS Time), CAST(N'17:30:00' AS Time), 1)
INSERT [dbo].[WorkingHours] ([WorkingHoursId], [EmployeeId], [DayOfWeek], [StartTime], [EndTime], [IsActive]) VALUES (3, 1, 4, CAST(N'08:00:00' AS Time), CAST(N'17:30:00' AS Time), 1)
INSERT [dbo].[WorkingHours] ([WorkingHoursId], [EmployeeId], [DayOfWeek], [StartTime], [EndTime], [IsActive]) VALUES (4, 1, 5, CAST(N'08:00:00' AS Time), CAST(N'17:30:00' AS Time), 1)
SET IDENTITY_INSERT [dbo].[WorkingHours] OFF
GO
/****** Object:  Index [IX_Appointments_CustomerId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Appointments]') AND name = N'IX_Appointments_CustomerId')
CREATE NONCLUSTERED INDEX [IX_Appointments_CustomerId] ON [dbo].[Appointments]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Appointments_EmployeeId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Appointments]') AND name = N'IX_Appointments_EmployeeId')
CREATE NONCLUSTERED INDEX [IX_Appointments_EmployeeId] ON [dbo].[Appointments]
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Appointments_ServiceId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Appointments]') AND name = N'IX_Appointments_ServiceId')
CREATE NONCLUSTERED INDEX [IX_Appointments_ServiceId] ON [dbo].[Appointments]
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND name = N'IX_AspNetRoleClaims_RoleId')
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND name = N'RoleNameIndex')
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND name = N'IX_AspNetUserClaims_UserId')
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND name = N'IX_AspNetUserLogins_UserId')
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND name = N'IX_AspNetUserRoles_RoleId')
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = N'EmailIndex')
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = N'UserNameIndex')
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Customers_UserId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND name = N'IX_Customers_UserId')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_UserId] ON [dbo].[Customers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Employees_UserId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Employees]') AND name = N'IX_Employees_UserId')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Employees_UserId] ON [dbo].[Employees]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EmployeeServices_ServiceId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[EmployeeServices]') AND name = N'IX_EmployeeServices_ServiceId')
CREATE NONCLUSTERED INDEX [IX_EmployeeServices_ServiceId] ON [dbo].[EmployeeServices]
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_AppointmentId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Notifications]') AND name = N'IX_Notifications_AppointmentId')
CREATE NONCLUSTERED INDEX [IX_Notifications_AppointmentId] ON [dbo].[Notifications]
(
	[AppointmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkingHours_EmployeeId]    Script Date: 13.05.2025 17:03:08 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[WorkingHours]') AND name = N'IX_WorkingHours_EmployeeId')
CREATE NONCLUSTERED INDEX [IX_WorkingHours_EmployeeId] ON [dbo].[WorkingHours]
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Customers__IsAct__72C60C4A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsActive]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Customers_CustomerId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Customers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Customers_CustomerId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Customers_CustomerId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Employees_EmployeeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Employees_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([EmployeeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Employees_EmployeeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Employees_EmployeeId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Services_ServiceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([ServiceId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Services_ServiceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Services_ServiceId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetRoleClaims_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]'))
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetRoleClaims_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]'))
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]'))
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]'))
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]'))
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]'))
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserTokens_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]'))
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserTokens_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]'))
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Customers_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customers]'))
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Customers_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customers]'))
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Employees_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Employees]'))
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Employees_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Employees]'))
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EmployeeServices_Employees_EmployeeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EmployeeServices]'))
ALTER TABLE [dbo].[EmployeeServices]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeServices_Employees_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([EmployeeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EmployeeServices_Employees_EmployeeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EmployeeServices]'))
ALTER TABLE [dbo].[EmployeeServices] CHECK CONSTRAINT [FK_EmployeeServices_Employees_EmployeeId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EmployeeServices_Services_ServiceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EmployeeServices]'))
ALTER TABLE [dbo].[EmployeeServices]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeServices_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([ServiceId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EmployeeServices_Services_ServiceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EmployeeServices]'))
ALTER TABLE [dbo].[EmployeeServices] CHECK CONSTRAINT [FK_EmployeeServices_Services_ServiceId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notifications_Appointments_AppointmentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notifications]'))
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Appointments_AppointmentId] FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[Appointments] ([AppointmentId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Notifications_Appointments_AppointmentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notifications]'))
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Appointments_AppointmentId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkingHours_Employees_EmployeeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkingHours]'))
ALTER TABLE [dbo].[WorkingHours]  WITH CHECK ADD  CONSTRAINT [FK_WorkingHours_Employees_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([EmployeeId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkingHours_Employees_EmployeeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkingHours]'))
ALTER TABLE [dbo].[WorkingHours] CHECK CONSTRAINT [FK_WorkingHours_Employees_EmployeeId]
GO
USE [master]
GO
ALTER DATABASE [OnlineAppointmentSystemDB] SET  READ_WRITE 
GO
