namespace netcore.fcimiddleware.fondos.application.Exceptions
{
    public class AlreadyExistsException : ApplicationException
    {
        public AlreadyExistsException(string name, object key) : base($"Entidad \"{name}\", ya contiene el valor ({key})")
        {
        }
    }
}
