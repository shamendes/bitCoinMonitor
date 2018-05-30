using bitCoinMonitor.model;
using System;
using bitCoinMonitor.tools;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitCoinMonitor.control
{
    class clsCtrSenha
    {
        private const string cStrSegredo = "shamendes84";
        private clsModParametros _Parametros;
        
        public clsCtrSenha()
        {
            try
            {
                this._Parametros = new clsModParametros();
            }
            catch { throw; }
        }


        public bool validarSenha(string aStrSenhaLimpa)
        {
            bool vBooRetorno = false;
            string vStrSenhaCripto = String.Empty;

            try
            {
                vStrSenhaCripto = clsTooCriptografia.criptografarHMACSHA256(aStrSenhaLimpa, cStrSegredo);
                vBooRetorno = (vStrSenhaCripto == this._Parametros.buscarSenha());
            }
            catch { throw; }

            return vBooRetorno;
        }

        public string atualizarSenha(string aStrSenhaAtualLimpa, string aStrNovaSenhaLimpa, string aStrConfirmaNovaSenhaLimpa)
        {
            string vStrRetorno = String.Empty;
            string vStrNovaSenhaCripto = String.Empty;

            try
            {
                if (this.validarSenha(aStrSenhaAtualLimpa) || Program.Parametros.pPossuiSenha == false)
                {
                    if(aStrNovaSenhaLimpa == aStrConfirmaNovaSenhaLimpa)
                    {
                        vStrNovaSenhaCripto = clsTooCriptografia.criptografarHMACSHA256(aStrNovaSenhaLimpa, cStrSegredo);
                        this._Parametros.atualizarSenha(vStrNovaSenhaCripto);
                    }
                    else
                        vStrRetorno = "Nova senha e Confirmação estão diferentes!";
                }
                else
                    vStrRetorno = "Senha atual incorreta!";
            }
            catch { throw; }

            return vStrRetorno;
        }




    }
}
