using Microsoft.EntityFrameworkCore;
 
namespace belt1.Models
{

    public class BeltContext : DbContext
    {
        
        public DbSet<User> users { get; set; }
        public DbSet<Event> events { get; set; }
        public DbSet<Rsvp> rsvps { get; set; }
        
        // base() calls the parent class' constructor passing the "options" parameter along
        public BeltContext(DbContextOptions<BeltContext> options) : base(options) { }
    }
}