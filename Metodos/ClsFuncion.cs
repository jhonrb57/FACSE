using Firsoft.Security.Encryption;
using System;

namespace Metodos
{
    public class ClsFuncion
    {
        public string Encriptar(string str)
        {
            string a = "";
            try
            {
                Crypto.EncryptionAlgorithm = Crypto.Algorithm.TripleDES;
                Crypto.Encoding = Crypto.EncodingType.HEX;
                Crypto.Key = MdlVariables.gsKey;
                Crypto.EncryptString(str);
                a = Crypto.Content;
                Crypto.Clear();
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Descifrar(string str)
        {
            string a = "";
            try
            {
                Crypto.EncryptionAlgorithm = Crypto.Algorithm.TripleDES;
                Crypto.Encoding = Crypto.EncodingType.HEX;
                Crypto.Key = MdlVariables.gsKey;
                Crypto.Content = str;
                Crypto.DecryptString();
                a = Crypto.Content;
                Crypto.Clear();
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
