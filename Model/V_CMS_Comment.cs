namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_CMS_Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cmid { get; set; }

        public int? aid { get; set; }

        public int? uid { get; set; }

        public DateTime? cmtime { get; set; }

        [Column(TypeName = "text")]
        public string cmhtml { get; set; }

        [StringLength(20)]
        public string nname { get; set; }

        [StringLength(50)]
        public string face { get; set; }
    }
}
