namespace CustomerService.Data.DbInitializer
{
    public interface IDbInitializer
    {
        void Initialize(CustomerServiceContext context);
    }
}
