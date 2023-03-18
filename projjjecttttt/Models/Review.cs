using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using projjjecttttt.Data;

namespace projjjecttttt.Models
{
    [Table("Review")]
    public class Review
    {


        [Key]
        public int ID { get; set; }
        [Unicode(false)]
        public string Details { get; set; }

        public string nameReviwer { get; set; }
        public String UserID { get; set; }
        public int UnitID { get; set; }
        [ForeignKey("UnitID")]
        public virtual Unit Unit { get; set; }
         [ForeignKey("UserID")]
         public virtual ApplicationUser User { get; set; }

    }
}

