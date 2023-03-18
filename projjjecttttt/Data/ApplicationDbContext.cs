using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using projjjecttttt.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace projjjecttttt.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Range(18,80)]
        public int Age  { get; set; }
        [Required]
        [MinLength(7)]
        [MaxLength(50)]
        public string FullName { get; set; }
        public virtual ICollection<Reserve> Reserves { get; set; }=new HashSet<Reserve>();
        public virtual ICollection<Review> Reviews { get; set; }=new HashSet<Review>();

        public virtual ICollection<Unit> Units { get; set; }=new HashSet <Unit>();
        public virtual ICollection<Message> Messages { get; set; }




    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
      
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => new { e.UnitID, e.ImgSrc }); entity.HasOne(d => d.Unit)
                .WithMany(p => p.Images)
                .HasForeignKey(d => d.UnitID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Image_Unit");
            });
            modelbuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => new { e.UserID, e.ID }); entity.HasOne(d => d.User)
                .WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Message_User");
            });
            base.OnModelCreating(modelbuilder);

        }
        public DbSet<projjjecttttt.Models.Area> Area { get; set; }
        public DbSet<projjjecttttt.Models.Category> Category { get; set; }
        public DbSet<projjjecttttt.Models.Unit> Unit { get; set; }
        public DbSet<projjjecttttt.Models.Image> Image { get; set; }
        public DbSet<projjjecttttt.Models.Reserve> Reserve { get; set; }
        public DbSet<projjjecttttt.Models.Message> Message { get; set; }
        public DbSet<projjjecttttt.Models.Review> Review { get; set; }
        public DbSet<projjjecttttt.Models.Aminity> Aminity { get; set; }
    }
    
}