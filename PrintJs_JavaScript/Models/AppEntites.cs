namespace PrintJs_JavaScript.Models
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class AppEntites : DbContext
	{
		public AppEntites()
			: base("name=AppEntites")
		{
		}

		public virtual DbSet<Gender> Genders { get; set; }
		public virtual DbSet<Person> People { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Gender>()
				.HasMany(e => e.People)
				.WithRequired(e => e.Gender)
				.WillCascadeOnDelete(false);
		}
	}
}
