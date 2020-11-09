using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    class No<Dado>
    {
        // Atributo que representa a informação do nó
        private Dado info;
        // Atributo que representa o próximo nó
        private No<Dado> prox;

        // Construtor da classe
        public No(Dado info, No<Dado> prox)
        {
            Info = info;
            Prox = prox;
        }

        // Construtor da classe
        public No(Dado info)
        {
            Info = info;
        }

        // Propriedade do atributo info
        public Dado Info
        {
            get => info;
            set
            {
                info = value;
            }
        }

        // Propriedade do atributo prox
        public No<Dado> Prox
        {
            get => prox;
            set
            {
                prox = value;
            }
        }
    }
}
