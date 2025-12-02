USE [master]
GO
/****** Object:  Database [project_management]    Script Date: 11/29/2025 10:33:41 AM ******/
CREATE DATABASE [project_management]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'project_management_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\project_management.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'project_management_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\project_management.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [project_management] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [project_management].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [project_management] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [project_management] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [project_management] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [project_management] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [project_management] SET ARITHABORT OFF 
GO
ALTER DATABASE [project_management] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [project_management] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [project_management] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [project_management] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [project_management] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [project_management] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [project_management] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [project_management] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [project_management] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [project_management] SET  DISABLE_BROKER 
GO
ALTER DATABASE [project_management] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [project_management] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [project_management] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [project_management] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [project_management] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [project_management] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [project_management] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [project_management] SET RECOVERY FULL 
GO
ALTER DATABASE [project_management] SET  MULTI_USER 
GO
ALTER DATABASE [project_management] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [project_management] SET DB_CHAINING OFF 
GO
ALTER DATABASE [project_management] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [project_management] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [project_management] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [project_management] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'project_management', N'ON'
GO
ALTER DATABASE [project_management] SET QUERY_STORE = OFF
GO
USE [project_management]
GO
/****** Object:  Table [dbo].[ActivityLog]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityLog](
	[id] [varchar](200) NOT NULL,
	[title] [varchar](max) NULL,
	[message] [varchar](max) NULL,
	[code] [varchar](max) NULL,
	[error] [varchar](max) NULL,
	[url] [varchar](max) NULL,
	[source] [varchar](max) NULL,
	[logtype] [varchar](max) NULL,
	[status] [varchar](max) NULL,
	[notes] [varchar](max) NULL,
	[loglevel] [varchar](max) NULL,
	[insertdate] [varchar](max) NULL,
	[insertuser] [varchar](max) NULL,
 CONSTRAINT [PK_ActivityLog] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Adminfaq]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Adminfaq](
	[id] [varchar](200) NOT NULL,
	[articletitle] [varchar](200) NULL,
	[description] [varchar](max) NULL,
	[files] [varchar](200) NULL,
	[categoryid] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
	[doc_order] [varchar](200) NULL,
	[expdate] [date] NULL,
	[notes] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[url] [varchar](200) NULL,
	[doctype] [varchar](200) NULL,
	[updatedate] [datetime] NULL,
	[createdby] [varchar](200) NULL,
	[updateby] [varchar](200) NULL,
 CONSTRAINT [PK_Adminfaq] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Adminfaqcategory]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Adminfaqcategory](
	[id] [varchar](200) NOT NULL,
	[categoryname] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[insertdate] [datetime] NULL,
	[updatedate] [datetime] NULL,
	[createdby] [varchar](200) NULL,
	[updateby] [varchar](200) NULL,
 CONSTRAINT [PK_Adminfaqcategory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appreciatiion]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appreciatiion](
	[id] [varchar](200) NOT NULL,
	[award] [varchar](200) NULL,
	[employeeid] [varchar](200) NULL,
	[date] [varchar](200) NULL,
	[summary] [varchar](200) NULL,
	[photo] [varchar](200) NULL,
 CONSTRAINT [PK_Appreciatiion] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[billing]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[billing](
	[id] [varchar](200) NOT NULL,
	[companyid] [varchar](200) NULL,
	[packageid] [varchar](200) NULL,
	[paymentdate] [varchar](200) NULL,
	[nextpaymentdate] [varchar](200) NULL,
	[transactionid] [varchar](200) NULL,
	[amount] [varchar](200) NULL,
	[paymentgateway] [varchar](200) NULL,
 CONSTRAINT [PK_billing] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Calendar]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Calendar](
	[EventID] [varchar](200) NOT NULL,
	[Subject] [varchar](200) NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[Description] [varchar](max) NULL,
	[Location] [varchar](200) NULL,
	[IsFullDay] [bit] NULL,
	[CreatedBy] [varchar](200) NULL,
	[CreatedDate] [datetime] NULL,
	[ThemeColor] [varchar](200) NULL,
 CONSTRAINT [PK__Calendar__7944C870BF0B40B4] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[email] [varchar](200) NULL,
	[country] [varchar](200) NULL,
	[mobile] [varchar](200) NULL,
	[gender] [varchar](200) NULL,
	[Language] [varchar](200) NULL,
	[clientcategory] [varchar](200) NULL,
	[clientsubcategory] [varchar](200) NULL,
	[Companyname] [varchar](200) NULL,
	[companywebsite] [varchar](200) NULL,
	[taxname] [varchar](200) NULL,
	[gstvatnumber] [varchar](200) NULL,
	[companyphone] [varchar](200) NULL,
	[city] [varchar](200) NULL,
	[state] [varchar](200) NULL,
	[postalcode] [varchar](200) NULL,
	[companyaddress] [varchar](200) NULL,
	[shippingaddress] [varchar](200) NULL,
	[addedby] [varchar](200) NULL,
	[note] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
	[employeeid] [varchar](200) NULL,
	[companyid] [varchar](200) NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[id] [varchar](200) NOT NULL,
	[packageid] [varchar](200) NULL,
	[companyname] [varchar](200) NULL,
	[companyemail] [varchar](200) NULL,
	[companyphone] [varchar](200) NULL,
	[companywebsite] [varchar](200) NULL,
	[timezone] [varchar](200) NULL,
	[language] [varchar](200) NULL,
	[currency] [varchar](200) NULL,
	[address] [varchar](200) NULL,
	[acctname] [varchar](200) NULL,
	[acctemail] [varchar](200) NULL,
	[packageid1] [varchar](200) NULL,
	[details] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[photo] [varchar](max) NULL,
	[url] [varchar](200) NULL,
	[expdate] [varchar](200) NULL,
	[employees] [varchar](200) NULL,
	[clients] [varchar](200) NULL,
	[invoices] [varchar](200) NULL,
	[storage] [varchar](200) NULL,
	[estimates] [varchar](200) NULL,
	[projects] [varchar](200) NULL,
	[tasks] [varchar](200) NULL,
	[leads] [varchar](200) NULL,
	[orders] [varchar](200) NULL,
	[tickets] [varchar](200) NULL,
	[contacts] [varchar](200) NULL,
	[amount] [varchar](200) NULL,
	[paymentdate] [varchar](200) NULL,
	[nextpaymentdate] [varchar](200) NULL,
	[licenceexpireson] [varchar](200) NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[company001]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[company001](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[email] [varchar](200) NULL,
	[mobile] [varchar](200) NULL,
	[officemobile] [varchar](200) NULL,
	[gstvatnumber] [varchar](200) NULL,
	[taxname] [varchar](200) NULL,
	[website] [varchar](200) NULL,
	[country] [varchar](200) NULL,
	[state] [varchar](200) NULL,
	[city] [varchar](200) NULL,
	[postalcode] [varchar](200) NULL,
	[address] [varchar](200) NULL,
	[shippingaddress] [varchar](200) NULL,
	[addedby] [varchar](200) NULL,
	[note] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
	[status] [varchar](200) NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Deals]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deals](
	[id] [varchar](200) NOT NULL,
	[leadcontactid] [varchar](200) NULL,
	[name] [varchar](200) NULL,
	[pipeline] [varchar](200) NULL,
	[dealstages] [varchar](200) NULL,
	[dealvalue] [varchar](200) NULL,
	[closedate] [varchar](200) NULL,
	[dealcategory] [varchar](200) NULL,
	[dealagent] [varchar](200) NULL,
	[product] [varchar](200) NULL,
	[dealwatcher] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
 CONSTRAINT [PK_Deals] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[parent] [varchar](200) NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Designation]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designation](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[parent] [varchar](200) NULL,
 CONSTRAINT [PK_Designation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[documents]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[documents](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[description] [varchar](max) NULL,
	[insertdate] [datetime] NULL,
	[notes] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[updatedate] [datetime] NULL,
	[createdby] [varchar](200) NULL,
	[updateby] [varchar](200) NULL,
 CONSTRAINT [PK_documents] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[dob] [varchar](200) NULL,
	[designation] [varchar](200) NULL,
	[department] [varchar](200) NULL,
	[email] [varchar](200) NULL,
	[country] [varchar](200) NULL,
	[mobile] [varchar](200) NULL,
	[gender] [varchar](200) NULL,
	[joiningdate] [varchar](200) NULL,
	[reportingto] [varchar](200) NULL,
	[Language] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[userrole] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
	[about] [varchar](200) NULL,
	[hourlyrate] [varchar](200) NULL,
	[slackmemberid] [varchar](200) NULL,
	[skills] [varchar](200) NULL,
	[probationenddate] [varchar](200) NULL,
	[noticeperiodstartdate] [varchar](200) NULL,
	[noticeperiodenddate] [varchar](200) NULL,
	[employmenttype] [varchar](200) NULL,
	[maritalstatus] [varchar](200) NULL,
	[businessaddress] [varchar](200) NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lead_Contact]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lead_Contact](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[email] [varchar](200) NULL,
	[leadsource] [varchar](200) NULL,
	[addedby] [varchar](200) NULL,
	[leadowner] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
	[companyid] [varchar](200) NULL,
 CONSTRAINT [PK_Lead_Contact] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Leave]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leave](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[leavetype] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[Duration] [varchar](200) NULL,
	[date] [varchar](200) NULL,
	[leavefile] [varchar](200) NULL,
	[createdby] [varchar](200) NULL,
	[reasonforabsence] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
	[employeeid] [varchar](200) NULL,
 CONSTRAINT [PK_Leave] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[notifications]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[notifications](
	[id] [varchar](200) NOT NULL,
	[sender] [varchar](200) NULL,
	[receiver] [varchar](200) NULL,
	[title] [varchar](200) NULL,
	[description] [varchar](200) NULL,
	[isread] [varchar](200) NULL,
	[insertuser] [varchar](200) NULL,
	[insertdate] [datetime] NULL,
	[status] [varchar](200) NULL,
	[notes] [varchar](200) NULL,
 CONSTRAINT [PK_notifications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Offlinerequest]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Offlinerequest](
	[id] [varchar](200) NOT NULL,
	[company] [varchar](200) NULL,
	[package] [varchar](200) NULL,
	[paymentby] [varchar](200) NULL,
	[created] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Packages]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Packages](
	[id] [varchar](200) NOT NULL,
	[packagename] [varchar](200) NULL,
	[monthlyprice] [varchar](200) NULL,
	[annualprice] [varchar](200) NULL,
	[currency] [varchar](200) NULL,
	[filestoragemb] [varchar](200) NULL,
	[maxemployees] [varchar](200) NULL,
	[moduleinpackage1] [bit] NOT NULL,
	[moduleinpackage2] [bit] NOT NULL,
	[moduleinpackage3] [bit] NOT NULL,
	[moduleinpackage4] [bit] NOT NULL,
	[moduleinpackage5] [bit] NOT NULL,
	[moduleinpackage6] [bit] NOT NULL,
	[moduleinpackage7] [bit] NOT NULL,
	[moduleinpackage8] [bit] NOT NULL,
	[moduleinpackage9] [bit] NOT NULL,
	[moduleinpackage10] [bit] NOT NULL,
	[moduleinpackage11] [bit] NOT NULL,
	[moduleinpackage12] [bit] NOT NULL,
	[moduleinpackage13] [bit] NOT NULL,
	[moduleinpackage14] [bit] NOT NULL,
	[moduleinpackage15] [bit] NOT NULL,
	[moduleinpackage16] [bit] NOT NULL,
	[moduleinpackage17] [bit] NOT NULL,
	[moduleinpackage18] [bit] NOT NULL,
	[moduleinpackage19] [bit] NOT NULL,
	[moduleinpackage20] [bit] NOT NULL,
	[moduleinpackage21] [bit] NOT NULL,
	[moduleinpackage22] [bit] NOT NULL,
	[moduleinpackage23] [bit] NOT NULL,
	[moduleinpackage24] [bit] NOT NULL,
	[moduleinpackage25] [bit] NOT NULL,
	[moduleinpackage26] [bit] NOT NULL,
	[moduleinpackage27] [bit] NOT NULL,
	[moduleinpackage28] [bit] NOT NULL,
	[moduleinpackage29] [bit] NOT NULL,
	[moduleinpackage30] [bit] NOT NULL,
	[moduleinpackage31] [bit] NOT NULL,
	[moduleinpackage32] [bit] NOT NULL,
	[moduleinpackage33] [bit] NOT NULL,
	[moduleinpackage34] [bit] NOT NULL,
	[status] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
	[packagetype] [varchar](200) NULL,
	[description] [varchar](max) NULL,
	[packagecategory] [varchar](200) NULL,
	[paymentdate] [varchar](200) NULL,
	[nextpaymentdate] [varchar](200) NULL,
	[licenceexpireson] [varchar](200) NULL,
 CONSTRAINT [PK__Packages__3213E83F087C69B3] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[personel_files_cat]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[personel_files_cat](
	[id] [varchar](200) NOT NULL,
	[cat_name] [varchar](200) NULL,
	[description] [varchar](max) NULL,
	[cat_order] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[insertdate] [datetime] NULL,
	[updatedate] [datetime] NULL,
	[createdby] [varchar](200) NULL,
	[updateby] [varchar](200) NULL,
 CONSTRAINT [PK_personel_files_cat] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[personneldocument]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[personneldocument](
	[id] [varchar](200) NOT NULL,
	[doc_order] [varchar](200) NULL,
	[description] [varchar](max) NULL,
	[expdate] [date] NULL,
	[insertdate] [datetime] NULL,
	[notes] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[url] [varchar](200) NULL,
	[doctype] [varchar](200) NULL,
	[documentid] [varchar](200) NULL,
	[cat_id] [varchar](200) NULL,
 CONSTRAINT [PK_personneldocument] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[settings]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[settings](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[description] [varchar](max) NULL,
	[insertdate] [datetime] NULL,
	[notes] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[updatedate] [datetime] NULL,
	[createdby] [varchar](200) NULL,
	[updateby] [varchar](200) NULL,
	[categoryid] [varchar](200) NULL,
 CONSTRAINT [PK_settings] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[settings_categories]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[settings_categories](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[description] [varchar](max) NULL,
	[cat_order] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[insertdate] [datetime] NULL,
	[updatedate] [datetime] NULL,
	[createdby] [varchar](200) NULL,
	[updateby] [varchar](200) NULL,
 CONSTRAINT [PK_settings_categories] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shift_Roster]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shift_Roster](
	[id] [varchar](200) NOT NULL,
	[Departmentid] [varchar](200) NULL,
	[employeeshift] [varchar](200) NULL,
	[shiftfrom] [varchar](200) NULL,
	[shiftto] [varchar](200) NULL,
	[remarks] [varchar](200) NULL,
	[shiftfile] [varchar](200) NULL,
	[createdby] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
	[employeeid] [varchar](200) NULL,
 CONSTRAINT [PK_Shift_Roster] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StickyNotes]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StickyNotes](
	[id] [varchar](200) NOT NULL,
	[title] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[insertdate] [datetime] NULL,
	[updatedate] [datetime] NULL,
	[createdby] [varchar](200) NULL,
	[updateby] [varchar](200) NULL,
	[description] [varchar](max) NULL,
	[color] [varchar](200) NULL,
 CONSTRAINT [PK_StickyNotes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Superadmin]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Superadmin](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[email] [varchar](200) NULL,
	[userrole] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
 CONSTRAINT [PK_Superadmin] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[useraccount]    Script Date: 11/29/2025 10:33:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[useraccount](
	[id] [varchar](200) NOT NULL,
	[name] [varchar](200) NULL,
	[email] [varchar](200) NULL,
	[password] [varchar](200) NULL,
	[phone] [varchar](200) NULL,
	[address] [varchar](200) NULL,
	[notes] [varchar](200) NULL,
	[status] [varchar](200) NULL,
	[usertype] [varchar](200) NULL,
	[insertdate] [varchar](200) NULL,
 CONSTRAINT [PK_useraccount] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActivityLog] ADD  CONSTRAINT [DF_ActivityLog_insertdate]  DEFAULT (getdate()) FOR [insertdate]
GO
ALTER TABLE [dbo].[Adminfaq] ADD  CONSTRAINT [DF_Adminfaq_updatedate]  DEFAULT (getdate()) FOR [updatedate]
GO
ALTER TABLE [dbo].[Adminfaqcategory] ADD  CONSTRAINT [DF_Adminfaqcategory_insertdate]  DEFAULT (getdate()) FOR [insertdate]
GO
ALTER TABLE [dbo].[Adminfaqcategory] ADD  CONSTRAINT [DF_Adminfaqcategory_updatedate]  DEFAULT (getdate()) FOR [updatedate]
GO
ALTER TABLE [dbo].[Calendar] ADD  CONSTRAINT [DF__Calendar__IsAllD__3F115E1A]  DEFAULT ((0)) FOR [IsFullDay]
GO
ALTER TABLE [dbo].[Calendar] ADD  CONSTRAINT [DF__Calendar__Create__40058253]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[documents] ADD  CONSTRAINT [DF_documents_insertdate_1]  DEFAULT (getdate()) FOR [insertdate]
GO
ALTER TABLE [dbo].[documents] ADD  CONSTRAINT [DF_documents_updatedate]  DEFAULT (getdate()) FOR [updatedate]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module16]  DEFAULT ((0)) FOR [moduleinpackage16]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module17]  DEFAULT ((0)) FOR [moduleinpackage17]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module18]  DEFAULT ((0)) FOR [moduleinpackage18]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module19]  DEFAULT ((0)) FOR [moduleinpackage19]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module20]  DEFAULT ((0)) FOR [moduleinpackage20]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module21]  DEFAULT ((0)) FOR [moduleinpackage21]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module22]  DEFAULT ((0)) FOR [moduleinpackage22]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module23]  DEFAULT ((0)) FOR [moduleinpackage23]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module24]  DEFAULT ((0)) FOR [moduleinpackage24]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module25]  DEFAULT ((0)) FOR [moduleinpackage25]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module26]  DEFAULT ((0)) FOR [moduleinpackage26]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module27]  DEFAULT ((0)) FOR [moduleinpackage27]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module28]  DEFAULT ((0)) FOR [moduleinpackage28]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module29]  DEFAULT ((0)) FOR [moduleinpackage29]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module30]  DEFAULT ((0)) FOR [moduleinpackage30]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module31]  DEFAULT ((0)) FOR [moduleinpackage31]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module32]  DEFAULT ((0)) FOR [moduleinpackage32]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module33]  DEFAULT ((0)) FOR [moduleinpackage33]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_module34]  DEFAULT ((0)) FOR [moduleinpackage34]
GO
ALTER TABLE [dbo].[personel_files_cat] ADD  CONSTRAINT [DF_personel_files_cat_insertdate]  DEFAULT (getdate()) FOR [insertdate]
GO
ALTER TABLE [dbo].[personel_files_cat] ADD  CONSTRAINT [DF_personel_files_cat_updatedate]  DEFAULT (getdate()) FOR [updatedate]
GO
ALTER TABLE [dbo].[personneldocument] ADD  CONSTRAINT [DF_personneldocument_insertdate]  DEFAULT (getdate()) FOR [insertdate]
GO
ALTER TABLE [dbo].[settings] ADD  CONSTRAINT [DF_settings_insertdate]  DEFAULT (getdate()) FOR [insertdate]
GO
ALTER TABLE [dbo].[settings] ADD  CONSTRAINT [DF_settings_updatedate]  DEFAULT (getdate()) FOR [updatedate]
GO
ALTER TABLE [dbo].[settings_categories] ADD  CONSTRAINT [DF_settings_categories_insertdate]  DEFAULT (getdate()) FOR [insertdate]
GO
ALTER TABLE [dbo].[settings_categories] ADD  CONSTRAINT [DF_settings_categories_updatedate]  DEFAULT (getdate()) FOR [updatedate]
GO
ALTER TABLE [dbo].[StickyNotes] ADD  CONSTRAINT [DF_StickyNotes_insertdate]  DEFAULT (getdate()) FOR [insertdate]
GO
ALTER TABLE [dbo].[StickyNotes] ADD  CONSTRAINT [DF_StickyNotes_updatedate]  DEFAULT (getdate()) FOR [updatedate]
GO
ALTER TABLE [dbo].[Adminfaq]  WITH CHECK ADD  CONSTRAINT [FK_Adminfaq_Adminfaqcategory] FOREIGN KEY([categoryid])
REFERENCES [dbo].[Adminfaqcategory] ([id])
GO
ALTER TABLE [dbo].[Adminfaq] CHECK CONSTRAINT [FK_Adminfaq_Adminfaqcategory]
GO
ALTER TABLE [dbo].[Appreciatiion]  WITH CHECK ADD  CONSTRAINT [FK_Appreciatiion_Employees] FOREIGN KEY([employeeid])
REFERENCES [dbo].[Employees] ([id])
GO
ALTER TABLE [dbo].[Appreciatiion] CHECK CONSTRAINT [FK_Appreciatiion_Employees]
GO
ALTER TABLE [dbo].[billing]  WITH CHECK ADD  CONSTRAINT [FK_billing_Companies] FOREIGN KEY([companyid])
REFERENCES [dbo].[Companies] ([id])
GO
ALTER TABLE [dbo].[billing] CHECK CONSTRAINT [FK_billing_Companies]
GO
ALTER TABLE [dbo].[billing]  WITH CHECK ADD  CONSTRAINT [FK_billing_Packages] FOREIGN KEY([packageid])
REFERENCES [dbo].[Packages] ([id])
GO
ALTER TABLE [dbo].[billing] CHECK CONSTRAINT [FK_billing_Packages]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_company001] FOREIGN KEY([companyid])
REFERENCES [dbo].[company001] ([id])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_company001]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Packages1] FOREIGN KEY([packageid])
REFERENCES [dbo].[Packages] ([id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Packages1]
GO
ALTER TABLE [dbo].[Deals]  WITH CHECK ADD  CONSTRAINT [FK_Deals_Lead_Contact] FOREIGN KEY([leadcontactid])
REFERENCES [dbo].[Lead_Contact] ([id])
GO
ALTER TABLE [dbo].[Deals] CHECK CONSTRAINT [FK_Deals_Lead_Contact]
GO
ALTER TABLE [dbo].[Lead_Contact]  WITH CHECK ADD  CONSTRAINT [FK_Lead_Contact_company001] FOREIGN KEY([companyid])
REFERENCES [dbo].[company001] ([id])
GO
ALTER TABLE [dbo].[Lead_Contact] CHECK CONSTRAINT [FK_Lead_Contact_company001]
GO
ALTER TABLE [dbo].[Lead_Contact]  WITH CHECK ADD  CONSTRAINT [FK_Lead_Contact_Employees1] FOREIGN KEY([addedby])
REFERENCES [dbo].[Employees] ([id])
GO
ALTER TABLE [dbo].[Lead_Contact] CHECK CONSTRAINT [FK_Lead_Contact_Employees1]
GO
ALTER TABLE [dbo].[Lead_Contact]  WITH CHECK ADD  CONSTRAINT [FK_Lead_Contact_Employees2] FOREIGN KEY([leadowner])
REFERENCES [dbo].[Employees] ([id])
GO
ALTER TABLE [dbo].[Lead_Contact] CHECK CONSTRAINT [FK_Lead_Contact_Employees2]
GO
ALTER TABLE [dbo].[Leave]  WITH CHECK ADD  CONSTRAINT [FK_Leave_Employees] FOREIGN KEY([employeeid])
REFERENCES [dbo].[Employees] ([id])
GO
ALTER TABLE [dbo].[Leave] CHECK CONSTRAINT [FK_Leave_Employees]
GO
ALTER TABLE [dbo].[notifications]  WITH CHECK ADD  CONSTRAINT [FK_notifications_useraccount] FOREIGN KEY([insertuser])
REFERENCES [dbo].[useraccount] ([id])
GO
ALTER TABLE [dbo].[notifications] CHECK CONSTRAINT [FK_notifications_useraccount]
GO
ALTER TABLE [dbo].[personneldocument]  WITH CHECK ADD  CONSTRAINT [FK_personneldocument_documents] FOREIGN KEY([documentid])
REFERENCES [dbo].[documents] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[personneldocument] CHECK CONSTRAINT [FK_personneldocument_documents]
GO
ALTER TABLE [dbo].[personneldocument]  WITH CHECK ADD  CONSTRAINT [FK_personneldocument_personel_files_cat] FOREIGN KEY([cat_id])
REFERENCES [dbo].[personel_files_cat] ([id])
GO
ALTER TABLE [dbo].[personneldocument] CHECK CONSTRAINT [FK_personneldocument_personel_files_cat]
GO
ALTER TABLE [dbo].[settings]  WITH CHECK ADD  CONSTRAINT [FK_settings_settings_categories] FOREIGN KEY([categoryid])
REFERENCES [dbo].[settings_categories] ([id])
GO
ALTER TABLE [dbo].[settings] CHECK CONSTRAINT [FK_settings_settings_categories]
GO
ALTER TABLE [dbo].[Shift_Roster]  WITH CHECK ADD  CONSTRAINT [FK_Shift_Roster_Department] FOREIGN KEY([Departmentid])
REFERENCES [dbo].[Department] ([id])
GO
ALTER TABLE [dbo].[Shift_Roster] CHECK CONSTRAINT [FK_Shift_Roster_Department]
GO
ALTER TABLE [dbo].[Shift_Roster]  WITH CHECK ADD  CONSTRAINT [FK_Shift_Roster_Employees] FOREIGN KEY([employeeid])
REFERENCES [dbo].[Employees] ([id])
GO
ALTER TABLE [dbo].[Shift_Roster] CHECK CONSTRAINT [FK_Shift_Roster_Employees]
GO
USE [master]
GO
ALTER DATABASE [project_management] SET  READ_WRITE 
GO
