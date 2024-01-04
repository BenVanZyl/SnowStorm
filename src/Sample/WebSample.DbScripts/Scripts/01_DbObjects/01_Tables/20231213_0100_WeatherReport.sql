IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.WeatherReport') AND type in (N'U'))
BEGIN
	CREATE TABLE dbo.WeatherReport
	(
		Id bigint Identity NOT NULL,
		ReportName varchar(50) NOT NULL,
		CreatedOn	datetime NOT NULL,
		ModifiedOn	datetime NOT NULL,
		
		CONSTRAINT PK_WeatherReport PRIMARY KEY NONCLUSTERED 
		(
			Id ASC
		)
	)
END