using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
    public class UtilClass
    {
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static string MakeRandomPhoneCode(int size)
        {
            Random rand = new Random();
            // Characters we will use to generate this random string.
            char[] allowableChars = "1234567890".ToCharArray();

            // Start generating the random string.
            string activationCode = string.Empty;
            for (int i = 0; i <= size - 1; i++)
            {
                if (i == 2)
                    activationCode += allowableChars[rand.Next(allowableChars.Length - 1)] + "-";
                else
                    activationCode += allowableChars[rand.Next(allowableChars.Length - 1)];
            }

            // Return the random string in upper case.
            return activationCode.ToUpper();
        }

        public static string EncryptString(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //TODO: Put Encryption Key Value in appsettings.json and call it from there
                                                                           // And Change it into something more secure.
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string DecryptString(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //TODO: Same As Above TODO
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string MakeRandomEmailCode(int size)
        {
            Random rand = new Random();
            // Characters we will use to generate this random string.
            char[] allowableChars = "X0qjn!Wopm$smBBasCii$856!p)".ToCharArray();

            // Start generating the random string.
            string activationCode = string.Empty;
            for (int i = 0; i <= size - 1; i++)
            {
                if (i == 3 || i == 7 || i == 11)
                    activationCode += allowableChars[rand.Next(allowableChars.Length - 1)] + "-";
                else
                    activationCode += allowableChars[rand.Next(allowableChars.Length - 1)];
            }

            // Return the random string in upper case.
            return activationCode.ToUpper();
        }

        public static string MakeRandomPassword(int size)
        {
            Random rand = new Random();
            // Characters we will use to generate this random string.
            char[] allowableChars = "X0qjn!Wopm$smBBasCii$856!p)".ToCharArray();

            // Start generating the random string.
            string pass = string.Empty;
            for (int i = 0; i <= size - 1; i++)
            {
                pass += allowableChars[rand.Next(allowableChars.Length - 1)];
            }

            // Return the random string in upper case.
            return pass.ToLower();
        }

        public static void SendEmail(string Email, string Subject, string MessageBody)
        {
            try
            {
                MailMessage message = new MailMessage("noreply.egety@gmail.com", Email, Subject, MessageBody);
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("noreply.egety@gmail.com", "ykumcogrfxvfbxjp"); // These are Real Credentials For Now
                client.Send(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // email sig generator
        // https://www.hubspot.com/email-signature-generator
        public static void SendZohoEmail(string Email, string Subject, string MessageBody)
        {
            try
            {
                MailMessage message = new MailMessage("info@wedcoo.com", Email, Subject, MessageBody);
                SmtpClient client = new SmtpClient("smtp.zoho.com", 587);
                client.EnableSsl = true;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("info@wedcoo.com", "KeEHq6816prR"); // These are Real Credentials For Now
                client.Send(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SendAdminVerificationEmail(string Email, string link)
        {
            try
            {
                string body = string.Empty;
                body = File.ReadAllText("./Utilities/EmailTemplates/AdminVerification.html").ToString();
                body = body.Replace("{VerificationLinkToUpdate}", link);
                MailMessage message = new MailMessage("info@wedcoo.com", Email, "Wedcoo Account Created !", body);
                SmtpClient client = new SmtpClient("smtp.zoho.com", 587);
                client.EnableSsl = true;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("info@wedcoo.com", "KeEHq6816prR"); // These are Real Credentials For Now
                client.Send(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string SendUserVerificationEmail(string Email, string link, string templatePath)
        {
            try
            {
                string body = string.Empty;
                body = File.ReadAllText(Path.Combine(templatePath + "UserVerification.html")).ToString();
                body = body.Replace("{VerificationLinkToUpdate}", link);
                body = body.Replace("{userEmail}", Email);
                MailMessage message = new MailMessage("info@wedcoo.com", Email, "Wedcoo Account Created !", body);
                SmtpClient client = new SmtpClient("smtp.zoho.com", 587);
                //SmtpClient client = new SmtpClient("smtppro.zoho.com", 465);
                client.EnableSsl = true;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("info@wedcoo.com", "KeEHq6816prR"); // These are Real Credentials For Now
                client.Send(message);
                Console.WriteLine("email success");
                return "email success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                //Console.WriteLine(ex.Message);
            }
        }


        public static string EncryptBackByAES(string input)
        {
            string EncryptionKey = "5v8y/B?E(H+MbQeTKimZq4t9q9z$C&F)"; // TODO: Put Key In appsettings.json and call it from there
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.FeedbackSize = 128;

                rijndaelManaged.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                rijndaelManaged.IV = Encoding.UTF8.GetBytes("eSkLmYq9P2w9z$l&"); // TODO: Put IV In appsettings.json and call it from there

                ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        byte[] bytes = msEncrypt.ToArray();
                        return Convert.ToBase64String(bytes);
                    }
                }
            }
        }

        public static string EncryptFrontByAES(string input)
        {
            string EncryptionKey = "5v8y/B?E(H+MbQeThWmZq4tPk9z$C&F)"; // TODO: Put Key In appsettings.json and call it from there
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.FeedbackSize = 128;

                rijndaelManaged.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                rijndaelManaged.IV = Encoding.UTF8.GetBytes("eSkLmYqKm!p9z$l&"); // TODO: Put IV In appsettings.json and call it from there

                ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        byte[] bytes = msEncrypt.ToArray();
                        return Convert.ToBase64String(bytes);
                    }
                }
            }
        }

        public static string DecryptBackByAES(string input)
        {
            string EncryptionKey = "5v8y/B?E(H+MbQeTKimZq4t9q9z$C&F)"; // TODO: Put Key In appsettings.json and call it from there
            var buffer = Convert.FromBase64String(input);
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.FeedbackSize = 128;

                rijndaelManaged.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                rijndaelManaged.IV = Encoding.UTF8.GetBytes("eSkLmYq9P2w9z$l&"); // TODO: Put IV In appsettings.json and call it from there

                ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream msEncrypt = new MemoryStream(buffer))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string DecryptFrontByAES(string input)
        {
            string EncryptionKey = "5v8y/B?E(H+MbQeThWmZq4tPk9z$C&F)"; // TODO: Put Key In appsettings.json and call it from there
            var buffer = Convert.FromBase64String(input);
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.FeedbackSize = 128;

                rijndaelManaged.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                rijndaelManaged.IV = Encoding.UTF8.GetBytes("eSkLmYqKm!p9z$l&"); // TODO: Put IV In appsettings.json and call it from there

                ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream msEncrypt = new MemoryStream(buffer))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string GenerateRandomUniqueString()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }

        public static string HashText(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }
        public static bool ValidateHash(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }

        public static string GenerateSignature(string timestamp, string method, string url, string body, string appSecret)
        {
            return GetHMACInHex(appSecret, timestamp + method + url + body);
        }
        public static string GetHMACInHex(string key, string data)
        {
            var hmacKey = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA256(hmacKey))
            {
                var sig = hmac.ComputeHash(dataBytes);
                return ByteToHexString(sig);
            }
        }
        static string ByteToHexString(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(87 + b + (((b - 10) >> 31) & -39));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(87 + b + (((b - 10) >> 31) & -39));
            }
            return new string(c);
        }

        public static string GenerateColorHex()
        {
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));
            return color;
        }

        /// Check If Image Validity
        public static string CheckIfImageValidity(IFormFile image)
        {
            try
            {
                try
                {
                    string imageInBase64 = "";
                    using (var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        imageInBase64 = Convert.ToBase64String(fileBytes);
                    }
                    byte[] bytes = Convert.FromBase64String(imageInBase64);

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        Image image1 = Image.FromStream(ms);
                    }
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "File Error";
                    //return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return "File Damaged";
                //return ex.Message;
            }

        }

        public static List<T> ShuffleList<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

    }
}
