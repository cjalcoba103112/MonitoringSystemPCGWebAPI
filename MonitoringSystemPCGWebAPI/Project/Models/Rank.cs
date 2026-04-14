
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Rank
    {
        [Key]
        public int? RankId { get; set; }
        public string? RankCode { get; set; }
        public string? RankName { get; set; }
        public int? RankLevel { get; set; }

        public int? RankCategoryId { get; set; }

        [ForeignKey("RankCategoryId")]
        public RankCategory? RankCategory { get; set; }

    }
}
