namespace OrderApi.Data.DbInitializer
{
    public interface IDbInitializer
    {
        void Initialize(OrderApiContext context);
    }
}
