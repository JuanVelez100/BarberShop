DROP TABLE Client;
DROP TABLE [Contract];
DROP TABLE Person;

CREATE TABLE Person(
    Id VARCHAR(100) PRIMARY KEY NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	LastName VARCHAR(100) NOT NULL,
    Passwork VARCHAR(100) NOT NULL,
	Mobile VARCHAR(10) NOT NULL,
	Mail VARCHAR(100) NOT NULL
);

CREATE TABLE [Contract](
	TypeAffiliation int PRIMARY KEY NOT NULL,
	StartAttentionTime time NOT NULL,
	EndAttentionTime time NOT NULL,
	MonthlyPayment int,
	DiscountRate int
);

CREATE TABLE Client(
	Id VARCHAR(100) PRIMARY KEY,
	TypeAffiliation int,
	FOREIGN KEY (TypeAffiliation) REFERENCES [Contract](TypeAffiliation),
    FOREIGN KEY (Id) REFERENCES Person(Id)
);

INSERT INTO Person VALUES
('1053819484','Juan','Velez','1234','3103708134','juan@gmail.com'),
('1053822568','Marcela','Higinio','5678','3107462762','marce@gmail.com');

INSERT INTO [Contract] VALUES
(1,CAST ('07:00:00.0000000' AS TIME),CAST ('16:00:00.0000000' AS TIME),30000,0),
(2,CAST ('07:00:00.0000000' AS TIME),CAST ('19:00:00.0000000' AS TIME),0,20);

INSERT INTO Client VALUES
('1053819484',1),
('1053822568',2);


