


declare @w1 varchar(100) set @w1 = 'шриланка'
declare @w2 varchar(100) set @w2 = 'шрилиан'
declare @swap varchar(100)
if (len(@w1) > len(@w2))
begin set @swap = @w2 set @w2 = @w1 set @w1 = @swap end
----------------------
declare @dictionary table(w1 char(1), w2 char(1))
declare @i int set @i = 1
declare @deleted int set @deleted = 0
declare @spaces int set @spaces = 0
declare @spaceDist int
declare @delDist int 
declare @dist int 
declare @flag bit 
declare @distance int set @distance = 0
-----------------------
while @i <= len(@w2)
begin
	set @flag = 1
	set @spaceDist = 0
	set @delDist = 0
	set @dist = 0
	if (substring(@w1, @i+@deleted-@spaces, 1) not like substring(@w2, @i, 1))
	begin
		declare @counter int set @counter = @i
		while @counter <= len(@w2)
		begin
			if (substring(@w1, @counter+@deleted-@spaces, 1) not like substring(@w2, @counter, 1))
			begin
				set @dist = @dist + 1
			end
			if (substring(@w1, @counter+@deleted-@spaces+1, 1) not like substring(@w2, @counter, 1))
			begin
				set @delDist = @delDist + 1
			end
			if (substring(@w1, @counter+@deleted-@spaces, 1) not like substring(@w2, @counter+1, 1))
			begin
				set @spaceDist = @spaceDist + 1
			end
			set @counter = @counter + 1
		end
		if (@spaceDist < @dist-1)
		begin
			set @flag = 0
			set @spaces = @spaces + 1
			insert into @dictionary select '', substring(@w2, @i, 1)
		end
		else if (@delDist < @dist)
		begin
			set @deleted = @deleted + 1
		end
		
	end

	if @flag  like 1
		insert into @dictionary select substring(@w1, @i+@deleted-@spaces, 1), substring(@w2, @i, 1)
	set @i = @i + 1
end
----------------------
select * from @dictionary
----------------------
set @distance = @distance + (select sum(iif(w1 like w2, 0, 1)) from @dictionary)
select @distance

