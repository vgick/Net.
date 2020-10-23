using System.Collections.Generic;

namespace NBCH_ASP.Models.PDF.PDF {
	public class IndexModel {
		public string FIO {get; set;}
		public List<Check> Regions { get; set;}
	}

	public class Check{
		public string Reg {get; set;}
		public bool ch {get; set;}
	}
}
