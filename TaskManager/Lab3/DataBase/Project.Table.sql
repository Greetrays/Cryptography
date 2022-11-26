CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProjectName] NCHAR(10) NULL, 
    [CastomerCompany] NCHAR(10) NULL, 
    [ContractorCompany] NCHAR(10) NULL, 
    [ProjectManager] NCHAR(10) NULL, 
    [ProjectExecutors] NCHAR(10) NULL, 
    [StartDate] NCHAR(10) NULL, 
    [EndDate] NCHAR(10) NULL, 
    [Priority] NCHAR(10) NULL
)
