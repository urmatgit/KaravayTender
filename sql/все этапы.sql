with s_2 as ( -- все этапы
select b.Id as ComStageId
	 , b.DeadlineDate
	 , b.Number
  from ComStages b 
 where b.ComOfferId = 12				   
)
, s_3 as ( -- статусы учатсников в этапах
select a.*
     , b.ContragentId
     , b.Status
	 , b.Description
  from s_2 a
  join StageParticipants b on b.ComStageId = a.ComStageId
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
-- where a.ContragentId = 3 
order by d.name, a.Number, a.ContragentId
