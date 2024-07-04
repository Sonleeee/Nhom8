using Microsoft.AspNetCore.Http;
using System.Numerics;
using System.Text;

namespace Nhom8.Helpers
{
    public class MyUtil
    {
        public static string UploadHinh(IFormFile hinh, string folder)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", folder, hinh.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    hinh.CopyTo(myfile);
                }
                return hinh.FileName;
            }
            catch(Exception ex) 
            {
                return string.Empty;
            }
        }

        public static string GenerateRandomKey(int length = 5)
        {
            var pattern = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM!@#$%^&*";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }

        public static int RandomOTP()
        {
            var rd = new Random();
            int otp = rd.Next(1000, 10000);
            return otp;
        }

        

    }
}
