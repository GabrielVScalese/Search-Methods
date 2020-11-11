using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    class Movimento
    {
        // Atributo que representa a cidade de origem
        private int origem;
        private int destino;
        // Atributo que representa os dados do percurso
        private LigacaoCidade lc;
        private int indice;
        private long totalCriterio;

        public Movimento (string origem, long totalCriterio)
        {
            Origem = int.Parse(origem);
            this.totalCriterio = totalCriterio;
        }

        public Movimento (int origem , int destino, LigacaoCidade lc, int indice)
        {
            Origem = origem;
            Destino = destino;
            Lc = lc;
            Indice = indice;
        }

        // Construtor da classe
        public Movimento (int origem, int destino, LigacaoCidade lc)
        {
            Origem = origem;
            Destino = destino;
            Lc = lc;
        }

        // Propriedade do atributo origem
        public int Origem
        {
            get => origem;
            set
            {
                if (value < 0)
                    throw new Exception("Origem invalida");

                origem = value;
            }
        }

        public int Destino
        {
            get => destino;
            set
            {
                if (value < 0)
                    throw new Exception("Destino invalido");

                destino = value;
            }
        }

        // Propriedade do atributo lc
        public LigacaoCidade Lc
        {
            get => lc;
            set
            {
                if (value == null)
                    throw new Exception("Ligacao de cidade invalida");

                lc = value;
            }
        }

        public int Indice
        {
            get => indice;
            set
            {
                if (value < 0)
                    throw new Exception("Indice invalido");

                indice = value;
            }
        }

        // Método de comparação entre um objeto da classe e outro objeto
        public override bool Equals (Object obj)
        {
            if (obj == null)
                return false;

            if (this == obj)
                return true;

            if (!GetType().Equals(obj.GetType()))
                return false;

            Movimento mov = (Movimento)obj;

            if (origem != mov.Origem)
                return false;


            if (!lc.Equals(mov.Lc))
                return false;

            return true;
        }

        // Método de comparação que verifica qual objeto é maior (objeto da classe ou objeto do parâmetro)
        public int CompareTo (Movimento m)
        {
            return origem.CompareTo(m.origem);
        }

        // Método que retorna o valor do objeto da classe em formato string
        public override string ToString()
        {
            return "| O: " + origem + " | Dados: " + lc;
        }
    }
}
