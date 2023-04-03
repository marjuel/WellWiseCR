--Si existe la base de datos DBWellWiseCR borrela y creela nuevamente
if exists (select name from dbo.sysdatabases where name ='DBWellWiseCR')
drop database [DBWellWiseCR]
go
create database [DBWellWiseCR]
go

--Utilice la base de datos DBWellWiseCR
use [DBWellWiseCR]
go

--Si existe la tabla Usuario borrela y creela nuevamente
if exists (select name from dbo.sysobjects where name='Usuario')
drop table [Usuario]
go
create table [Usuario](
	nombreUsuario varchar(150) not null,
	password varchar(150) not null,
	confirmacionPassword varchar(150) not null,
	email varchar(150) not null,
	rol varchar(150) not null,
	estado varchar(150) not null,
	primary key (nombreUsuario))
go