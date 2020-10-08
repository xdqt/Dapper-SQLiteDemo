using SQLiteDemo.Model;

namespace SQLiteDemo.Data
{
    public interface ICustomerRepository<T>
    {
        T Get<T>(long id, string sql);
        void Save(T customer,string sql);
    }
}