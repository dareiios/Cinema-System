let url = 'https://localhost:44315';
const email = Cookies.get('email');
document.getElementById('loginName').textContent = email;

axios.get(url + '/api/Ticket/GetTickets').then(function (response) {
	console.log(response.data);
	for (let ticket of response.data) {
		document.getElementById('tickets').insertAdjacentHTML(
			'beforeend',
			`
			<div class="col">
				<div class="account__ticket shadow-sm p-3 mb-5 bg-body-tertiary rounded">
					<img class="account__poster" src="${url}${ticket.img}" />
					<div>
						<div class="account__ticket-item">
							<strong>Название фильма: </strong> ${ticket.cinemaName}
						</div>
						<div class="account__ticket-item">
							<strong>Ряд: </strong> ${ticket.row}
						</div>
						<div class="account__ticket-item">
							<strong>Место: </strong> ${ticket.seat}
						</div>
						<div class="account__ticket-item">
							<strong>Зал: </strong> ${ticket.hall}
						</div>
						<div class="account__ticket-item">
							<strong>Сеанс: </strong> ${ticket.seanceDate}
						</div>
						<div class="account__ticket-item">
							<strong>Сумма: </strong> ${ticket.price}
						</div>
					</div>
					<img style="margin-left:30px; width: 200px" src="https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=https://localhost:44315/QR/Index/${ticket.row}${ticket.seat}"></img>
				</div>
			</div>
			`
		);
	}
});

//1.на стр аккаунта, див роу дать айди
//2.внутри роу кол див вырезать и вставить в accountjs. в цикле пробежаться по респонс.дата
//обратиться по айди к диву и через инсерт..html втсавить кусок и поменять места с данными
