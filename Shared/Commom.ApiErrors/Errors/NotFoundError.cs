namespace Commom.ApiErrors.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError()
            : base(ErrorCodes.NotFound)
        {
        }

        public NotFoundError(string detail)
            : base(ErrorCodes.NotFound, detail)
        {
        }
    }
}