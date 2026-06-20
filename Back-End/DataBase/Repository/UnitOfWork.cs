using DataBase.Contexts;
using Library.IRepository;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<Boat> Boats { get; private set; }
        public IGenericRepository<Trip> Trips { get; private set; }
        public IGenericRepository<Booking> Bookings { get; private set; }
        public IGenericRepository<Captain> Captains { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Boats = new GenericRepository<Boat>(_context);
            Trips = new GenericRepository<Trip>(_context);
            Bookings = new GenericRepository<Booking>(_context);
            Captains = new GenericRepository<Captain>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
