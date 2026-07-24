using AutoMapper;
using DataBase.Contexts;
using Library.IRepository;
using Library.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace DataBase.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public IBoatRepository Boats { get; private set; }
        public ITripRepository Trips { get; private set; }
        public IBookingRepository Bookings { get; private set; }
        public ICaptainRepository Captains { get; private set; }
        public IDashboardRepository Dashboard { get; private set; }

        public UnitOfWork(AppDbContext context, IMemoryCache cache, IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;

            Boats = new BoatRepository(_context, _cache);
            Trips = new TripRepository(_context, _cache);
            Bookings = new BookingRepository(_context, _cache);
            Captains = new CaptainRepository(_context, _cache);
            Dashboard = new DashboardRepository(_context, _mapper);
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