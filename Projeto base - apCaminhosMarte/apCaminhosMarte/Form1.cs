using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    // Nome: Gabriel Villar Scalese     RA: 19171
    // Nome: Nícolas Maisonnette Duarte RA: 19192
    public partial class FrmMapa : Form
    {
        // Árvore contendo as cidades do arquivo texto
        private ArvoreCidades arvoreCidades;
        // Matriz de adjacências contendo as ligações entre cidades
        private GrafoBacktracking grafoBacktracking;
        // Pilha contendo todos os caminhos possíveis entre duas cidades
        private PilhaLista<PilhaLista<Movimento>> caminhos;
        // Pilha contendo o melhor caminho dentre as pilhas de caminho de caminhos
        private PilhaLista<Movimento> melhorCaminho;
        // Matriz de adjacências contendo as arestas que representam ligações entre cidades
        private GrafoDijkstra grafoDijkstra;
        // Inteiro que representa o critério adotado pelo usuário
        private int criterio;

        public FrmMapa()
        {
            InitializeComponent();
        }

        // Evento click do botão que irá obter as cidades escolhidas pelo usuário e chamará o método de busca de caminhos
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            int idOrigem = GetOrigem();
            int idDestino = GetDestino();
            Movimento[] percurso = new Movimento[23];

            if (idOrigem == -1 || idDestino == -1)
            {
                MessageBox.Show("Cidades inválidas");
                return;
            }

            if (idOrigem == idDestino)
            {
                MessageBox.Show("Destino é igual à origem!");
                return;
            }

            if (GetCriterio() == null)
            {
                MessageBox.Show("Selecione o critério desejado!");
                return;
            }

           
            if (GetMetodo() == null)
            {
                MessageBox.Show("Selecione o método desejado!");
                return;
            }

            var rb = GetMetodo();

            switch (rb.Name)
            {
                case "rbPilhas": caminhos = grafoBacktracking.ProcurarCaminhos (idOrigem, idDestino);
                    break;

                case "rbRecursao": caminhos = grafoBacktracking.ProcurarCaminhosRec (idOrigem, idDestino);
                    break;

                case "rbDijkstra":  InicializarGrafo();
                                    percurso = grafoDijkstra.Caminho(idOrigem, idDestino);
                                    var pilhaLista = GetPilha(percurso);
                                    caminhos = new PilhaLista<PilhaLista<Movimento>>();
                                    caminhos.Empilhar(pilhaLista);
                                    txtTotal.Text = grafoDijkstra.GetTotalPercurso(percurso) + "";
                    break;           
            }
            
            LimparDados();
            if (caminhos.GetQtd() > 0)
            {
                ExibirCaminhos();
                ExibirMelhorCaminho();
            }

            if (caminhos.GetQtd() == 0)
                MessageBox.Show("Nenhum caminho foi encontrado!");
            else
                MessageBox.Show("Número de caminhos encontrados: " + caminhos.GetQtd().ToString());
        }

        // Método que obtém a cidade de origem escolhida pelo usuário
        private int GetOrigem ()
        {
            if (lsbOrigem.SelectedItem == null)
                return -1;

            string linhaSelecionada = lsbOrigem.SelectedItem.ToString();
            int id = int.Parse(linhaSelecionada.Split('-')[0].Trim());

            return id;
        }

        // Método que obtém a cidade de destino escolhida pelo usuário
        private int GetDestino ()
        {
            if (lsbDestino.SelectedItem == null)
                return -1;

            string linhaSelecionada = lsbDestino.SelectedItem.ToString();
            int id = int.Parse(linhaSelecionada.Split('-')[0].Trim());

            return id;
        }

        // Método que verifica qual caminho é mais curto dentre os caminhos obtidos
        private PilhaLista<Movimento> MelhorCaminhoDistancia ()
        {
            No<PilhaLista<Movimento>> umCaminho = caminhos.Inicio;
            PilhaLista<Movimento> melhorCaminho = umCaminho.Info;
            while (umCaminho != null)
            {
                if (umCaminho.Prox == null)
                    break;

                if (ObterDistancia(melhorCaminho) > ObterDistancia(umCaminho.Prox.Info))
                    melhorCaminho = umCaminho.Prox.Info;

                umCaminho = umCaminho.Prox;
            }

            return melhorCaminho;
        }

        // Método que obtém o caminho com menor tempo gasto
        private PilhaLista<Movimento> MelhorCaminhoTempo ()
        {
            No<PilhaLista<Movimento>> umCaminho = caminhos.Inicio;
            PilhaLista<Movimento> melhorCaminho = umCaminho.Info;

            while (umCaminho != null)
            {
                if (umCaminho.Prox == null)
                    break;

                if (ObterTempo(melhorCaminho) > ObterTempo(umCaminho.Prox.Info))
                    melhorCaminho = umCaminho.Prox.Info;

                umCaminho = umCaminho.Prox;
            }

            return melhorCaminho;
        }

        // Método que obtém o caminho com menor custo gasto
        private PilhaLista<Movimento> MelhorCaminhoCusto ()
        {
            No<PilhaLista<Movimento>> umCaminho = caminhos.Inicio;
            PilhaLista<Movimento> melhorCaminho = umCaminho.Info;
            while (umCaminho != null)
            {
                if (umCaminho.Prox == null)
                    break;

                if (ObterCusto(melhorCaminho) > ObterCusto(umCaminho.Prox.Info))
                    melhorCaminho = umCaminho.Prox.Info;

                umCaminho = umCaminho.Prox;
            }

            return melhorCaminho;
        }

        // Método que obtém o maior de número de movimentos contido um caminho 
        private int MaiorNumeroMovimentos ()
        {
            No<PilhaLista<Movimento>> umCaminho = caminhos.Inicio;
            int qtd = 0;
            int ant = 0;
            while (umCaminho != null)
            {
                qtd = umCaminho.Info.GetQtd();

                if (ant < qtd)
                    ant = qtd;

                umCaminho = umCaminho.Prox;
            }

            return ant;
        }

        // Método que obtém a distância total a ser percorrida em um determinado caminho
        private int ObterDistancia (PilhaLista<Movimento> umCaminho)
        {
            No<Movimento> aux = umCaminho.Inicio;
            int distancia = 0;
            while (aux != null)
            {
                distancia += aux.Info.Lc.Distancia;

                aux = aux.Prox;
            }

            return distancia;
        }

        // Método que obtém o tempo total gasto em um determinado caminho
        private int ObterTempo (PilhaLista<Movimento> umCaminho)
        {
            No<Movimento> aux = umCaminho.Inicio;
            int tempo = 0;
            while (aux != null)
            {
                tempo += aux.Info.Lc.Tempo;

                aux = aux.Prox;
            }

            return tempo;
        }

        // Método que obtém o custo necessário de um determinado caminho
        private int ObterCusto (PilhaLista<Movimento> umCaminho)
        {
            No<Movimento> aux = umCaminho.Inicio;
            int custo = 0;
            while (aux != null)
            {
                custo += aux.Info.Lc.Custo;

                aux = aux.Prox;
            }

            return custo;
        }

        // Método que inicializa um DataGridView com o cabeçalho da coluna nomeado
        private void InicializarColunas (int numeroColunas, DataGridView dgv)
        {
            for (int col = 0; col < numeroColunas; col++)
                dgv.Columns[col].HeaderText = "Cidade";
        }

        // Método que exibe no dgvCaminhos, todos os caminhos encontrados
        private void ExibirCaminhos ()
        {
            dgvCaminhos.RowCount = caminhos.GetQtd();
            dgvCaminhos.ColumnCount = MaiorNumeroMovimentos() + 1;

            InicializarColunas(dgvCaminhos.ColumnCount, dgvCaminhos);

            var umCaminho = caminhos.Inicio;
            for (int lin = 0; lin < caminhos.GetQtd(); lin++)
            {
                var umMovimento = umCaminho.Info.Inicio;
                for (int col = 0; col < dgvCaminhos.ColumnCount; col++)
                {
                    if (umMovimento == null)
                        break;

                    var primeiraCidade = arvoreCidades.GetCidade(umMovimento.Info.Origem);
                    var segundaCidade = arvoreCidades.GetCidade(umMovimento.Info.Destino);
                    dgvCaminhos[col, lin].Value = primeiraCidade.NomeCidade;
                    dgvCaminhos[col + 1, lin].Value = segundaCidade.NomeCidade;

                    umMovimento = umMovimento.Prox;
                }

                umCaminho = umCaminho.Prox;
            }

            dgvCaminhos.Refresh();
        }

        // Método que realiza a limpeza dos componentes que possuem escrita
        private void LimparDados ()
        {
            dgvCaminhos.Rows.Clear();
            dgvMelhorCaminho.Rows.Clear();
            pbMapa.Refresh();
        }

        // Método que exibe no dgvMelhorCaminho, o melhor caminho encontrado
        private void ExibirMelhorCaminho ()
        {
            var rb = GetCriterio();
            switch (rb.Name)
            {
                case "rbDistancia": melhorCaminho = MelhorCaminhoDistancia ();
                                    txtTotal.Text = ObterDistancia(melhorCaminho) + "";
                    break;

                case "rbTempo": melhorCaminho = MelhorCaminhoTempo ();
                                txtTotal.Text = ObterTempo(melhorCaminho) + "";
                    break;

                case "rbCusto": melhorCaminho = MelhorCaminhoCusto ();
                                txtTotal.Text = ObterCusto(melhorCaminho) + "";
                    break;
            }
            
            dgvMelhorCaminho.RowCount = 1;
            dgvMelhorCaminho.ColumnCount = melhorCaminho.GetQtd() + 1;
            InicializarColunas(dgvMelhorCaminho.ColumnCount, dgvMelhorCaminho);

            var umMovimento = melhorCaminho.Inicio;
            for (int col = 0; col < dgvMelhorCaminho.ColumnCount; col++)
            {
                if (umMovimento == null)
                    break;

                var primeiraCidade = arvoreCidades.GetCidade(umMovimento.Info.Origem);
                var segundaCidade = arvoreCidades.GetCidade(umMovimento.Info.Destino);
                dgvMelhorCaminho[col, 0].Value = primeiraCidade.NomeCidade;
                dgvMelhorCaminho[col + 1, 0].Value = segundaCidade.NomeCidade;

                umMovimento = umMovimento.Prox;
            }
            
            dgvMelhorCaminho.Refresh();
        }

        // Evento load do formulário que realiza a leitura dos arquivos texto 
        private void FrmMapa_Load(object sender, EventArgs e)
        {
            grafoBacktracking = new GrafoBacktracking(@"C:\Users\gabri\Downloads\CaminhosEntreCidadesMarte.txt");
            arvoreCidades = new ArvoreCidades(@"C:\Users\gabri\Downloads\CidadesMarte.txt");
            caminhos = new PilhaLista<PilhaLista<Movimento>>();
        }

        // Evento click do tbControl que desenha a árvore de cidades
        private void tbControl_Click(object sender, EventArgs e)
        {
            Graphics g = pbArvore.CreateGraphics();
            arvoreCidades.DesenharCidades(pbArvore.Width / 2, 5, g,  3 * Math.PI/ 2, 1.1, 260);
        }

        // Método que desenha no picture box, o caminho selecionado pelo usuário
        private void DesenharCaminho (PilhaLista<Movimento> umCaminho)
        {
            pbMapa.Refresh();
            No<Movimento> umMovimento = umCaminho.Inicio;
            while (umMovimento != null)
            {
                var pontoInicial = arvoreCidades.GetCidade(umMovimento.Info.Origem);
                var pontoFinal = arvoreCidades.GetCidade(umMovimento.Info.Destino);

                double x = pontoInicial.X;
                double y = pontoInicial.Y;
                double xf = pontoFinal.X;
                double yf = pontoFinal.Y;

                GetProporcao(ref x, ref y);
                GetProporcao(ref xf, ref yf);

                Pen caneta = new Pen(Color.Red);
                caneta.Width = 3;

                Graphics g = pbMapa.CreateGraphics();

                g.FillEllipse(new SolidBrush(Color.Black), Convert.ToInt32(x - 5), Convert.ToInt32(y - 5), 10, 10);
                g.DrawLine(caneta, Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(xf), Convert.ToInt32(yf));
                g.DrawString(pontoInicial.NomeCidade.Trim(), new Font("Comic Sans", 10, FontStyle.Bold), new SolidBrush(Color.Black), Convert.ToInt32(x - 10), Convert.ToInt32(y - 20));
                g.DrawString(pontoFinal.NomeCidade.Trim(), new Font("Comic Sans", 10, FontStyle.Bold), new SolidBrush(Color.Black), Convert.ToInt32(xf - 10), Convert.ToInt32(yf - 20));
                g.FillEllipse(new SolidBrush(Color.Black), Convert.ToInt32(xf - 5), Convert.ToInt32(yf - 5), 10, 10);

                umMovimento = umMovimento.Prox;
            }
        }

        // Método que retorna da pilha de caminhos, o caminho selecionado pelo usuário
        private PilhaLista<Movimento> ObterUmCaminho(int indiceCaminho)
        {
            No<PilhaLista<Movimento>> aux = caminhos.Inicio;
            for (int i = 0; i < caminhos.GetQtd(); i++)
            {
                if (i == indiceCaminho)
                    return aux.Info;
                else
                    aux = aux.Prox;
            }

            return null;
        }

        // Método que gera as coordenadas x e y proporcionais ao tamanho atual do mapa 
        private void GetProporcao (ref double x, ref double y)
        {
            double proporcaoX = pbMapa.Size.Width / 4096.0;
            double proporcaoY = pbMapa.Size.Height / 2048.0;

            x = x * proporcaoX;
            y = y * proporcaoY;
        }

        // Evento click do dgvCaminhos que obtém o caminho selecionado pelo usuário e desenha o mesmo
        private void dgvCaminhos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var umCaminho = ObterUmCaminho(dgvCaminhos.SelectedCells[0].RowIndex);
            MessageBox.Show("Distância a ser percorrida: " + ObterDistancia(umCaminho) + "\nTempo a ser gasto: " + ObterTempo (umCaminho) + "\nCusto necessário: " + ObterCusto (umCaminho));
            DesenharCaminho(umCaminho);
        }

        // Evento click do dgvMelhorCaminho que obtém o caminho selecionado pelo usuário e desenha o mesmo
        private void dgvMelhorCaminho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("Distância a ser percorrida: " + ObterDistancia(melhorCaminho) + "\nTempo a ser gasto: " + ObterTempo(melhorCaminho) + "\nCusto necessário: " + ObterCusto(melhorCaminho));
            DesenharCaminho(melhorCaminho);
        }

        // Método que obtém o RadioButton (gbCriterio) selecionado pelo usuário
        private RadioButton GetCriterio ()
        {
            foreach (RadioButton rdo in gbCriterio.Controls.OfType<RadioButton>())
                if (rdo.Checked == true)
                    return rdo;

            return null;
        }

        // Método que obtém o RadioButton (gbMetodo) selecionado pelo usuário
        private RadioButton GetMetodo()
        {
            foreach (RadioButton rdo in gbMetodo.Controls.OfType<RadioButton>())
                if (rdo.Checked == true)
                    return rdo;

            return null;
        }

        // Método que inicializa o grafoDijkstra a partir de um critério escolhido pelo usuário
        private void InicializarGrafo ()
        {
            var rb = GetCriterio();
            switch (rb.Name)
            {
                case "rbDistancia": grafoDijkstra = new GrafoDijkstra(grafoBacktracking, @"C:\Users\gabri\Downloads\CidadesMarteOrdenado.txt", @"C:\Users\gabri\Downloads\CaminhosEntreCidadesMarte.txt", 0);
                                    criterio = 0;
                    break;

                case "rbTempo": grafoDijkstra = new GrafoDijkstra(grafoBacktracking, @"C:\Users\gabri\Downloads\CidadesMarteOrdenado.txt", @"C:\Users\gabri\Downloads\CaminhosEntreCidadesMarte.txt", 1);
                                criterio = 1;
                    break;

                case "rbCusto": grafoDijkstra = new GrafoDijkstra(grafoBacktracking, @"C:\Users\gabri\Downloads\CidadesMarteOrdenado.txt", @"C:\Users\gabri\Downloads\CaminhosEntreCidadesMarte.txt", 2);
                                criterio = 2;
                    break;
            }
        }

        // Método que retorna uma pilha de Movimento a partir de um vetor de Movimento
        private PilhaLista<Movimento> GetPilha (Movimento[] percurso)
        {
            var pilhaLista = new PilhaLista<Movimento>();

            for (int i = 0; i < percurso.Length - 1; i++)
            {
                var lc = grafoBacktracking.GetLigacaoEntreCidades(percurso[i].Origem, percurso[i + 1].Origem);
                pilhaLista.Empilhar(new Movimento(percurso[i].Origem, percurso[i + 1].Origem, lc));
            }

            return pilhaLista;
        }
    }
}
