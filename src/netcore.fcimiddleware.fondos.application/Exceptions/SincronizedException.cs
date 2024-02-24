namespace netcore.fcimiddleware.fondos.application.Exceptions
{
    public class SincronizedException : ApplicationException
    {
        public SincronizedException(string name, object key) : base($"Entity \"{name}\" ({key}) no puede ser modificada porque se encuentra sincronizada.")
        {
        }
    }
}
