using Microsoft.EntityFrameworkCore;
using projjjecttttt.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projjjecttttt.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key]
        public string UserID { get; set; }
        [Unicode(false)]
        public string Messeage { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string Email { get; set; }

        [ForeignKey("UserID")]

        public virtual ApplicationUser User { get; set; }
    }
}
