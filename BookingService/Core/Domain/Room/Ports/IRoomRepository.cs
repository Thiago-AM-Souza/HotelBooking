namespace Domain.Room.Ports
{
    public interface IRoomRepository
    {
        Task<Entities.Room> Get(int Id);
        Task<Entities.Room> GetAggregate(int id);
        Task<int> Create(Entities.Room room);
        Task<int> Update(Entities.Room room);
    }
}
