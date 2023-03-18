using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using projjjecttttt.Data;

namespace projjjecttttt.Models
{
    [Table("Unit")]
    public class Unit
    {
       
      
            public Unit()
            {
              //  Aminities = new HashSet<Aminity>();
                Images = new HashSet<Image>();
                Reserves = new HashSet<Reserve>();
                Reviews = new HashSet<Review>();
            }
            [Key]
            public int ID { get; set; }
            public int guestNo { get; set; }
            public int Price { get; set; }
            [Unicode(false)]
            public string Address { get; set; }
            public int RoomNum { get; set; }
            public int PathRoomNum { get; set; }
            public string UserID { get; set; }
            public int CategoryID { get; set; }
            public int AminitiesID { get; set; }
            public int AreaID { get; set; }
            [Unicode(false)]
            public string Description { get; set; }
            [ForeignKey("AminitiesID")]
       
            public virtual Aminity Aminities { get; set; }
            [ForeignKey("AreaID")]
            public virtual Area Area { get; set; }
            [ForeignKey("CategoryID")]
            public virtual Category Category { get; set; }
            [ForeignKey("UserID")]
            public virtual ApplicationUser User  { get; set; }
         //   public virtual ICollection<Aminity> Aminities { get; set; }
            public virtual ICollection<Image> Images { get; set; }
            public virtual ICollection<Reserve> Reserves { get; set; }
            public virtual ICollection<Review> Reviews { get; set; }
         
        }
     
    }

