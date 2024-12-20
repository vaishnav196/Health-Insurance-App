﻿using HealthInsuranceApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceApplication.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<PurchaseDetails> PurchaseDetails { get; set; }
    }
}
