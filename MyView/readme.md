## .net core 3.1 MVC framework with mysl entity

#### EF packages
	Microsoft.EntityFrameworkCore
	Microsoft.EntityFrameworkCore.Tools
	Microsoft.EntityFrameworkCore.Design
	Microsoft.EntityFrameworkCore.Relational
	Pomelo.EntityFrameworkCore.MySql
  
#### create EF entity command:
 Scaffold-DbContext -Provider Pomelo.EntityFrameworkCore.MySql -Context DBContext -Project Common -Connection "Server=192.168.10.100;User Id={id};Password={pw};Database={dbname}" -outputdir DBModel
