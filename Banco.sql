
CREATE DATABASE Banco;
use Banco;


Create Table usuario(
usuario_id int  IDENTITY(1,1) PRIMARY KEY NOT NULL,
dni int NOT NULL,
nombre varchar(45) NOT NULL,
apellido varchar(45) NOT NULL,
mail varchar(45) ,
contrasenia varchar(45) NOT NULL,
intentos_fallidos int NOT NULL,
bloqueado bit NOT NULL,
admin bit,
);



Create table tarjeta_de_credito(
tarjeta_id int  IDENTITY(1,1) Primary key NOT NULL,
numero int not null,
codigov int not null,
limite float not null,
consumos float not null,
usuario_fk int
);


create table plazo_fijo(
plazo_id int IDENTITY(1,1) primary key not null,
monto float not null,
fecha_ini date not null,
fecha_fin date,
tasa float,
pagado bit,
usuario_fk int
);

create table caja_de_ahorro(
caja_id int  IDENTITY(1,1) primary key not null,
cbu int not null,
saldo float,
);

create table movimiento(
movimiento_id int  IDENTITY(1,1) primary key not null,
detalle varchar(45) not null,
monto float not null,
fecha date not null,
caja_fk int
);

create table pago(
pago_id int  IDENTITY(1,1) primary key not null,
monto float not null,
nombre varchar(45),
pagado bit not null,
metodo varchar(45),
usuario_fk int
);

create table caja_usuario(
caja_usuario_id int  IDENTITY(1,1) primary key not null,
caja_fk int ,
usuario_fk int 
);





ALTER TABLE caja_usuario ADD 
FOREIGN KEY (caja_fk) REFERENCES caja_de_ahorro (caja_id),
FOREIGN KEY (usuario_fk) REFERENCES usuario (usuario_id);


ALTER TABLE movimiento ADD
FOREIGN KEY(caja_fk) REFERENCES caja_de_ahorro (caja_id);


ALTER TABLE  pago ADD
FOREIGN KEY(usuario_fk) REFERENCES  pago (pago_id);

ALTER TABLE  plazo_fijo ADD
FOREIGN KEY(usuario_fk) REFERENCES  plazo_fijo (plazo_id);

ALTER TABLE  tarjeta_de_credito ADD
FOREIGN KEY(usuario_fk) REFERENCES  tarjeta_de_credito (tarjeta_id);


GO