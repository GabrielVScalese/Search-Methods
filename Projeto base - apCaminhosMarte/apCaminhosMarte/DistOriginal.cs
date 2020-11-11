using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class DistOriginal
    {
        public long distancia;
        public int verticePai;

        public DistOriginal(int vp, long d)
        {
            distancia = d;
            verticePai = vp;
        }
    }
}
