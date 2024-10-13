using MoviesAPI.Intefaces;

namespace MoviesAPI.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Genre>().HasQueryFilter(m => !m.IsDeleted);
        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public override int SaveChanges()
        {
            var softDeleteEntries = ChangeTracker.Entries<ISoftDelete>().Where(e => e.State == EntityState.Deleted);
            foreach(var entityEntr in softDeleteEntries)
            {
                entityEntr.State = EntityState.Modified;
                entityEntr.Property(nameof(ISoftDelete.IsDeleted)).CurrentValue = true;
                
            }
            return base.SaveChanges();
        }
    }
}
