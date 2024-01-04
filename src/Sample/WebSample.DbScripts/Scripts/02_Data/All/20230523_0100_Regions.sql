--Begin

--	Declare @data table ([RegionID] [int] NOT NULL, [RegionDescription] [nchar](50) NOT NULL)
--	Insert into @data(RegionID, RegionDescription) Values
--		(1,	'Eastern'),                                           
--		(2,	'Western'),                                          
--		(3,	'Northern'),
--		(4,	'Southern')

--	SET IDENTITY_INSERT dbo.Region ON

--	Insert Into dbo.Region (RegionID, RegionDescription)
--		Select d.RegionID, d.RegionDescription
--		From @data d
--		Where NOT EXISTS(
--				Select 1 From dbo.Region r Where r.RegionID = d.RegionID
--		)

--	SET IDENTITY_INSERT dbo.Region OFF

--End