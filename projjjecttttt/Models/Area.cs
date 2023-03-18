using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projjjecttttt.Models
{
    [Table("Area")]
    public class Area
    {
        
       
            public Area()
            {
                Units = new HashSet<Unit>();
            }
            [Key]
            public int ID { get; set; }
            [StringLength(50)]
            [Unicode(false)]
            public string name { get; set; }

        public double xAxis { get; set; }

        public double yAxis { get; set; }



        public virtual ICollection<Unit> Units { get; set; }
        }
    }

