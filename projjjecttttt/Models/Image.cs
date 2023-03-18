using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projjjecttttt.Models
{
    [Table("Image")]
    public class Image
    {
        
        

            [Key]
        [StringLength(200)]
        [Unicode(false)]
            public string ImgSrc { get; set; }
             [Key]
            public int UnitID { get; set; }
            
            
            [ForeignKey("UnitID")]
            public virtual Unit Unit { get; set; }
        }
    }

