using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NBCHASP.Models;
using NBCHLibrary.Interfaces;
using NBCHLibrary.SOAP1C;
using NBCHLibrary.SOAPNBCH;

namespace NBCHASP.Controllers {
	public class NBCHRequestController : Controller {
		private IService1C Service1C { get; set; } = default;
		public NBCHRequestController(IService1C service1C) {
			Service1C = service1C;
		}
		[HttpGet]
		public IActionResult Index() {
			NBCHRequestIndex data = new NBCHRequestIndex();

			data.PersonReq = new PersonReq {
				BirthDateTime = new DateTime(1957, 2, 25),
				Surname = "Петров",
				FirstName = "Иван",
				MiddleName = "Сергеевич",
				BirthPlace = "Владивосток"
			};

			data.AddressReq = new AddressReq[]{
				new AddressReq(){
					AddressType = ((int)AddressType.Registration).ToString(),
					HouseNumber = "12",
					City        = "Владивосток",
					Street      = "Великая"
				},
				new AddressReq(){
					AddressType = ((int)AddressType.Residence).ToString(),
					HouseNumber = "33",
					City        = "Владивосток",
					Street      = "Горная"
				}
			};

			data.IdReq = new IdReq() {
				DocumentNumber = "605557",
				DocumentSeries = "1225",
				DocumentType = ((int)DocumentType.RussianPassport).ToString(),
				issueDateDateTime = new DateTime(1997, 2, 4),
				IssueAuthority = "ОВД Владивостока"
			};

			return View(data);
		}

		[HttpPost]
		public IActionResult Index(NBCHRequestIndex data, string submit) {
			if (submit == "GetFrom1S") {
				CreditDocumentNResult dogovor1C = Service1C.GetCreditDocument(new string[] { "" }, "sap", "~sap", "ddd");
				CreditDocument creditDocument = dogovor1C.CreditDocument;

				data.AddressReq = (AddressReq[])creditDocument.Client;
				data.IdReq = (IdReq)creditDocument.Client;
				data.PersonReq = (PersonReq)creditDocument.Client;
			}
			return View(data);
		}


		public IActionResult GetDataFromNBCH(NBCHRequestIndex data) {
			return View();
		}

	}

}