namespace DataAccessLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NLog")]
    public partial class NLogEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Callsite { get; set; }

        public DateTime Logged { get; set; }

        [Required]
        [StringLength(5)]
        public string Level { get; set; }

        [Required]
        public string Message { get; set; }

    }
}
