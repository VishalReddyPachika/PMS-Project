USE [master]
GO
/****** Object:  Database [PassportManagementSystem]    Script Date: 11/17/2022 4:26:42 PM ******/
CREATE DATABASE [PassportManagementSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PassportVisaManagementSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PassportVisaManagementSystem.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PassportVisaManagementSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PassportVisaManagementSystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [PassportManagementSystem] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PassportManagementSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PassportManagementSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PassportManagementSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PassportManagementSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PassportManagementSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PassportManagementSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PassportManagementSystem] SET  MULTI_USER 
GO
ALTER DATABASE [PassportManagementSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PassportManagementSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PassportManagementSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PassportManagementSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PassportManagementSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PassportManagementSystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'PassportManagementSystem', N'ON'
GO
ALTER DATABASE [PassportManagementSystem] SET QUERY_STORE = OFF
GO
USE [PassportManagementSystem]
GO
/****** Object:  Table [dbo].[PassportApplication]    Script Date: 11/17/2022 4:26:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PassportApplication](
	[PassportNumber] [nvarchar](20) NOT NULL,
	[UserID] [nvarchar](20) NOT NULL,
	[Country] [nvarchar](20) NOT NULL,
	[State] [nvarchar](20) NOT NULL,
	[City] [nvarchar](20) NOT NULL,
	[Zip] [nvarchar](10) NOT NULL,
	[TypeOfService] [nvarchar](20) NOT NULL,
	[BookletType] [nvarchar](10) NOT NULL,
	[IssueDate] [date] NOT NULL,
	[ExpiryDate] [date] NOT NULL,
	[Amount] [int] NOT NULL,
	[ReasonForReIssue] [nvarchar](30) NOT NULL,
	[ProofOfCitizenship] [varbinary](max) NOT NULL,
	[Photo] [image] NOT NULL,
	[BirthCertificate] [varbinary](max) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[Comments] [nvarchar](max) NULL,
 CONSTRAINT [PK_PassportApplication] PRIMARY KEY CLUSTERED 
(
	[PassportNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRegistration]    Script Date: 11/17/2022 4:26:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRegistration](
	[UserID] [nvarchar](20) NOT NULL,
	[FirstName] [nvarchar](30) NOT NULL,
	[LastName] [nvarchar](30) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Address] [nvarchar](100) NOT NULL,
	[ContactNumber] [nvarchar](15) NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[ApplyType] [nvarchar](20) NOT NULL,
	[SecurityQuestion] [nvarchar](100) NOT NULL,
	[SecurityAnswer] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[CitizenType] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_UserRegistration] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PassportApplication] ADD  CONSTRAINT [DF_PassportApplication_ReasonForReIssue]  DEFAULT (N'NA') FOR [ReasonForReIssue]
GO
ALTER TABLE [dbo].[PassportApplication]  WITH CHECK ADD  CONSTRAINT [FK_PassportUserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserRegistration] ([UserID])
GO
ALTER TABLE [dbo].[PassportApplication] CHECK CONSTRAINT [FK_PassportUserID]
GO
INSERT INTO [dbo].[UserRegistration]
     VALUES
           ('ADMIN-0001'
           ,'Admin'
           ,'Passport Department'
           ,'1988-10-04'
           ,'India'
           ,'9999999999'
           ,'admin@passport.in'
           ,'Male'
           ,'Admin'
           ,'What is your Favourite Movie ?'
           ,'Avengers'
           ,'admin@123'
           ,'Adult')
GO
USE [master]
GO
ALTER DATABASE [PassportManagementSystem] SET  READ_WRITE 
GO
