using Microsoft.AspNetCore.Identity;

namespace be.Models
{
    public class ApplicationUser:IdentityUser
    {
        public String ProfileID { get; set; } = "";
        public String UserType { get; set; } = "";

        public void SetProfileID() {
            this.ProfileID = System.Guid.NewGuid().ToString();
        }
        //public ApplicationUser(String UserName, String Password,
        //    String Email, String UserType) {
        //    this.UserName = UserName;
        //    this.UserType = UserType;
        //    this.Email= Email;
        //    this.PasswordHash = Password;
        //    //this.PasswordHash = Password;//GetHashCode(Password, ).ToString();
        //    //SetProfileID();
        //}

    }
}
