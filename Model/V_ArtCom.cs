using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class V_ArtCom
	{
		public int cmid { get; set; }
		public int? aid { get; set; }

		public int? uid { get; set; }

		public string nname { get; set; }
		public DateTime? cmtime { get; set; }

		public string title { get; set; }

		public string cmhtml { get; set; }
	}
}
