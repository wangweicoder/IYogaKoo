﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace IYogaKoo.Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class iyogakoodbEntities : DbContext
    {
        public iyogakoodbEntities()
            : base("name=iyogakoodbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Centers> Centers { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<ClassFile> ClassFile { get; set; }
        public DbSet<ClassReport> ClassReport { get; set; }
        public DbSet<ClassTeacher> ClassTeacher { get; set; }
        public DbSet<ClassTopic> ClassTopic { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<InterestedClass> InterestedClass { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Space> Space { get; set; }
        public DbSet<YogaArtClass> YogaArtClass { get; set; }
        public DbSet<YogaArticle> YogaArticle { get; set; }
        public DbSet<YogaDicItem> YogaDicItem { get; set; }
        public DbSet<YogaPicture> YogaPicture { get; set; }
        public DbSet<YogiProfile> YogiProfile { get; set; }
        public DbSet<YogaMenus> YogaMenus { get; set; }
        public DbSet<tMessage> tMessage { get; set; }
        public DbSet<tBanner> tBanner { get; set; }
        public DbSet<tWriteLog> tWriteLog { get; set; }
        public DbSet<CenterStare> CenterStare { get; set; }
        public DbSet<Evaluates> Evaluates { get; set; }
        public DbSet<tZanModels> tZanModels { get; set; }
        public DbSet<YogisModels> YogisModels { get; set; }
        public DbSet<YogaUserDetail> YogaUserDetail { get; set; }
        public DbSet<YogaUser> YogaUser { get; set; }
        public DbSet<tLearing> tLearing { get; set; }
        public DbSet<tSign> tSign { get; set; }
        public DbSet<LevelOrder> LevelOrder { get; set; }
        public DbSet<tUserLoginInfo> tUserLoginInfo { get; set; }
        public DbSet<tInstationInfo> tInstationInfo { get; set; }
        public DbSet<tQuestion> tQuestion { get; set; }
        public DbSet<tKeyWord> tKeyWord { get; set; }
        public DbSet<ClassDetail> ClassDetail { get; set; }
    }
}
