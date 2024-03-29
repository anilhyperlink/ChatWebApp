USE [master]
GO
/****** Object:  Database [ChatAppDB]    Script Date: 07-03-2024 15:37:03 ******/
CREATE DATABASE [ChatAppDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ChatAppDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ChatAppDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ChatAppDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ChatAppDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ChatAppDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ChatAppDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ChatAppDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ChatAppDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ChatAppDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ChatAppDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ChatAppDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ChatAppDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ChatAppDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ChatAppDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ChatAppDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ChatAppDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ChatAppDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ChatAppDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ChatAppDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ChatAppDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ChatAppDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ChatAppDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ChatAppDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ChatAppDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ChatAppDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ChatAppDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ChatAppDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ChatAppDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ChatAppDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ChatAppDB] SET  MULTI_USER 
GO
ALTER DATABASE [ChatAppDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ChatAppDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ChatAppDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ChatAppDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ChatAppDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ChatAppDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ChatAppDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [ChatAppDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ChatAppDB]
GO
/****** Object:  Table [dbo].[Tbl_Users]    Script Date: 07-03-2024 15:37:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Users](
	[UserID] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[EmailAddress] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tble_Messages]    Script Date: 07-03-2024 15:37:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tble_Messages](
	[MessageId] [uniqueidentifier] NOT NULL,
	[SenderId] [uniqueidentifier] NULL,
	[ReceiverId] [uniqueidentifier] NULL,
	[MessageText] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime2](7) NULL,
	[GroupName] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tbl_Users] ADD  DEFAULT (newid()) FOR [UserID]
GO
ALTER TABLE [dbo].[Tbl_Users] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Tble_Messages] ADD  DEFAULT (newid()) FOR [MessageId]
GO
ALTER TABLE [dbo].[Tble_Messages] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  StoredProcedure [dbo].[sp_proc_AddUser]    Script Date: 07-03-2024 15:37:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_proc_AddUser] 
	@FirstName NVARCHAR(MAX),
	@LastName NVARCHAR(MAX),
	@Email NVARCHAR(MAX),
	@Password NVARCHAR(MAX)
AS
BEGIN
	INSERT INTO Tbl_Users (FirstName,LastName,EmailAddress,Password) VALUES (@FirstName,@LastName,@Email,@Password);
END
GO
/****** Object:  StoredProcedure [dbo].[sp_proc_GetUserListWithLastMessage]    Script Date: 07-03-2024 15:37:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_proc_GetUserListWithLastMessage]   
    @UserId UNIQUEIDENTIFIER  
AS  
BEGIN  
    WITH UserMessagesCTE AS (  
    SELECT  
        u.UserId,  
        u.FirstName + ' ' + u.LastName AS UserName,  
        m.MessageText,  
        m.CreateDate,  
        ROW_NUMBER() OVER (PARTITION BY u.UserId ORDER BY m.CreateDate DESC) AS RowNum  
    FROM  
        Tbl_Users u  
    LEFT JOIN  
        Tble_Messages m ON (u.UserId = m.SenderId AND m.ReceiverId = @UserId)  
                        OR (u.UserId = m.ReceiverId AND m.SenderId = @UserId)  
        
)  
SELECT  
    umc.UserId,  
    umc.UserName,  
    umc.MessageText,  
    umc.CreateDate,  
    CASE  
        WHEN umc.RowNum = 1 THEN 'Yes'  -- Has a chat with the logged-in user  
        ELSE 'No'                      -- No chat with the logged-in user  
    END AS HasChatWithLoggedInUser  
FROM  
    UserMessagesCTE umc  
WHERE  
    umc.UserId != @UserId  -- Records for the logged-in user  
    AND umc.RowNum = 1;   
  
END
GO
/****** Object:  StoredProcedure [dbo].[sp_proc_GetUserMessages]    Script Date: 07-03-2024 15:37:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_proc_GetUserMessages]     
    @rId UNIQUEIDENTIFIER,
    @sId UNIQUEIDENTIFIER
AS    
BEGIN    
    WITH LastMessageCTE AS (
    SELECT
        u.UserId,
        (u.FirstName + ' ' + u.LastName) As UserName ,
        m.MessageText,
		m.GroupName,
        m.CreateDate,
        ROW_NUMBER() OVER (PARTITION BY m.MessageText ORDER BY m.CreateDate DESC) AS RowNum
    FROM
        Tble_Messages m
    JOIN
        Tbl_Users u ON m.SenderId = u.UserId
    WHERE
        (m.SenderId = @sId AND m.ReceiverId = @rId)
        OR (m.SenderId = @rId AND m.ReceiverId = @sId)
)
SELECT
    UserId,
    UserName,
    MessageText,
	GroupName,
    CreateDate
FROM
    LastMessageCTE
WHERE
    RowNum = 1
ORDER BY
    CreateDate ASC;
   
    
END
GO
/****** Object:  StoredProcedure [dbo].[sp_proc_InsertMessage]    Script Date: 07-03-2024 15:37:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_proc_InsertMessage]
    @SenderId uniqueidentifier,
    @ReceiverId uniqueidentifier,
    @MessageText NVARCHAR(MAX),
    @GroupName NVARCHAR(MAX)
AS
BEGIN
    
	INSERT INTO Tble_Messages(SenderId, ReceiverId, MessageText, GroupName)
    VALUES (@SenderId, @ReceiverId, @MessageText, @GroupName);

END;
GO
/****** Object:  StoredProcedure [dbo].[sp_proc_UserLogin]    Script Date: 07-03-2024 15:37:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_proc_UserLogin] 
	@Email NVARCHAR(MAX),
	@Password NVARCHAR(MAX)
AS
BEGIN
	SELECT * FROM Tbl_Users WHERE EmailAddress = @Email AND Password = @Password;
END
GO
USE [master]
GO
ALTER DATABASE [ChatAppDB] SET  READ_WRITE 
GO
