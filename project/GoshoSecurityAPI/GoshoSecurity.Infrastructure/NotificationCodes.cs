namespace GoshoSecurity.Infrastructure
{
    public static class NotificationCodes
    {
        public const string RequiredInput = "Some data is missing. Please try again";
        public const string UserExists = "User with specified information exists";
        public const string UserIncorrectData = "Username or password is incorrect";
        public const string NoUserExists = "User doesn't exist";
        public const string UserDataSuccess = "User data successfully changed";
        public const string Error = "An error occured. Please try again";
        public const string ImageNullError = "Please choose photo to upload";
        public const string ImageSuccess = "Image has been successfully uploaded";
        public const string UserDeleted = "User successfully deleted";

        public const string PasswordSuccessfullyChanged = "Password has been successfully changed";

    }
}
