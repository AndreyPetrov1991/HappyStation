﻿using System.Collections.Generic;
using System.Linq;

using HappyStation.Core.Entities;

namespace HappyStation.Core.Services.Implementations
{
    public class NewsRepository : RepositoryBase<News>
    {
        public NewsRepository(DatabaseContext.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IEnumerable<News> GetHottest(int count = 4)
        {
            return Db.News.OrderByDescending(n => n.Id).Take(count);
        }
    }
}