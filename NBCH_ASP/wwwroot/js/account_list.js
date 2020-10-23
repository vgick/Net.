let account_list_module = {}

// Идет обновление, повторный вызов не требуется
account_list_module.Updating = false;

// При выборе договора, выбрать подставить его номер для запроса договора из 1С
account_list_module.SetSelectedAccountID = function SetSelectedAccountID(element) {
	$('#Account1CCode').val(element.dataset["account-1cCodeSelected"])
}

// Обновить список договоров
account_list_module.UpdateAccountTable = function UpdateAccountTable() {
	if (account_list_module.Updating) return;
	account_list_module.Updating = true;

	let controller = $('#controller').val()
	let url = "/" + controller + "/GetAccountTable";

	let params = new Object;
	params.region = $('#RegionWebServiceListName').val();

	let orgParam = [];
	let organizations = document.getElementsByName('orgs');
	for (var i = 0; i < organizations.length; i++) {
		if (organizations[i].checked)
			orgParam.push(organizations[i].value);
	}

	params.orgs = orgParam;
	$('#account_table').load(url, params, function (response, status, xhr) { account_list_module.Updating = false; });
}

// Таймер обновления списка договоров
setInterval(account_list_module.UpdateAccountTable, 20000);
