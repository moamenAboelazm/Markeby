using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Boat> Boats { get; }
        IGenericRepository<Trip> Trips { get; }
        IGenericRepository<Booking> Bookings { get; }
        IGenericRepository<Captain> Captains { get; }

        Task<int> CompleteAsync();
    }
}
