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


if exists (select name from dbo.sysobjects where name='Especialidad')
drop table [Especialidad]
go
create table [Especialidad](
	IdEspecialidad int not null,
	NombreEspecialidad varchar(150) not null,
	Descripcion varchar (1000) not null,
	Estado varchar(150) not null,
	primary key (IdEspecialidad))
go

---------------------------------------------------------------------------
insert into [Usuario]
values ('marjueladmin','11111111','11111111', 'marcel.fabri21@gmail.com', 'Marcel Campos', '26/11/2001', 'Alajuela', 'Grecia','Administrador','Activo')

insert into [Usuario]
values ('Paciente1','11111111','11111111', 'marcel.fabri21@gmail.com', 'Paciente Uno', '30/01/1980', 'Puntarenas', 'Esparza','Paciente','Activo')

insert into [Especialidad]
values (1, 'Cardiolog�a', 'Se especializa en las enfermedades relacionadas con el coraz�n', 'Activo')
insert into [Especialidad]
values (2, 'Oftalmolog�a', 'Se especializa en las enfermedades relacionadas con la vista', 'Activo')
insert into [Especialidad]
values (3, 'Nefrolog�a', 'Se especializa en las enfermedades relacionadas con los ri�ones', 'Activo')

--delete from [Usuario] where nombreUsuario = 'choque'
select * from [Usuario]


select * from especialidad

select count(*)+1 from especialidad


