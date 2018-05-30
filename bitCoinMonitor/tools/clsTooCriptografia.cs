using System.Text;
using System.Security.Cryptography;

namespace bitCoinMonitor.tools
{
    class clsTooCriptografia
    {
        public static string criptografarHMACSHA256(string aStrMensagem, string aStrSegredo)
        {
            byte[] vBtyChave = Encoding.UTF8.GetBytes(aStrSegredo);
            byte[] vBtyMsg = Encoding.UTF8.GetBytes(aStrMensagem);

            using (HMACSHA256 vObjMAC256 = new HMACSHA256(vBtyChave))
            {
                var vVarHashedInputBytes = vObjMAC256.ComputeHash(vBtyMsg);

                // Convert to text
                // StringBuilder Capacity is 64, because 256 bits / 8 bits in byte * 2 symbols for byte 
                var vVarStringBuilder = new StringBuilder(64);
                foreach (var b in vVarHashedInputBytes)
                    vVarStringBuilder.Append(b.ToString("X2"));

                return vVarStringBuilder.ToString().ToLower();
            }
        }       
        public static string criptografarHMACSHA512(string aStrMensagem, string aStrSegredo)
        {
            byte[] vBtyChave = Encoding.UTF8.GetBytes(aStrSegredo);
            byte[] vBtyMsg = Encoding.UTF8.GetBytes(aStrMensagem);

            using (HMACSHA512 vObjMAC512 = new HMACSHA512(vBtyChave))
            {
                var vVarHashedInputBytes = vObjMAC512.ComputeHash(vBtyMsg);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var vVarStringBuilder = new StringBuilder(128);
                foreach (var b in vVarHashedInputBytes)
                    vVarStringBuilder.Append(b.ToString("X2"));

                return vVarStringBuilder.ToString().ToLower();
            }
        }
        

    }
}
