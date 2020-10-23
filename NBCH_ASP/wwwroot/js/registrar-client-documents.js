function uploadFiles(element, div, spin, error_message) {
	spin.style.visibility	= "visible";

	var formData = new FormData();

	formData.append("idFileDescription", element.dataset["fileDescription"])
	formData.append("client1CCode", element.dataset["client-1cCode"])
	formData.append("account1CCode", element.dataset["account-1cCode"])

	x = new Date()
	clientTimeZone = -x.getTimezoneOffset() / 60
	formData.append("clientTimeZone", clientTimeZone)

	for (var i = 0; i != element.files.length; i++) {
		formData.append("files", element.files[i]);
	}

	$.ajax(
		{
			url: "/RegistrarDocuments/UploadFiles",
			data: formData,
			processData: false,
			contentType: false,
			type: "POST",
			success: function (data) {
				UpdateRegistrar(element, div)
			},
			error: function (error) {
				spin.style.visibility = "hidden";

				$(error_message).append(' \
					<div class="alert alert-danger alert-dismissible fade show" role="alert"> \
						<strong>Ошибка загрузки!</strong >&nbspФайл не загружен. Вы можете повторить попытку. \
						<hr>' +
					'<strong>' + error.status + '</strong>&nbsp' + error.responseText + '\
						<button type="button" class="close" data-dismiss="alert" aria-label="Close"> \
							<span aria-hidden="true">&times;</span> \
						</button> \
					</div>')


				document.getElementById(element.id).value = "";
			}
		}
	);
}

function UpdateRegistrar(element, div) {
	var s = "/RegistrarDocuments/UpdateRegistrar";

	var params = new Object;
	params.account1CCode		= element.dataset["account-1cCode"];
	params.client1CCode			= element.dataset["client-1cCode"];
	params.affiliationOfAccount = element.dataset["affiliationAccount"];

	$(div).load(s, params);
}

function DeleteFile(element, idFile, div, result_message) {
	var formData = new FormData();
	formData.append("idFile", idFile)

	$.ajax(
		{
			url: "/RegistrarDocuments/DeleteFile",
			data: formData,
			processData: false,
			contentType: false,
			type: "POST",
			success: function () {
				UpdateRegistrar(element, div)
			},
			error: function (error) {
				$(result_message).append(' \
					<div class="alert alert-danger alert-dismissible fade show" role="alert"> \
						<strong>Файл не удален</strong >\
						<hr>' +
					'<strong>' + error.status + '</strong>&nbsp' + error.responseText + '\
						<button type="button" class="close" data-dismiss="alert" aria-label="Close"> \
							<span aria-hidden="true">&times;</span> \
						</button> \
					</div>')

				document.getElementById(element.id).value = "";
			}
		}
	);

}

