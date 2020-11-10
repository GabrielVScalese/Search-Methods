using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    class Grafo
    {
        //private const int NUM_VERTICES = 20;
        private Vertice[] vertices;
        private int[,] adjMatrix;
        int numVerts;

        /// DIJKSTRA
        DistOriginal[] percurso;
        int infinity = int.MaxValue;
        int verticeAtual; // global usada para indicar o vértice atualmente sendo visitado
        int doInicioAteAtual; // global usada para ajustar menor caminho com Djikstra
        int nTree;

        public Grafo (GrafoBacktracking gb, string cidadesArquivo, string caminhosArquivo)
        {
            vertices = new Vertice[gb.Matriz.GetLength(0)];
            adjMatrix = new int[gb.Matriz.GetLength(0), gb.Matriz.GetLength(0)];
            numVerts = 0;
            nTree = 0;

            for (int j = 0; j < adjMatrix.GetLength(0); j++) // zera toda a matriz
                for (int k = 0; k < adjMatrix.GetLength(0); k++)
                    adjMatrix[j, k] = infinity; // distância tão grande que não existe

            percurso = new DistOriginal[adjMatrix.GetLength(0)];
            LerArquivos(cidadesArquivo, caminhosArquivo);
        }

        public void NovoVertice (string rotulo)
        {
            vertices[numVerts] = new Vertice(rotulo);
            numVerts++;
        }

        public void NovaAresta (int origem, int destino, int peso)
        {
            adjMatrix[origem, destino] = peso;
        }

        public string[] Caminho (int inicioDoPercurso, int finalDoPercurso)
        {
            for (int j = 0; j < numVerts; j++)
                vertices[j].foiVisitado = false;

            vertices[inicioDoPercurso].foiVisitado = true;

            for (int j = 0; j < numVerts; j++)
            {
                // anotamos no vetor percurso a distância entre o inicioDoPercurso e cada vértice
                // se não há ligação direta, o valor da distância será infinity
                int tempDist = adjMatrix[inicioDoPercurso, j];
                percurso[j] = new DistOriginal(inicioDoPercurso, tempDist);
            }

            for (int nTree = 0; nTree < numVerts; nTree++)
            {
                // Procuramos a saída não visitada do vértice inicioDoPercurso com a menor distância
                int indiceDoMenor = ObterMenor();
                // e anotamos essa menor distância
                int distanciaMinima = percurso[indiceDoMenor].distancia;
                // o vértice com a menor distância passa a ser o vértice atual
                // para compararmos com a distância calculada em AjustarMenorCaminho()
                verticeAtual = indiceDoMenor;
                doInicioAteAtual = percurso[indiceDoMenor].distancia;
                // visitamos o vértice com a menor distância desde o inicioDoPercurso
                vertices[verticeAtual].foiVisitado = true;
                AjustarMenorCaminho();
            }

            return ExibirPercursos(inicioDoPercurso, finalDoPercurso);
        }

        public string[] ExibirPercursos (int inicioPercurso, int fimPercurso)
        {
            int cont = 0;

            Stack<string> pilha = new Stack<string>();

            int onde = fimPercurso;
            while (onde != inicioPercurso)
            {
                onde = percurso[onde].verticePai;
                pilha.Push(vertices[onde].rotulo);
                cont++;
            }

            int i = 0;
            string[] caminho = new string[pilha.Count + 1];
            while (pilha.Count != 0)
            {
                caminho[i] = pilha.Pop();

                i++;
            }

            if ((cont == 1) && (percurso[fimPercurso].distancia == infinity))
                caminho = null;
            else
                caminho[i] += vertices[fimPercurso].rotulo;

            return caminho;
        }

        public int ObterMenor ()
        {
            int distanciaMinima = infinity;
            int indiceDaMinima = 0;
            for (int j = 0; j < numVerts; j++)
                if (!(vertices[j].foiVisitado) && (percurso[j].distancia < distanciaMinima))
                {
                    distanciaMinima = percurso[j].distancia;
                    indiceDaMinima = j;
                }
            return indiceDaMinima;
        }

        public void AjustarMenorCaminho ()
        {
            for (int coluna = 0; coluna < numVerts; coluna++)
                if (!vertices[coluna].foiVisitado) // para cada vértice ainda não visitado
                {
                    // acessamos a distância desde o vértice atual (pode ser infinity)
                    int atualAteMargem = adjMatrix[verticeAtual, coluna];
                    // calculamos a distância desde inicioDoPercurso passando por vertice atual até
                    // esta saída
                    int doInicioAteMargem = doInicioAteAtual + atualAteMargem;
                    // quando encontra uma distância menor, marca o vértice a partir do
                    // qual chegamos no vértice de índice coluna, e a soma da distância
                    // percorrida para nele chegar
                    int distanciaDoCaminho = percurso[coluna].distancia;
                    if (doInicioAteMargem < distanciaDoCaminho)
                    {
                        percurso[coluna].verticePai = verticeAtual;
                        percurso[coluna].distancia = doInicioAteMargem;
                    }
                }
        }

        private void LerArquivos (string cidadesArquivo, string caminhosArquivo)
        {
            var cidades = new StreamReader(cidadesArquivo);
            var caminhos = new StreamReader(caminhosArquivo);

            while (!cidades.EndOfStream)
            {
                string linha = cidades.ReadLine();
                int cidade = int.Parse(linha.Substring(0, 3));

                NovoVertice(cidade.ToString());
            }
            cidades.Close();

            while (!caminhos.EndOfStream)
            {
                string linha = caminhos.ReadLine();
                int origem = int.Parse(linha.Substring(0, 3));
                int destino = int.Parse(linha.Substring(3, 3));
                int distancia = int.Parse(linha.Substring(6, 5));

                NovaAresta(origem, destino, distancia);
            }
            caminhos.Close();
        }
    }
}
