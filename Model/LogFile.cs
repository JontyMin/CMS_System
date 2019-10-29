namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogFile")]
    public partial class LogFile
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        public string Logtext { get; set; }

        public DateTime Errtime { get; set; }

        [Required]
        [StringLength(50)]
        public string Logsite { get; set; }

        [Required]
        [StringLength(50)]
        public string Errip { get; set; }
    }
}
