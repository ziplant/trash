

if db_id('жд') is not null
drop database жд
go


CREATE DATABASE жд
ON PRIMARY
(  
NAME=lib1, 
FILENAME='C:\graf\lib1.mdf',
SIZE=1024 MB,
MAXSIZE=2048 MB,
FILEGROWTH=512 MB
),
(  
NAME=lib2,  
FILENAME='C:\graf\lib2.mdf', 
SIZE=1024 MB,
MAXSIZE=2048 MB,
FILEGROWTH=512 MB
)
LOG ON
(
NAME=lib_log,
FILENAME='C:\graf\lib_log.ldf',
SIZE=100 MB,
MAXSIZE=300 MB,
FILEGROWTH=50 MB
) 
go
use жд

Create table Остановочный_пункт (
Код_ост_пункта int primary key,
Название varchar(100),
Мин_время_ост_минут int
);

Create table Маршруты (
Код_ост_нач int,
Код_ост_кон int,
Время_в_пути_минут int not null,
primary key (Код_ост_нач, Код_ост_кон),
foreign key (Код_ост_нач) references Остановочный_пункт (Код_ост_пункта),
foreign key (Код_ост_кон) references Остановочный_пункт (Код_ост_пункта)
);

go

use master