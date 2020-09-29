namespace Api.Dtos
{
    public class UserEditPasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}