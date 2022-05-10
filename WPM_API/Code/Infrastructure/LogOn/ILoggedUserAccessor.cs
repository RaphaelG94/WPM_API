namespace WPM_API.Code.Infrastructure.LogOn
{
    public interface ILoggedUserAccessor
    {
        LoggedClaims Claims { get; }
        LoggedUserDbInfo DbInfo { get; }
        string Id { get; }
        string IdOrNull { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        bool IsInRole(string role);
    }
}
