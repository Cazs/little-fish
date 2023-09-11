using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LittleFish.Core.Models
{
    [Table("users", Schema = "users")]
    public class User
    {
        [Key]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        #nullable enable
        public string? Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(14)]
        public string name { get; set; }

        [Required]
        [Column("email")]
        [StringLength(16)]
        public string email { get; set; }

        [Column("dateOfBirth", TypeName = "date")]
        public DateTime dateOfBirth { get; set; }
        
        [Column("lastLoginDate", TypeName = "date")]
        public DateTime lastLoginDate { get; set; }
    }
}
