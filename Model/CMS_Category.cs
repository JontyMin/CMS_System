namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CMS_Category
    {
        [Key]
        public int cid { get; set; }

        [StringLength(20)]
        public string ctitle { get; set; }

        [StringLength(20)]
        public string cname { get; set; }

        public bool? nav { get; set; }

        public int? navorder { get; set; }

        public bool? search { get; set; }
    }
}
