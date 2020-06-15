//async public Task<Combination3DDTO> GetCell(CombinationSearchDTO search)
//{
//    // Row
//    var applications = _context.Application.Select(item => new
//    {
//        Id = item.Id,
//        Name = item.Name
//    }).ToList();

//    // Column
//    var probes = _context.Probe.Select(item => new
//    {
//        Id = item.Id,
//        Name = item.SaleName
//    }).ToList();

//    // Cell
//    var options = _context.Option.Select(item => new
//    {
//        Id = item.Id,
//        Name = item.Name
//    }).ToList();

//    var combination3d = new Combination3DDTO();

//    applications.ForEach(application =>
//    {
//        var combination2d = new Combination2DDTO
//        {
//            Description = application.Name,
//            Allow = true
//        };

//        probes.ForEach(probe =>
//        {
//            var ombination1d = new Combination1DDTO
//            {
//                Description = "",
//                Allow = true
//            };

//            options.ForEach(option =>
//            {
//                var normalRules = _context
//                    .NormalRule
//                    .Where(item =>
//                    (item.LogicalModelId == search.Model.Id || !item.LogicalModelId.HasValue) &&
//                    (item.CountryId == search.Country.Id || !item.CountryId.HasValue) &&
//                    (item.UserLevel == (short)search.UserLevel || !item.UserLevel.HasValue) &&
//                    (item.ApplicationId == application.Id || !item.ApplicationId.HasValue) &&
//                    (item.ProbeId == probe.Id || !item.ProbeId.HasValue) &&
//                    (item.KitId == search.Kit.Id || !item.KitId.HasValue) &&
//                    (item.OptionId == option.Id || !item.OptionId.HasValue));

//                var allow = normalRules.Count() > 0 && !normalRules.Any(item => item.Allow == 0);

//                if (allow)
//                {
//                    ombination1d.Combinations1D.Add(new Combination1DDTO
//                    {
//                        Description = option.Name,
//                        Allow = allow
//                    });
//                }
//            });

//            ombination1d.Combinations1D.Add(ombination1d);
//        });

//        combination3d.Combinations1D.Add(combination2d);
//    });

//    return await Task.FromResult(combination3d);
//}

//async public Task<IEnumerable<Combination1DDTO>> GetColumn(CombinationSearchDTO search)
//{
//    throw new NotImplementedException();
//    //var rowLayout = _mapper.Map<LayoutType>(search.RowLayout);
//    //var rowSelectedField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(rowLayout.ToString());

//    //var columnLayout = _mapper.Map<LayoutType>(search.ColumnLayout);
//    //var columnSelectedField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(columnLayout.ToString());

//    //var query = _context.Application.SelectMany(item => item.NormalRule
//    //        .Where(item =>
//    //                (item.LogicalModelId == search.Model.Id || !item.LogicalModelId.HasValue) &&
//    //                (item.CountryId == search.Country.Id || !item.CountryId.HasValue) &&
//    //                (item.UserLevel == (short)search.UserLevel || !item.UserLevel.HasValue) &&
//    //                (item.ApplicationId == search.Application.Id || !item.ApplicationId.HasValue) &&
//    //                (item.ProbeId == search.Probe.Id || !item.ProbeId.HasValue) &&
//    //                (item.KitId == search.Kit.Id || !item.KitId.HasValue) &&
//    //                (item.OptionId == search.Option.Id || !item.OptionId.HasValue)));

//    //_context.Application.Select(item => item.)

//    //return await Task.FromResult(combinations.ToList());

//    //// Row
//    //var applications = _context.Application.Select(item => new
//    //{
//    //    Id = item.Id,
//    //    Name = item.Name
//    //}).ToList();

//    //// Column
//    //var probes = _context.Probe.Select(item => new
//    //{
//    //    Id = item.Id,
//    //    Name = item.SaleName
//    //}).ToList();

//    //var rowCombinations = new List<CompositeCombinationDTO>();

//    //applications.ForEach(application =>
//    //{
//    //    var combination = new CompositeCombinationDTO
//    //    {
//    //        Description = application.Name,
//    //        Allow = true
//    //    };

//    //    probes.ForEach(probe =>
//    //    {
//    //        var normalRules = _context
//    //            .NormalRule
//    //            .Where(item =>
//    //            (item.LogicalModelId == search.Model.Id || !item.LogicalModelId.HasValue) &&
//    //            (item.CountryId == search.Country.Id || !item.CountryId.HasValue) &&
//    //            (item.UserLevel == (short)search.UserLevel || !item.UserLevel.HasValue) &&
//    //            (item.ApplicationId == application.Id || !item.ApplicationId.HasValue) &&
//    //            (item.ProbeId == probe.Id || !item.ProbeId.HasValue) &&
//    //            (item.KitId == search.Kit.Id || !item.KitId.HasValue) &&
//    //            (item.OptionId == search.Option.Id || !item.OptionId.HasValue));

//    //        var allow = normalRules.Count() > 0 && !normalRules.Any(item => item.Allow == 0);

//    //        var columnCombination = new CompositeCombinationDTO
//    //        {
//    //            Description = application.Name,
//    //            Allow = true
//    //        };

//    //        combination.Combinations.Add(columnCombination);
//    //    });

//    //    rowCombinations.Add(combination);
//    //});

//    //return await Task.FromResult(rowCombinations);
//}

//async public Task<IEnumerable<Combination1DDTO>> GetRow(CombinationSearchDTO search)
//{
//    var rowLayout = _mapper.Map<LayoutType>(search.RowLayout);
//    var selectedRowField = _context.GetPropertyValue<IQueryable<IQueryableCombination>>(rowLayout.ToString());

//    var combinations = (from field in selectedRowField
//                        join normalRule in _context.NormalRule on field.Id equals normalRule.ApplicationId
//                        where
//                              (normalRule.LogicalModelId == search.Model.Id || !normalRule.LogicalModelId.HasValue) &&
//                              (normalRule.CountryId == search.Country.Id || !normalRule.CountryId.HasValue) &&
//                              (normalRule.UserLevel == (short)search.UserLevel || !normalRule.UserLevel.HasValue) &&
//                              (normalRule.ProbeId == search.Probe.Id || !normalRule.ProbeId.HasValue) &&
//                              (normalRule.ApplicationId == search.Application.Id || !normalRule.ApplicationId.HasValue) &&
//                              (normalRule.KitId == search.Kit.Id || !normalRule.KitId.HasValue) &&
//                              (normalRule.OptionId == search.Option.Id || !normalRule.OptionId.HasValue)
//                        select new
//                        {
//                            RowFieldId = field.Id,
//                            RowFieldName = field.Name,
//                            Rule = normalRule
//                        })
//                        .AsEnumerable()
//                        .GroupBy(item => item.RowFieldId)
//                        .Select(itemGroup => new Combination1DDTO
//                        {
//                            Description = itemGroup.Single(item => item.RowFieldId == itemGroup.Key).RowFieldName,
//                            Allow = itemGroup.All(item => item.Rule.Allow == 0)
//                        })
//                        .OrderBy(item => item.Description);

//    return await Task.FromResult(combinations.ToList());
//}

//async public Task<IEnumerable<Combination1DDTO>> GetSingle(CombinationSearchDTO search)
//{
//    var normalRules = _context
//        .NormalRule
//        .Where(item =>
//        (item.LogicalModelId == search.Model.Id || !item.LogicalModelId.HasValue) &&
//        (item.CountryId == search.Country.Id || !item.CountryId.HasValue) &&
//        (item.UserLevel == (short)search.UserLevel || !item.UserLevel.HasValue) &&
//        (item.ApplicationId == search.Application.Id || !item.ApplicationId.HasValue) &&
//        (item.ProbeId == search.Probe.Id || !item.ProbeId.HasValue) &&
//        (item.KitId == search.Kit.Id || !item.KitId.HasValue) &&
//        (item.OptionId == search.Option.Id || !item.OptionId.HasValue)).ToList();

//    var allow = normalRules.All(item => item.Allow != 0);

//    var combination = new Combination1DDTO
//    {
//        Description = "",
//        Allow = allow
//    };

//    return await Task.FromResult(new List<Combination1DDTO> { combination });
//}
//    }
