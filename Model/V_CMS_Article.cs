namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_CMS_Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int aid { get; set; }

        public int? cid { get; set; }

        [StringLength(40)]
        public string title { get; set; }

        [StringLength(10)]
        public string author { get; set; }

        public int? uid { get; set; }

        public DateTime? ctime { get; set; }

        public DateTime? ptime { get; set; }

        public bool? istop { get; set; }

        public int? state { get; set; }

        public int? hits { get; set; }

        public int? comments { get; set; }

        [Column(TypeName = "text")]
        public string ahtml { get; set; }

        [StringLength(20)]
        public string ctitle { get; set; }

        [StringLength(20)]
        public string cname { get; set; }

        [StringLength(20)]
        public string nname { get; set; }
    }
}
