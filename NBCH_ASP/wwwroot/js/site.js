(function () {

	x = new Date()
	currentTimeZoneOffsetInHours = -x.getTimezoneOffset() / 60
	document.cookie = "ClientTimeZone="+currentTimeZoneOffsetInHours

})()

