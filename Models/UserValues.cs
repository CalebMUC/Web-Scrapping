using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Scrapping.Models
{
    public class UserValues
    {
        public static List<UserInfo> users = new List<UserInfo>()
        {
            new UserInfo()
            {
                Username="Caleb Muchiri",
                Password="caleb",
                EmailAddress="muchiricaleb05@gmail.com",
                Role="adminstrator"
            },
           new UserInfo()
            {
                Username="mark",
                Password="mark",
                EmailAddress="m@gmail.com",
                Role="user"
            },
        };
    }
}
