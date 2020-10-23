(function () {
	// При выборе клиента, подгрузить его список его анкет НБКИ
	$(".client-clickable-row").click(function () {
		var s = '/ArchiveCH/GetCreditHistoryList?client1CCode=' + $(this).data("client-1c-code");
		$('#credit_history').load(s);
	});

})()