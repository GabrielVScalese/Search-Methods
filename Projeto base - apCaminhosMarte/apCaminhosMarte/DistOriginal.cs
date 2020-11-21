using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Guilherme Augusto Teixeira RA: 19174
    class DistOriginal
    {
        // Atributo que representa a distância do vértice até o seu vértice pai
        public long distancia;
        // Atributo que representa o seu vértice pai
        public int verticePai;

        // Construtor da classe
        public DistOriginal(int vp, long d)
        {
            distancia = d;
            verticePai = vp;
        }
    }
}
