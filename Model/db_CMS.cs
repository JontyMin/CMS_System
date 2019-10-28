namespace Model
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class db_CMS : DbContext
	{
		public db_CMS()
			: base("name=db_CMS")
		{
		}

		public virtual DbSet<CMS_Article> CMS_Article { get; set; }
		public virtual DbSet<CMS_Category> CMS_Category { get; set; }
		public virtual DbSet<CMS_Comment> CMS_Comment { get; set; }
		public virtual DbSet<CMS_Keyword> CMS_Keyword { get; set; }
		public virtual DbSet<CMS_User> CMS_User { get; set; }
		public virtual DbSet<V_CMS_Article> V_CMS_Article { get; set; }
		public virtual DbSet<V_CMS_Comment> V_CMS_Comment { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CMS_Article>()
				.Property(e => e.title)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_Article>()
				.Property(e => e.author)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_Article>()
				.Property(e => e.ahtml)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_Category>()
				.Property(e => e.ctitle)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_Category>()
				.Property(e => e.cname)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_Comment>()
				.Property(e => e.cmhtml)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_Keyword>()
				.Property(e => e.keyword)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_User>()
				.Property(e => e.uname)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_User>()
				.Property(e => e.upwd)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_User>()
				.Property(e => e.nname)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_User>()
				.Property(e => e.mobile)
				.IsUnicode(false);

			modelBuilder.Entity<CMS_User>()
				.Property(e => e.face)
				.IsUnicode(false);

			modelBuilder.Entity<V_CMS_Article>()
				.Property(e => e.title)
				.IsUnicode(false);

			modelBuilder.Entity<V_CMS_Article>()
				.Property(e => e.author)
				.IsUnicode(false);

			modelBuilder.Entity<V_CMS_Article>()
				.Property(e => e.ahtml)
				.IsUnicode(false);

			modelBuilder.Entity<V_CMS_Article>()
				.Property(e => e.ctitle)
				.IsUnicode(false);

			modelBuilder.Entity<V_CMS_Article>()
				.Property(e => e.cname)
				.IsUnicode(false);

			modelBuilder.Entity<V_CMS_Article>()
				.Property(e => e.nname)
				.IsUnicode(false);

			modelBuilder.Entity<V_CMS_Comment>()
				.Property(e => e.cmhtml)
				.IsUnicode(false);

			modelBuilder.Entity<V_CMS_Comment>()
				.Property(e => e.nname)
				.IsUnicode(false);

			modelBuilder.Entity<V_CMS_Comment>()
				.Property(e => e.face)
				.IsUnicode(false);
		}
	}
}
