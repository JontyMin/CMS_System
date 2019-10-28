namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_Keyword
    {
        [Key]
        public int kid { get; set; }

        [StringLength(20)]
        public string keyword { get; set; }

        public int? stimes { get; set; }

        public DateTime? ltimes { get; set; }

        public bool? show { get; set; }
    }
}
