using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyLaundering
{
    public class MoneyLundary
    {
        private readonly IBatchService _batchService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public MoneyLundary(IBatchService batchService, IConfiguration Configuration, IEmailSender emailSender)
        {
            _batchService = batchService;
            _emailSender = emailSender;
            _configuration = Configuration;
        }
        public void Run()
        {

            var currentdate = DateTime.Now;
            var startdate = currentdate.AddDays(-20);
            var finsish = currentdate.AddDays(-1);
            var amountPerDay = _configuration.GetSection("AmountPerDay").Value;
            var amountPerThreeDays = _configuration.GetSection("AmountPerThreeDays").Value;

            var suspectpeople = _batchService.GetSuspectedPeople(Convert.ToInt32(amountPerDay), Convert.ToInt32(amountPerThreeDays), startdate, finsish);

            var suspectedPeopleByEachCountry = suspectpeople.GroupBy(x => x.Country).Distinct().ToList();

            _emailSender.SendEmail(suspectedPeopleByEachCountry);

            foreach (var suspectedpeople in suspectedPeopleByEachCountry)
            {
                Console.WriteLine(suspectedpeople.Key);
                foreach (var suspectedperson in suspectedpeople)
                {
                    Console.WriteLine(suspectedperson.CustomerName);
                    Console.WriteLine(suspectedperson.Accountnumber);
                    Console.WriteLine(suspectedperson.Country);
                    Console.WriteLine(".................");

                    foreach (var transaction in suspectedperson.Transactions)
                    {
                        Console.WriteLine(transaction.Amount);
                        Console.WriteLine(transaction.Date);
                    }
                    Console.WriteLine(".................");


                }
            }

        }
    }
}
