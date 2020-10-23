using System;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;

namespace NBCH_LIB.SOAP.SOAP1C {
	public class AccountLegendMethod : SOAPMethod<AccountLegendNResult> {
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="userName">Пользователь для подключения к вебслужбе</param>
		/// <param name="userPassword">Пароль для подключения к веб службе</param>
		/// <param name="dateFrom">Начальная дата выборки</param>
		/// <param name="dateTo">Дата окончания выборки</param>
		/// <param name="sellPoint1CCode">Код 1С точки заключения сделки</param>
		/// <param name="quantityParam">Максимальное кол-во строк в выборке</param>
		/// <param name="status">Статусы договоров для выборки</param>
		public AccountLegendMethod(string userName, string userPassword, DateTime dateFrom, DateTime dateTo, string sellPoint1CCode, int quantityParam, params string[] status) {
			_UserName			= userName;
			_UserPassword		= userPassword;
			_DateFrom			= dateFrom;
			_DateTo				= dateTo;
			_QuantityParam		= quantityParam;
			_Status				= status;
			_SellPoint1CCode	= sellPoint1CCode;
		}

		/// <summary>
		/// Сервис сконфигурирован.
		/// </summary>
		public bool Ready { 
			get {
				if (string.IsNullOrEmpty(_UserName) || string.IsNullOrEmpty(_UserPassword) || _DateFrom == default || _DateTo == default) return false;

				return true;
			}
		}

		/// <summary>
		/// Пользователь для подключения к вебслужбе
		/// </summary>
		private string _UserName { get; set; }

		/// <summary>
		/// Пароль для подключения к веб службе
		/// </summary>
		private string _UserPassword { get ; set; }

		/// <summary>
		/// Начальная дата выборки
		/// </summary>
		private DateTime _DateFrom { get; set; }

		/// <summary>
		/// Дата окончания выборки
		/// </summary>
		private DateTime _DateTo { get; set; }

		/// <summary>
		/// Код 1С точки заключения сделки
		/// </summary>
		private string _SellPoint1CCode { get; set; }

		/// <summary>
		/// Максимальное кол-во строк в выборке
		/// </summary>
		private int _QuantityParam { get; set; }

		/// <summary>
		/// Статусы договоров для выборки
		/// </summary>
		private string[] _Status { get; set; }

		/// <summary>
		/// Строка для уникального ключа для кэша.
		/// </summary>
		public override string AdditionKey => string.Join("", _Status ?? new string[0]) + (_SellPoint1CCode ?? "") + _DateFrom.ToShortDateString() + _DateTo.ToShortDateString();

		/// <summary>
		/// Получить данные с веб сервиса
		/// </summary>
		/// <param name="server">Сервер</param>
		/// <param name="requestString">Строка запроса</param>
		/// <returns></returns>
		private AccountLegendNResult GetMethodData(string server, string requestString) {
			AccountLegendNResult result;

			SOAPResponse response	= GetDataFromWebService(_UserName, _UserPassword, requestString, server);

			if (response.Errors.Length != 0)
				result	= ErrorResponse(response, server);
			else
				result	= SuccessResponse(response, server);

			return result;
		}

		/// <summary>
		/// Преобразовать SOAP ответ с ошибкой в ответ для службы
		/// </summary>
		/// <param name="response">SOAP ответ</param>
		/// <returns></returns>
		private AccountLegendNResult ErrorResponse(SOAPResponse response, string server) {
			AccountLegendNResult result = new AccountLegendNResult() {
				AccountLegends	= new AccountLegend[0],
				Errors			= response.Errors
			};

			return result;
		}

		/// <summary>
		/// Преобразовать SOAP ответ с ошибкой в ответ для службы
		/// </summary>
		/// <param name="response">SOAP ответ</param>
		/// <returns></returns>
		private AccountLegendNResult ErrorResponse(string error) {
			AccountLegendNResult result	= new AccountLegendNResult() {
				AccountLegends	= new AccountLegend[0],
				Errors			= new string[] { error }
			};

			return result;
		}

		/// <summary>
		/// Преобразовать успешный SOAP ответ в ответ для службы
		/// </summary>
		/// <param name="response">SOAP ответ</param>
		/// <returns></returns>
		private AccountLegendNResult SuccessResponse(SOAPResponse response, string server) {
			SOAPResponseGetAccountsList soapResponseObject = default;
			try {
				soapResponseObject = DeserializeResponse<SOAPResponseGetAccountsList>(response.Response);
			}
			catch (Exception ex) {
				return ErrorResponse(ex.Message);
			}

			AccountLegend[] sbList	= soapResponseObject?.Body?.GetAccountsListResponse?.Return?.AccountLegends ?? new AccountLegend[0];
			AccountLegendNResult result	= new AccountLegendNResult {
				AccountLegends	= sbList,
				Errors			= new string[0]
			};

			return result;
		}

		public override AccountLegendNResult GetData(string server) {
			string requestString	= SOAPQuery.GetAccountsForCheckSoapMessage(_DateFrom, _DateTo, _QuantityParam, _SellPoint1CCode, _Status);
			AccountLegendNResult result	= GetMethodData(server, requestString);
			return result;
		}
	}
}
