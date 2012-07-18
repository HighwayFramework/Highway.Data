﻿using Highway.Data.EntityFramework.StructureMap.Example.Domain;
using Highway.Data.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highway.Data.EntityFramework.StructureMap.Example.Queries
{
    public class LastNameQuery : Query<Person>
    {
        public LastNameQuery(string lastName)
        {
            ContextQuery = m => m.AsQueryable<Person>().Where(x => x.LastName == lastName);
        }
    }
}