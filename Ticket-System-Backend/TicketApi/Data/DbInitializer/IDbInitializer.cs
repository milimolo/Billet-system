namespace TicketApi.Data.DbInitializer
{
    public interface IDbInitializer
    {
        void Initialize(TicketApiContext context);
    }
}
