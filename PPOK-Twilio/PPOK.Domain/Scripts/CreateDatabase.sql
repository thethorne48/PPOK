if object_id('EventHistory', 'U') is not null
	drop table EventHistory
if object_id('FillHistory', 'U') is not null
	drop table FillHistory
if object_id('Event', 'U') is not null
	drop table [Event]
if object_id('Prescription', 'U') is not null
	drop table Prescription
if object_id('Patient', 'U') is not null
	drop table Patient
if object_id('Job', 'U') is not null
	drop table Job
if object_id('Pharmacy', 'U') is not null
	drop table Pharmacy
if object_id('Pharmacist', 'U') is not null
	drop table Pharmacist
if object_id('Drug', 'U') is not null
	drop table Drug
if object_id('MessageTemplate', 'U') is not null
	drop table MessageTemplate
if object_id('SystemAdmin', 'U') is not null
	drop table SystemAdmin
if object_id('EventType', 'U') is not null
	drop table EventType
if object_id('EventStatus', 'U') is not null
	drop table EventStatus
if object_id('ContactPreference', 'U') is not null
	drop table ContactPreference

-- maps to enum in C#
create table ContactPreference(
	Code int not null unique identity,
	Name varchar(max) not null,
	primary key(Code)
);
-- maps to enum in C#
create table EventStatus(
	Code int not null unique identity,
	Name varchar(max) not null,
	primary key(Code)
);
-- maps to enum in C#
create table EventType(
	Code int not null unique identity,
	Name varchar(max) not null,
	primary key(Code)
);
create table SystemAdmin(
	Code int not null unique identity,
	Name varchar(max) not null,
	Email varchar(max) not null,
	PasswordHash binary(32) not null,
	primary key(Code)
);
create table MessageTemplate(
	Code int not null unique identity,
	Name varchar(max) not null,
	Content varchar(max) not null,
	primary key(Code)
);
create table Drug(
	Code int not null unique identity,
	Name varchar(max) not null,
	primary key(Code)
);
create table Pharmacist(
	Code int not null unique identity,
	FirstName varchar(max) not null,
	ListName varchar(max) not null,
	PasswordHash binary(32) not null,
	Phone varchar(max),
	Email varchar(max) not null,
	primary key(Code)
);
create table Pharmacy(
	Code int not null unique identity,
	Name varchar(max) not null,
	Phone varchar(max),
	[Address] varchar(max),
	primary key(Code)
);
create table Job(
	PharmacyCode int not null,
	PharmacistCode int not null,
	IsActive bit not null,
	IsAdmin bit not null,
	primary key(PharmacyCode, PharmacistCode),
	unique(PharmacyCode, PharmacistCode),
	foreign key(PharmacyCode) references Pharmacy 
		on delete cascade 
		on update cascade,
	foreign key(PharmacistCode) references Pharmacist 
		on delete cascade 
		on update cascade
);
create table Patient(
	Code int not null unique identity,
	PharmacyCode int not null,
	ContactPreference int not null,
	FirstName varchar(max) not null,
	LastName varchar(max) not null,
	DOB Date not null,
	ZipCode int,
	Phone varchar(max) not null,
	Email varchar(max),
	primary key(Code),
	foreign key(PharmacyCode) references Pharmacy
		on delete cascade
		on update cascade,
	foreign key(ContactPreference) references ContactPreference
		on delete cascade
		on update cascade
);
create table Prescription(
	Code int not null unique identity,
	PatientCode int not null,
	DrugCode int not null,
	Supply int not null,
	Refills int not null,
	primary key(Code),
	foreign key(PatientCode) references Patient
		on delete cascade
		on update cascade,
	foreign key(DrugCode) references Drug
		on delete cascade
		on update cascade
);
create table [Event](
	Code int not null unique identity,
	PrescriptionCode int not null,
	TypeCode int not null,
	[Message] varchar(max),
	primary key(Code),
	foreign key(PrescriptionCode) references Prescription
		on delete cascade
		on update cascade,
	foreign key(TypeCode) references EventType
		on delete cascade
		on update cascade
);
create table FillHistory(
	Code int not null unique identity,
	PrescriptionCode int not null,
	PharmacistCode int not null,
	[Date] Date not null,
	primary key(Code),
	foreign key(PrescriptionCode) references Prescription
		on update cascade,
	foreign key(PharmacistCode) references Pharmacist
		on update cascade
);
create table EventHistory(
	Code int not null unique identity,
	EventCode int not null,
	StatusCode int not null,
	[Date] Date not null,
	primary key(Code),
	foreign key(EventCode) references [Event]
		on update cascade,
	foreign key(StatusCode) references EventStatus
		on update cascade
);

insert into ContactPreference values ('PHONE');
insert into ContactPreference values ('TEXT');
insert into ContactPreference values ('EMAIL');
insert into ContactPreference values ('NONE');

--TODO: add the other enum values once they are determined