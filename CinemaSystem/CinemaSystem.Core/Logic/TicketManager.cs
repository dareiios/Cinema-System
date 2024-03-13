using CinemaSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Core.Logic
{
    public class TicketManager
    {
        private readonly CinemaSystemContext _context;
        private readonly EMailSender _emailSender;
        private static readonly object _sync = new object();

        public TicketManager(CinemaSystemContext context, EMailSender eMailSender)
        {
            _context = context;
            _emailSender = eMailSender;
        }

        public async Task BuyTickets(int[] ticketIds, string email, int? userId)
        {
            
            if(ticketIds == null || ticketIds.Length == 0)
            {
                throw new TicketException("Empty order");
            }

            List<Ticket> tickets;
            List<Ticket> buyedTickets = new List<Ticket>();
            Seance seance;
            lock (_sync)
            {
                tickets = _context.Tickets.Where(x => ticketIds.Contains(x.Id)).ToList();

                if (tickets.Select(x => x.SeanceId).Distinct().Count() > 1)
                {
                    throw new TicketException("Can not order tickets from many seanses");
                }

                seance = _context.Seances.Include(x => x.Cinema).First(x => x.Id == tickets.First().SeanceId);

                foreach (var ticketId in tickets.Select(x => x.Id))
                {
                    var ticket = _context.Tickets.Find(ticketId);
                    if(ticket.Status != TicketStatus.Sold)
                    {
                        buyedTickets.Add(ticket);
                        ticket.Status = TicketStatus.Sold;
                        ticket.UserId = userId;
                        _context.Tickets.Update(ticket);
                    }
                }
            }

            MailMessage message = null;

            try
            {
                await _context.SaveChangesAsync();
                message = BuildEmail(email, buyedTickets, seance);
            }
            catch (Exception)
            {
                message = BuildErrorEmail(email, tickets, seance);
            }
            finally
            {
                await _emailSender.SendMessage(message);
            }

        }

        private MailMessage BuildEmail(string email, List<Ticket> tickets, Seance seance)
        {
            var from = new MailAddress("dareiiiios@yandex.ru", "Kinomax");
            var to = new MailAddress(email);
            var message = new MailMessage(from, to);
            message.Subject = $"Покупка билетов на фильм {seance.Cinema.Name}";
            message.Body = $"Вы купили {tickets.Count} билетов на сеанс {seance.Cinema.Name} : \n";
            message.Body += $"Время сеанса: {seance.Date}\n";
            message.Body += $"Длительность рекламного блока: {seance.PromoDuration}\n";

            var i = 1;
            foreach (var ticket in tickets)
            {
                var seat = _context.Seats.First(x => x.Id == ticket.SeatId);
                message.Body += $"Билет {i++} : ряд {seat.RowNumber} место {seat.Number} цена: {ticket.Price}\n";
            }
            return message;
        }

        private MailMessage BuildErrorEmail(string email, List<Ticket> tickets, Seance seance)
        {
            var from = new MailAddress("dareiiiios@yandex.ru", "Kinomax");
            var to = new MailAddress(email);
            var message = new MailMessage(from, to);
            message.Subject = $"Ошиbка при покупке билетов на фильм {seance.Cinema.Name}";
            message.Body = $"Не удалось купить {tickets.Count} билетов на сеанс {seance.Cinema.Name} : \n";
            message.Body += $"Время сеанса: {seance.Date}\n";
            message.Body += $"Длительность рекламного блока: {seance.PromoDuration}\n";

            return message;
        }
    }
}
