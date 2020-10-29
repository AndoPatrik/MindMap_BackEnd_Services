using MindMap_General_Purpose_API.Models;

namespace MindMap_General_Purpose_API.Utils
{
    public class ValidatorUtil
    {
        public static bool ValidateUser(User user) 
        {
            if (!string.IsNullOrWhiteSpace(user.Email) && !string.IsNullOrWhiteSpace(user.Password)) return true;
            return false;
        }
    }
}
