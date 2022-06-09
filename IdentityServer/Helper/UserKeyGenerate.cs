using System;
using System.Linq;

namespace Identity.Helper
{
    public class UserKeyGenerate
    {
        public static string Create()
        {
            return $"{DateTime.UtcNow.ToString("yyyyMMddhhMMssfff")}-{Guid.NewGuid().ToString()[..5]}";
        }
    }
}
