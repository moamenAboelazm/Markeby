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
        IBoatRepository Boats { get; }
        ITripRepository Trips { get; }
        IBookingRepository Bookings { get; }
        ICaptainRepository Captains { get; }

        Task<int> CompleteAsync();
    }
}
