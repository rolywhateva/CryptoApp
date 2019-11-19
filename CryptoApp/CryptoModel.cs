using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace CryptoApp
{
    class CryptoModel
    {
        private SymmetricAlgorithm algorithm;
        private CipherMode mode;

        private string clearText;
        private byte[] cryptedText;
        public string ClearText
        {
            set
            {
                clearText = value;
            }
            get
            {
                return clearText;
            }
        }
        public  byte[]  CryptedText
        {
            set
            {
                cryptedText = value;
            }
            get
            {
                return cryptedText;
            }
        }
        public CryptoModel(SymmetricAlgorithm algorithm,CipherMode mode)
        {
         
            if (algorithm.Key == null || algorithm.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (algorithm.IV == null || algorithm.IV.Length <= 0)
                throw new ArgumentNullException("IV");
            this.algorithm = algorithm;
            try
            {
                algorithm.Mode = mode;
            }catch(Exception e)
            {
                throw new InCompatibleFormatException();
            }
           

        }
        public byte[] Encryption()
        {
            if (clearText == null || clearText.Length <= 0)
                throw new ArgumentNullException("plainText");

            byte[] encrypted;
         
            ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {

                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(clearText);
                    }
                   encrypted = msEncrypt.ToArray();
                }
            }
            cryptedText = encrypted;
            return encrypted;
        }
        public string Decryption()
        {
            string plaintext = null;
            if (cryptedText == null || cryptedText.Length <= 0)
                throw new Exception("Crypted text");

            try
            {
                ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cryptedText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }catch(Exception e)
            {
                return e.Message;
            }
            this.clearText = plaintext;
            return plaintext;
        }
      
    }
}
