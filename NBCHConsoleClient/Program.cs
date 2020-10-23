using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Office.Interop.Excel;
using NBCH_LIB.WCFProxy;

namespace NBCHConsoleClient {
	class Program {
		/// <summary>
		/// Приложение для отладки на первоначальном этапе
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		static async Task Main(string[] args) {
			Console.WriteLine("Start...");

			
			
			// await LoadPDNAsync();
			//			GetPDN();
			await UpdateAccount();
			
			Console.WriteLine("Загрузка окончена.");
			Console.ReadLine();
			return;



			//PDFFiles.DeleteTMPFiles();

			//CancellationTokenSource cancellationTokenSource	= new CancellationTokenSource();
			//CancellationToken cancellationToken				= cancellationTokenSource.Token;

			//// Данные для чтения pdf с диска, сохранения в хранилище
			//string folder		= "z:\\";
			//IPDFSaver pdfSaver	= new PDFSaverProxy();
			//string adUser		= WindowsPrincipal.Current.Identity.Name;
			//string region		= "Регион 1";
			//// Загружаем файлы из директории в хранилище
			//Task task = PDFFiles.UploadPFDsToStorageAsync(folder, pdfSaver, adUser, region, new DateTime(2020, 1, 1), new DateTime(2020, 1, 5),
			//	cancellationToken);

			//// Ждем окончания загрузки или отмены пользователя
			//bool cancelKeyPress	= false;
			//Console.WriteLine("Для отмены нажмите 'c'");
			//while (!task.IsCompleted && !cancelKeyPress) {
			//	if (Console.KeyAvailable && Console.ReadKey().KeyChar == 'c') {
			//		cancellationTokenSource.Cancel();
			//		Console.WriteLine();
			//		cancelKeyPress = true;
			//	}
			//	else { await Task.Delay(200);}
			//}

			//Console.WriteLine("Exit:" + Thread.CurrentThread.GetHashCode());
			//Trace.WriteLine("Exit");
			//Console.ReadKey();
		}

		private static async Task UpdateAccount() {
			await Task.Delay(10);
			
			string[] lines = File.ReadAllLines(@"C:\Data\VS19\trash\NBCH\NBCHConsoleClient\Accounts.txt");
			
			lines.AsParallel().ForAll( i => {
				try {
					using (Service1CsoapProxy pdnProxy = new Service1CsoapProxy()) {
						var gg = pdnProxy.GetCreditDocument(new string[] { "http://ma-1c-main:82/sap/cash_loan.1cws" }, "soapuser", "UfD45", i);
						Console.WriteLine(i.Trim());
					}
				}
				catch (Exception ex) {
					string path = @"C:\Data\VS19\trash\NBCH\NBCHConsoleClient\Error.txt";

					if (!File.Exists(path)) {
						string createText = $"Ошибка обновления данных по договору {i}{Environment.NewLine}" + 
						                    (ex.InnerException?.Message ?? ex.Message) +  Environment.NewLine;
						File.WriteAllText(path, createText, Encoding.UTF8);
					}

					string appendText = $"Ошибка обновления данных по договору {i}{Environment.NewLine}" +
					                    (ex.InnerException?.Message ?? ex.Message) +  Environment.NewLine;
					File.AppendAllText(path, appendText, Encoding.UTF8);
				}
			});
		}

		private static void GetPDN() {
			//using (PDNProxy pdnProxy = new PDNProxy()) {
			//	var gg	= pdnProxy.GetPDNPercents(new string[] { "CDB164974", "CDB164989" });
			//}
		}

		private static async Task LoadPDNAsync() {
			Worksheet objWorkSheet				= GetWorksheet();
			Dictionary<string, double> pdns		= ReadPDNS(objWorkSheet);
			Dictionary<string, double> errors	= new Dictionary<string, double>();

			await Task.Delay(10);

			pdns.AsParallel().ForAll((i) => {
				try {
					LoadPDNAsync(i);
				}
				catch (Exception ex) {
					Console.WriteLine(new String('=', 120));
					Console.WriteLine(ex.Message);
					Console.WriteLine($"{i.Key} {i.Value}");

					errors.Add(i.Key, i.Value);
				}
			});

			if (errors.Keys.Count > 0) {
				Console.WriteLine(new String('*', 120));
				Console.WriteLine(new String('*', 120));
				Console.WriteLine("Вторая попытка.");

				errors.AsParallel().ForAll((i) => {
					try {
						LoadPDNAsync(i);
					}
					catch (Exception ex) {
						Console.WriteLine(new String('=', 120));
						Console.WriteLine(ex.Message);
						Console.WriteLine($"{i.Key} {i.Value}");
					}
				});
			}

			Console.WriteLine(new String('#', 120));
			Console.WriteLine("Загрузка завершена.");

			Console.ReadLine();
		}

		private static void LoadPDNAsync(KeyValuePair<string, double> pdn) {
			using (Service1CsoapProxy service1CProxy = new Service1CsoapProxy()) {
				service1CProxy.LoadPDNFromFile(new string[] { "http://ma-1c-main:82/sap/cash_loan.1cws" }, "soapuser", "UfD45", pdn.Key, pdn.Value);
				Console.WriteLine($"{pdn.Key}, {pdn.Value}");
			}
		}


		/// <summary>
		/// Открыть на чтение файл ПДН
		/// </summary>
		/// <returns></returns>
		private static Worksheet GetWorksheet() {
			string fileName				= @"C:\Data\VS19\trash\NBCH\NBCHConsoleClient\bin\Debug\1.xlsx";
			Application ObjWorkExcel	= new Application();
			Workbook ObjWorkBook		= ObjWorkExcel.Workbooks.Open(fileName);
			return (Worksheet)ObjWorkBook.Sheets[1];
		}

		/// <summary>
		/// Прочитать из файла ПДН
		/// </summary>
		/// <param name="ObjWorkSheet">Файл с ПДН</param>
		/// <returns>Данные ПДН</returns>
		private static Dictionary<string, double> ReadPDNS(Worksheet ObjWorkSheet) {
			var lastCell = ObjWorkSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell);
			int lastRow = (int)lastCell.Row;

			Dictionary<string, double> pdns = new Dictionary<string, double>();

			for (int row = 1; row < lastRow; row++) {
				string accountCode = ObjWorkSheet.Cells[row + 1, 1].Text.ToString();
				double pdnValue = Double.Parse(ObjWorkSheet.Cells[row + 1, 5].Text);
				pdns.Add(accountCode, pdnValue);
			}

			return pdns;

		}

		/// <summary>
		/// Проверяем работоспособность веб сервисов
		/// </summary>
		private static void TestWebService() {
			//var ff	= SOAP1C.GetAccountsLegends(new string[] { "http://ma-1c-main:82/sap/cash_loan.1cws" }, "soapuser", "UfD45", new DateTime(2020, 6, 23), DateTime.Now, default, 100, "Действующий");
			//Service1CProxy service1CProxy	= new Service1CProxy();
			
			//ServiceNBCHProxy serviceNBCHProxy	= new ServiceNBCHProxy();
			////var ff	= serviceNBCHProxy.GetReport("dd", new ProductRequest(), "", "");


			////Task<Report> task0	= SOAPNBCH.GetReport("", new ProductRequest());
			////Task<(CreditDocument document, string[] errors)> task1	= SOAP1C.GetCreditDocumentAsync(new string[] {""}, "", "", "");

			////Task.WaitAll(task0, task1);

			////Report response					= task0.Result;
			////(CreditDocument, string[]) data	= task1.Result;
		}
	}
}
