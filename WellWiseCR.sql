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

if exists (select name from dbo.sysobjects where name='Especialista')
drop table [Especialista]
go
create table [Especialista](
	IdEspecialista int not null,
	IdEspecialidad int not null,
	Email  varchar(150) not null,
	NombreCompleto varchar(150) not null,
	Provincia varchar(150) not null,
	Canton varchar(150) not null,
	Estado varchar(150) not null,
	primary key (IdEspecialista))
go
alter table Especialista
	add foreign key (IdEspecialidad)
	references Especialidad(IdEspecialidad);
go


if exists (select name from dbo.sysobjects where name='Enfermedad')
drop table [Enfermedad]
go
create table [Enfermedad](
	IdEnfermedad int not null,
	IdEspecialidad int not null,
	NombreEnfermedad varchar(150) not null,
	Sintomas varchar(1000) not null,
	NivelAlerta varchar(150) not null,
	Recomendaciones varchar(1000) not null,
	Estado varchar(150) not null,
	primary key (IdEnfermedad))
go
alter table Enfermedad
	add foreign key (IdEspecialidad)
	references Especialidad(IdEspecialidad);
go

if exists (select name from dbo.sysobjects where name='Diagnostico')
drop table [Diagnostico]
go
create table [Diagnostico](
	IdDiagnostico int not null,
	NombreUsuario varchar (150) not null,
	FechaHora datetime not null,
	Peso varchar(150) not null,
	Estatura varchar(150) not null,
	ActividadFisica varchar(150) not null,
	CondicionCardiaca varchar(150) not null,
	Estado varchar(150) not null,
	primary key (IdDiagnostico))
go
alter table [Diagnostico]
	add foreign key (NombreUsuario)
	references Usuario(NombreUsuario);
go

if exists (select name from dbo.sysobjects where name='Detalle')
drop table [Detalle]
create table [Detalle](
	IdDiagnostico int not null,
	IdEnfermedad int not null)
go
alter table [Detalle]
	add foreign key (IdDiagnostico)
	references Diagnostico(IdDiagnostico);
	alter table [Detalle]
	add foreign key (IdEnfermedad)
	references Enfermedad(IdEnfermedad);
go

---------------------------------------------------------------------------
insert into [Usuario]
values ('marjueladmin','11111111','11111111', 'marcel.fabri21@gmail.com', 'Marcel Campos', '26/11/2001', 'Alajuela', 'Grecia','Administrador','Activo')

insert into [Usuario]
values ('Paciente1','11111111','11111111', 'marcel.fabri21@gmail.com', 'Paciente Uno', '30/01/1980', 'Puntarenas', 'Esparza','Paciente','Activo')

insert into [Especialidad]
values (1, 'Cardiología', 'Se especializa en las enfermedades relacionadas con el corazón', 'Activo')
insert into [Especialidad]
values (2, 'Oftalmología', 'Se especializa en las enfermedades relacionadas con la vista', 'Activo')
insert into [Especialidad]
values (3, 'Nefrología', 'Se especializa en las enfermedades relacionadas con los riñones', 'Activo')

insert into [Especialidad]
values (300, 'Neumologia', 'Se especializa en las enfermedades relacionadas con los pulmones', 'Activo')

insert into [Especialidad] (NombreEspecialidad, Descripcion, Estado)
values ('Neumologia', 'Se especializa en las enfermedades relacionadas con los pulmones', 'Activo')

insert into [Especialista] 
values (51, 2, 'Dr. Kenzo Tenma', 'tenma@email.com', 'Alajuela', 'Grecia', 'Activo')

insert into Diagnostico values (1, 'PACIENTE1', '10/05/2023 19:38:15','70kg', '170cm', 'Alta', 'Saludable', 'Activo')

insert into Detalle values (5, 1)
insert into Detalle values (5, 2)

delete from diagnostico
select * from Diagnostico
select max(iddiagnostico)+1 from diagnostico

--delete from [Usuario]
select * from [Usuario]

select SYSDATETIME ( )

delete from especialidad
select * from especialidad

select * from especialista
delete from especialista

select * from Enfermedad
delete from Enfermedad

select * from diagnostico
delete from diagnostico
delete from diagnostico where idDiagnostico = 1

select * from detalle order by IdDiagnostico
delete from detalle where idDiagnostico = 1

select count(*)+1 from especialidad


