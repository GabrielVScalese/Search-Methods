using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Guilherme Augusto Teixeira RA: 19174
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMapa());
        }
    }
}
