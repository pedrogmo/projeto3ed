using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing.Drawing2D;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    public partial class Form1 : Form
    {
        ArvoreBinaria<Cidade> arvore;
        int qtdCidades;
        Grafo grafo;
        List<Caminho> possibilidades;
        Caminho caminhoAtual;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arvore = new ArvoreBinaria<Cidade>();
            var leitorCidades = new StreamReader("../../Arquivos/CidadesMarte.txt", Encoding.UTF7);
            while (!leitorCidades.EndOfStream)
            {
                arvore.Inserir(new Cidade(leitorCidades.ReadLine()));
                ++qtdCidades;
            }
            leitorCidades.Close();
            qtdCidades = arvore.Tamanho;

            arvore.InOrdem((Cidade c) =>
            {
                lsbOrigem.Items.Add(c);
                lsbDestino.Items.Add(c);
            });
            int[,] matriz = new int[qtdCidades, qtdCidades];
            for (int l = 0; l < qtdCidades; ++l)
                for (int c = 0; c < qtdCidades; ++c)
                    matriz[l, c] = 0;
            var leitorCaminhos = new StreamReader("../../Arquivos/CaminhosEntreCidadesMarte.txt");
            while (!leitorCaminhos.EndOfStream)
            {
                //codigo1, codigo2, distância
                string linha = leitorCaminhos.ReadLine();
                int l = int.Parse(linha.Substring(0, Cidade.TAMANHO_CODIGO).Trim());
                //linha da matriz é cidade de origem
                int c = int.Parse(linha.Substring(Cidade.TAMANHO_CODIGO, Cidade.TAMANHO_CODIGO).Trim());
                //coluna da matriz é cidade de destino
                int d = int.Parse(linha.Substring(2 * Cidade.TAMANHO_CODIGO, 5).Trim());
                //distância entre cidades
                matriz[l, c] = d;
            }
            grafo = new Grafo(matriz);
            leitorCaminhos.Close();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (lsbOrigem.SelectedIndex == -1)
                MessageBox.Show("Selecione uma cidade de origem", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (lsbDestino.SelectedIndex == -1)
                MessageBox.Show("Selecione uma cidade de destino", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (lsbOrigem.SelectedIndex == lsbDestino.SelectedIndex)
                MessageBox.Show("Selecione uma cidade de destino diferente da origem", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                possibilidades = grafo.Caminhos(lsbOrigem.SelectedIndex, lsbDestino.SelectedIndex);
                bool nenhumCaminho = possibilidades.Count == 0;
                if (nenhumCaminho)
                    MessageBox.Show("Nenhum caminho foi encontrado", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    dgvCaminhos.RowCount = possibilidades.Count;
                    dgvCaminhos.ColumnCount = qtdCidades;
                    int l = 0, c = 0;
                    foreach (Caminho caminho in possibilidades)
                    {
                        c = 0;
                        foreach (int cidade in caminho.Rota)
                            dgvCaminhos.Rows[l].Cells[c++].Value = arvore.Buscar(new Cidade(cidade));
                        ++l;
                    }
                    dgvMelhorCaminho.RowCount = 1;
                    dgvMelhorCaminho.ColumnCount = qtdCidades;
                    Caminho melhor = null;
                    int menorDist = int.MaxValue;
                    foreach (Caminho cam in possibilidades)
                        if (cam.DistanciaTotal < menorDist)
                        {
                            menorDist = cam.DistanciaTotal;
                            melhor = cam;
                        }

                    c = 0;
                    foreach (int cidade in melhor.Rota)
                        dgvMelhorCaminho.Rows[0].Cells[c++].Value = arvore.Buscar(new Cidade(cidade));
                    caminhoAtual = melhor;
                    MessageBox.Show("Caminhos foram encontrados, clique em algum deles para visualizar", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            for (int l = 0; l < qtdCidades; ++l)
                for (int c = 0; c < qtdCidades; ++c)
                    if (grafo.DistanciaEntre(l, c) != 0)
                    {
                        Cidade um = arvore.Buscar(new Cidade(l));
                        Cidade dois = arvore.Buscar(new Cidade(c));
                        if (dois.Nome == "Gondor" && um.Nome == "Arrakeen") //um.Nome == "Senzeni Na"
                        {
                            Desenhador.DesenhaLinha(gfx, pbMapa, 0, um.Y + (dois.Y - um.Y), um, false);
                            Desenhador.DesenhaLinha(gfx, pbMapa, 4095, um.Y + (dois.Y - um.Y), dois, true);
                        }
                        else if (dois.Nome == "Gondor" && um.Nome == "Senzeni Na")
                        {
                            Desenhador.DesenhaLinha(gfx, pbMapa, 0, um.Y - (dois.Y - um.Y), um, false);
                            Desenhador.DesenhaLinha(gfx, pbMapa, 4095, um.Y - (dois.Y - um.Y), dois, true);
                        }
                        else
                            Desenhador.DesenhaLinha(gfx, pbMapa, um, dois, true);
                    }
            if (caminhoAtual != null)
            {
                List<Cidade> caminho = new List<Cidade>();
                foreach (int i in caminhoAtual.Rota)
                    caminho.Add(new Cidade(i));
                Desenhador.DesenhaCaminho(gfx, pbMapa, caminho);
            }
            int origem = lsbOrigem.SelectedIndex;
            int destino = lsbDestino.SelectedIndex;
            arvore.InOrdem((Cidade c) =>
            {
                Desenhador.DesenhaCidade(gfx, pbMapa, c, c.Codigo == origem, c.Codigo == destino);
            });
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            Desenhador.DesenharArvore(e.Graphics, tpArvore, arvore.Raiz);
        }

        private void dgvCaminhos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            caminhoAtual = possibilidades[index];
        }

        private void lsbOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            caminhoAtual = null;
        }
    }
}
