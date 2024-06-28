// original file: https://gist.github.com/JonHaywood/996aa010e9a3858339c3
namespace CONTROLLER;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
public static class EncryptionServices
    {
        private const int BufferSizeInBytes = 16 * 1024;

        // you can have a number of other lengths but these will do
        private const int RequiredKeyLengthInBytes = 32;
        private const int RequiredInitialisationVectorLengthInBytes = 16;

        public static void EncryptFile(string inputFile, string outputFile, string keyy, string iv)
        {
            string key = keyy;
            string initialisationVector = iv;

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                byte[] keyBytes = ASCIIEncoding.UTF8.GetBytes(key);
                byte[] initialisationVectorBytes = ASCIIEncoding.UTF8.GetBytes(initialisationVector);

                key = ValidateKey(keyBytes, key);   
                initialisationVector = ValidateInitialisation(initialisationVectorBytes, initialisationVector);

                keyBytes = ASCIIEncoding.UTF8.GetBytes(key);
                initialisationVectorBytes = ASCIIEncoding.UTF8.GetBytes(initialisationVector);

                using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create))
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor(keyBytes, initialisationVectorBytes))
                    {
                        using (CryptoStream outputCryptoStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
                            {
                                byte[] buffer = new byte[BufferSizeInBytes];
                                int count;
                                while ((count = inputFileStream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    outputCryptoStream.Write(buffer, 0, count);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void DecryptFile(string inputFile, string outputFile, string keyy, string iv)
        {
            string key = keyy;
            string initialisationVector = iv;

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                byte[] keyBytes = ASCIIEncoding.UTF8.GetBytes(key);
                byte[] initialisationVectorBytes = ASCIIEncoding.UTF8.GetBytes(initialisationVector);

                key = ValidateKey(keyBytes, key);
                initialisationVector = ValidateInitialisation(initialisationVectorBytes, initialisationVector);

                keyBytes = ASCIIEncoding.UTF8.GetBytes(key);
                initialisationVectorBytes = ASCIIEncoding.UTF8.GetBytes(initialisationVector);

                using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
                {
                    using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform decryptor = aes.CreateDecryptor(keyBytes, initialisationVectorBytes))
                        {
                            using (CryptoStream inputCryptoStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] buffer = new byte[BufferSizeInBytes];
                                int count;
                                while ((count = inputCryptoStream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    outputFileStream.Write(buffer, 0, count);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string ValidateKey(byte[] key, string keyS)
        {
            if (key.Length == RequiredKeyLengthInBytes)
            {
                return keyS;
            }
            return tamamla(keyS,32);
            //throw new ArgumentException(string.Format("Please use a key length of {0} bytes ({1} bits)", RequiredKeyLengthInBytes, RequiredKeyLengthInBytes * 8));
        }

        private static string ValidateInitialisation(byte[] initialisationVector, string ivS)
        {
            if (initialisationVector.Length == RequiredInitialisationVectorLengthInBytes)
            {
                return ivS;
            }
            return tamamla(ivS,16);
            //throw new ArgumentException(string.Format("Please use an initialisation vector length of {0} bytes ({1} bits)", RequiredInitialisationVectorLengthInBytes, RequiredInitialisationVectorLengthInBytes * 8));
        }
    
        private static string tamamla(string girdi, int kacaKadar)
        {
            int hedef = kacaKadar - girdi.Length;
            int sinir = girdi.Length;
            string sonuc = girdi;
            int sayac = 0;
            int ekle = 0;

            do
            {
                sayac++;
                if (ekle > sinir -1){ekle = 0;} //= (sayac>sinir - 1 ? 0 : sayac);
                sonuc = sonuc + (girdi[ekle]);
                ekle++;
            }
            while(sayac<hedef);
            return sonuc;
        }


    }