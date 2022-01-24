USE [RabobankTraining]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ModelPortfolioLine](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[extRef] [int] NULL,
	[secRef] [varchar](12) NULL,
	[secName] [varchar](50) NULL,
	[percComp] [decimal](38, 7) NULL,
	[rate] [decimal](38, 7) NULL,
	[refPos] [decimal](38, 7) NULL,
	[LstDynmcRblRtDt] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


