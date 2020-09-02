use жд
if object_id ('getRoute') is not null
drop function getRoute
go

create function getRoute(@a int, @b int)
returns @result table (Код_остановки int, Время_минут_с_ост int)
as
begin
--текущий/предыдущий шаги
declare @w int
declare @@w int
--таблицы с вершинами и с дугами+метка посещения
declare @v table(Код_ост_пункта Int, Мин_время_ост_минут int)
declare @d table(Код_ост_нач int, Код_ост_кон int, Время_в_пути_минут int, метка bit)
insert @d select м.*, 0 from Маршруты м
insert @v select Код_ост_пункта, Мин_время_ост_минут from Остановочный_пункт с

declare @len int set @len = (select sum(Время_в_пути_минут) from @d) --минимальная длина пути
declare @@len int --длина пути в стеке
declare @steps table (Шаг int, Код_остановки int, время_минут int) --стек пути
declare @current int --текущая вершина в курсоре

declare ways cursor for select Код_ост_кон from Маршруты where Код_ост_нач = @a --вершины, которые смежны с @a
open ways
fetch next from ways into @current

while @@fetch_status = 0
begin
set @@w = @a
set @w = @current

update @d set метка = 1 where Код_ост_нач = @a and Код_ост_кон = @current
--добавление записи в стек
insert into @steps values((select count(*) from @steps) + 1,@w, 
(select Время_в_пути_минут+Мин_время_ост_минут from @d inner join @v on Код_ост_пункта = Код_ост_кон
where Код_ост_нач = @a and Код_ост_кон = @w))
--пока есть куда идти дальше
while (select count(Код_ост_кон) from @d d
where Код_ост_нач = @w and Код_ост_кон != @a and метка = 0 and Код_ост_кон not in (select Код_остановки from @steps)) > 0 
or (select count(*) from @steps) > 0
begin
set @@w = @w
--первый доступный конечный путь
select top 1 @w = Код_ост_кон from @d d
where Код_ост_нач = @w and Код_ост_кон != @a and метка = 0 
and Код_ост_кон not in (select Код_остановки from @steps) 
order by Код_ост_кон

update @d set метка = 1 where Код_ост_нач = @@w and Код_ост_кон = @w
--не добавлять в стек, если @a смежен c @b
if (select Время_в_пути_минут+Мин_время_ост_минут from @d inner join @v on Код_ост_пункта = Код_ост_кон
	where Код_ост_нач = @@w and Код_ост_кон = @w) is not null
insert into @steps values((select count(*) from @steps) + 1,@w, (select Время_в_пути_минут+Мин_время_ост_минут from @d inner join @v on Код_ост_пункта = Код_ост_кон
															     where Код_ост_нач = @@w and Код_ост_кон = @w))
--зашел в тупик или найдена конечная точка
if(select count(Код_ост_кон) from @d d
where Код_ост_нач = @w and Код_ост_кон != @a and метка = 0 and Код_ост_кон not in (select Код_остановки from @steps)) = 0 or @w = @b
begin

if(@w = @b)
begin
select @@len = sum(время_минут) from @steps
if (@@len < @len)
begin
set @len = @@len
delete from @result
insert into @result select Код_остановки, время_минут from @steps
end
end

delete from @steps where Код_остановки = @w
select top 1 @w = Код_остановки from @steps order by Шаг desc

end
end

update @d set метка = 0
fetch next from ways into @current
end
close ways
deallocate ways

return
end
go


Select * from dbo.getRoute(1, 4)
Select * from dbo.getRoute(1, 7)
Select * from dbo.getRoute(1, 2)
Select * from dbo.getRoute(4, 6)