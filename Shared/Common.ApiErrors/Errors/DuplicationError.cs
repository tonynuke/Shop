namespace Common.ApiErrors.Errors
{
    public class DuplicationError : Error
    {
        public DuplicationError()
            : base(ErrorCodes.Duplication)
        {
        }

        public DuplicationError(string detail)
            : base(ErrorCodes.Duplication, detail)
        {
        }
    }
}