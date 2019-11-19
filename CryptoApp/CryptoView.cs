using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp
{
   public   class CryptoView
    {
        public  void GiveResults(string v,Action<string> toDo)
        {
            toDo(v);
        }
       
        
    }
}
