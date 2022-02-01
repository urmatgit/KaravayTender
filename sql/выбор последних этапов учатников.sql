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
select a.*
     , b.Id as ComStageId
	 , b.DeadlineDate
  from s_1 a
  join ComStages b on b.ComOfferId = a.ComOfferId
                  and b.Number = a.Number  
)
, s_3 as ( -- статусы учатсников в последних этапах
select a.*
     , b.Status
	 , b.Description
  from s_2 a
  join StageParticipants b on b.ComStageId = a.ComStageId
                          and b.ContragentId = a.ContragentId
)
select a.*
     , e.name as ContragentName
	 , b.ComPositionId
     , d.name as NomenclatureName
     , b.Price
	 , b.RequestPrice
  from  s_3 a
  join StageCompositions b on b.ComStageId = a.ComStageId
                          and b.ContragentId = a.ContragentId
  left join ComPositions c on c.Id = b.ComPositionId
  left join Nomenclatures d on d.Id = c.NomenclatureId
  left join Contragents e on e.Id = a.ContragentId
 --where a.ContragentId = 3 
