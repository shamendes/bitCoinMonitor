using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

namespace bitCoinMonitor.tools
{
    class clsTooUtil
    {

        public static DateTime converterUnixTimeStamp(decimal aDecUnixTimeStamp)
        {

            System.DateTime vData;

            try
            {
                vData = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                vData = vData.AddSeconds(Convert.ToDouble(aDecUnixTimeStamp)).ToLocalTime();
            }
            catch
            {
                throw;
            }

            return vData;
        }

        public static decimal converterStringDecimal_US(string aStrValor)
        {
            CultureInfo vObjCulture = new CultureInfo("en-US");

            return Convert.ToDecimal(aStrValor, vObjCulture);

        }

        public static string converterDecimalString_US(decimal aDecValor)
        {
            CultureInfo vObjCulture = new CultureInfo("en-US");

            return Convert.ToString (aDecValor, vObjCulture);

        }

        public static string buscarNomeMes(int aIntMes)
        {
            string vStrNomeMes = "Mes inválido";
            string[] vStrArrayMeses = new string[12] { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };

            if (aIntMes >= 1 && aIntMes <= 12)
                vStrNomeMes = vStrArrayMeses[aIntMes - 1];

            return vStrNomeMes;


        }

        public static int retornarUltimoDiaMes(DateTime aDatReferenciaMes)
        {
            //--Criando data no meio do mês informado
            DateTime vDatProxMes = new DateTime(aDatReferenciaMes.Year, aDatReferenciaMes.Month, 15);
            //--Pegando um mês pra frente
            vDatProxMes = vDatProxMes.AddMonths(1);
            //--Voltando para o primeiro dia do próximo mês
            vDatProxMes = new DateTime(vDatProxMes.Year, vDatProxMes.Month, 1);
            //--Retornando um dia anterior do primeiro dia do próximo mês
            return vDatProxMes.AddDays(-1).Day;
        }
    }
}
