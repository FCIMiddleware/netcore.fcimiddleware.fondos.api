namespace netcore.fcimiddleware.fondos.application.Exceptions
{
    public class DeletedException : ApplicationException
    {
        public DeletedException(string name, object key) : base($"Entity \"{name}\" ({key}) ya se encuentra anulado.")
        {
        }
    }
}
