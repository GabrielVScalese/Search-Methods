using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    class ListaSimples<Dado>
    {
        // Ponteiros da lista ligada
        private No<Dado> primeiro, ultimo;

        // Construtor da classe
        public ListaSimples ()
        { }

        // Propriedade do atributo primeiro
        public No<Dado> Primeiro
        {
            get => primeiro;
        }

        // Método de inserção de um valor na lista
        public void InserirNoFim (Dado info)
        {
            if (info == null)
                throw new Exception("Informacao invalida");

            if (IsVazia())
            {
                primeiro = new No<Dado>(info, primeiro);
                ultimo = primeiro;
            }
            else
            {
                No<Dado> aux = new No<Dado>(info, null);
                ultimo.Prox = aux;
                ultimo = aux;
            }
        }

        // Método de remoção do último elemento da lista
        public void RemoverDoFim ()
        {
            if (IsVazia())
                throw new Exception("Nada a remover");

            if (GetQtd() == 1)
            {
                primeiro = null;
                ultimo = null;
            }
            else
            {
                No<Dado> aux = primeiro;
                while (aux != null)
                {
                    if (aux.Prox.Equals(ultimo))
                    {
                        aux.Prox = null;
                        ultimo = aux;
                        break;
                    }

                    aux = aux.Prox;
                }
            }
        }

        // Método que retorna verdadeiro ou falso para o fato de a lista estar vazia
        public bool IsVazia ()
        {
            return primeiro == null;
        }

        // Método que retorna a quantidade de elementos da lista
        public int GetQtd ()
        {
            int qtd = 0;
            No<Dado> aux = primeiro;
            while (aux != null)
            {
                qtd++;
                aux = aux.Prox;
            }

            return qtd;
        }

        // Método que retona o último elemento da lista
        public Dado GetDoFim ()
        {
            if (IsVazia())
                throw new Exception("Lista vazia");

            return ultimo.Info;
        }

        // Método que verifica a existência de uma determinada informação
        public bool ExistsInfo (Dado info)
        {
            No<Dado> aux = primeiro;
            bool ret = false;
            while (aux != null)
            {
                if (aux.Info.Equals(info))
                {
                    ret = true;
                    break;
                }
                    
                aux = aux.Prox;
            }

            return ret;
        }

        // Método que retorna o valor do objeto da classe em formato string
        public override string ToString()
        {
            string ret = "{ ";
            No<Dado> aux = primeiro;
            while (aux != null)
            {
                if (aux.Prox == null)
                    ret += aux.Info;
                else
                    ret += aux.Info + ", ";

                aux = aux.Prox;
            }

            return ret + " }";
        }

        // Método que gera um clone do objeto da classe
        public Object Clone ()
        {
            ListaSimples<Dado> ret = null;
            try
            {
                ret = new ListaSimples<Dado>(this);
            }
            catch (Exception e)
            { }

            return ret;
        }

        // Construtor de cópia utilizado no método Clone
        public ListaSimples(ListaSimples<Dado> modelo)
        {
            if (modelo == null)
                throw new Exception("Modelo ausente");

            No<Dado> modeloAux = modelo.primeiro;
            while (modeloAux != null)
            {
                InserirNoFim(modeloAux.Info);

                modeloAux = modeloAux.Prox;
            }
        }
    }
}
