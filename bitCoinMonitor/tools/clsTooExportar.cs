using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace bitCoinMonitor.tools
{
    class clsTooExportar
    {

        public static void exportarExcel(System.Data.DataTable aObjDados,string[] aStrNomeColunas )
        {
            try{


            Application vObjExcel = new Application();

            int vIntAux =0 ;

                if (aObjDados.Rows.Count > 0)
                {

                    vObjExcel.Application.Workbooks.Add(Type.Missing);
                    for (int i = 1; i < aObjDados.Columns.Count + 1; i++)
                    {

                        if (aStrNomeColunas[i - 1] != String.Empty)
                        {
                            vIntAux++;
                            vObjExcel.Cells[1, vIntAux] = aStrNomeColunas[i - 1];
                        }
                    }
                    //

                    for (int i = 0; i < aObjDados.Rows.Count; i++)
                    {
                        vIntAux = 0;
                        for (int j = 0; j < aObjDados.Columns.Count; j++)
                        {
                            if (aStrNomeColunas[j] != String.Empty)
                            {
                                vIntAux++;
                                vObjExcel.Cells[i + 2, vIntAux] = aObjDados.Rows[i][j].ToString();
                            }
                        }
                    }
                    //
                    vObjExcel.Columns.AutoFit();
                    //
                    vObjExcel.Visible = true;
                }
             }
            catch
            {
                throw;
            }
        }

    }

}
