CREATE USER [webuser1] FOR LOGIN [webuser1] WITH DEFAULT_SCHEMA=[dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tActivity](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fLauncher] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fStartTime] [datetime] NULL,
	[fEndTime] [datetime] NULL,
	[fTitle] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fPhotoPath] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
 CONSTRAINT [PK_tActivity] PRIMARY KEY CLUSTERED 
(
	[fId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tAlbum](
	[fAlbumID] [int] IDENTITY(1,1) NOT NULL,
	[fAlbumName] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fMaker] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fAccount] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fYear] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fType] [int] NULL,
	[fStatus] [int] NULL,
	[fALPrice] [money] NULL,
	[fCoverPath] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fPublisher] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fKinds] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fDiscount] [real] NULL,
	[fActivityID] [int] NULL,
 CONSTRAINT [PK_tAlbum] PRIMARY KEY CLUSTERED 
(
	[fAlbumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tAlbumKind](
	[KindID] [int] IDENTITY(1,1) NOT NULL,
	[KindName] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fColor] [nchar](10) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fPhotoPath] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
 CONSTRAINT [PK_tProductKind] PRIMARY KEY CLUSTERED 
(
	[KindID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tAlbumType](
	[fTypeID] [int] IDENTITY(1,1) NOT NULL,
	[fTypeName] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fColor] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fPhotoPath] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
 CONSTRAINT [PK_tAlbumType] PRIMARY KEY CLUSTERED 
(
	[fTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tLog](
	[fLogId] [int] IDENTITY(1,1) NOT NULL,
	[fIssuerFrom] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fActionTo] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fCategory] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fMessage] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
 CONSTRAINT [PK_tLog] PRIMARY KEY CLUSTERED 
(
	[fLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tMember](
	[fAccount] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NOT NULL,
	[fPassword] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NOT NULL,
	[fEmail] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fPrivilege] [int] NULL,
	[fNickName] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fMoney] [money] NULL,
	[fPicPath] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fSubscriptStartDate] [datetime] NULL,
	[fSubscriptEndDate] [datetime] NULL,
	[fLastPlaySong] [int] NULL,
	[fLineId] [int] NULL,
	[fLineName] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fLineTimeMark] [datetime] NULL,
 CONSTRAINT [PK_tMember] PRIMARY KEY CLUSTERED 
(
	[fAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tMessage](
	[fMessageId] [int] IDENTITY(1,1) NOT NULL,
	[fAccountFrom] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fAccountTo] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fContent] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fTime] [datetime] NULL,
	[fStatus] [int] NULL,
	[fRead] [int] NULL,
	[fTitle] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
 CONSTRAINT [PK_tMessage] PRIMARY KEY CLUSTERED 
(
	[fMessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tPlayLists](
	[fPlayId] [int] IDENTITY(1,1) NOT NULL,
	[fAccount] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NOT NULL,
	[fProductID] [int] NOT NULL,
 CONSTRAINT [PK_tPlayLists] PRIMARY KEY CLUSTERED 
(
	[fPlayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tProducts](
	[fProductID] [int] IDENTITY(1,1) NOT NULL,
	[fAlbumID] [int] NULL,
	[fProductName] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fSinger] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fSIPrice] [money] NULL,
	[fComposer] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fArranger] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fLyricist] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fKind] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fMusic] [varbinary](max) NULL,
	[fFilePath] [nvarchar](max) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fPlayStart] [float] NULL,
	[fPlayEnd] [float] NULL,
	[fStatus] [int] NULL,
	[fDownloadCount] [int] NULL,
	[fModifiedDate] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fPlaybackCount] [int] NULL,
 CONSTRAINT [PK_tProducts] PRIMARY KEY CLUSTERED 
(
	[fProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tPurchaseItem](
	[fPurchaseItemID] [int] NOT NULL,
	[fCustomer] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fProductID] [int] NOT NULL,
	[fDate] [datetime] NULL,
	[fPrice] [money] NULL,
	[fQuanity] [int] NULL,
	[fisAlbum] [int] NULL,
	[fDiscount] [real] NULL,
 CONSTRAINT [PK_tPurchaseItem] PRIMARY KEY CLUSTERED 
(
	[fPurchaseItemID] ASC,
	[fProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tShoppingCart](
	[fCartID] [int] IDENTITY(1,1) NOT NULL,
	[fCustomer] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
	[fDate] [datetime] NULL,
	[fPrice] [money] NULL,
	[fType] [int] NULL,
 CONSTRAINT [PK_tShoppingCart] PRIMARY KEY CLUSTERED 
(
	[fCartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tStatistic](
	[fId] [int] IDENTITY(1,1) NOT NULL,
	[fProductID] [int] NULL,
	[fType] [int] NULL,
	[fTime] [datetime] NULL,
	[fAccount] [nvarchar](50) COLLATE Chinese_Taiwan_Stroke_CI_AS NULL,
 CONSTRAINT [PK_tStatistic] PRIMARY KEY CLUSTERED 
(
	[fId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
ALTER TABLE [dbo].[tAlbum] ADD  CONSTRAINT [DF_tAlbum_fStatus]  DEFAULT ((0)) FOR [fStatus]
GO
ALTER TABLE [dbo].[tAlbum] ADD  CONSTRAINT [DF_tAlbum_fDiscount]  DEFAULT ((1)) FOR [fDiscount]
GO
ALTER TABLE [dbo].[tMember] ADD  CONSTRAINT [DF_tMember_fPrivilege]  DEFAULT ((0)) FOR [fPrivilege]
GO
ALTER TABLE [dbo].[tProducts] ADD  CONSTRAINT [DF_tProducts_fDownloadCount]  DEFAULT ((0)) FOR [fDownloadCount]
GO
ALTER TABLE [dbo].[tPurchaseItem] ADD  CONSTRAINT [DF_tPurchaseItem_fPrice]  DEFAULT ((0)) FOR [fPrice]
GO
ALTER TABLE [dbo].[tPurchaseItem] ADD  CONSTRAINT [DF_tPurchaseItem_fQuanity]  DEFAULT ((1)) FOR [fQuanity]
GO
ALTER TABLE [dbo].[tPurchaseItem] ADD  CONSTRAINT [DF_tPurchaseItem_fDiscount]  DEFAULT ((1)) FOR [fDiscount]
GO
ALTER TABLE [dbo].[tActivity]  WITH CHECK ADD  CONSTRAINT [FK_tActivity_tMember] FOREIGN KEY([fLauncher])
REFERENCES [dbo].[tMember] ([fAccount])
GO
ALTER TABLE [dbo].[tActivity] CHECK CONSTRAINT [FK_tActivity_tMember]
GO
ALTER TABLE [dbo].[tAlbum]  WITH CHECK ADD  CONSTRAINT [FK_tAlbum_tActivity] FOREIGN KEY([fActivityID])
REFERENCES [dbo].[tActivity] ([fId])
GO
ALTER TABLE [dbo].[tAlbum] CHECK CONSTRAINT [FK_tAlbum_tActivity]
GO
ALTER TABLE [dbo].[tAlbum]  WITH CHECK ADD  CONSTRAINT [FK_tAlbum_tAlbumType] FOREIGN KEY([fType])
REFERENCES [dbo].[tAlbumType] ([fTypeID])
GO
ALTER TABLE [dbo].[tAlbum] CHECK CONSTRAINT [FK_tAlbum_tAlbumType]
GO
ALTER TABLE [dbo].[tAlbum]  WITH CHECK ADD  CONSTRAINT [FK_tAlbum_tMember] FOREIGN KEY([fAccount])
REFERENCES [dbo].[tMember] ([fAccount])
GO
ALTER TABLE [dbo].[tAlbum] CHECK CONSTRAINT [FK_tAlbum_tMember]
GO
ALTER TABLE [dbo].[tLog]  WITH CHECK ADD  CONSTRAINT [FK_tLog_tMember] FOREIGN KEY([fIssuerFrom])
REFERENCES [dbo].[tMember] ([fAccount])
GO
ALTER TABLE [dbo].[tLog] CHECK CONSTRAINT [FK_tLog_tMember]
GO
ALTER TABLE [dbo].[tLog]  WITH CHECK ADD  CONSTRAINT [FK_tLog_tMember1] FOREIGN KEY([fActionTo])
REFERENCES [dbo].[tMember] ([fAccount])
GO
ALTER TABLE [dbo].[tLog] CHECK CONSTRAINT [FK_tLog_tMember1]
GO
ALTER TABLE [dbo].[tMessage]  WITH CHECK ADD  CONSTRAINT [FK_tMessage_tMember] FOREIGN KEY([fAccountFrom])
REFERENCES [dbo].[tMember] ([fAccount])
GO
ALTER TABLE [dbo].[tMessage] CHECK CONSTRAINT [FK_tMessage_tMember]
GO
ALTER TABLE [dbo].[tMessage]  WITH CHECK ADD  CONSTRAINT [FK_tMessage_tMember1] FOREIGN KEY([fAccountTo])
REFERENCES [dbo].[tMember] ([fAccount])
GO
ALTER TABLE [dbo].[tMessage] CHECK CONSTRAINT [FK_tMessage_tMember1]
GO
ALTER TABLE [dbo].[tPlayLists]  WITH CHECK ADD  CONSTRAINT [FK_tPlayLists_tMember] FOREIGN KEY([fAccount])
REFERENCES [dbo].[tMember] ([fAccount])
GO
ALTER TABLE [dbo].[tPlayLists] CHECK CONSTRAINT [FK_tPlayLists_tMember]
GO
ALTER TABLE [dbo].[tPlayLists]  WITH CHECK ADD  CONSTRAINT [FK_tPlayLists_tProducts] FOREIGN KEY([fProductID])
REFERENCES [dbo].[tProducts] ([fProductID])
GO
ALTER TABLE [dbo].[tPlayLists] CHECK CONSTRAINT [FK_tPlayLists_tProducts]
GO
ALTER TABLE [dbo].[tProducts]  WITH CHECK ADD  CONSTRAINT [FK_tProducts_tAlbum] FOREIGN KEY([fAlbumID])
REFERENCES [dbo].[tAlbum] ([fAlbumID])
GO
ALTER TABLE [dbo].[tProducts] CHECK CONSTRAINT [FK_tProducts_tAlbum]
GO
ALTER TABLE [dbo].[tPurchaseItem]  WITH CHECK ADD  CONSTRAINT [FK_tPurchaseItem_tMember1] FOREIGN KEY([fCustomer])
REFERENCES [dbo].[tMember] ([fAccount])
GO
ALTER TABLE [dbo].[tPurchaseItem] CHECK CONSTRAINT [FK_tPurchaseItem_tMember1]
GO
ALTER TABLE [dbo].[tPurchaseItem]  WITH CHECK ADD  CONSTRAINT [FK_tPurchaseItem_tProducts1] FOREIGN KEY([fProductID])
REFERENCES [dbo].[tProducts] ([fProductID])
GO
ALTER TABLE [dbo].[tPurchaseItem] CHECK CONSTRAINT [FK_tPurchaseItem_tProducts1]
GO
ALTER TABLE [dbo].[tPurchaseItem]  WITH CHECK ADD  CONSTRAINT [FK_tPurchaseItem_tShoppingCart] FOREIGN KEY([fPurchaseItemID])
REFERENCES [dbo].[tShoppingCart] ([fCartID])
GO
ALTER TABLE [dbo].[tPurchaseItem] CHECK CONSTRAINT [FK_tPurchaseItem_tShoppingCart]
GO
ALTER TABLE [dbo].[tShoppingCart]  WITH CHECK ADD  CONSTRAINT [FK_tShoppingCart_tMember] FOREIGN KEY([fCustomer])
REFERENCES [dbo].[tMember] ([fAccount])
GO
ALTER TABLE [dbo].[tShoppingCart] CHECK CONSTRAINT [FK_tShoppingCart_tMember]
GO
ALTER TABLE [dbo].[tStatistic]  WITH CHECK ADD  CONSTRAINT [FK_tStatistic_tProducts] FOREIGN KEY([fProductID])
REFERENCES [dbo].[tProducts] ([fProductID])
GO
ALTER TABLE [dbo].[tStatistic] CHECK CONSTRAINT [FK_tStatistic_tProducts]
GO
