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
	nombreCompleto varchar(150) not null,
	email varchar(150) not null,
	rol varchar(150) not null,
	estado varchar(150) not null,
	primary key (nombreUsuario))
go

---------------------------------------------------------------------------
insert into [Usuario]
values ('stylesadmin','123','123', 'Kevin Ramirez','stylesadmin@gmail.com','Administrador','Activo')

delete from [Usuario]
select * from [Usuario]

insert into [Usuario]
values ('marjueladmin','987','987', 'Marcel Campos','marcel.fabri21@gmail.com','Administrador','Activo')

insert into [Usuario]
values ('nanaempleado','987','987', 'Dayanara Campos','dayanaracampos134@gmail.com','Empleado','Activo')

insert into [Usuario]
values ('fannyempleado','456','456', 'Fanny Murillo','fannymurillo34@yahoo.com','Empleado','Activo')

--Trigger para transformar los nombres de usuario
--que se registren en mayuscula
CREATE TRIGGER ToUpperCase
        ON [Usuario]
        AFTER INSERT, UPDATE
AS
BEGIN

    UPDATE  [Usuario]
    SET     NombreUsuario = UPPER(NombreUsuario)
    WHERE   NombreUsuario IN (SELECT NombreUsuario FROM inserted)

END