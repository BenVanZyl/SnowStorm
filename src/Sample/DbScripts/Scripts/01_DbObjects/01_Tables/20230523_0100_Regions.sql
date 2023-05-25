IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Region]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Region]
	(
		[RegionID] [int] NOT NULL Identity,
		[RegionDescription] [nchar](50) NOT NULL,
		 CONSTRAINT [PK_Region] PRIMARY KEY NONCLUSTERED 
		(
			[RegionID] ASC
		)
	)
END