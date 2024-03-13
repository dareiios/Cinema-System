let url = 'https://localhost:44315';
let filmsContainer = document.querySelector('#films'); //первый элемент из класса films
let promos = document.querySelector('#Promos');
let cinemaidForSeances;
let cinemaForPushkinCard;

//выбранный фильм
function initCardMovie(cinemaId) {
	cinemaidForSeances = cinemaId;
	axios //для выполнения HTTP-запросов
		.get(url + '/api/Cinema/Get?cinemaId=' + cinemaId)
		.then(function (response) {
			console.log(response);
			cinemaForPushkinCard = response;
			let cinema = response.data;
			// document.getElementById('formFileSm').files[0]
			document.querySelector('.modal-buy__poster').src = 'https://localhost:44315' + cinema.poster;
			document.querySelector('.modal-buy__film h2').textContent = cinema.name;
			document.querySelector('.modal-buy__duration').textContent = cinema.duration + ' мин';
			document.querySelector('.modal-buy__age-limit').textContent = cinema.ageLimit + '+';
			document.querySelector('#prokatEnd').textContent = 'c ' + moment(cinema.endDate).format('D MMMM YYYY');
			document.querySelector('#prokatStart').textContent = 'c ' + moment(cinema.startDate).format('D MMMM YYYY');
			document.querySelector('#director').textContent = cinema.producer;
			document.querySelector('#descriptionFilm').textContent = cinema.description;
			// initSeancesMovie(cinemaId);
		})
		.catch(function (error) {
			// handle error
			console.log(error);
		})
		.then(function () {
			// always executed
		});
}

function initSeances(response) {
	let seances = response.data;
	document.querySelector('.model-buy__seances').textContent = '';
	for (let seance of seances) {
		document.querySelector('.model-buy__seances').insertAdjacentHTML(
			'beforeend',
			`
					<div class="model-buy__seances-item" data-id="${seance.id}" style="margin-right: 18px;">
					<h3>${moment(seance.date).format('D MMMM')}
					<img src="./img/delete2.png" class="deleteSeance d-none" type="button" ></h3>
					<div class="model-buy__card">

						<button class="model-buy__card-Btn">
							<div>${seance.cinema.format}</div>
							${moment(seance.date).format('HH:mm')}
							<span>${seance.price} p</span>
						</button>
					</div>
				</div>
			`
		);
	}

	document.querySelector('.model-buy__seances').insertAdjacentHTML(
		'beforeend',
		`
		<center type="button" class="d-none" id="createSeanceBtn"><img src="./img/add.png" style="width: 30px;"> </center>
		`
	);

	const role = Cookies.get('role');
	if (role == 'Admin') {
		document.querySelectorAll('.deleteSeance').forEach((btn) => {
			btn.classList.remove('d-none');
		});
		document.getElementById('createSeanceBtn').classList.remove('d-none');
	}

	document.getElementById('createSeanceBtn').addEventListener('click', function (x) {
		// let halls;
		// let hallsIds;
		// axios.get(url + '/api/Hall/GetAll').then(function (response) {
		// 	response.data.forEach(function (hall) {
		// 		halls = hall.name;
		// 		hallsIds = hall.id;
		// 	});
		// });

		// <input id="CreateSeanceDate" type="DateTime" placeholder="Дата и время">

		document.querySelector('.model-buy__seances').insertAdjacentHTML(
			'beforeend',
			`
					<div class="model-buy__card" data-id="">
						<span>
						<input id="CreateSeanceDate" type="text" placeholder="Дата и время">
							<select id="CreateSeanceHall" name="CompanyName" style="height:21px; width:150px">
								<option selected disabled>
									Выберете Зал
								</option>
								<option value=3>
									2D/3D
								</option>
								<option value=1002>
									VIP
								</option>
								<option value=1003>
									Комфорт
								</option>
							</select>

							<input id="CreateSeancePrice" type="number" placeholder="Цена">
						<span>
						<img id="saveSeance" type="button" style=" margin-left: 172px;margin-bottom: -33px;	width: 20px;"
							src="./img/check.png"/>
					</div>
            `
		);
		new AirDatepicker('#CreateSeanceDate', {
			timepicker: true,
			isMobile: true,
			autoClose: true,
		});
		document.getElementById('saveSeance').addEventListener('click', function (x) {
			//добавить ВЫБРАННЫЙ зал
			// var date = document.getElementById('CreateSeanceDate').value;

			// 		onst start = document.getElementById('inputCreateDate').value.split(' - ')[0].split('.');
			// const startDate = start[2] + '-' + start[1] + '-' + start[0];
			const start = document.getElementById('CreateSeanceDate').value.split(' ');
			var date = start[0].split('.');
			const seanceDateTime = date[2] + '-' + date[1] + '-' + date[0] + 'T' + start[1];
			console.log(seanceDateTime);
			var hall = document.getElementById('CreateSeanceHall').value;
			var price = document.getElementById('CreateSeancePrice').value;
			let data = { CinemaId: cinemaidForSeances, HallId: hall, Date: seanceDateTime, Price: price };
			axios.post(url + '/api/Seance/Create', data).then(function (response) {
				console.log(response.data);
			});
		});
	});

	document.querySelectorAll('.deleteSeance').forEach((btn) => {
		btn.addEventListener('click', function () {
			let cardId = this.parentElement.parentElement.getAttribute('data-id');
			console.log(cardId);
			axios.post(url + '/api/Seance/Delete?seanceId=' + cardId).then(function (response) {
				console.log(response.data);
			});
		});
	});

	document.querySelectorAll('.model-buy__card-Btn').forEach((x) => {
		x.addEventListener('click', function () {
			document.querySelector('[data-name].active').classList.remove('active');
			document.querySelector('[data-name="seats"]').classList.add('active');

			var seanceId = x.parentElement.parentElement.dataset.id;
			console.log(seanceId);
			document.getElementById('modalByuView').style.display = 'flex';
			document.querySelector('.modal-buy__info').style.display = 'none';
			axios
				.get(url + '/api/Seance/Get?seanceId=' + seanceId)
				.then(function (response) {
					var seance = response.data;
					document.getElementById('time').textContent = moment(seance.date).format('HH:mm');
					document.getElementById('seanceDate').textContent = moment(seance.date).format('сегодня, D MMMM');
					document.getElementById('seanceTitle').textContent = seance.cinema.name;
					document.getElementById('seanceDuration').textContent = seance.cinema.duration + ' мин';
					document.getElementById('seancePromoDuration').textContent = seance.promoDuration;
					document.getElementById('seanceAge').textContent = seance.cinema.ageLimit + '+';
					document.getElementById('senaceFormat').textContent = seance.cinema.format;
					document.getElementById('seanceHall').textContent = seance.hall.name;
					let rowNums = document.querySelectorAll('.modal-buy__nums');
					rowNums[0].textContent = '';
					rowNums[1].textContent = '';
					document.querySelector('.modal-buy__seats').textContent = '';
					document.querySelector('.modal-buy__tickets').textContent = '';

					let row = 0;
					let seatTypes = [];
					for (let ticket of seance.tickets) {
						// console.log(ticket);
						//почему???
						if (ticket.seat.rowNumber != row) {
							row = ticket.seat.rowNumber;
							rowNums[0].insertAdjacentHTML('beforeend', `<div>${row}</div>`);
							rowNums[1].insertAdjacentHTML('beforeend', `<div>${row}</div>`);
							document.querySelector('.modal-buy__seats').insertAdjacentHTML(
								'beforeend',
								`
									<div class="modal-buy__row">
											<button
												class="modal-buy__seat ${ticket.status != 0 ? 'saled' : ''} ${ticket.seat.seatType.name == 'VIP' ? 'vip' : ''}"
												data-row="${row}"
												data-number="${ticket.seat.number}"
												data-price="${ticket.price}"
												data-id="${ticket.id}"
											></button>

										</div>
									`
							);
						} else {
							var rows = document.querySelectorAll('.modal-buy__seats .modal-buy__row');
							//почему???
							if (rows.length == 0) {
								document.querySelector('.modal-buy__seats').insertAdjacentHTML(
									'beforeend',
									`
										<div class="modal-buy__row">

											</div>
										`
								);
								rows = document.querySelectorAll('.modal-buy__seats .modal-buy__row');
							}
							//почему???
							rows[rows.length - 1].insertAdjacentHTML(
								'beforeend',
								`
										<button
												class="modal-buy__seat ${ticket.status != 0 ? 'saled' : ''} ${ticket.seat.seatType.name == 'VIP' ? 'vip' : ''}"
												data-row="${row}"
												data-number="${ticket.seat.number}"
												data-price="${ticket.price}"
												data-id="${ticket.id}"
											></button>
										`
							);
						}
						//почему???
						if (!seatTypes.includes(ticket.seat.seatType.name)) {
							document.querySelector('.modal-buy__tickets').insertAdjacentHTML(
								'beforeend',
								`
									<div><span class="circle ${ticket.seat.seatType.name == 'VIP' ? 'vip' : 'normal'}"></span> ${ticket.price}р</div>

									`
							);
							seatTypes.push(ticket.seat.seatType.name);
						}
					}
					document.querySelector('.modal-buy__tickets').insertAdjacentHTML(
						'beforeend',
						`
							<div><span class="circle saled"></span> Занято</div>

							`
					);
					InitSeats();
				})
				.catch(function (error) {
					// handle error
					console.log(error);
				})
				.then(function () {
					// always executed
				});
		});
	});
}

//сегодняшние сеансы у фильма
function initTodaySeancesMovie(cinemaId) {
	axios
		.get(url + '/api/Seance/GetToday?cinemaId=' + cinemaId)
		.then(function (response) {
			// console.log(response);
			initSeances(response);
		})
		.catch(function (error) {
			// handle error
			console.log(error);
		})
		.then(function () {
			// always executed
		});
}

function initAllSeancesMovie(cinemaId) {
	axios
		.get(url + '/api/Seance/GetAll?cinemaId=' + cinemaId)
		.then(function (response) {
			console.log(response);
			initSeances(response);
		})
		.catch(function (error) {
			// handle error
			console.log(error);
		})
		.then(function () {
			// always executed
		});
}

//функция для получения карточек сегодня и все
function GetCards(response, callback) {
	for (let film of response.data) {
		filmsContainer.insertAdjacentHTML(
			'beforeend',
			`
			<div class="col">
					<div class="btn btn-group-link toEdit d-none" id="EditBtn">
						<img src="./img/edit.png" style="width: 20px; background-color: #bd257f">
					</div>	
					<div class="btn btn-group-link toDelete d-none" id="DeleteBtn">
						<img src="./img/delete2.png" style="width: 20px; background-color: #bd257f">
					</div>		
				<div class="card films-card" id="cinema" data-cinema="${film.id}">
					<img
						src="${url + film.poster}"
						class="card-img-top img"
						alt="..."
					/>
					<div class="films-card__footer">
						<h4 class="films-card__title">${film.name}</h4>
						<div class="films-card__extantion">
							<div class="films-card__genre">${film.genre}</div>
							<div class="films-card__format">${film.format}</div>
						</div>
					</div>
					<div class="film-card-age">${film.ageLimit}+</div>
				</div>
			</div>
			`
		);
		if (film.isPushkin) {
			document
				.querySelector('.films-card')
				.insertAdjacentHTML('beforeend', `<div class="films-card__tag">Пушкинская карта</div>`);
		}
	}
	const role = Cookies.get('role');
	if (role == 'Admin') {
		document.querySelectorAll('.toEdit').forEach((btn) => {
			btn.classList.remove('d-none');
		});
		document.querySelectorAll('.toDelete').forEach((btn) => {
			btn.classList.remove('d-none');
		});
	}

	document.querySelectorAll('.toDelete').forEach((btn) => {
		btn.addEventListener('click', function (e) {
			e.stopPropagation();
			let cardId = this.nextElementSibling.getAttribute('data-cinema');
			console.log(cardId);
			console.log(cardId);
			axios.post(url + '/api/Cinema/Delete?cinemaId=' + cardId).then(function (response) {
				console.log(response.data);
			});
		});
	});

	document.querySelectorAll('.toEdit').forEach(function (btn) {
		btn.addEventListener('click', function (e) {
			e.stopPropagation();
			let cardId = btn.nextElementSibling.nextElementSibling.getAttribute('data-cinema');
			axios.get(url + '/api/Cinema/Get?cinemaId=' + cardId).then(function (response) {
				const CreateModal = new bootstrap.Modal('#CreateCinema');
				CreateModal.show();
				document.getElementById('CreateBtn').style.display = 'none';
				document.getElementById('UpdateBtn').style.display = 'block';

				document.querySelector('#TitleCinema').value = response.data.name;
				document.querySelector('#CreateDuration').value = response.data.duration;
				document.querySelector('#CreateAgeLimit').value = response.data.ageLimit;
				document.querySelector('#CreateDescription').value = response.data.description;
				document.querySelector('#CreateProducer').value = response.data.producer;
				document.getElementById('createImg').src = response.data.poster;
				document.querySelector('#CreateFormat').value = response.data.format;
				document.querySelector('#CreateFormat').value = response.data.format;
				if (response.data.isPushkin == true) {
					document.querySelector('#IsPushkinCard').checked = true;
				}
				let responseDate = ParseDateFromDB(response.data.startDate, response.data.endDate);
				document.querySelector('#inputCreateDate').value = responseDate.Start + ' - ' + responseDate.End;
			});

			document.getElementById('UpdateBtn').addEventListener('click', function () {
				const isPushkinCard = document.getElementById('IsPushkinCard').checked;
				if (isPushkinCard.checked) {
					isPushkinCard.value = 'true';
				}
				let dateForm = ParseDateToDB(document.getElementById('inputCreateDate'));
				let input = {
					Id: cardId,
					Name: document.getElementById('TitleCinema').value,
					Duration: document.getElementById('CreateDuration').value,
					AgeLimit: document.getElementById('CreateAgeLimit').value,
					StartDate: dateForm.startDate,
					EndDate: dateForm.endDate,
					Poster: document.getElementById('createImg').src,
					Producer: document.getElementById('CreateProducer').value,
					Description: document.getElementById('CreateDescription').value,
					IsPushkin: isPushkinCard,
					Format: document.getElementById('CreateFormat').value,
				};
				console.log(input);
				axios.post(url + '/api/Cinema/Update', input).then(function (response) {
					console.log(response.data);
				});
			});
		});
	});

	document.querySelectorAll('.films-card').forEach((card) => {
		card.addEventListener('click', function (e) {
			let cinemaId = this.getAttribute('data-cinema'); //значение атбрибута data-cinema(id фильма)
			initCardMovie(cinemaId);
			callback(cinemaId);
			//функция приходит на вход и тут вызывается
			document.querySelector(`.modal-buy__ticketsoldstatus`).style.display = 'none';
			const modalBuy = new bootstrap.Modal('#BuyModal', {
				keyboard: false,
			});
			modalBuy.show();
		});
	});
}
//из точек в тире(из формы в бд)
// 09.03.2024-09.03.2025
//2024-03-09
//2025-03-09
function ParseDateToDB(data) {
	const start = data.value.split(' - ')[0].split('.');
	const startDate = start[2] + '-' + start[1] + '-' + start[0];
	const end = data.value.split(' - ')[1].split('.');
	const endDate = end[2] + '-' + end[1] + '-' + end[0];
	return { startDate, endDate };
}
//из тире в точки(из бд в форму)
// "startDate": "2024-02-19T00:00:00",
//   "endDate": "2024-03-03T00:00:00",
// 19.02.2024-03.03.2024
function ParseDateFromDB(start, end) {
	let dateStart = start.split('T')[0].split('-');
	let Start = dateStart[2] + '.' + dateStart[1] + '.' + dateStart[0];
	let dateEnd = end.split('T')[0].split('-');
	let End = dateEnd[2] + '.' + dateEnd[1] + '.' + dateEnd[0];
	return { Start, End };
}
// карточки сегодняшних фильмов
function getTodayCards() {
	filmsContainer.textContent = ''; //чтобы не показывались данные с верстки
	axios
		.get(url + '/api/Cinema/GetTodayCinemas')
		.then(function (response) {
			// console.log(response);
			GetCards(response, initTodaySeancesMovie);
			//GetCards(response, функция сегодняшние сеансы);
		})
		.catch(function (error) {
			// handle error
			console.log(error);
		})
		.then(function () {
			// always executed
		});
}
//чтобы сразу отображались, а ниже вызов для отображения по клику
getTodayCards();

function getAllCards() {
	filmsContainer.textContent = ''; //чтобы не показывались данные с верстки
	axios
		.get(url + '/api/Cinema/GetAll')
		.then(function (response) {
			console.log(response);
			GetCards(response, initAllSeancesMovie);
		})
		.catch(function (error) {
			// handle error
			console.log(error);
		})
		.then(function () {
			// always executed
		});
}

//рекламные картинки
axios
	.get(url + '/api/Promos/GetAll')
	.then(function (response) {
		// console.log(response);
		let count = 0;
		for (let promo of response.data) {
			document.querySelector('.carousel-indicators').insertAdjacentHTML(
				'beforeend',
				`
				<button type="button" data-bs-target="#Promos" data-bs-slide-to="${count}" class="${count == 0 && 'active'}"></button>
            `
			);
			document.querySelector('.carousel-inner').insertAdjacentHTML(
				'beforeend',
				`
				<div class="carousel-item ${(count == 0 && 'active') || ''}">
					<img src="${promo.img}" class="d-block w-100" alt="..." />
					<div class="carousel-caption d-none d-md-block"></div>
				</div>
            `
			);
			count++;
		}
	})
	.catch(function (error) {
		// handle error
		console.log(error);
	})
	.then(function () {
		// always executed
	});

// для каждого эдемента из btn-group-link. если нажали на него-срабатывает функция.
//определяется тип того, куда нажали
//и вызывается метод getTodaysCards
document.querySelectorAll('.btn-group-link').forEach((x) => {
	x.addEventListener('click', function (e) {
		//е-событие, таргет-конкретный элемент, который вызывал событие. те е.data-данные о событии клик. e.target.data-данные о нажатой карточке
		let type = e.target.dataset.type;
		console.log('type is' + type);
		if (type == 'today') {
			getTodayCards();
		} else {
			getAllCards();
		}
	});
});

function initSelectedTickets() {
	document.querySelector('.model-buy__selected-seats-tickets').textContent = '';
	let selectedSeats = document.querySelectorAll('.modal-buy__seat.selected');
	if (selectedSeats.length > 0) {
		document.getElementById('next').disabled = false;
	} else {
		document.getElementById('next').disabled = true;
	}

	document.getElementById('selectedSeats').textContent = '';

	let sum = 0;
	document.getElementById('sum').textContent = '';
	selectedSeats.forEach((x) => {
		let row = x.dataset.row;
		let number = x.dataset.number;
		let price = x.dataset.price;
		sum += +price;
		document.getElementById('selectedSeats').insertAdjacentHTML(
			'beforeend',
			`
		<div class="model-buy__selected-seat" data-id="${x.dataset.id}">
			<div>
				${row}ряд, ${number}место
				<span>${price}p</span>
			</div>
		</div>
		`
		);

		document.querySelector('.model-buy__selected-seats-tickets').insertAdjacentHTML(
			'beforeend',
			`
		<div class="model-buy__selected-seat">
			<div>
				${row}ряд, ${number}место
				<span>${price}p</span>
			</div>
			<button class="remove" data-row="${row}" data-number="${number}">
				<svg
					viewBox="0 0 20 22"
					fill="currentColor"
					xmlns="http://www.w3.org/2000/svg"
					class="cb-w-6 cb-h-6"
				>
					<path
						fill-rule="evenodd"
						clip-rule="evenodd"
						d="M6 1C6 0.447715 6.44772 0 7 0H13C13.5523 0 14 0.447715 14 1C14 1.55228 13.5523 2 13 2H7C6.44772 2 6 1.55228 6 1ZM0.5 4C0.5 3.44772 0.947715 3 1.5 3H18.5C19.0523 3 19.5 3.44772 19.5 4C19.5 4.55228 19.0523 5 18.5 5H17.9429L16.9983 21.0587C16.9672 21.5873 16.5295 22 16 22H4C3.47052 22 3.03282 21.5873 3.00173 21.0587L2.05709 5H1.5C0.947715 5 0.5 4.55228 0.5 4ZM4.06055 5L4.9429 20H15.0571L15.9394 5H4.06055ZM7.58825 7.00043C8.14029 6.9842 8.60098 7.41855 8.61722 7.9706L8.88192 16.9706C8.89816 17.5226 8.4638 17.9833 7.91175 17.9996C7.35971 18.0158 6.89902 17.5814 6.88278 17.0294L6.61808 8.0294C6.60184 7.47735 7.0362 7.01667 7.58825 7.00043ZM12.4118 7.00043C12.9638 7.01667 13.3982 7.47735 13.3819 8.0294L13.1172 17.0294C13.101 17.5814 12.6403 18.0158 12.0882 17.9996C11.5362 17.9833 11.1018 17.5226 11.1181 16.9706L11.3828 7.9706C11.399 7.41855 11.8597 6.9842 12.4118 7.00043Z"
					></path>
				</svg>
			</button>
		</div>
		`
		);
	});
	document.getElementById('sum').textContent = sum + 'p';
	document.querySelectorAll('.model-buy__selected-seat .remove').forEach((x) => {
		x.addEventListener('click', function () {
			this.parentElement.remove();
			let row = this.dataset.row;
			let number = this.dataset.number;
			let seat = document.querySelector(`.modal-buy__seats [data-row="${row}"][data-number="${number}"]`);
			//создаем событие клик для седенья и вызываем его намеренно.
			seat.dispatchEvent(new Event('click'));
		});
	});
}

function InitSeats() {
	document.querySelectorAll('.modal-buy__seat').forEach((x) => {
		x.addEventListener('click', function (e) {
			// что означает [data-name].active
			let activeSection = document.querySelector('[data-name].active').dataset.name;
			if (activeSection == 'buy') {
				return;
			}
			//есть много элементов, события на которых нужно обрабатывать похожим образом,
			// то вместо того, чтобы назначать обработчик каждому, мы ставим один обработчик на их общего предка.
			//?????
			let number = e.target.dataset.number;
			//добавляет класс, если нет и удаляет, если есть
			e.target.classList.toggle('selected');
			if (e.target.classList.contains('selected')) {
				e.target.textContent = number;
			} else {
				e.target.textContent = '';
			}
			initSelectedTickets();
		});
	});
}
//какие картинки активны при переключении кнопок
document.getElementById('next').addEventListener('click', function () {
	document.querySelector('.model-buy__pay').style.display = 'block';
	document.querySelector('.model-buy__selected-seats').style.display = 'none';
	if (cinemaForPushkinCard.data.isPushkin) {
		document.querySelector('.modal-buy__variants').insertAdjacentHTML(
			`beforeend`,
			` <div class="modal-buy__variant">
				<div>
					<img
						class="modal-buy__variant-img"
						src="./pushkinsCard.d965fe48.svg"
						alt=""
					/>Пушкинская карта
				</div>

				<svg
					viewBox="0 0 18 17"
					xmlns="http://www.w3.org/2000/svg"
					fill="currentColor"
					class="m:cb-w-3 d:cb-w-4 cb-text-[color:var(--mainLevel1)]"
				>
					<path
						fill-rule="evenodd"
						clip-rule="evenodd"
						d="M16.7004 0.654089C17.2123 0.981346 17.3619 1.66156 17.0346 2.1734L8.02993 16.2569C7.83828 16.5567 7.51349 16.7452 7.15816 16.763C6.80283 16.7808 6.46084 16.6256 6.24021 16.3465L1.0679 9.80313C0.691167 9.32653 0.772124 8.63477 1.24872 8.25804C1.72532 7.88131 2.41708 7.96226 2.79381 8.43886L7.00837 13.7706L15.1811 0.988299C15.5084 0.476464 16.1886 0.326833 16.7004 0.654089Z"
					></path>
				</svg>
			</div>`
		);
	}
	//выбор карты
	document.querySelectorAll('.modal-buy__variant').forEach((x) =>
		x.addEventListener('click', function () {
			// откуда этот селектед
			if (this.classList.contains('selected')) {
				this.classList.remove('selected');
			} else {
				document.querySelectorAll('.modal-buy__variant').forEach((x) => x.classList.remove('selected'));
				this.classList.add('selected');
			}
		})
	);

	document.querySelector('[data-name].active').classList.remove('active');
	document.querySelector('[data-name="buy"]').classList.add('active');
	this.style.display = 'none';
	document.getElementById('buy').style.display = 'block';
});

//покупка билета
document.getElementById('buyBtn').addEventListener('click', function (e) {
	const cardHolder = document.getElementById('CardHolder').value;
	const cardNum = document.getElementById('CardNum').value;
	const cardYear = document.getElementById('CardYear').value;
	const cardMonth = document.getElementById('CardMonth').value;
	const cardCvv = document.getElementById('CardCvv').value;
	const errorsCard = document.getElementById('errorsCard');
	const buyBtn = document.getElementById('buyBtn');
	let input = { Email: document.getElementById('email').value, TicketIds: [] };

	//нет же такого класса???
	document.querySelectorAll('.modal-buy__seat.selected').forEach((x) => {
		input.TicketIds.push(x.dataset.id);
	});
	if (cardCvv != '' && cardHolder != '' && cardNum != '' && cardYear != 'year' && cardMonth != 'month') {
		if (!isNaN(cardNum) && cardNum.length == 16) {
			if (cardCvv.length == 3) {
				errorsCard.textContent = '';
				axios
					.post(url + '/api/Ticket/Buy', input)
					.then(function (response) {
						input.TicketIds.forEach((id) => {
							//как выбирается билет? что за data-id
							document.querySelector(`.modal-buy__seats [data-id="${id}"]`).classList.add('saled');
							document.querySelector(`.modal-buy__seats [data-id="${id}"]`).classList.remove('selected');
							document.querySelector(`.modal-buy__seats [data-id="${id}"]`).textContent = '';
							console.log('Buy ticket:' + id);
							buyBtn.setAttribute('data-bs-dismiss', 'modal');
							buyBtn.click();
						});
					})
					.catch(function (error) {
						console.log(error);
					})
					.then(function () {});
			} else {
				errorsCard.textContent = 'Неверный код';
			}
		} else {
			errorsCard.textContent = 'Неверный номер карты';
		}
	} else {
		errorsCard.textContent = 'Не все поля заполнены';
	}
});

//09.03.2024-09.03.2025
document.getElementById('CreateBtn').addEventListener('click', function (e) {
	const name = document.getElementById('TitleCinema').value;
	const duration = document.getElementById('CreateDuration').value;
	const ageLimit = document.getElementById('CreateAgeLimit').value;
	const description = document.getElementById('CreateDescription').value;
	const producer = document.getElementById('CreateProducer').value;
	const poster = document.getElementById('formFileSm').files[0];
	const format = document.getElementById('CreateFormat').value;
	const isPushkinCard = document.getElementById('IsPushkinCard').checked;

	let date = ParseDateToDB(document.getElementById('inputCreateDate'));
	// const errors = document.getElementById('errorsCreateFilm');
	if (isPushkinCard.checked) {
		isPushkinCard.value = 'true';
	}
	const data = {
		Name: name,
		Duration: duration,
		AgeLimit: ageLimit,
		StartDate: date.startDate,
		EndDate: date.endDate,
		Format: format,
		IsPushkin: isPushkinCard,
		Poster: poster,
		Producer: producer,
		Description: description,
	};
	console.log(data);
	axios
		.post(url + '/api/Cinema/Create', data, {
			headers: {
				'Content-Type': 'multipart/form-data',
			},
		})
		.then(function (response) {
			console.log(response.data);
		})
		.catch(function (error) {
			console.log(error);
		});
});

//меняются модальные окошки при выборе и покупке билетов
document.getElementById('prev').addEventListener('click', function () {
	//что рассматривается? что за data-name
	let activeSection = document.querySelector('[data-name].active').dataset.name;
	if (activeSection == 'buy') {
		document.querySelector('.model-buy__pay').style.display = 'none';
		document.querySelector('.model-buy__selected-seats').style.display = 'block';

		document.querySelector('[data-name].active').classList.remove('active');
		document.querySelector('[data-name="seats"]').classList.add('active');
		document.getElementById('next').style.display = 'block';
		document.getElementById('buy').style.display = 'none';
	}
	if (activeSection == 'seats') {
		document.querySelector('.modal-buy__info').style.display = 'flex';
		document.querySelector('.model-buy__selected-seats').style.display = 'none';

		document.querySelector('[data-name].active').classList.remove('active');
		document.querySelector('[data-name="seance"]').classList.add('active');
		document.getElementById('modalByuView').style.display = 'none';
	}
});

//вход
document.getElementById('loginBtn').addEventListener('click', function (e) {
	const login = document.getElementById('login').value;
	const password = document.getElementById('password').value;
	const errors = document.getElementById('errorsLogIn');

	if (login != '' && password != '') {
		errors.textContent = '';
		axios
			.post(url + '/LogIn?username=' + login + '&password=' + password)
			.then(function (response) {
				console.log(response.data);
				const token = response.data.access_token;
				const email = response.data.username;
				const role = response.data.role;
				Cookies.set('role', role);
				Cookies.set('token', token);
				Cookies.set('email', email);
				console.log(login);
				location.href = 'index.html';
			})
			.catch(function (error) {
				errors.textContent = error.response.data.errorText;
			});
	} else {
		errors.textContent = 'Не все поля заполнены';
	}
});

//регистрация
document.getElementById('registerBtn').addEventListener('click', function (e) {
	const email = document.getElementById('emailReg').value;
	const password = document.getElementById('passwordReg').value;
	const confirmPassword = document.getElementById('confirmPassword').value;
	const errors = document.getElementById('errorsReg');
	if (email != '' && password != '' && confirmPassword != '') {
		errors.textContent = '';
		axios
			.post(url + '/Register?email=' + email + '&password=' + password + '&confirmPassword=' + confirmPassword)
			.then(function (response) {
				console.log(response.data);

				const token = response.data.token;
				const email = response.data.email;
				Cookies.set('token', token);
				Cookies.set('email', email);
				location.href = 'index.html';
			})
			.catch(function (error) {
				errors.textContent = error.response.data.errorText;
			});
	} else {
		errors.textContent = 'Не все поля заполнены';
	}
});

//для личного кабинета вход
document.querySelectorAll('.accountUser').forEach((x) => {
	x.addEventListener('click', function (e) {
		if (Cookies.get('token') != null) {
			location.href = 'Account.html';
		}
	});
});

let createDate = new AirDatepicker('#inputCreateDate', {
	range: true,
	multipleDatesSeparator: ' - ',
	isMobile: true,
	autoClose: true,
});

document.getElementById('formFileSm').addEventListener('change', function (e) {
	if (e.target.files[0]) {
		document.getElementById('createImg').src = window.URL.createObjectURL(e.target.files[0]);
	}
});

const initAdmin = () => {
	const role = Cookies.get('role');
	if (role == 'Admin') {
		console.log('d');
		document.getElementById('CreateCinemaBtn').classList.remove('d-none');
		// document.getElementById('toEdit').classList.remove('d-none');
		// document.getElementById('toDelete').classList.remove('d-none');
	}
};

initAdmin();
