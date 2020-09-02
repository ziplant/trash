
create database BugReports

GO

use BugReports

-- TABLES

create table UserLevels (
user_level int primary key,
level_name nvarchar(75) unique not null,
rating_required int,
[description] nvarchar(255)
)

GO

create table AdminLevels (
admin_level int primary key,
level_name nvarchar(75) unique not null,
hourly_pay decimal(5,2) not null,
hours int not null,
[description] nvarchar(255)
)

GO

create table Account (
account_id int primary key identity(1,1),
account_name nvarchar(50) unique not null,
first_name nvarchar(75) not null,
last_name nvarchar(75) not null,
email nvarchar(100) unique not null,
[password] nvarchar(60) not null,
portrait_path nvarchar(255)
)

GO

create table [User] (
account_id int primary key,
user_rating decimal(10,2) not null default 0,
user_level int not null default 1,
foreign key (account_id) references Account(account_id),
foreign key (user_level) references UserLevels(user_level) on update cascade
)

GO

create table [Admin] (
account_id int primary key,
admin_level int not null default 1,
foreign key (account_id) references Account(account_id),
foreign key (admin_level) references AdminLevels(admin_level) on update cascade
)

GO

create table BugStatus (
status_code int primary key,
status_name nvarchar(30),
status_description nvarchar(50)
)

GO

create table Product (
product_id int identity(1,1) primary key,
product_name nvarchar(150)
)

GO

create table Contacts (
product_id int,
account_id int,
primary key (product_id, account_id),
foreign key (product_id) references Product(product_id) on update cascade on delete cascade,
foreign key (account_id) references [Admin](account_id) on update cascade on delete cascade
)

GO

create table Bug (
bug_id int primary key identity(1,1),
date_reported date not null,
title nvarchar(100) not null,
[description] nvarchar(max),
resolution nvarchar(max),
reported_by int not null,
verified_by int,
status_code int not null default 1,
[priority] nvarchar(10) default 'Средний',
check ([priority] like 'Высокий' or [priority] like 'Средний' or [priority] like 'Низкий'),
foreign key (reported_by) references Account(account_id),
foreign key (verified_by) references [Admin](account_id) on delete set null,
foreign key (status_code) references BugStatus(status_code) on update cascade
)

GO

create table BugsProducts (
bug_id int,
product_id int,
primary key (bug_id, product_id),
foreign key (bug_id) references Bug(bug_id) on update cascade on delete cascade,
foreign key (product_id) references Product(product_id) on update cascade on delete cascade
)

GO

create table AdminTask (
bug_id int,
product_id int,
account_id int,
date_of_appointment date,
primary key (bug_id, product_id, account_id),
foreign key (bug_id) references Bug(bug_id),
foreign key (product_id, account_id) references Contacts(product_id, account_id)
)

GO
create table Comment (
comment_id int primary key identity(1,1),
bug_id int not null,
author int not null,
comment_date datetime not null,
comment_text nvarchar(max) not null,
foreign key (bug_id) references Bug(bug_id),
foreign key (author) references Account(account_id)
)

GO

create table CommentTrees (
parent_id int,
children_id int,
level int not null default 0,
primary key (parent_id, children_id),
foreign key (parent_id) references Comment(comment_id),
foreign key (children_id) references Comment(comment_id)
)

GO

create table Screenshots (
image_id int identity(1,1) primary key,
bug_id int,
image_path nvarchar(255) not null,
foreign key (bug_id) references Bug(bug_id) on update cascade on delete cascade
)

GO

create table Tags (
bug_id int,
tag nvarchar(40),
primary key (bug_id, tag),
foreign key (bug_id) references Bug(bug_id) on update cascade on delete cascade
)

GO

-- TRIGGERS

create trigger AddUser
on [User]
instead of insert
as
declare users cursor for select account_id from inserted
declare @id int
open users
fetch next from users into @id
while @@FETCH_STATUS = 0
begin
if not exists (select * from [Admin] a join inserted i on a.account_id = i.account_id)
insert into [user] select * from inserted
else
RAISERROR ('User contains in Admin', 16, 1); 
fetch next from users into @id
end
close users
deallocate users

GO

create trigger AddAdmin
on [Admin]
instead of insert
as
declare admins cursor for select account_id from inserted
declare @id int
open admins
fetch next from admins into @id
while @@FETCH_STATUS = 0
begin
if not exists (select * from [User] a join inserted i on a.account_id = i.account_id)
insert into [Admin] select * from inserted
else
RAISERROR ('Admin contains in User', 16, 1); 
fetch next from admins into @id
end
close admins
deallocate admins

GO

create trigger UpdateLevel
on [User]
after insert, update
as
declare @rating decimal
set @rating = (select user_rating from inserted)

declare levels cursor for select user_level, rating_required from UserLevels
declare @level int
declare @required int
open levels
fetch next from levels into @level, @required

while @@FETCH_STATUS = 0
begin
if @rating >= @required
update [User] set user_level = @level where account_id = (select account_id from inserted)

fetch next from levels into @level, @required
end
close levels
deallocate levels

GO

create trigger TaskAdded
on AdminTask
after insert, update
as
declare @bugId int set @bugId = (select bug_id from inserted)

if (select status_code from Bug where bug_id = @bugId) > 2 or
(select admin_level from Admin 
where account_id = (select account_id from inserted)) = 1 or
(select count(product_id) from (select product_id from inserted) p
where product_id 
in (
select product_id from BugsProducts where bug_id = @bugId)
) = 0
rollback transaction
else
update bug set status_code = 2 where bug_id = @bugId

GO

create trigger CommentDeleted
on Comment
instead of delete
as
declare @commentId int
select @commentId = comment_id from deleted
delete from CommentTrees where children_id = @commentId
delete from Comment where comment_id = @commentId

GO

-- VIEWS

create view ShowProductStatus
as
select p.product_id, p.product_name, count(bp.bug_id) bugs, sum(iif(status_code > 2, 1, 0)) resolved from Product p
left join BugsProducts bp on p.product_id = bp.product_id
left join Bug b on b.bug_id = bp.bug_id
group by p.product_id, p.product_name

GO

create view ShowAccountRole
as
select ac.account_id, account_name, 
iif(u.account_id is null and a.account_id is null, null, CONCAT(ul.level_name, al.level_name)) account_role,
iif(a.account_id is not null, 1, iif(u.account_id is not null, 0, null)) admin_rights
from Account ac
left join [User] u on u.account_id = ac.account_id
left join UserLevels ul on ul.user_level = u.user_level
left join [Admin] a on a.account_id = ac.account_id
left join AdminLevels al on al.admin_level = a.admin_level
GO

-- FUNCTIONS

create function ShowWorkGroup (@bug_id int, @product_id int)
returns table as return
(
select account_name, first_name, last_name, date_of_appointment from AdminTask t
inner join Account a on a.account_id = t.account_id
where t.bug_id = @bug_id and t.product_id = @product_id
)

GO

create function ShowCommentAnswer(@root int)
returns table as return
(
select a.account_name, c.comment_text, c.comment_date from CommentTrees ct
inner join Comment c on c.comment_id = ct.children_id
inner join Account a on a.account_id = c.author 
where [level] = (select [level] from CommentTrees where parent_id = @root and children_id = @root) + 1
and parent_id = @root
)

GO

-- PROCEDURES

create proc UserAdminSwitch 
@accountId int
as
set nocount on
if (select account_id from [Admin] where account_id = @accountId) is not null
begin
delete from [Admin] where account_id = @accountId
insert into [User] values (@accountId, 500, 3)
end
else if (select account_id from [User] where account_id = @accountId) is not null
begin
delete from [User] where account_id = @accountId
insert into [Admin] values(@accountId, 1)
end
else
insert into [User] values (@accountId, 0, 1)

GO

create proc AddCommentTree 
@parentId int, @commentId int
as
set nocount on
declare @level int 
set @level = (select [level] from CommentTrees 
where parent_id = @parentId and children_id = @parentId)
if @level is not null
set @level = @level + 1
else
set @level = 0

if (select count(*) from CommentTrees where parent_id = @commentId) = 0
insert into CommentTrees values (@commentId, @commentId, @level)

declare tree cursor for select parent_id from CommentTrees where children_id = @parentId
declare @id int
open tree
fetch next from tree into @id

while @@FETCH_STATUS = 0
begin
if (select count(*) from CommentTrees 
	where parent_id = @id and [level] = @level) = 0
insert into CommentTrees values (@id, @commentId, @level)
fetch next from tree into @id
end
close tree
deallocate tree

GO
