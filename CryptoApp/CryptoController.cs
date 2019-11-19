using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp
{
    class CryptoController
    {
        private CryptoModel model;
        private CryptoView view;
     
        public CryptoController(CryptoModel model,CryptoView view)
        {
            this.model = model;
            this.view = view;
        }
        public  enum Operation {Encryption,Decryption };
        public string ClearText
        {
            set
            {
                if (value == null || value.Length <= 0)
                    throw new Exception("Clear text error");
                this.model.ClearText = value;
            }
            get
            {
                return this.model.ClearText;
            }
        }
        public byte[] CryptedText
        {
            set
            {
                if (value == null || value.Length <= 0)
                    throw new Exception("Clear text error");
                this.model.CryptedText = value;
            }
            get
            {
                return this.model.CryptedText;
            }
        }
        public byte[] GetEncryption()
        {
            return this.model.Encryption();
        }
        public string GetDecryption()
        {
            return this.model.Decryption();
        }
       public void UpdateView(Operation operation,Action<string> toDo)
        {
            if (operation == Operation.Encryption)
                this.view.GiveResults(string.Join("",this.GetEncryption()),toDo);
            else
                if (operation == Operation.Decryption)
                this.view.GiveResults(this.GetDecryption(),toDo);
        }

    }
}
