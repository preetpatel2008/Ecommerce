using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Repository.Services.Infrastructure
{
    public class EncryptedQueryString : Dictionary<string, string>
    {
        protected byte[] _keyBytes = new byte[8] { 17, 18, 19, 20, 21, 22, 23, 24 };

        protected string _keyString = "ABC12345";

        protected string _checksumKey = "__$$";

        public EncryptedQueryString()
        {
        }

        public EncryptedQueryString(string encryptedData)
        {
            string text = Decrypt(encryptedData);
            string text2 = null;
            string[] array = text.Split(new char[1] { '&' });
            string[] array2 = array;
            foreach (string text3 in array2)
            {
                int num = text3.IndexOf('=');
                if (num != -1)
                {
                    string text4 = text3.Substring(0, num);
                    string text5 = text3.Substring(num + 1);
                    if (text4 == _checksumKey)
                    {
                        text2 = text5;
                    }
                    else
                    {
                        Add(HttpUtility.UrlDecode(text4), HttpUtility.UrlDecode(text5));
                    }
                }
            }

            if (text2 == null || text2 != ComputeChecksum())
            {
                Clear();
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string key in base.Keys)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append('&');
                }

                stringBuilder.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(base[key]));
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append('&');
            }

            stringBuilder.AppendFormat("{0}={1}", _checksumKey, ComputeChecksum());
            return Encrypt(stringBuilder.ToString());
        }

        protected string ComputeChecksum()
        {
            int num = 0;
            using (Enumerator enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, string> current = enumerator.Current;
                    for (int i = 0; i < current.Key.Length; i++)
                    {
                        num += current.Key[i] - 48;
                    }

                    for (int i = 0; i < current.Value.Length; i++)
                    {
                        num += current.Value[i] - 48;
                    }
                }
            }

            return num.ToString("X");
        }

        protected string Encrypt(string text)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(_keyString.Substring(0, 8));
                DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                byte[] bytes2 = Encoding.UTF8.GetBytes(text);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, _keyBytes), CryptoStreamMode.Write);
                cryptoStream.Write(bytes2, 0, bytes2.Length);
                cryptoStream.FlushFinalBlock();
                return GetString(memoryStream.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        protected string Decrypt(string text)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(_keyString.Substring(0, 8));
                DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                byte[] bytes2 = GetBytes(text);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(bytes, _keyBytes), CryptoStreamMode.Write);
                cryptoStream.Write(bytes2, 0, bytes2.Length);
                cryptoStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        protected string GetString(byte[] data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in data)
            {
                stringBuilder.Append(b.ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        protected byte[] GetBytes(string data)
        {
            byte[] array = new byte[data.Length / 2];
            for (int i = 0; i < data.Length; i += 2)
            {
                array[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
            }

            return array;
        }
    }
}
