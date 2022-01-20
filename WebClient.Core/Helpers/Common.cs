using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebClient.Core.Entities;

namespace WebClient.Core.Helpers
{
    public class Common
    {
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Generate reset password
        /// </summary>
        /// <returns>password reser</returns>
        public static string GenerateResetPassword()
        {
            const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
            const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMBERS = "123456789";
            const string SPECIALS = @"!@£$%^&*()#€";
            int length = Constants.ResetPassword.LengthPassword;

            char[] _password = new char[length];
            string charSet = ""; // Initialise to blank
            System.Random _random = new Random();
            int counter;

            // Build up the character set to choose from
            charSet += LOWER_CASE;

            charSet += UPPER_CAES;

            charSet += NUMBERS;

            charSet += SPECIALS;

            for (counter = 0; counter < length; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="toMail">to mail</param>
        /// <param name="subject">subject mail</param>
        /// <param name="body">content mail</param>
        /// <param name="hostMail">host mail</param>
        /// <param name="portMail">port mail</param>
        /// <param name="username">from mail</param>
        /// <param name="password">password of username</param>
        public static void SendMail(string toMail, string subject, string body, string hostMail, int portMail, string username, string password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(username);

                mail.To.Add(toMail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient smtpServer = new SmtpClient
                {
                    Host = hostMail,
                    Port = portMail,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(username, password)
                };

                smtpServer.SendMailAsync(mail).Wait();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string HtmlToPlainText(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }

            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        public static int SaveFile(IFormFile file, string folderPath)
        {
            var fileName = string.Format(".{0}", folderPath);
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            return 1;
        }

        public static string HandlerMainKML(IFormFile fileKML, string fileKMLName)
        {
            if (fileKML == null)
            {
                return string.Empty;
            }
            var fileName = ContentDispositionHeaderValue.Parse(fileKML.ContentDisposition).FileName.Trim('"');
            var extensionKML = Path.GetExtension(fileName);
            if (!Constants.DinhDangFileKML.Equals(extensionKML.ToLower()))
            {
                throw new Exception(string.Format(
                    "Định dạng file kml không hợp lệ. Hệ thống chỉ hỗ trợ định dạng: {0}",
                    string.Join(",", Constants.DinhDangFileKML)));
            }
            var folderPath = string.Format("{0}{1}.kml.json", Constants.DuongDanThuMuc.KML, fileKMLName);
            var saveFolder = "/wwwroot" + folderPath;
            SaveFile(fileKML, saveFolder);
            return folderPath;
        }

        public static string HandlerKML(IFormFile fileKML)
        {
            if (fileKML == null)
            {
                return string.Empty;
            }
            var fileName = ContentDispositionHeaderValue.Parse(fileKML.ContentDisposition).FileName.Trim('"');
            var extensionKML = Path.GetExtension(fileName);
            if (!Constants.DinhDangFileKML.Equals(extensionKML.ToLower()))
            {
                throw new Exception(string.Format(
                    "Định dạng file kml không hợp lệ. Hệ thống chỉ hỗ trợ định dạng: {0}",
                    string.Join(",", Constants.DinhDangFileKML)));
            }
            var folderPath = string.Format("{0}{1}.kml.json",Constants.DuongDanThuMuc.KML, Guid.NewGuid().ToString());
            var saveFolder = "/wwwroot" + folderPath;
            SaveFile(fileKML, saveFolder);
            return folderPath;
        }

        public static bool CheckForStrongPassword(String password)
        {
            var regex = new Regex(Constants.StrongPasswordRegex);
            return regex.IsMatch(password);
        }
    }
}
