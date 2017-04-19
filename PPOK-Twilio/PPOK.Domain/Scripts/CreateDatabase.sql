if object_id('EventSchedule', 'U') is not null
	drop table EventSchedule
if object_id('PharmacistToken', 'U') is not null
	drop table PharmacistToken
if object_id('SystemAdminToken', 'U') is not null
	drop table SystemAdminToken
if object_id('PatientToken', 'U') is not null
	drop table PatientToken
if object_id('EventHistory', 'U') is not null
	drop table EventHistory
if object_id('FillHistory', 'U') is not null
	drop table FillHistory
if object_id('EventRecall', 'U') is not null
	drop table EventRecall
if object_id('EventBirthday', 'U') is not null
	drop table EventBirthday
if object_id('EventRefill', 'U') is not null
	drop table EventRefill
if object_id('Event', 'U') is not null
	drop table [Event]
if object_id('Prescription', 'U') is not null
	drop table Prescription
if object_id('Patient', 'U') is not null
	drop table Patient
if object_id('Job', 'U') is not null
	drop table Job
if object_id('MessageResponseOption', 'U') is not null
	drop table MessageResponseOption
if object_id('MessageTemplate', 'U') is not null
	drop table MessageTemplate
if object_id('Pharmacy', 'U') is not null
	drop table Pharmacy
if object_id('Pharmacist', 'U') is not null
	drop table Pharmacist
if object_id('Drug', 'U') is not null
	drop table Drug
if object_id('SystemAdmin', 'U') is not null
	drop table SystemAdmin

create table SystemAdmin(
	Code int not null unique identity,
	FirstName varchar(max) not null,
	LastName varchar(max) not null,
	Email varchar(max) not null,
	Phone varchar(max),
	PasswordHash binary(32) not null,
	PasswordSalt binary(32) not null,
	primary key(Code)
);
create table Drug(
	Code bigint not null unique,
	Name varchar(max) not null,
	primary key(Code)
);
create table Pharmacist(
	Code int not null unique identity,
	FirstName varchar(max) not null,
	LastName varchar(max) not null,
	Email varchar(max) not null,
	Phone varchar(max),
	PasswordHash binary(32) not null,
	PasswordSalt binary(32) not null,
	primary key(Code)
);
create table Pharmacy(
	Code int not null unique,
	Name varchar(max) not null,
	Phone varchar(max),
	[Address] varchar(max),
	LastUploader int,
	LastUploaded Date,
	primary key(Code),
	foreign key(LastUploader) references Pharmacy
);
create table MessageTemplate(
	Code int not null unique identity,
	PharmacyCode int not null,
	[Type] int not null,
	Media int not null,
	Content varchar(max) not null,
	primary key(Code),
	foreign key(PharmacyCode) references Pharmacy
		on delete cascade
		on update cascade,
	unique([Type], Media, PharmacyCode)
);
create table MessageResponseOption(
	Code int not null unique identity,
	CallbackFunction varchar(max),
	LongDescription varchar(max),
	ShortDescription varchar(max),
	Verb varchar(10),
	[Type] int not null,
	primary key(Code)
);
create table Job(
	Code int not null unique identity,
	PharmacyCode int not null,
	PharmacistCode int not null,
	IsActive bit not null,
	IsAdmin bit not null,
	primary key(Code),
	unique(PharmacyCode, PharmacistCode),
	foreign key(PharmacyCode) references Pharmacy 
		on delete cascade 
		on update cascade,
	foreign key(PharmacistCode) references Pharmacist 
		on delete cascade
);
create table Patient(
	Code int not null unique,
	PharmacyCode int not null,
	ContactPreference int not null,
	FirstName varchar(max) not null,
	LastName varchar(max) not null,
	DOB Date,
	ZipCode varchar(max),
	Phone varchar(max) not null,
	Email varchar(max),
	primary key(Code),
	foreign key(PharmacyCode) references Pharmacy
		on delete cascade
		on update cascade
);
create table Prescription(
	Code int not null unique,
	PatientCode int not null,
	DrugCode bigint not null,
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
	PatientCode int not null,
	[Status] int not null,
	[Type] int not null,
	[Message] varchar(max),
	primary key(Code),
	foreign key(PatientCode) references Patient
		on update cascade
);
create table EventRefill(
	Code int not null unique identity,
	EventCode int not null,
	PrescriptionCode int not null,
	primary key(Code),
	foreign key(EventCode) references [Event],
	foreign key(PrescriptionCode) references Prescription
		on update cascade,
);
create table EventRecall(
	Code int not null unique identity,
	EventCode int not null,
	DrugCode bigint not null,
	primary key(Code),
	foreign key(EventCode) references [Event]
		on update cascade,
	foreign key(DrugCode) references Drug
		on update cascade
);
create table FillHistory(
	Code int not null unique identity,
	EventRefillCode int not null,
	PharmacistCode int not null,
	[Date] Date not null,
	primary key(Code),
	foreign key(EventRefillCode) references EventRefill,
	foreign key(PharmacistCode) references Pharmacist
);
create table EventHistory(
	Code int not null unique identity,
	ExternalId varchar(max),
	EventCode int not null,
	[Status] int not null,
	[Date] Date not null,
	primary key(Code),
	foreign key(EventCode) references [Event]
);
create table PatientToken(
	Code int not null unique identity,
	PatientCode int not null,
	Token varchar(max) not null,
	Expires DateTime not null,
	primary key(Code),
	foreign key(PatientCode) references Patient
		on delete cascade
		on update cascade
);
create table SystemAdminToken(
	Code int not null unique identity,
	SystemAdminCode int not null,
	Token varchar(max) not null,
	Expires DateTime not null,
	primary key(Code),
	foreign key(SystemAdminCode) references SystemAdmin
		on delete cascade
);
create table PharmacistToken(
	Code int not null unique identity,
	PharmacistCode int not null,
	Token varchar(max) not null,
	Expires DateTime not null,
	primary key(Code),
	foreign key(PharmacistCode) references Pharmacist
		on delete cascade
);
create table EventSchedule(
	Code int not null unique identity,
	EventCode int not null,
	[Date] Date not null,
	primary key(Code),
	foreign key(EventCode) references [Event]
		on delete cascade
);

--insert into MessageTemplate values (0, 'Hello {Patient.FirstName} {Patient.LastName}, this is a phone call.')
--insert into MessageTemplate values (1, 'Hello {Patient.FirstName} {Patient.LastName}, this is an email.')