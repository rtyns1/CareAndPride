namespace CarePrideSystem.Application.Exceptions
{
    public class AccountNotApprovedException : Exception
    {
        public AccountNotApprovedException() : base("Your account is pending approval by the administrator.") { }
    }
}
