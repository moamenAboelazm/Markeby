namespace Library.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IBoatRepository Boats { get; }
        ITripRepository Trips { get; }
        IBookingRepository Bookings { get; }
        ICaptainRepository Captains { get; }
        IDashboardRepository Dashboard { get; }
        Task<int> CompleteAsync();
    }
}
