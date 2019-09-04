﻿using Common.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Highway.Data.EntityFramework.Test.TestDomain
{
    public class TestDataContext : DataContext
    {
        public TestDataContext(string connectionString, IMappingConfiguration mapping, ILog logger)
            : base(connectionString, mapping, null, logger)
        {
        }

        public string ConnectionString { get; set; }
    }
}
