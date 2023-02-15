namespace wedcoo_api.Authentication
{
    public interface IJwtAuthentication
    {
        string AdminAuthentication(string Email, string Role);
        string UserAuthentication(string Email);
    }
}
