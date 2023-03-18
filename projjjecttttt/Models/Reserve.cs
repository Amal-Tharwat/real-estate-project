using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using projjjecttttt.Data;

namespace projjjecttttt.Models
{
    [Table("Reserve")]
    public class Reserve
    {


        [Key]
        public int Id { get; set; }
       
        public int UnitID { get; set; }
        
        public String UserID { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime Checkin { get; set; }
        [Column(TypeName = "date")]
        public DateTime Checkout { get; set; }
        public int GuestNum { get; set; }
     
        [StringLength(200)]
        public string IdImg { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; }
        [ForeignKey("UnitID")]
        public virtual Unit Unit { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

    }

}