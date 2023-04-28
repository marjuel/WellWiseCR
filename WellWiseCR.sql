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
	NombreUsuario varchar (150) not null,
	Password varchar (150) not null,
	ConfirmacionPassword varchar (150) not null,
	Email varchar (150) not null,
	NombreCompleto varchar(150) not null,
	FechaNacimiento date not null,
	Provincia varchar(150) not null,
	Canton varchar(150) not null,
	Rol varchar(150) not null,
	Estado varchar(150) not null,
	primary key (NombreUsuario))
go
--Trigger para transformar los nombres de usuario
--que se registren en mayuscula
CREATE TRIGGER ToUpperCase
        ON [Usuario]
        after INSERT
AS
BEGIN

    UPDATE  [Usuario]
    SET     NombreUsuario = UPPER(NombreUsuario)
    WHERE   NombreUsuario IN (SELECT NombreUsuario FROM inserted)

END

---------------------------------------------------------------------------
insert into [Usuario]
values ('marjueladmin','11111111','11111111', 'marcel.fabri21@gmail.com', 'Marcel Campos', '26/11/2001', 'Alajuela', 'Grecia','Administrador','Activo')

insert into [Usuario]
values ('Paciente1','11111111','11111111', 'marcel.fabri21@gmail.com', 'Paciente Uno', '30/01/1980', 'Puntarenas', 'Esparza','Paciente','Activo')



--delete from [Usuario] where nombreUsuario = 'choque'
select * from [Usuario]



