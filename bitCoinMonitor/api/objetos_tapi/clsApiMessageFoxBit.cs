using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitCoinMonitor.api.objetos_tapi
{
    class clsApiMessageFoxBit
    {
        public clsApiMessageFoxBit(string msg, int id)
        {
            this.MsgType = msg;
            this.BalanceReqID = id;

        }


        public string MsgType { get; set; }
        public int BalanceReqID { get; set; }
    
    }



}
