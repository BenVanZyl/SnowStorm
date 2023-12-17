Begin
    Declare @dbNameRoot nvarchar(1024) = '$(DatabaseNameRoot)',
			@dbNameToday nvarchar(1024) = '$(DatabaseNameToday)',
			@dbName nvarchar(1024) = '$(DatabaseName)',
			@dbNameValue nvarchar(1024) = '',
            @cmd    nvarchar(2048) = '',
			@id		int,
			@rowCount int
			
	Declare @data table(Id int identity, dbname varchar(1024))
	Insert Into @data
		Select name from sys.databases 
		Where name = @dbName
		   or (name like @dbNameRoot+'%' and NOT (name like @dbNameToday+'%'))

	Select @id = 1, @rowCount = Count(*) From @data
	While (@id <= @rowCount)
	Begin
		Select @dbNameValue = dbname From @data
			Where Id = @id

		PRINT '## Dropping Database: [' + @dbNameValue + '] ...' 

		PRINT '    * SET SINGLE_USER ...'
		Set @cmd = 'ALTER DATABASE [' + @dbNameValue + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;'
		EXEC sys.sp_executesql @cmd

		PRINT '    * DROP DATABASE ...'
		Set @cmd = 'DROP DATABASE [' + @dbNameValue + '];'                             
		EXEC sys.sp_executesql @cmd

		PRINT '## Dropping Database: [' + @dbNameValue + '] Done.  Success.' 
	

		Set @id = @id + 1
	End


End
