using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Vertice
    {
        public string rotulo;
        public bool foiVisitado;
        private bool estaAtivo;

        public Vertice(string rotulo)
        {
            this.rotulo = rotulo;
            foiVisitado = false;
            estaAtivo = true;
        }
    }
}
