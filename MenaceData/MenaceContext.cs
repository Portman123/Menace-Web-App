using Microsoft.EntityFrameworkCore;
using Noughts_and_Crosses; 

namespace MenaceData
{
    public class MenaceContext : DbContext
    {
        // Web apps need databases 
        // Microsoft Entity Framework Core is an Object Relational Mapper
        //          It takes objects I've written in C# and maps them to a Relational DB
        //                      (SQL in this case)
        // Entity Framework is a "code first" ORM
        //      Meaning you can code your entity classes first 
        //      Then Entity Framework looks at the models created
        //      Compares it to the databse we connect it to
        //      and updates the DB to match
        //      
        // Entity Framework has DbContext objects
        //      which are the link between the entity classes and the Database
        //
        // This class will inherit from DbContext and be the custom created link

        public MenaceContext()
        {

        }

        public MenaceContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Menace;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<PlayerHumanOnWeb>()
            //    .HasBaseType<Player>();
            //modelBuilder.Entity<LinkedList<ValueTuple<Coordinate, Coordinate>>>().HasNoKey();
            //var schemaName = "SYSTEM";
            //modelBuilder.Entity<Player>().ToTable("Player", schemaName);
        }

        // Series of DbSets
        //  Represents an entity used for CRUD operations
        //      DbContext represents the database 
        //      DbSet represents the tables in the DB
        //public DbSet<AIMenace> Menace { get; set; }

        //public DbSet<Game> Game { get; set; }

        public DbSet<BoardPosition> BoardPosition { get; set; }

        public DbSet<Bead> Bead { get; set; }

        public DbSet<Matchbox> Matchbox { get; set; }

        public DbSet<AIMenace> AIMenace { get; set; }

        public DbSet<Player> Player { get; set; }

        public DbSet<PlayerMenace> PlayerMenace { get; set; }

        public DbSet<PlayerOptimal> PlayerOptimal { get; set; }

        public DbSet<PlayerRandom> PlayerRandom { get; set; }

        public DbSet<PlayerHumanOnWeb> PlayerHumanOnWeb { get; set; }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; }
    }
}