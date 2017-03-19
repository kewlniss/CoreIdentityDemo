namespace CoreIdentityDemo.Common.IdentityApi
{
    public class LoginModel
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
    }
}
