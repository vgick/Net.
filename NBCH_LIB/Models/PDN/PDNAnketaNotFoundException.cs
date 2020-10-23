using System;

namespace NBCH_LIB.Models.PDN {
	//[DataContract]
	public class PDNAnketaNotFoundException : Exception {
		public PDNAnketaNotFoundException(string message)
			: base(message) { }
	}
}
