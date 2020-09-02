
if OBJECT_ID('search_compare') is not null
drop function search_compare
go


create function search_compare(@what varchar(255), @for varchar(255), @len int)
returns bit
as
begin
declare @swap varchar(255)
if (len(@for) > len(@what))
begin set @swap = @what set @what = @for set @for = @swap end
-----------------------
declare @distance int set @distance = 0
declare @i int set @i = 1
declare @deleted int set @deleted = 0
declare @spaces int set @spaces = 0
declare @spaceDist int
declare @delDist int 
declare @dist int 
declare @flag bit set @flag = 0
-----------------------
while @i <= len(@what)
begin
	set @spaceDist = 0
	set @delDist = 0
	set @dist = 0
	if (substring(@for, @i+@deleted-@spaces, 1) not like substring(@what, @i, 1))
	begin
		declare @counter int set @counter = @i
		while @counter <= len(@what)
		begin
			if (substring(@for, @counter+@deleted-@spaces, 1) not like substring(@what, @counter, 1))
				set @dist = @dist + 1
			-----------------------
			if (substring(@for, @counter+@deleted-@spaces+1, 1) not like substring(@what, @counter, 1))
				set @delDist = @delDist + 1
			-----------------------
			if (substring(@for, @counter+@deleted-@spaces, 1) not like substring(@what, @counter+1, 1))
				set @spaceDist = @spaceDist + 1
			-----------------------
			set @counter = @counter + 1
		end
		if (@spaceDist < @dist-1)
			set @spaces = @spaces + 1
		-----------------------
		else if (@delDist < @dist)
			set @deleted = @deleted + 1
		-----------------------
		set @distance = @distance + 1
	end
	if (@len < @distance)
	begin
		set @flag = 1
		break
	end
	set @i = @i + 1
end
-----------------------
return(@flag)
end
go

select dbo.search_compare('BAY RIDGE', 'lololo',3)
