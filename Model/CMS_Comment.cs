namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_Comment
    {
        [Key]
        public int cmid { get; set; }

        public int? aid { get; set; }

        public int? uid { get; set; }

        public DateTime? cmtime { get; set; }

        [Column(TypeName = "text")]
        public string cmhtml { get; set; }
    }
}
