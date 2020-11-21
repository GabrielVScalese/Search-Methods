using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Guilherme Augusto Teixeira RA: 19174
    class Vertice
    {
        // Atributo que representa o rótulo do vértice
        public string rotulo;
        // Atributo que representa se o vértice já foi visitado
        public bool foiVisitado;
        // Atributo que representa se o vértice está ativo
        private bool estaAtivo;

        // Construtor da classe
        public Vertice(string rotulo)
        {
            this.rotulo = rotulo;
            foiVisitado = false;
            estaAtivo = true;
        }
    }
}
