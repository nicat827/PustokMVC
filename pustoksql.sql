USE [master]
GO
/****** Object:  Database [PustokAB202]    Script Date: 11/17/2023 7:28:38 PM ******/
CREATE DATABASE [PustokAB202]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PustokAB202', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\PustokAB202.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PustokAB202_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\PustokAB202_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [PustokAB202] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PustokAB202].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PustokAB202] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PustokAB202] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PustokAB202] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PustokAB202] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PustokAB202] SET ARITHABORT OFF 
GO
ALTER DATABASE [PustokAB202] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [PustokAB202] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PustokAB202] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PustokAB202] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PustokAB202] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PustokAB202] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PustokAB202] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PustokAB202] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PustokAB202] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PustokAB202] SET  ENABLE_BROKER 
GO
ALTER DATABASE [PustokAB202] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PustokAB202] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PustokAB202] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PustokAB202] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PustokAB202] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PustokAB202] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [PustokAB202] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PustokAB202] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PustokAB202] SET  MULTI_USER 
GO
ALTER DATABASE [PustokAB202] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PustokAB202] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PustokAB202] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PustokAB202] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PustokAB202] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PustokAB202] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [PustokAB202] SET QUERY_STORE = ON
GO
ALTER DATABASE [PustokAB202] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [PustokAB202]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/17/2023 7:28:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 11/17/2023 7:28:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fullname] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Authors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookImages]    Script Date: 11/17/2023 7:28:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookImages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
	[IsPrimary] [bit] NULL,
	[BookId] [int] NOT NULL,
 CONSTRAINT [PK_BookImages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 11/17/2023 7:28:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[PageCount] [int] NOT NULL,
	[IsAviable] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CostPrice] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[SalePrice] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[GenreId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Features]    Script Date: 11/17/2023 7:28:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Features](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IconURL] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Features] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genres]    Script Date: 11/17/2023 7:28:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Slides]    Script Date: 11/17/2023 7:28:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Slides](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Subtitle] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Order] [int] NOT NULL,
	[ImageURL] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Slides] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231116141009_DbCreated', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231116141811_SlidesTableCreated', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231116144005_FeatureTableCreated', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231117141610_GenresAndAuthorsTablesCreated', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231117142545_BooksAndBookImagesTableAdded', N'7.0.14')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231117150052_IsPrimaryNullableAdded', N'7.0.14')
GO
SET IDENTITY_INSERT [dbo].[Authors] ON 

INSERT [dbo].[Authors] ([Id], [Fullname]) VALUES (1, N'Pushkin')
INSERT [dbo].[Authors] ([Id], [Fullname]) VALUES (2, N'Nizami')
INSERT [dbo].[Authors] ([Id], [Fullname]) VALUES (3, N'Mauqli')
INSERT [dbo].[Authors] ([Id], [Fullname]) VALUES (4, N'Onegin')
INSERT [dbo].[Authors] ([Id], [Fullname]) VALUES (5, N'William')
INSERT [dbo].[Authors] ([Id], [Fullname]) VALUES (6, N'Marcel')
INSERT [dbo].[Authors] ([Id], [Fullname]) VALUES (7, N'Jhon')
SET IDENTITY_INSERT [dbo].[Authors] OFF
GO
SET IDENTITY_INSERT [dbo].[BookImages] ON 

INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (1, N'product-3.jpg', 1, 1)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (2, N'product-2.jpg', 0, 1)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (7, N'product-4.jpg', NULL, 1)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (9, N'product-5.jpg', 1, 5)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (10, N'product-6.jpg', 0, 5)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (11, N'product-7.jpg', NULL, 5)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (12, N'product-8.jpg', 1, 11)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (13, N'product-9.jpg', 0, 11)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (14, N'product-10.jpg', NULL, 11)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (15, N'product-11.jpg', 1, 12)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (16, N'product-5.jpg', 0, 12)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (17, N'product-4.jpg', NULL, 12)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (18, N'product-1.jpg', 1, 14)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (19, N'product-13.jpg', 0, 14)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (21, N'product-12.jpg', NULL, 14)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (22, N'product-5.jpg', 1, 15)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (23, N'product-3.jpg', 0, 15)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (24, N'product-2.jpg', NULL, 15)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (25, N'product-12.jpg', 1, 16)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (26, N'product-11.jpg', 0, 16)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (27, N'product-5.jpg', NULL, 16)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (28, N'product-9.jpg', 1, 17)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (29, N'product-6.jpg', 0, 17)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (30, N'product-5.jpg', NULL, 17)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (32, N'product-10.jpg', 1, 20)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (33, N'product-5.jpg', 0, 20)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (34, N'product-4.jpg', NULL, 20)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (35, N'product-8.jpg', 1, 21)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (36, N'product-5.jpg', 0, 21)
INSERT [dbo].[BookImages] ([Id], [Image], [IsPrimary], [BookId]) VALUES (37, N'product-12.jpg', NULL, 21)
SET IDENTITY_INSERT [dbo].[BookImages] OFF
GO
SET IDENTITY_INSERT [dbo].[Books] ON 

INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (1, N'REMEMBRANCE OF THINGS PAST', 345, 1, 0, CAST(35.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), CAST(55.00 AS Decimal(18, 2)), N'You could say this one is cheating, a little bit. Proust wrote his 1913 seven-volume masterpiece in French, with the title À la recherche du temps perdu, which more directly translates into In Search of Lost Time.', 2, 6)
INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (5, N'THE FAULT IN OUR STARS', 143, 1, 0, CAST(42.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(67.00 AS Decimal(18, 2)), N'Maybe I was wrong about Brave New World being the most famous Shakespeare-inspired title.', 1, 7)
INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (11, N'A TIME TO KILL ', 645, 1, 0, CAST(80.00 AS Decimal(18, 2)), CAST(16.00 AS Decimal(18, 2)), CAST(124.00 AS Decimal(18, 2)), N'This quotation for Faulkner’s 1936 novel comes from the Books of Samuel – more specifically, 19:4 in 2 Samuel, which is in the Old Testament and relates some of the history of Israel. Absalom, the third son of David, rebelled against his father and was killed in battle. ', 4, 5)
INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (12, N'EAST OF EDEN ', 345, 1, 0, CAST(44.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(94.99 AS Decimal(18, 2)), N'Steinbeck apparently considered this 1952 novel to be his magnum opus, the one which all other novels before it had merely been practice for.', 3, 3)
INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (14, N'THE SUN ALSO RISES ', 412, 1, 0, CAST(35.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), N'More Ecclesiastes! This particular quotation is from 1:5, which states that The sun also ariseth, and the sun goeth down, and hasteth to his place where he arose. Hemingway’s modernist novel came out in 1926.', 1, 2)
INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (15, N'NUMBER THE STARS ', 123, 1, 0, CAST(21.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(33.99 AS Decimal(18, 2)), N'Although she’s most famous for her dystopian novel The Giver, Lowry’s 1989 novel Number the Stars focuses on the life of a Jewish family living in Copenhagen during World War II. In line 147:4, the Psalms declares that He [God] telleth the number of the stars; he calleth them all by their names. ', 2, 4)
INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (16, N'BRAVE NEW WORLD ', 245, 1, 0, CAST(45.00 AS Decimal(18, 2)), CAST(8.00 AS Decimal(18, 2)), CAST(75.00 AS Decimal(18, 2)), N'This is possibly the most famous book to take its title from a Shakespeare play – in this case, The Tempest. In Act V Scene I, Miranda declares:

How beauteous mankind is! O brave new world
That has such people in ’t! ', 2, 2)
INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (17, N'PALE FIRE', 432, 1, 0, CAST(70.00 AS Decimal(18, 2)), CAST(9.99 AS Decimal(18, 2)), CAST(99.00 AS Decimal(18, 2)), N'Timon of Athens is one of Shakespeare’s less well-known and less-read plays, so it’s not often quoted. But Timon’s speech here in Act IV Scene III is an excellent one. Suitably for a 1962 postmodernist novel full of cross-quotations and complex footnotes, there’s also a possible secondary Shakespeare reference here. ', 4, 4)
INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (20, N'COLD COMFORT FARM ', 333, 1, 0, CAST(55.00 AS Decimal(18, 2)), CAST(15.55 AS Decimal(18, 2)), CAST(80.00 AS Decimal(18, 2)), N'Gibbons’s 1932 classic about a deeply unpleasant farm, a satire of typical Victorian rural fiction, has a title taken from Act V Scene VII of King John, spoken by the titular character.', 3, 1)
INSERT [dbo].[Books] ([Id], [Name], [PageCount], [IsAviable], [IsDeleted], [CostPrice], [Discount], [SalePrice], [Description], [GenreId], [AuthorId]) VALUES (21, N'IN COLD BLOOD', 654, 1, 0, CAST(126.00 AS Decimal(18, 2)), CAST(26.99 AS Decimal(18, 2)), CAST(193.00 AS Decimal(18, 2)), N'Here we have another Timon of Athens quotation. For his 1966 nonfiction account of a notorious family murder, Capote selected a line from Alcibiades’ speech in Act III Scene V – Who cannot condemn rashness in cold blood?', 2, 7)
SET IDENTITY_INSERT [dbo].[Books] OFF
GO
SET IDENTITY_INSERT [dbo].[Features] ON 

INSERT [dbo].[Features] ([Id], [IconURL], [Name], [Description]) VALUES (1, N'fas fa-redo-alt', N'Money Back Guarantee', N'100% money back')
INSERT [dbo].[Features] ([Id], [IconURL], [Name], [Description]) VALUES (2, N'fas fa-piggy-bank', N'Cash On Delivery', N'Lorem ipsum dolor amet')
INSERT [dbo].[Features] ([Id], [IconURL], [Name], [Description]) VALUES (3, N'fas fa-life-ring', N'Help & Support', N'Call us : + 0123.4567.89')
INSERT [dbo].[Features] ([Id], [IconURL], [Name], [Description]) VALUES (4, N'fas fa-shipping-fast', N'Free Shipping Item', N'Orders over $500')
SET IDENTITY_INSERT [dbo].[Features] OFF
GO
SET IDENTITY_INSERT [dbo].[Genres] ON 

INSERT [dbo].[Genres] ([Id], [Name]) VALUES (1, N'Horror')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (2, N'Historical')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (3, N'Romance')
INSERT [dbo].[Genres] ([Id], [Name]) VALUES (4, N'Thriller')
SET IDENTITY_INSERT [dbo].[Genres] OFF
GO
SET IDENTITY_INSERT [dbo].[Slides] ON 

INSERT [dbo].[Slides] ([Id], [Title], [Subtitle], [Description], [Order], [ImageURL]) VALUES (4, N'H.G. Wells', N'De Vengeance', N'Cover Up Front Of Books and Leave Summary', 1, N'home-slider-2-ai.png')
INSERT [dbo].[Slides] ([Id], [Title], [Subtitle], [Description], [Order], [ImageURL]) VALUES (5, N'J.D. Kurtness', N'De Vengeance', N'Cover Up Front Of Books and Leave Summary', 2, N'home-slider-1-ai.png')
SET IDENTITY_INSERT [dbo].[Slides] OFF
GO
/****** Object:  Index [IX_BookImages_BookId]    Script Date: 11/17/2023 7:28:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_BookImages_BookId] ON [dbo].[BookImages]
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Books_AuthorId]    Script Date: 11/17/2023 7:28:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_Books_AuthorId] ON [dbo].[Books]
(
	[AuthorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Books_GenreId]    Script Date: 11/17/2023 7:28:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_Books_GenreId] ON [dbo].[Books]
(
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BookImages]  WITH CHECK ADD  CONSTRAINT [FK_BookImages_Books_BookId] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookImages] CHECK CONSTRAINT [FK_BookImages_Books_BookId]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_Authors_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_Books_Authors_AuthorId]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_Genres_GenreId] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genres] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_Books_Genres_GenreId]
GO
USE [master]
GO
ALTER DATABASE [PustokAB202] SET  READ_WRITE 
GO
