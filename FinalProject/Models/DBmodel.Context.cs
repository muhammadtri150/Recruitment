﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinalProject.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBEntities : DbContext
    {
        public DBEntities()
            : base("name=DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TB_ACCESS_MENU> TB_ACCESS_MENU { get; set; }
        public virtual DbSet<TB_MENU> TB_MENU { get; set; }
        public virtual DbSet<TB_ROLE> TB_ROLE { get; set; }
        public virtual DbSet<TB_SUBMENU> TB_SUBMENU { get; set; }
        public virtual DbSet<TB_USER> TB_USER { get; set; }
        public virtual DbSet<JOB_PORTAL> JOB_PORTAL { get; set; }
        public virtual DbSet<TB_CLIENT> TB_CLIENT { get; set; }
        public virtual DbSet<TB_SKILL> TB_SKILL { get; set; }
        public virtual DbSet<TB_PREFIX> TB_PREFIX { get; set; }
        public virtual DbSet<TB_JOB_POSITION> TB_JOB_POSITION { get; set; }
        public virtual DbSet<TB_LOG_USER_ACTIVITY> TB_LOG_USER_ACTIVITY { get; set; }
    }
}
