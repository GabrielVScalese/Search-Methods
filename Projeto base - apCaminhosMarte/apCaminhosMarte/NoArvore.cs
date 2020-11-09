using System;
using System.Collections.Generic;
using System.Text;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    public class NoArvore<Dado> : IComparable<NoArvore<Dado>> where Dado : IComparable<Dado>
    {
        // Atributo que representa a informação do ramo/raiz
        private Dado info;
        // Atributo que representa o ramo descendente esquerdo
        private NoArvore<Dado> esq;
        // Atributo que representa o ramo descendente direito
        private NoArvore<Dado> dir;

        // Construtor da classe
        public NoArvore(Dado info, NoArvore<Dado> esq, NoArvore<Dado> dir)
        {
            Info = info;
            Esq = esq;
            Dir = dir;
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

        // Propriedade do atributo esq
        public NoArvore<Dado> Esq
        {
            get => esq;
            set
            {
                esq = value;
            }
        }

        // Propriedade do atributo dir
        public NoArvore<Dado> Dir
        {
            get => dir;
            set
            {
                dir = value;
            }
        }

        // Método de comparação de informações entre dois nós
        public int CompareTo(NoArvore<Dado> obj)
        {
            return info.CompareTo(obj.info);
        }

        // Método de comparação entre dois nós
        public bool Equals(NoArvore<Dado> obj)
        {
            if (obj == null)
                return false;

            return info.Equals(obj.info);
        }
    }
}
