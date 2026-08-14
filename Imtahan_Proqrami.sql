--Ev tapsirigi 1
--Imtahan proqrami - MS SQL Server

--drop database ImtahanDB
IF DB_ID('ImtahanDB') IS NULL
	CREATE DATABASE ImtahanDB
GO

USE ImtahanDB
GO

--evvelce dersler yaradilir, imtahanlar cedveli buna baglidir
CREATE TABLE Dersler
(
	DersKodu char(3) NOT NULL,
	DersAdi varchar(30) NOT NULL,
	Sinif numeric(2,0) NOT NULL,
	MuellimAdi varchar(20) NOT NULL,
	MuellimSoyadi varchar(20) NOT NULL,
	CONSTRAINT PK_Dersler PRIMARY KEY (DersKodu),
	CONSTRAINT CK_Dersler_Sinif CHECK (Sinif BETWEEN 1 AND 11)
)
GO

CREATE TABLE Sagirdler
(
	Nomre numeric(5,0) NOT NULL,
	Adi varchar(30) NOT NULL,
	Soyadi varchar(30) NOT NULL,
	Sinif numeric(2,0) NOT NULL,
	CONSTRAINT PK_Sagirdler PRIMARY KEY (Nomre),
	CONSTRAINT CK_Sagirdler_Sinif CHECK (Sinif BETWEEN 1 AND 11)
)
GO

CREATE TABLE Imtahanlar
(
	DersKodu char(3) NOT NULL,
	SagirdNomresi numeric(5,0) NOT NULL,
	ImtahanTarixi date NOT NULL,
	Qiymet numeric(1,0) NULL,   --qiymet sonra da qoyula biler

	--acar 3 sutundan ibaretdir: eyni sagird eyni dersden eyni gunde 1 defe
	CONSTRAINT PK_Imtahanlar PRIMARY KEY (DersKodu, SagirdNomresi, ImtahanTarixi),
	CONSTRAINT FK_Imtahanlar_Dersler FOREIGN KEY (DersKodu) REFERENCES Dersler(DersKodu),
	CONSTRAINT FK_Imtahanlar_Sagirdler FOREIGN KEY (SagirdNomresi) REFERENCES Sagirdler(Nomre),
	CONSTRAINT CK_Imtahanlar_Qiymet CHECK (Qiymet BETWEEN 2 AND 5)
)
GO


--yoxlamaq ucun bir nece setir
INSERT INTO Dersler VALUES ('RIY', 'Riyaziyyat', 9, 'Elnur', 'Memmedov')
INSERT INTO Dersler VALUES ('FIZ', 'Fizika', 9, 'Sevinc', 'Aliyeva')
INSERT INTO Dersler VALUES ('KIM', 'Kimya', 9, 'Rasim', 'Huseynov')

INSERT INTO Sagirdler VALUES (10001, 'Nihat', 'Quliyev', 9)
INSERT INTO Sagirdler VALUES (10002, 'Aysel', 'Kerimova', 9)
INSERT INTO Sagirdler VALUES (10003, 'Tural', 'Sadigov', 9)

INSERT INTO Imtahanlar VALUES ('RIY', 10001, '2026-05-20', 5)
INSERT INTO Imtahanlar VALUES ('RIY', 10002, '2026-05-20', 4)
INSERT INTO Imtahanlar VALUES ('RIY', 10003, '2026-05-20', 3)
INSERT INTO Imtahanlar VALUES ('FIZ', 10001, '2026-05-25', 4)
INSERT INTO Imtahanlar VALUES ('FIZ', 10002, '2026-05-25', 5)
INSERT INTO Imtahanlar VALUES ('KIM', 10003, '2026-05-28', 2)
GO


--butun neticeler
SELECT s.Nomre, s.Adi + ' ' + s.Soyadi AS Sagird, d.DersAdi, i.ImtahanTarixi, i.Qiymet
FROM Imtahanlar i
	INNER JOIN Sagirdler s ON s.Nomre = i.SagirdNomresi
	INNER JOIN Dersler d ON d.DersKodu = i.DersKodu
ORDER BY s.Soyadi, i.ImtahanTarixi


--her ders uzre orta bal
--cast olmasa AVG tam eded qaytarir, 4.6 -> 4
SELECT d.DersAdi, COUNT(*) AS ImtahanSayi, AVG(CAST(i.Qiymet AS decimal(3,2))) AS OrtaBal
FROM Dersler d
	INNER JOIN Imtahanlar i ON i.DersKodu = d.DersKodu
GROUP BY d.DersAdi
ORDER BY OrtaBal DESC


--kesir alanlar
SELECT s.Adi, s.Soyadi, d.DersAdi, i.ImtahanTarixi
FROM Imtahanlar i
	INNER JOIN Sagirdler s ON s.Nomre = i.SagirdNomresi
	INNER JOIN Dersler d ON d.DersKodu = i.DersKodu
WHERE i.Qiymet = 2


--hele imtahan vermeyen sagirdler
SELECT s.Nomre, s.Adi, s.Soyadi, s.Sinif
FROM Sagirdler s
WHERE NOT EXISTS (SELECT 1 FROM Imtahanlar i WHERE i.SagirdNomresi = s.Nomre)
GO
