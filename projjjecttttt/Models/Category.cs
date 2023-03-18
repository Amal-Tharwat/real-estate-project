using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projjjecttttt.Models
{
    [Table("Category")]
    public class Category
    {
      
      
            public Category()
            {
                Units = new HashSet<Unit>();
            }
            [Key]
            public int ID { get; set; }
            [StringLength(20)]
            [Unicode(false)]
            public string Type { get; set; }
            public virtual ICollection<Unit> Units { get; set; }
        }
    }

