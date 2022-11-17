
CREATE DATABASE Banco;
use Banco;


Create Table usuario(
usuario_id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
dni int NOT NULL,
nombre varchar(45) NOT NULL,
apellido varchar(45) NOT NULL,
mail varchar(45) ,
usuario varchar(45) NOT NULL,
contrasenia varchar(45) NOT NULL,
intentos_fallidos int NOT NULL,
bloqueado bit NOT NULL,
administrador bit,
);



Create table tarjeta_de_credito(
tarjeta_id int IDENTITY(1,1) Primary key NOT NULL,
numero int not null,
codigov int not null,
limite real not null,
consumos real not null,
usuario_fk int
);


create table plazo_fijo(
plazo_id int IDENTITY(1,1) primary key not null,
monto real not null,
fecha_ini date ,
fecha_fin date,
tasa real,
pagado bit,
usuario_fk int
);

create table caja_de_ahorro(
caja_id int IDENTITY(1,1) primary key not null,
cbu int not null,
saldo real not null,
);

create table movimiento(
movimiento_id int  IDENTITY(1,1) primary key not null,
detalle varchar(45) not null,
monto real not null,
fecha date not null,
caja_fk int
);

create table pago(
pago_id int  IDENTITY(1,1) primary key not null,
monto real not null,
nombre varchar(45),
pagado bit ,
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
FOREIGN KEY(usuario_fk) REFERENCES  usuario (usuario_id);

ALTER TABLE  plazo_fijo ADD
FOREIGN KEY(usuario_fk) REFERENCES  usuario (usuario_id);

ALTER TABLE  tarjeta_de_credito ADD
FOREIGN KEY(usuario_fk) REFERENCES  usuario (usuario_id);


GO