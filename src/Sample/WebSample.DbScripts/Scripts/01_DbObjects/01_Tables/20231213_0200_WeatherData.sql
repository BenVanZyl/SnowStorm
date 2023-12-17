IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.WeatherData') AND type in (N'U'))
BEGIN
	CREATE TABLE dbo.WeatherData
	(
		Id			bigint Identity NOT NULL,
		ReportId	bigint NOT NULL FOREIGN KEY REFERENCES WeatherReport (id),
		ForecastDate date NOT NULL,
		TemperatureC int  NOT NULL,
		Summary		varchar(255) NULL,
		CreatedOn	datetime NOT NULL,
		ModifiedOn	datetime NOT NULL,
		
		CONSTRAINT PK_WeatherData PRIMARY KEY NONCLUSTERED 
		(
			Id ASC
		)
	)
END