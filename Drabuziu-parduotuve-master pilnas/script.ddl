--@(#) script.ddl

CREATE TABLE 2_esybių_klasės.kategorija
(
	id int NOT NULL,
	pavadinimas varchar (255) NOT NULL,
	aprasas character large object NOT NULL,
	fk_tevinekategorija int,
	PRIMARY KEY(id),
	CONSTRAINT Priklauso3 FOREIGN KEY(fk_tevinekategorija) REFERENCES 2_esybių_klasės.kategorija (id)
);

CREATE TABLE 2_esybių_klasės.lytis
(
	id int,
	name varchar (255) NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO lytis(id, name) VALUES(1, 'vyras');
INSERT INTO lytis(id, name) VALUES(2, 'moteris');
INSERT INTO lytis(id, name) VALUES(3, 'kita');

CREATE TABLE 2_esybių_klasės.mokėjimostatusas
(
	id int,
	name varchar (255),
	PRIMARY KEY(id)
);

CREATE TABLE 2_esybių_klasės.mokejimotipas
(
	id int,
	name varchar (255) NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO mokejimotipas(id, name) VALUES(1, 'kortelė+parduotuvės_kreditas');
INSERT INTO mokejimotipas(id, name) VALUES(2, 'parduotuvės_kreditas');
INSERT INTO mokejimotipas(id, name) VALUES(3, 'kortelė');
INSERT INTO mokejimotipas(id, name) VALUES(4, 'paypal');
INSERT INTO mokejimotipas(id, name) VALUES(5, 'paypal+parduotuvės_kreditas');

CREATE TABLE 2_esybių_klasės.nuolaidosstatusas
(
	id int,
	name varchar (255) NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO nuolaidosstatusas(id, name) VALUES(1, 'suplanuotas');
INSERT INTO nuolaidosstatusas(id, name) VALUES(2, 'aktyvus');
INSERT INTO nuolaidosstatusas(id, name) VALUES(3, 'pasibaigęs');

CREATE TABLE 2_esybių_klasės.nuolaidossukūrimobūdas
(
	id int,
	name varchar (255) NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO nuolaidossukūrimobūdas(id, name) VALUES(1, 'automatinis');
INSERT INTO nuolaidossukūrimobūdas(id, name) VALUES(2, 'rankinis');

CREATE TABLE 2_esybių_klasės.paymenttype
(
	id,
	name NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO paymenttype(id, name) VALUES('1', 'emplyee');
INSERT INTO paymenttype(id, name) VALUES('2', 'client');
INSERT INTO paymenttype(id, name) VALUES('3', 'product');

CREATE TABLE 2_esybių_klasės.role
(
	id,
	name NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO role(id, name) VALUES('1', 'buyer');
INSERT INTO role(id, name) VALUES('2', 'seller');
INSERT INTO role(id, name) VALUES('3', 'administrator');

CREATE TABLE 2_esybių_klasės.sandelis
(
	id int,
	pavadinimas varchar (255) NOT NULL,
	vieta varchar (255),
	PRIMARY KEY(id)
);

CREATE TABLE 2_esybių_klasės.vartotojas
(
	age int,
	id int NOT NULL,
	vardas varchar (255),
	pavarde varchar (255),
	slaptazodis varchar (255),
	telefononumeris varchar (255),
	PRIMARY KEY(id)
);

CREATE TABLE 2_esybių_klasės.siuntimostatusas
(
	id int,
	name varchar (255) NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO siuntimostatusas(id, name) VALUES(1, 'laukiama');
INSERT INTO siuntimostatusas(id, name) VALUES(2, 'išsiųsta');
INSERT INTO siuntimostatusas(id, name) VALUES(3, 'priduota');
INSERT INTO siuntimostatusas(id, name) VALUES(4, 'atšaukta');

CREATE TABLE 2_esybių_klasės.administratorius
(
	id int NOT NULL,
	arvadovas boolean,
	atlyginimas double precision,
	kortelesnumeris varchar (255),
	PRIMARY KEY(id),
	FOREIGN KEY(id) REFERENCES 2_esybių_klasės.vartotojas (id)
);

CREATE TABLE 2_esybių_klasės.nuolaidoskodas
(
	id int,
	sukurimodata date,
	veikimopradziosdata date NULL,
	panaudojimuskaičius int NULL,
	kodas varchar (255),
	verte float,
	aprasymas varchar (255) NULL,
	pavadinimas varchar (255) NULL,
	galiojimopabaigosdata date NULL,
	yraribotas boolean,
	statusas int,
	sukurimobudas int,
	PRIMARY KEY(id),
	FOREIGN KEY(statusas) REFERENCES 2_esybių_klasės.nuolaidosstatusas (id),
	FOREIGN KEY(sukurimobudas) REFERENCES 2_esybių_klasės.nuolaidossukūrimobūdas (id)
);

CREATE TABLE 2_esybių_klasės.pardavejas
(
	id int NOT NULL,
	vieta varchar (255),
	PRIMARY KEY(id),
	FOREIGN KEY(id) REFERENCES 2_esybių_klasės.vartotojas (id)
);

CREATE TABLE 2_esybių_klasės.atlyginimas
(
	mokestis double precision,
	bonusas double precision,
	fk_administratorius int NOT NULL,
	fk_administratorius1 int NOT NULL,
	PRIMARY KEY(fk_administratorius, fk_administratorius1),
	CONSTRAINT Gauna1 FOREIGN KEY(fk_administratorius) REFERENCES 2_esybių_klasės.administratorius (id),
	CONSTRAINT Moka FOREIGN KEY(fk_administratorius1) REFERENCES 2_esybių_klasės.administratorius (id)
);

CREATE TABLE 2_esybių_klasės.pirkejas
(
	age int,
	id int NOT NULL,
	gimimodata date,
	vieta varchar (255),
	parduotuveskreditas double precision,
	lytis int,
	fk_nuolaidoskodas int NOT NULL,
	PRIMARY KEY(id),
	UNIQUE(fk_nuolaidoskodas),
	FOREIGN KEY(lytis) REFERENCES 2_esybių_klasės.lytis (id),
	CONSTRAINT priklauso4 FOREIGN KEY(fk_nuolaidoskodas) REFERENCES 2_esybių_klasės.nuolaidoskodas (id),
	FOREIGN KEY(id) REFERENCES 2_esybių_klasės.vartotojas (id)
);

CREATE TABLE 2_esybių_klasės.produktas
(
	id int,
	pavadinimas varchar (255) NOT NULL,
	kaina float NOT NULL,
	aprasas character large object NOT NULL,
	kiekis int NOT NULL,
	mase float NOT NULL,
	gamintojas varchar (255) NOT NULL,
	nuotraukos_url varchar (255) NOT NULL,
	fk_pardavėjas int NOT NULL,
	PRIMARY KEY(id),
	UNIQUE(null),
	CONSTRAINT Sukuria3 FOREIGN KEY(fk_pardavėjas) REFERENCES 2_esybių_klasės.pardavejas (id)
);

CREATE TABLE 2_esybių_klasės.nuolaidoskodas_produktas
(
	minkiekis int,
	fk_produktas int NOT NULL,
	fk_nuolaidoskodas int NOT NULL,
	PRIMARY KEY(fk_nuolaidoskodas, fk_produktas),
	CONSTRAINT priklauso2 FOREIGN KEY(fk_produktas) REFERENCES 2_esybių_klasės.produktas (id),
	CONSTRAINT priklauso5 FOREIGN KEY(fk_nuolaidoskodas) REFERENCES 2_esybių_klasės.nuolaidoskodas (id)
);

CREATE TABLE 2_esybių_klasės.sandelisproduktas
(
	fk_sandelis int NOT NULL,
	fk_produktas int NOT NULL,
	PRIMARY KEY(fk_produktas, fk_sandelis),
	CONSTRAINT dalis_jo FOREIGN KEY(fk_sandelis) REFERENCES 2_esybių_klasės.sandelis (id),
	CONSTRAINT r FOREIGN KEY(fk_produktas) REFERENCES 2_esybių_klasės.produktas (id)
);

CREATE TABLE 2_esybių_klasės.uzsakymas
(
	id int NOT NULL,
	kiekis int,
	verte double precision,
	pradzia date,
	pabaiga date,
	siuntimoadresas varchar (255),
	fk_pirkejas int NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Sukuria2 FOREIGN KEY(fk_pirkejas) REFERENCES 2_esybių_klasės.pirkejas (id)
);

CREATE TABLE 2_esybių_klasės.kategorijaProduktas
(
	fk_kategorija int NOT NULL,
	fk_produktas int NOT NULL,
	PRIMARY KEY(fk_kategorija, fk_produktas),
	CONSTRAINT Priklauso1 FOREIGN KEY(fk_kategorija) REFERENCES 2_esybių_klasės.kategorija (id),
	CONSTRAINT Priklauso6 FOREIGN KEY(fk_produktas) REFERENCES 2_esybių_klasės.produktas (id)
);

CREATE TABLE 2_esybių_klasės.mokestis
(
	count int,
	id int NOT NULL,
	mokestis double precision,
	fk_mokejimotipas int NOT NULL,
	fk_mokejimostatusas int NOT NULL,
	fk_pirkejas int NOT NULL,
	fk_uzsakymas int NOT NULL,
	fk_pirkejas1 int NOT NULL,
	PRIMARY KEY(id),
	UNIQUE(fk_mokejimotipas),
	UNIQUE(fk_mokejimostatusas),
	UNIQUE(fk_uzsakymas),
	FOREIGN KEY(fk_mokejimostatusas) REFERENCES 2_esybių_klasės.mokėjimostatusas (id),
	CONSTRAINT Gauna2 FOREIGN KEY(fk_pirkejas) REFERENCES 2_esybių_klasės.pirkejas (id),
	CONSTRAINT sukuria1 FOREIGN KEY(fk_uzsakymas) REFERENCES 2_esybių_klasės.uzsakymas (id),
	CONSTRAINT Moka2 FOREIGN KEY(fk_pirkejas1) REFERENCES 2_esybių_klasės.pirkejas (id)
);

CREATE TABLE 2_esybių_klasės.uzsakymasproduktas
(
	kaina float,
	fk_produktas int NOT NULL,
	fk_uzsakymas int NOT NULL,
	PRIMARY KEY(fk_produktas, fk_uzsakymas),
	CONSTRAINT prideda1 FOREIGN KEY(fk_produktas) REFERENCES 2_esybių_klasės.produktas (id),
	CONSTRAINT duoda1 FOREIGN KEY(fk_uzsakymas) REFERENCES 2_esybių_klasės.uzsakymas (id)
);
