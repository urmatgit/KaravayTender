with s_1 as ( --выбор последних этапов учатников
select a.ComOfferId
     , b.ContragentId
	 , MAX(a.Number) as Number
  from ComStages a
  join StageParticipants b on b.ComStageId = a.Id 
 where a.ComOfferId =  12
group by a.ComOfferId
       , b.ContragentId
)
, s_2 as ( -- определение id этапа по номеру для каждого участника
select a.*, b.Id
  from s_1 a
  join ComStages b on b.ComOfferId = a.ComOfferId
                  and b.Number = a.Number  
)
select a.*
     , b.Status
	 , b.Description
  from s_2 a
  join StageParticipants b on b.ComStageId = a.Id
                          and b.ContragentId = a.ContragentId
