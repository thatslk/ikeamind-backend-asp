using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikeamind_backend.Core.Models.EFModels
{
    [Index(nameof(Id), IsUnique = true)]
    public abstract partial class AbstractIkeaProduct
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("article")]
        public string Article { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("category")]
        public string Category { get; set; }
        [Column("measure")]
        public string Measure { get; set; }
        [Column("price")]
        public double? Price { get; set; }
        [Column("url")]
        public string Url { get; set; }
        [Column("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
