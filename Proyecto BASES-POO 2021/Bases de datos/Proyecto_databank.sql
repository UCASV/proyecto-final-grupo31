-- Creando base de datos y seleccionandola para realizar consultas 

CREATE DATABASE VaccinationDB;
USE VaccinationDB; 				
SET LANGUAGE us_english;

-- Creación de tablas

CREATE TABLE Citizen(
	Id INT PRIMARY KEY IDENTITY,
	Dui CHAR(10) NOT NULL,
	FullName VARCHAR(75) NOT NULL,
	HomeAddress VARCHAR(100) NOT NULL,
	PhoneNumber CHAR(8) NOT NULL,
	EmailAddress VARCHAR(75) NULL,
	IdInstitution INT NULL,
	InstitutionIdentification VARCHAR(15) NULL,
	IdPriorityGroup INT NOT NULL
); 

CREATE TABLE Appointment(
	Id INT PRIMARY KEY IDENTITY,
	AppointmentDateTime DATETIME NOT NULL,
	IdVaccinationPlace INT NOT NULL,
	ArrivalDateTime DATETIME NULL,
	VaccinationDateTime DATETIME NULL,
	IdAppointmentType INT NOT NULL,
	IdManager INT NOT NULL,
	IdCitizen INT NOT NULL
);

CREATE TABLE Booth(
	Id INT PRIMARY KEY IDENTITY,
	BoothAddress VARCHAR(100) NOT NULL,
	PhoneNumber CHAR(8) NOT NULL,
	EmailAddress VARCHAR(75) NOT NULL
);

CREATE TABLE Employee(
	Id INT PRIMARY KEY IDENTITY,
	FullName VARCHAR(75) NOT NULL,
	HomeAddress VARCHAR(100) NOT NULL,
	IdEmployeeType INT NOT NULL,
	IdWorkedBooth INT NOT NULL,
	IdManagedBooth INT NULL,
	IdManager INT NULL   
);

CREATE TABLE Manager(
	Id INT PRIMARY KEY IDENTITY,
	Username VARCHAR(15) NOT NULL,
	KeyCode VARCHAR(15) NOT NULL,
);

CREATE TABLE LogInHistory(
	Id INT PRIMARY KEY IDENTITY,
	LogInDateTime DATETIME NOT NULL,
	IdManager INT NOT NULL
);

CREATE TABLE Disease(
	Id INT PRIMARY KEY IDENTITY,
	Disease VARCHAR(50) NOT NULL,
	IdCitizen INT NOT NULL
);

CREATE TABLE Institution(
	Id INT PRIMARY KEY IDENTITY,
	InstitutionName Varchar(50) NOT NULL
);

CREATE TABLE PriorityGroup(
	Id INT PRIMARY KEY IDENTITY,
	PriorityGroup Varchar(50) NOT NULL
);

CREATE TABLE VaccinationPlace(
	Id INT PRIMARY KEY IDENTITY,
	Place Varchar(50) NOT NULL
);

CREATE TABLE AppointmentType(
	Id INT PRIMARY KEY IDENTITY,
	AppointmentType VARCHAR(50) NOT NULL
);

CREATE TABLE EmployeeType(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeType Varchar(50) NOT NULL
);

CREATE TABLE SideEffect(
	Id INT PRIMARY KEY IDENTITY,
	SideEffect VARCHAR(50) NOT NULL
);

CREATE TABLE VaccineReaction(
	IdAppointment INT NOT NULL,
	IdSideEffect INT NOT NULL,
	AppearenceTime INT NOT NULL,
	CONSTRAINT PK_FirstVaccineReaction PRIMARY KEY (IdAppointment,IdSideEffect)
);

-- Definicion de llaves foraneas

-- Citizen -> Disease
ALTER TABLE Disease ADD FOREIGN KEY (IdCitizen) REFERENCES Citizen (id);
-- Institution -> Citizen
ALTER TABLE Citizen ADD FOREIGN KEY (IdInstitution) REFERENCES Institution (id);
-- PriorityGroup -> Citizen
ALTER TABLE Citizen ADD FOREIGN KEY (IdPriorityGroup) REFERENCES PriorityGroup (id);
-- Citizen -> Appointment
ALTER TABLE Appointment ADD FOREIGN KEY (IdCitizen) REFERENCES Citizen (id);
-- Appointment -> VaccineReaction
ALTER TABLE VaccineReaction ADD FOREIGN KEY (IdAppointment) REFERENCES Appointment (id);
-- SideEffect -> VaccineReaction
ALTER TABLE VaccineReaction ADD FOREIGN KEY (IdSideEffect) REFERENCES SideEffect (id);
-- VaccinationPlace -> Appointment
ALTER TABLE Appointment ADD FOREIGN KEY (IdVaccinationPlace) REFERENCES VaccinationPlace (id);
-- AppointmentType -> Appointment
ALTER TABLE Appointment ADD FOREIGN KEY (IdAppointmentType) REFERENCES AppointmentType (id);
-- Manager -> Appointment
ALTER TABLE Appointment ADD FOREIGN KEY (IdManager) REFERENCES Manager (id);
-- Manager -> LogInHistory
ALTER TABLE LogInHistory ADD FOREIGN KEY (IdManager) REFERENCES Manager (id);
-- Manager -> Employee
ALTER TABLE Employee ADD FOREIGN KEY (IdManager) REFERENCES Manager (id);
-- WorkedBooth -> Employee
ALTER TABLE Employee ADD FOREIGN KEY (IdWorkedBooth) REFERENCES Booth (id);
-- ManagedBooth -> Employee
ALTER TABLE Employee ADD FOREIGN KEY (IdManagedBooth) REFERENCES Booth (id);
-- EmployeeType -> Employee
ALTER TABLE Employee ADD FOREIGN KEY (IdEmployeeType) REFERENCES EmployeeType (id);

--Insertando catalogos

--Institution
INSERT INTO Institution VALUES ('Educación');
INSERT INTO Institution VALUES ('Salud');
INSERT INTO Institution VALUES ('PNC');
INSERT INTO Institution VALUES ('Gobierno');
INSERT INTO Institution VALUES ('Fuerza Armada');
INSERT INTO Institution VALUES ('Periodismo');

--Priority group
INSERT INTO PriorityGroup VALUES ('Mayor de 60 años');
INSERT INTO PriorityGroup VALUES ('Personal del sistema integrado de salud');
INSERT INTO PriorityGroup VALUES ('Encargados de la seguridad nacional');
INSERT INTO PriorityGroup VALUES ('Mayor de 18 años con discapacidad');
INSERT INTO PriorityGroup VALUES ('Personal del gobierno central');

--Employee type
INSERT INTO EmployeeType VALUES ('Gestor');
INSERT INTO EmployeeType VALUES ('Enfermero');
INSERT INTO EmployeeType VALUES ('Doctor');
INSERT INTO EmployeeType VALUES ('Ordenanza');

--Vaccination place
INSERT INTO VaccinationPlace VALUES ('Hospital El Salvador');
INSERT INTO VaccinationPlace VALUES ('Hospital Nacional Rosales');
INSERT INTO VaccinationPlace VALUES ('Hospital CECLISA Ahuachapan');
INSERT INTO VaccinationPlace VALUES ('Hospital Nacional "San Juan de Dios"');
INSERT INTO VaccinationPlace VALUES ('Hospital Merliot');
INSERT INTO VaccinationPlace VALUES ('Hospital Nacional Ilobasco');

--Side Effect
INSERT INTO SideEffect VALUES ('Dolor en el sitio de la inyección');
INSERT INTO SideEffect VALUES ('Enrojecimiento en el sitio de la inyección');
INSERT INTO SideEffect VALUES ('Fatiga');
INSERT INTO SideEffect VALUES ('Dolor de cabeza');
INSERT INTO SideEffect VALUES ('Fiebre');
INSERT INTO SideEffect VALUES ('Mialgia');
INSERT INTO SideEffect VALUES ('Artralgia');
INSERT INTO SideEffect VALUES ('Anafilaxia');

--AppointmentType
INSERT INTO AppointmentType VALUES ('Primera dosis');
INSERT INTO AppointmentType VALUES ('Segunda dosis');

--Entidades
-- Booth
INSERT INTO Booth VALUES ('Calle Los Sisimiles, Metrocentro, San Salvador','76583922','CabinSV01@salud.gob.sv');
INSERT INTO Booth VALUES ('Salón Centro Americano, Avenida De La Revolucion 222, San Salvador','76985698','CabinSV02@salud.gob.sv');
INSERT INTO Booth VALUES ('25 Avenida Sur y, Alameda Franklin Delano Roosevelt, San Salvador','25244253','CabinSV03@salud.gob.sv');
INSERT INTO Booth VALUES ('2A Avenida sur, Ahuachapan','76743663','CabinSV04@salud.gob.sv');
INSERT INTO Booth VALUES ('2a Calle Pte, Santa Ana','77924411','CabinSV05@salud.gob.sv');

--Manager
INSERT INTO Manager VALUES ('GestorSV01','Custi1789');
INSERT INTO Manager VALUES ('GestorSV02','Clem969');
INSERT INTO Manager VALUES ('GestorSV03','Tej553');
INSERT INTO Manager VALUES ('GestorSV04','Diaz101');
INSERT INTO Manager VALUES ('GestorSV05','Ch1nch1ll4');
INSERT INTO Manager VALUES ('GestorSV06','Esc0023');
INSERT INTO Manager VALUES ('GestorSV07','C4rr4nz4');

--Employees
INSERT INTO Employee VALUES ('Jose Francisco Custodio Gonzalez','13 Cl Ote Cond Resid Aries No 7 Ent Av España y 2 Av Nte, San Salvador, San Salvador',1,1,1,1);
INSERT INTO Employee VALUES ('Alejandra Clementina Perez Puentes','Col Flor Blanca 1 Cl Pte No 2023, San Salvador',3,2,2,2);
INSERT INTO Employee VALUES ('Noe Raul Tejedor Castro','Col Médica Av Dr Emilio Alvarez No 43, San Salvador, San Salvador',3,3,3,3);
INSERT INTO Employee VALUES ('Diana Sofia Alvarez Diaz','Col Libertad Av Don Bosco Edif Ex-Ivu, San Salvador, San Salvador',1,4,4,4);
INSERT INTO Employee VALUES ('Juan Antonio Caceres Chinchilla','Colonia Médica Diag Dr Luis E Vásquez Cond MD Loc 504, San Salvador, San Salvador',1,5,5,5);
INSERT INTO Employee VALUES ('Luis Alberto Escobar Molina','Carrt A Los Planes De Renderos Km 2 1/2 Qta La Escondida Fca Los 3 Monos, San Salvador, San Salvador',1,1,NULL,6);
INSERT INTO Employee VALUES ('Juan Carlos Hernández Carranza','1 Cl Pte Y 45 Av Nte, San Salvador, San Salvador',1,2,NULL,7);
INSERT INTO Employee VALUES ('Elias Alejandro Chávez Martinez','4 Av Nte Edif Presidente No 118, San Salvador, San Salvador',3,1,NULL,NULL);
INSERT INTO Employee VALUES ('Diego Tomas Marroquin Hernandez','Col Escalón 7 Av Nte Pje Francisco Campos No 179, San Salvador, San Salvador',3,4,NULL,NULL);
INSERT INTO Employee VALUES ('Ilsy Niccole Tobar Vallecillos','Colonia Escalón 9 Cl Pte y 89 Av Nte No 4626, San Salvador, San Salvador',3,5,NULL,NULL);
INSERT INTO Employee VALUES ('Emilia Alexandra Lopez Sánchez','Col Escalón Av Olímpica No 3838,San Salvador',2,1,NULL,NULL);
INSERT INTO Employee VALUES ('Daniela Maria Valdez Mendez','Blvd Venezuela No 1201 Ent 23 Y 25 Av Sur, Santa Ana',2,1,NULL,NULL);
INSERT INTO Employee VALUES ('Isabella Lucía Montoya Mendoza','C C Plaza Camelot 2A Plt Loc 9-B Sta Tecla, Santa Tecla',2,2,NULL,NULL);
INSERT INTO Employee VALUES ('Madison Abigail Gomez Muñoz','Bo La Merced 1 Av Sur Y 5 Cl Pte No 10, Santa Ana',2,2,NULL,NULL);
INSERT INTO Employee VALUES ('Kevin Davin Rivas Perez','Bo Distrito Comercial Central 1 Cl Pte No 1008, Santa Ana',2,3,NULL,NULL);
INSERT INTO Employee VALUES ('Marlon Eduardo Gomez Caishpal','Ps Gral Escalón Col Escalón No 4650, San Salvador',2,3,NULL,NULL);
INSERT INTO Employee VALUES ('Jackeline Daniela Arriaza Gonzalez','Col La Rábida 35 Cl Ote No 312, Santa Ana',2,4,NULL,NULL);
INSERT INTO Employee VALUES ('Monica Stephanie Barahona Pineda','Bo El Centro Av Juan Bertis No 80-A S A De Cdad Delg, San Salvador',2,4,NULL,NULL);
INSERT INTO Employee VALUES ('Erika Alejandra Acosta Granados','Bo San Esteban 12 Av Sur No 334, Santa Ana',2,5,NULL,NULL);
INSERT INTO Employee VALUES ('Stanley Rafael Sintigo Hidalgo','Urb Jard De La Cima Iv Av Guacalchía No 182-E, Santa Ana',2,5,NULL,NULL);
INSERT INTO Employee VALUES ('Christian Alejandro Perez Sozo','Col Sta Rosa I Cl Ppal Fnl No 22 Cusca',4,5,NULL,NULL);
INSERT INTO Employee VALUES ('Henry Alexis Flores Campos','Cl Ruta Militar Fnl 3 Av Nte No 1019, Santa Ana',4,5,NULL,NULL);
INSERT INTO Employee VALUES ('Marck Yohalmo Andrade Peña','Bo El Calvario Av Isidro Menéndez No 525 Ent 8 Y 10 Cl Pte, Santa Ana',4,5,NULL,NULL);
INSERT INTO Employee VALUES ('Nataly Sofia Benavides Rosales','Resid Altavista Cl Las Delicias Políg 35 No 17, San Martín',4,5,NULL,NULL);
INSERT INTO Employee VALUES ('Mauro David Ramirez Peralta','Col Sta Lucía Pje 7 No 28 Ilop, Soyapango, San Salvador',4,5,NULL,NULL);

