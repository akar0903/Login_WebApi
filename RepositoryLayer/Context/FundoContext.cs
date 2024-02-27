﻿using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using User = RepositoryLayer.Entity.User;

namespace RepositoryLayer.Context
{
    public class FundoContext : DbContext
    {
        public FundoContext(DbContextOptions options) : base(options)
        { }
        public DbSet<User> UserTable { get; set; }

        public DbSet<User> login { get; set; }
    }
}