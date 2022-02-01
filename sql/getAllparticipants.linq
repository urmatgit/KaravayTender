<Query Kind="Statements">
  <Connection>
    <ID>f2417feb-acf3-419d-9185-ee22f0a3b9a6</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AttachFileName>C:\Git\Karavay\KaravayTender\src\SmartAdmin.WebUI\KaravayTender.db</AttachFileName>
    <DriverData>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.Sqlite</EFProvider>
      <EFVersion>5.0.10</EFVersion>
    </DriverData>
  </Connection>
  <Namespace>Microsoft.EntityFrameworkCore</Namespace>
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

 var dataStep1N1 = from c in ComParticipants
                              where c.ComOfferId == 18
                              from s in StageParticipants.Where(b => b.ComOfferId == c.ComOfferId && b.ContragentId == c.ContragentId).DefaultIfEmpty()
                              from cs in ComStages.Where(x => x.Id == s.ComStageId).DefaultIfEmpty()
                              group new { c, cs } by new { c.ComOfferId, c.ContragentId } into gr

                              select new 
                              {
                                  ComOfferId = gr.Key.ComOfferId,
                                  ContragentId = gr.Key.ContragentId,
                                  Number = gr.Max(x => x.cs == null ? 0 : x.cs.Number),
                              };
            var data2 = from a in dataStep1N1
                        join b in ComStages on new { comID = a.ComOfferId, num = a.Number } equals new { comID = b.ComOfferId, num = b.Number } into gr
                        from b1 in gr.DefaultIfEmpty()
                        select new 
                        {
                            ComOfferId = a.ComOfferId,
                            ContragentId =a.ContragentId,
                            Number = a.Number,
                            
                            ComStageId =b1!=null? b1.Id : default(int),
                            DeadlineDate = b1 != null ? b1.DeadlineDate: default(string)
                        };
            var dataStep3 = from a in data2
                            join b in StageParticipants on new { Id = a.ComStageId, ContrId = a.ContragentId } equals new { Id = b.ComStageId, ContrId = b.ContragentId } into gr
                            from b1 in gr.DefaultIfEmpty()
                            select new 
                            {
                                ComOfferId = a.ComOfferId,
                                ContragentId = a.ContragentId,
								Number = a.Number,
								ComStageId = a.ComStageId,
								DeadlineDate = a.DeadlineDate,
								Status = b1 != null ? b1.Status :1,
								Description = b1 != null ? b1.Description : ""
							};
Console.WriteLine(dataStep3);