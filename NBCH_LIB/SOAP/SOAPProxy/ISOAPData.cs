using System;

namespace NBCH_LIB.SOAP.SOAPProxy {
	/// <summary>
	/// Ответ SOAP сервиса.
	/// </summary>
	public interface ISOAPData : ICloneable {
		string[] Errors { get; set; }
	}
}