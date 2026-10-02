namespace CarePrideSystem.Application.Exceptions
{
    public class AccountDisabledException : Exception
    {
        public AccountDisabledException() : base("Your account has been disabled. Contact the administrator.") { }
    }
}
