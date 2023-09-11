using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LittleFish.Core.Models
{
    [Table("products", Schema = "products")]
    public class Product
    {
        [Key]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        #nullable enable
        public string? Id { get; set; }

        [Required]
        [Column("name")]
        // [StringLength(14)]
        public string name { get; set; }

        [Required]
        [Column("sellingPrice", TypeName = "decimal")]
        public decimal sellingPrice { get; set; }

        [Column("costPrice", TypeName = "decimal")]
        public decimal costPrice { get; set; }
        
        // [Column("image", TypeName = "date")]
        // public DateTime lastLoginDate { get; set; }
    }
}
