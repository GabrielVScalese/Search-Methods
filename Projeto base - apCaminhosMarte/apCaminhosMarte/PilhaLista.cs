using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    class PilhaLista<Dado>
    {
        // Atributo que representa uma lista ligada
        private ListaSimples<Dado> listaSimples;

        // Construtor da classe
        public PilhaLista ()
        {
            listaSimples = new ListaSimples<Dado>();
        }

        // Método que insere no final da lista a informação do parâmetro
        public void Empilhar (Dado info)
        {
            listaSimples.InserirNoFim(info);
        }

        // Método que remove o elemento final da lista
        public Dado Desempilhar ()
        {
            Dado topo = listaSimples.GetDoFim();
            listaSimples.RemoverDoFim();

            return topo;
        }

        // Propriedade do atributo inicio
        public No<Dado> Inicio
        {
            get => listaSimples.Primeiro;
        }

        // Propriedade que retorna o último elemento da lista
        public Dado Topo
        {
            get => listaSimples.GetDoFim();
        }

        // Método que retorna verdadeiro ou falso para o fato de a lista estar vazia
        public bool IsVazia ()
        {
            return listaSimples.IsVazia();
        }

        // Método que retorna a quantidade de elementos da lista
        public int GetQtd ()
        {
            return listaSimples.GetQtd();
        }

        // Método que gera um clone do objeto da classe
        public Object Clone ()
        {
            PilhaLista<Dado> ret = null;
            try
            {
                ret = new PilhaLista<Dado>(this);
            }
            catch (Exception e)
            { }

            return ret;
        }

        // Método que verifica a existência de uma determinada informação
        public bool ExistsInfo (Dado info)
        {
            return listaSimples.ExistsInfo(info);
        }

        // Construtor de cópia utilizado no método Clone
        public PilhaLista(PilhaLista<Dado> modelo)
        {
            if (modelo == null)
                throw new Exception("Modelo ausente");

            listaSimples = (ListaSimples<Dado>) modelo.listaSimples.Clone();
        }

        // Método que retorna o valor do objeto da classe em formato string
        public override string ToString()
        {
            return listaSimples.ToString();
        }
    }
}
