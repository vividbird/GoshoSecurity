using GoshoSecurityAPI.Models;
using System;

namespace GoshoSecurity
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            RestService restService = new RestService();

            restService.LoginAsync(new UserLoginModel
            {
                Username = "test",
                Password = "*Helloworld*2001"

            }).Wait();


        }
    }
}
