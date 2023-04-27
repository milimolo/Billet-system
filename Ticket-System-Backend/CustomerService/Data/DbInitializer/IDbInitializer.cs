namespace CustomerApi.Data.DbInitializer
{
    public interface IDbInitializer
    {
        void Initialize(CustomerApiContext context);
    }
}
