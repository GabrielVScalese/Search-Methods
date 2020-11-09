using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    class CidadeMarte : IComparable<CidadeMarte>
    {
        // Atributo que representa o id da cidade
        private int id;
        // Atributo que representa o nome da cidade
        private string nomeCidade;
        // Atributo que representa a coordenada x da cidade
        private int x;
        // Atributo que representa a coordenada y da cidade
        private int y;

        // Construtor da classe
        public CidadeMarte (int id, string nomeCidade, int x, int y)
        {
            Id = id;
            NomeCidade = nomeCidade;
            X = x;
            Y = y;
        }

        // Propriedade do atributo id
        public int Id
        {
            get => id;
            set
            {
                if (value < 0)
                    throw new Exception("Id invalido");

                id = value;
            }
        }

        // Propriedade do atributo nomeCidade
        public string NomeCidade
        {
            get => nomeCidade;
            set
            {
                if (value == null || value.Equals(""))
                    throw new Exception("Nome de cidade invalido");

                nomeCidade = value;
            }
        }

        // Propriedade do atributo x
        public int X
        {
            get => x;
            set
            {
                x = value;
            }
        }

        // Propriedade do atributo y
        public int Y
        {
            get => y;
            set
            {
                y = value;
            }
        }

        // Método de comparação de informações entre dois objetos da classe
        public int CompareTo (CidadeMarte cm)
        {
            return id.CompareTo(cm.id);
        }

        // Método que retorna o valor do objeto da classe em formato string
        public override string ToString()
        {
            return id + " - " + nomeCidade.Trim();
        }
    }
}
