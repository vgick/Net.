using System;

namespace NBCH_LIB.SOAP.SOAP1C {
	static public class SOAPQuery {
		/// <summary>
		/// SOAP сообщение для запроса к методу GetClientDataResponse веб сервиса,
		/// со встроенным параметром "Account1CCode", который перед отправкой необходимо заменить
		/// </summary>
		private static readonly string CreditDocumentSoapMessage =
				@"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
					<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
						<q1:GetClientData xmlns:q1=""ns_credit"">
							<Code xsi:type=""xsd:string"">Account1CCode</Code>
						</q1:GetClientData>
					</s:Body>
				</s:Envelope>";
		/// <summary>
		/// SOAP сообщение для запроса к методу GetTableSB веб сервиса,
		/// со встроенными параметрами "DateFromParam", "DateToParam", "StatusParams", "QuantityParam", который перед отправкой необходимо заменить
		/// </summary>
		private static readonly string AccountsForCheckSoapMessage =
				@"<x:Envelope
					xmlns:x=""http://schemas.xmlsoap.org/soap/envelope/""
					xmlns:nsc=""ns_credit""
					xmlns:loa=""Loan"">
					<x:Header/>
					<x:Body>
						<nsc:GetAccountsList>
							<nsc:DateFrom>DateFromParam</nsc:DateFrom>
							<nsc:DateTo>DateToParam</nsc:DateTo>
							<nsc:StatusDoc>
								StatusParams
							</nsc:StatusDoc>
							<nsc:Quantity>QuantityParam</nsc:Quantity>
							<nsc:Point>PointCodeParam</nsc:Point>
						</nsc:GetAccountsList>
					</x:Body>
				</x:Envelope>";
		/// <summary>
		/// Шаблон для для StatusParams (замена StatusParam)
		/// </summary>
		private static string StatusParam = @"<loa:List>StatusParam</loa:List>";
		/// <summary>
		/// Сгенерировать параметр Status для SOAP запроса.
		/// </summary>
		/// <param name="statuss">Список статусов</param>
		/// <returns>Параметр Status для SOAP запроса</returns>
		private static string GetStatusParams(params string[] statuss){
			string result = default;
			foreach (string status in statuss ?? new string [0]) {
				result	+= StatusParam.Replace("StatusParam", status);
				result	+= '\n';
			}
			return result;
		}

		/// <summary>
		/// SOAP запрос для запроса к методу GetClientDataResponse веб сервиса
		/// </summary>
		/// <param name="account1CCode">Кол договора</param>
		/// <returns></returns>
		public static string GetCreditDocumentSoapMessage(string account1CCode) => CreditDocumentSoapMessage.Replace("Account1CCode", account1CCode);

		/// <summary>
		/// SOAP запрос для запроса к методу GetTableSB веб сервиса 1C
		/// Список договоров за период с заданным статусом
		/// </summary>
		/// <param name="dateFromParam">Начало периода отбора</param>
		/// <param name="dateToParam">Окончание периода отбора</param>
		/// <param name="quantityParam">Кол-во договоров для отображения</param>
		/// <param name="pointCodeParam">Код точки</param>
		/// <param name="statusParams">Статусы договоров</param>
		/// <returns></returns>
		public static string GetAccountsForCheckSoapMessage(DateTime dateFromParam, DateTime dateToParam, int quantityParam,
				string pointCodeParam, params string[] statusParams) =>
			AccountsForCheckSoapMessage.
				Replace("DateFromParam", SOAPNBCH.SOAPNBCH.DateTimeToString(dateFromParam)).
				Replace("DateToParam", SOAPNBCH.SOAPNBCH.DateTimeToString(dateToParam)).
				Replace("QuantityParam", quantityParam == default ? "" : quantityParam.ToString()).
				Replace("PointCodeParam", quantityParam == default ? "" : pointCodeParam).
				Replace("StatusParams", GetStatusParams(statusParams));
	}
}
