IF DB_ID('NLogDb') is null
BEGIN
	CREATE DATABASE [NLogDb]
	PRINT 'Logging database created...'
END

USE NLogDb

if not exists (Select * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'NLog')
BEGIN
	PRINT 'Creating Log Table'
	CREATE TABLE [dbo].[NLog] (
	   [ID] [int] IDENTITY(1,1) NOT NULL,
	   [Callsite] [nvarchar](max) NOT NULL,
	   [Logged] [datetime] NOT NULL,
	   [Level] [varchar](5) NOT NULL,
	   [Message] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED ([ID] ASC) 
	   WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
END
IF NOT EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'NLog_AddEntry_p')
                    AND type IN ( N'P', N'PC' ) ) 
BEGIN
	declare @proc_string varchar(max)
	set @proc_string = 'CREATE PROCEDURE [dbo].[NLog_AddEntry_p] (
	  @callsite nvarchar(max),
	  @logged datetime,
	  @level varchar(5),
	  @message nvarchar(max)
	) AS
	BEGIN
	  INSERT INTO [dbo].[NLog] (
		[Callsite],
		[Logged],
		[Level],
		[Message]
	  ) VALUES (
	    @callsite,
		@logged,
		@level,
		@message
	  );
	END'
	EXEC(@proc_string)
	PRINT 'logging procedure created...'
END