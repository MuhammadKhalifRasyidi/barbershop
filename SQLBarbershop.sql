create database Barbershop
use barbershop

create table Pelanggan(
Nama_pelanggan varchar (30),
No_telp varchar(13),
idPelanggan char(3) primary key,
);

create table TukangCukur(
Nama varchar (30),
Jadwal_kerja varchar (15),
pengalaman text,
Id_TkgCukur char (3) primary key,
idPelanggan char(3) foreign key references Pelanggan(idPelanggan)
);

create table Pembookingan(
Id_booking char(3) primary key,
Nama_pelanggan varchar (30),
No_telp varchar(13),
Waktu_booking varchar (15),
No_antrian char(2),
idPelanggan char(3) foreign key references Pelanggan(idPelanggan),
Id_TkgCukur char (3) foreign key references TukangCukur(Id_TkgCukur)
);

create table Antrian(
Nama_pelanggan varchar (30),
No_antrian char(2),
Waktu_tunggu varchar (15),
Id_antrian char (3) primary key
);


