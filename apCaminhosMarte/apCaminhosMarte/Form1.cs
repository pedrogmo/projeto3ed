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
        Bitmap gif;

        Color corNo = Color.Blue;
        Color corLinhaArvore = Color.Red;
        Color corLinhaCidade = Color.FromArgb(51, 77, 201);
        Color corCidade = Color.Black;
        const int DIAMETRO_NO = 30;
        const int DIAMETRO_CIDADE = 10;

        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (lsbOrigem.SelectedIndex == -1)
                MessageBox.Show("Selecione uma cidade de origem", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if(lsbDestino.SelectedIndex == -1)
                MessageBox.Show("Selecione uma cidade de destino", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (lsbOrigem.SelectedIndex == lsbDestino.SelectedIndex)
                MessageBox.Show("Selecione uma cidade de destino diferente da origem", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                List<Caminho> possibilidades = grafo.Caminhos(lsbOrigem.SelectedIndex, lsbDestino.SelectedIndex);
                dgvCaminhos.RowCount = possibilidades.Count;
                dgvCaminhos.ColumnCount = qtdCidades;
                int l = 0;
                foreach (Caminho caminho in possibilidades)
                {
                    int c = 0;
                    foreach (int cidade in caminho.Rota)
                        dgvCaminhos.Rows[l].Cells[c++].Value = arvore.Buscar(new Cidade(cidade));
                    ++l;
                }

                bool nenhumCaminho = possibilidades.Count == 0;
                dgvMelhorCaminho.RowCount = nenhumCaminho ? 0 : 1;
                dgvMelhorCaminho.ColumnCount = nenhumCaminho ? 0 : qtdCidades;

                if (!nenhumCaminho)
                {
                    Caminho melhor = null;
                    int menorDist = int.MaxValue;
                    foreach (Caminho cam in possibilidades)
                        if (cam.DistanciaTotal < menorDist)
                        {
                            menorDist = cam.DistanciaTotal;
                            melhor = cam;
                        }
                    int c = 0;
                    foreach(int cidade in melhor.Rota)
                        dgvMelhorCaminho.Rows[0].Cells[c++].Value = arvore.Buscar(new Cidade(cidade));
                }

                if (nenhumCaminho)
                    MessageBox.Show("Nenhum caminho foi encontrado", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);                
                else
                    MessageBox.Show("Caminhos foram encontrados, clique em algum deles para visualizar", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gif = new Bitmap("../../../../Imagens/cidadeDestino.gif");
            ImageAnimator.Animate(gif, drawFrame);
            arvore = new ArvoreBinaria<Cidade>();
            var leitorCidades = new StreamReader("cidades.txt", Encoding.UTF7);           
            while (!leitorCidades.EndOfStream)
            {
                arvore.Inserir(new Cidade(leitorCidades.ReadLine()));
                ++qtdCidades;
            }
            leitorCidades.Close();

            qtdCidades = arvore.Tamanho;

            arvore.InOrdem((Cidade c) => {
                lsbOrigem.Items.Add(c);
                lsbDestino.Items.Add(c);
            });
            int[,] matriz = new int[qtdCidades, qtdCidades];
            for (int l = 0; l < qtdCidades; ++l)
                for (int c = 0; c < qtdCidades; ++c)
                    matriz[l, c] = 0;
            var leitorCaminhos = new StreamReader("caminhos.txt");
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

        private void Form1_Resize(object sender, EventArgs e)
        {
            pbMapa.Update();
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            for (int l = 0; l < qtdCidades; ++l)
                for (int c = 0; c < qtdCidades; ++c)
                {
                    int dist = grafo.DistanciaEntre(l, c);
                    if (dist != 0)
                        DesenhaLinha(l,c,dist,gfx);
                }
            int origem = lsbOrigem.SelectedIndex;
            int destino = lsbDestino.SelectedIndex;
            arvore.InOrdem((Cidade c) => {
                DesenhaCidade(c, gfx, c.Codigo == origem, c.Codigo == destino);
            });
        }

        private void DesenhaLinha(int cod1, int cod2, int dist, Graphics gfx)
        {
            Cidade um = arvore.Buscar(new Cidade(cod1));
            Cidade dois = arvore.Buscar(new Cidade(cod2));
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            int x1 = pbMapa.Size.Width * um.X / 4096;
            int y1 = pbMapa.Size.Height * um.Y / 2048;
            int x2 = pbMapa.Size.Width * dois.X / 4096;
            int y2 = pbMapa.Size.Height * dois.Y / 2048;
            if (um.Nome == "Gondor" && (dois.Nome == "Arrakeen" || dois.Nome == "Senzeni Na"))
            {
                gfx.DrawLine(caneta, x1, y1, pbMapa.Size.Width - 1, y2);
                caneta.CustomEndCap = bigArrow;
                gfx.DrawLine(caneta, 0, y2, x2, y2);
            }
            else if (dois.Nome == "Gondor" && (um.Nome == "Arrakeen" || um.Nome == "Senzeni Na"))
            {
                gfx.DrawLine(caneta, x2, y2, pbMapa.Size.Width - 1, y1);
                caneta.CustomEndCap = bigArrow;
                gfx.DrawLine(caneta, 0, y1, x1, y1);
            }
            else
            {
                caneta.CustomEndCap = bigArrow;
                gfx.DrawLine(caneta, x1, y1, x2, y2);
            }
        }

        private void DesenhaCidade(Cidade c, Graphics gfx, bool origem, bool destino)
        {
            int x = pbMapa.Size.Width * c.X / 4096 - DIAMETRO_CIDADE / 2;
            int y = pbMapa.Size.Height * c.Y / 2048 - DIAMETRO_CIDADE / 2;
            gfx.FillEllipse(new SolidBrush(corCidade), x , y , DIAMETRO_CIDADE, DIAMETRO_CIDADE);
            gfx.DrawString(c.Nome, new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(corCidade), new Point(x - 10, y + 10));
            if (origem)
                gfx.DrawImage(Image.FromFile("../../../../Imagens/cidadeOrigem.png"), x - DIAMETRO_CIDADE * 1.5f, y - DIAMETRO_CIDADE * 3.5f, DIAMETRO_CIDADE * 4, DIAMETRO_CIDADE * 4);
            if (destino)
            {
                ImageAnimator.UpdateFrames(gif);
                using (Bitmap bmp = new Bitmap(gif.Width, gif.Height))
                {
                    using (Graphics gr = Graphics.FromImage(bmp))
                    {
                        gr.DrawImage(gif, 0, 0);
                    }
                    gfx.DrawImage(bmp, x - DIAMETRO_CIDADE * 2.5f, y - DIAMETRO_CIDADE * 2.5f, DIAMETRO_CIDADE * 6, DIAMETRO_CIDADE * 6);
                }
            }
        }
        private void drawFrame(object sender, EventArgs e)
        {
            pbMapa.Invalidate();
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            DesenhaArvore(true, arvore.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 2.5, 450, gfx);
        }

        private void DesenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,
                           int x, int y, double angulo, double incremento,
                           double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);
                var esq = raiz.Esquerdo;
                DesenhaArvore(false, esq, xf, yf, Math.PI / 2 + incremento,
                                                 incremento * 0.60, comprimento * 0.8, g);
                var dir = raiz.Direito;
                DesenhaArvore(false, dir, xf, yf, Math.PI / 2 - incremento,
                                                  incremento * 0.60, comprimento * 0.8, g);
                // sleep(100);
                SolidBrush preenchimento = new SolidBrush(Color.Blue);
                g.FillEllipse(preenchimento, xf - 15, yf - 15, 30, 30);
                g.DrawString(Convert.ToString(raiz.Info.Codigo), new Font("Comic Sans", 12),
                              new SolidBrush(Color.Yellow), xf - 15, yf - 10);
            }
        }
    }
}
