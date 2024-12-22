CREATE TABLE produktas
(
	id int,
	pavadinimas varchar (255) NOT NULL,
	kaina float NOT NULL,
	aprasas VARCHAR(MAX) NOT NULL,
	kiekis int NOT NULL,
	mase float NOT NULL,
	gamintojas varchar (255) NOT NULL,
	nuotraukos_url varchar (255) NOT NULL,
	fk_pardavėjas int NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Sukuria3 FOREIGN KEY(fk_pardavėjas) REFERENCES  pardavejas (id)
);