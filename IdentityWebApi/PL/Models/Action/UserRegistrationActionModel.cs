namespace IdentityWebApi.PL.Models.Action
{
    public class UserRegistrationActionModel : UserSignInActionModel
    {
        public string UserName { get; set; }
        
        public string Role { get; set; }
    }
}