﻿using Xamarin.Forms;

namespace GoshoSecurity
{
	public static class Constants
	{
        // The iOS simulator can connect to localhost. However, Android emulators must use the 10.0.2.2 special alias to your host loopback interface.
        public static string BaseAddress = Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:5001" : "https://localhost:5001";
        public static string UsersUrl = BaseAddress + "/api/users/{0}";
        public static string PhotosUrl = BaseAddress + "/api/photos/{0}";
    }
}
 