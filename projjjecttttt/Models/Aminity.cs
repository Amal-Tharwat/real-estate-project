using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projjjecttttt.Models
{
    public class Aminity
    {
        public Aminity()
        {
            Units = new HashSet<Unit>();
        }
        [Key]
        public int ID { get; set; }
       // public int? UnitID { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string Wifi { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string AC { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string TV { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string Fan { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string Parking { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string Landline { get; set; }
       // [ForeignKey("UnitID")]
        //[InverseProperty("Aminities")]
       // public virtual Unit Unit { get; set; }
      //  [InverseProperty("Aminities")]
        public virtual ICollection<Unit> Units { get; set; }
    }
}
