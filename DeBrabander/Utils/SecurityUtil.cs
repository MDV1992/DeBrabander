using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;

namespace DeBrabander.Utils
{
    public class SecurityUtil
    {
        public static string hashPassword(string password)
        {
            var hashed = Crypto.Hash(password, "sha256");
            return password;
        }
    }
}