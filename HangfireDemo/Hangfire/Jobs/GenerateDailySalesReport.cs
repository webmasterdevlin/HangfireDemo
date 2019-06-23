using System;
using HangfireDemo.Data;

namespace HangfireDemo.Jobs
{
    public interface IGenerateDailySalesReport
    {
        void ForAllCustomers();
    }

    public class GenerateDailySalesReport : IGenerateDailySalesReport
    {
        private HangfireDemoContext _context;

        public GenerateDailySalesReport(HangfireDemoContext context)
        {
            _context = context;
        }

        public void ForAllCustomers()
        {
            Console.WriteLine("Business logic and data persistence here");
            Console.WriteLine("a Recurring job!");
        }
    }
}
