namespace gremlin_eye.Server.Enums
{
    public enum UserRole
    {
        User,
        Admin
    }

    public static class UserRoleExtensions
    {
        public static string ToStringValue(this UserRole role)
        {
            return role.ToString().ToLower();
        }
    }
}
