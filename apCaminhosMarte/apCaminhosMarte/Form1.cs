using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    public partial class Form1 : Form
    {
        #region Variáveis e Constantes
        ArvoreBinaria<Cidade> arvore;
        int qtdCidades;
        Grafo grafo;
        List<Caminho> possibilidades;
        Caminho caminhoAtual;
        bool[] jaDesenhouCaminhoAtual;
        Bitmap gif;
        Color corNo = Color.Blue;
        Color corLinhaArvore = Color.Red;
        Color corLinhaCidade = Color.DarkSlateGray;
        Color corCidade = Color.Black;
        Color corLinhaCaminho = Color.FromArgb(51, 77, 201); //para caminhos possíveis
        Color corLinhaCaminhoSelecionado = Color.Red; //para caminho selecionado        
        const int DIAMETRO_NO = 30;
        const int DIAMETRO_CIDADE = 10;
        const int LARGURA = 4096;
        const int ALTURA = 2048;
        const int DIST_CIDADES = 20;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gif = new Bitmap("../../Imagens/cidadeDestino.gif");
            ImageAnimator.Animate(gif, DrawFrame);
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
            dgvCaminhos.Rows.Clear();
            dgvMelhorCaminho.Rows.Clear();

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
                    int maiorCaminho = 0;
                    foreach (Caminho caminho in possibilidades)
                    {
                        if (caminho.Rota.Count > maiorCaminho)
                            maiorCaminho = caminho.Rota.Count;
                        c = 0;
                        foreach (int cidade in caminho.Rota)
                            dgvCaminhos.Rows[l].Cells[c++].Value = arvore.Buscar(new Cidade(cidade));
                        ++l;
                    }
                    dgvCaminhos.ColumnCount = maiorCaminho;

                    dgvMelhorCaminho.RowCount = 1;                    
                    Caminho melhor = null;
                    int menorDist = int.MaxValue;
                    foreach (Caminho cam in possibilidades)
                        if (cam.DistanciaTotal < menorDist)
                        {
                            menorDist = cam.DistanciaTotal;
                            melhor = cam;
                        }
                    dgvMelhorCaminho.ColumnCount = melhor.Rota.Count;

                    c = 0;
                    foreach (int cidade in melhor.Rota)
                        dgvMelhorCaminho.Rows[0].Cells[c++].Value = arvore.Buscar(new Cidade(cidade));
                    caminhoAtual = possibilidades[0];
                    jaDesenhouCaminhoAtual = new bool[caminhoAtual.Rota.Count];
                    MessageBox.Show("Caminhos foram encontrados, clique em algum deles para visualizar", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }        

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            DesenharArvore(e.Graphics, tpArvore, arvore.Raiz);
        }

        private void dgvCaminhos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index >= 0)
            {
                jaDesenhouCaminhoAtual = new bool[caminhoAtual.Rota.Count];
                caminhoAtual = possibilidades[index];
            }
        }

        private void lsbOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            caminhoAtual = null;
            possibilidades = null;
            dgvCaminhos.RowCount = dgvCaminhos.ColumnCount = dgvMelhorCaminho.RowCount = dgvMelhorCaminho.ColumnCount = 0;
        }

        private void DrawFrame(object sender, EventArgs e)
        {
            pbMapa.Invalidate();
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            for (int l = 0; l < qtdCidades; ++l)
                for (int c = 0; c < qtdCidades; ++c)
                {
                    int dist = grafo.DistanciaEntre(l, c);
                    if (dist != 0)
                    {
                        Cidade um = arvore.Buscar(new Cidade(l));
                        Cidade dois = arvore.Buscar(new Cidade(c));                        
                        if (dois.Nome == "Gondor" && um.Nome == "Arrakeen") //um.Nome == "Senzeni Na"
                        {
                            DesenhaLinha(gfx, 0, um.Y + DIST_CIDADES, um, false);
                            DesenhaLinha(gfx, LARGURA - 1, um.Y + DIST_CIDADES, dois, true);
                            var ponto1 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
                            var ponto2 = new PointF(pbMapa.Size.Width-1, pbMapa.Size.Height * (um.Y + DIST_CIDADES) / ALTURA);
                            DesenhaDistancia(gfx, ponto1, ponto2, dist);
                        }
                        else if (dois.Nome == "Gondor" && um.Nome == "Senzeni Na")
                        {
                            DesenhaLinha(gfx, 0, um.Y - DIST_CIDADES, um, false);
                            DesenhaLinha(gfx, LARGURA - 1, um.Y - DIST_CIDADES, dois, true);
                            var ponto1 = new PointF(0, pbMapa.Size.Height * (um.Y - DIST_CIDADES) / ALTURA);
                            var ponto2 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA);
                            DesenhaDistancia(gfx, ponto1, ponto2, dist);
                        }
                        else
                        {                            
                            DesenhaLinha(gfx, um, dois, true);
                            var ponto1 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA);
                            var ponto2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
                            DesenhaDistancia(gfx, ponto1, ponto2, dist);
                        }
                    }
                }
            if (caminhoAtual != null)
            {
                List<Cidade> caminho = new List<Cidade>();
                foreach (Caminho c in possibilidades)
                {
                    if (c != caminhoAtual)
                    {
                        caminho = new List<Cidade>();
                        foreach (int i in c.Rota)
                            caminho.Add(arvore.Buscar(new Cidade(i)));
                        DesenhaCaminho(gfx, caminho, false);
                    }
                }
                caminho = new List<Cidade>();
                foreach (int i in caminhoAtual.Rota)
                    caminho.Add(arvore.Buscar(new Cidade(i)));
                DesenhaCaminho(gfx, caminho, true);
            }
            int origem = lsbOrigem.SelectedIndex;
            int destino = lsbDestino.SelectedIndex;
            arvore.InOrdem((Cidade c) =>
            {
                DesenhaCidade(gfx, c, c.Codigo == origem, c.Codigo == destino);
            });
        }

        private void DesenhaCidade(Graphics gfx, Cidade c, bool origem, bool destino)
        {
            int x = pbMapa.Size.Width * c.X / LARGURA - DIAMETRO_CIDADE / 2;
            int y = pbMapa.Size.Height * c.Y / ALTURA - DIAMETRO_CIDADE / 2;
            gfx.FillEllipse(new SolidBrush(corCidade), x, y, DIAMETRO_CIDADE, DIAMETRO_CIDADE);
            gfx.DrawString(c.Nome, new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(corCidade), new PointF(x - 10, y + 10));
            if (origem)
                gfx.DrawImage(Image.FromFile("../../Imagens/cidadeOrigem.png"), x - DIAMETRO_CIDADE * 1.5f, y - DIAMETRO_CIDADE * 3.5f, DIAMETRO_CIDADE * 4, DIAMETRO_CIDADE * 4);
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

        private void DesenhaDistancia (Graphics gfx, PointF ponto1, PointF ponto2, int dist)
        {            
            float xString = 0, yString = 0;
            if (ponto1.X < ponto2.X)
                xString = ponto1.X + (ponto2.X - ponto1.X) / 2;
            else
                xString = ponto2.X + (ponto1.X - ponto2.X) / 2;
            if (ponto1.Y < ponto2.Y)
                yString = ponto1.Y + (ponto2.Y - ponto1.Y) / 2;
            else
                yString = ponto2.Y + (ponto1.Y - ponto2.Y) / 2;
            SizeF size = gfx.MeasureString(dist + "", new Font("Century Gothic", 8, FontStyle.Bold));
            xString -= size.Width / 2;
            gfx.DrawString(dist + "", new Font("Century Gothic", 8, FontStyle.Bold), new SolidBrush(corCidade), xString, yString);
        }

        private void DesenhaLinha(Graphics gfx, Cidade um, Cidade dois, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA);
            PointF p2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }

        private void DesenhaLinha(Graphics gfx, Pen caneta, Cidade um, Cidade dois, bool comSeta)
        {
            PointF p1 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA);
            PointF p2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
            if (comSeta)
                caneta.CustomEndCap = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            gfx.DrawLine(caneta, p1, p2);
        }        

        private void DesenhaLinha(Graphics gfx, float x1, float y1, Cidade dois, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pbMapa.Size.Width * x1 / LARGURA, pbMapa.Size.Height * y1 / ALTURA);
            PointF p2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }

        private void DesenhaLinha(Graphics gfx, Pen caneta, float x1, float y1, Cidade dois, bool comSeta)
        {
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pbMapa.Size.Width * x1 / LARGURA, pbMapa.Size.Height * y1 / ALTURA);
            PointF p2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }

        private void DesenhaLinha(Graphics gfx, float x1, float y1, PointF p2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pbMapa.Size.Width * x1 / LARGURA, pbMapa.Size.Height * y1 / ALTURA);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }

        private void DesenhaLinha(Graphics gfx, PointF p1, PointF p2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }
        
        private void DesenhaLinhaAnimada(Graphics gfx, Pen caneta, PointF p1, PointF p2, int indice)
        {
            int qtdPassos = 0;
            int intervalo = 10;
            double c = p2.Y - p1.Y;
            double b = p2.X - p1.X;
            double duracao = b / intervalo;
            double tamanhoPasso = 10;
            double angulo = c / b;
            if (b > 0)
                angulo *= -1;
            bool acabou = false;
            Timer tmr = new Timer();
            tmr.Interval = intervalo;
            void Tick(Graphics gfxT)
            {
                if (!acabou)
                {
                    qtdPassos++;
                    double x = qtdPassos * tamanhoPasso;
                    double y = angulo * x;
                    x = b < 0 ? x * -1 : x;
                    gfxT.DrawLine(new Pen(corLinhaCaminhoSelecionado, 3), p1, new PointF(Convert.ToSingle(x + p1.X), Convert.ToSingle(p1.Y - y)));
                    if (qtdPassos * tamanhoPasso > Math.Abs(b))
                    {
                        acabou = true;
                        gfxT.DrawLine(caneta, p1, p2);
                        jaDesenhouCaminhoAtual[indice] = true;
                        tmr.Stop();
                    }
                }
            }
            tmr.Tick += (object sender, EventArgs e) => { Tick(pbMapa.CreateGraphics()); };
            tmr.Start();
        }

        private void DesenhaCaminho(Graphics gfx, List<Cidade> caminhoAtual, bool selecionado)
        {
            Cidade anterior = caminhoAtual[0];
            int indice = 0;
            foreach (Cidade atual in caminhoAtual)
            {                
                Pen caneta = new Pen(selecionado ? corLinhaCaminhoSelecionado : corLinhaCaminho, 3);
                if (!atual.Equals(anterior))
                {
                    PointF p1 = new PointF(pbMapa.Size.Width * anterior.X / LARGURA, pbMapa.Size.Height * anterior.Y / ALTURA);
                    PointF p2 = new PointF(pbMapa.Size.Width * atual.X / LARGURA, pbMapa.Size.Height * atual.Y / ALTURA);
                    /*bool jaDesenhou = true;
                    foreach (bool d in jaDesenhouCaminhoAtual)
                        if (!d)
                            jaDesenhou = false;
                    if (jaDesenhou)
                        DesenhaLinha(gfx, caneta, anterior, atual, true);                    
                    else
                        DesenhaLinhaAnimada(gfx, caneta, p1, p2, indice);*/

                    if (atual.Nome == "Gondor" && anterior.Nome == "Arrakeen") //anterior.Nome == "Senzeni Na"
                    {
                        DesenhaLinha(gfx, caneta, 0, anterior.Y + DIST_CIDADES, anterior, false);
                        DesenhaLinha(gfx, caneta, LARGURA - 1, anterior.Y + DIST_CIDADES, atual, true);
                    }
                    else if (atual.Nome == "Gondor" && anterior.Nome == "Senzeni Na")
                    {
                        DesenhaLinha(gfx, caneta, 0, anterior.Y - DIST_CIDADES, anterior, false);
                        DesenhaLinha(gfx, caneta, LARGURA - 1, anterior.Y - DIST_CIDADES, atual, true);
                    }
                    else
                        DesenhaLinha(gfx, caneta, anterior, atual, true);

                    anterior = atual;
                }
                ++indice;
            }
        }

        private void DesenharArvore(Graphics gfx, TabPage tpArvore, NoArvore<Cidade> raiz)
        {
            DesenhaArvore(true, raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 2.5, 450, gfx);
        }

        private void DesenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,
                   float x, float y, double angulo, double incremento,
                   double comprimento, Graphics g)
        {
            float xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (float) Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (float) Math.Round(y + Math.Sin(angulo) * comprimento);
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

        private void dgvMelhorCaminho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indiceMelhorCaminho = 0;
            for (int l = 0; l < dgvCaminhos.RowCount; ++l)
            {
                bool ehOMelhor = true;
                for (int c = 0; c < dgvCaminhos.ColumnCount; ++c)
                {
                    if (l >= dgvMelhorCaminho.RowCount)
                    {
                        ehOMelhor = false;
                        break;
                    }
                    else
                    {
                        string cidadeCaminho = dgvCaminhos.Rows[l].Cells[c].Value == null ? "" : dgvCaminhos.Rows[l].Cells[c].Value.ToString().Substring(0, 2);
                        string cidadeMelhorCaminho = dgvMelhorCaminho.Rows[l].Cells[c].Value == null ? "" : dgvMelhorCaminho.Rows[l].Cells[c].Value.ToString().Substring(0, 2);
                        if ((cidadeCaminho == "") != (cidadeMelhorCaminho == ""))
                        {
                            ehOMelhor = false;
                            break;
                        }
                        else if (cidadeCaminho == "" && cidadeMelhorCaminho == "")
                            break;
                        else if (int.Parse(cidadeCaminho) != int.Parse(cidadeMelhorCaminho))
                        {
                            ehOMelhor = false;
                            break;
                        }
                    }
                }
                if (ehOMelhor)
                {
                    indiceMelhorCaminho = l;
                    break;
                }
            }
            caminhoAtual = possibilidades[indiceMelhorCaminho];
        }
    }
}

/*      private void DesenhaLinha(Graphics gfx, Pen caneta, Cidade um, Cidade dois)
        {
            PointF p1 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA);
            PointF p2 = new PointF(pbMapa.Size.Width * dois.X / LARGURA, pbMapa.Size.Height * dois.Y / ALTURA);
            gfx.DrawLine(caneta, p1, p2);
        }

        private void DesenhaLinha(Graphics gfx, float x1, float y1, float x2, float y2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pbMapa.Size.Width * x1 / LARGURA, pbMapa.Size.Height * y1 / ALTURA);
            PointF p2 = new PointF(pbMapa.Size.Width * x2 / LARGURA, pbMapa.Size.Height * y2 / ALTURA);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }

        private void DesenhaLinha(Graphics gfx, Cidade um, float x2, float y2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pbMapa.Size.Width * um.X / LARGURA, pbMapa.Size.Height * um.Y / ALTURA);
            PointF p2 = new PointF(pbMapa.Size.Width * x2 / LARGURA, pbMapa.Size.Height * y2 / ALTURA);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }

        private void DesenhaLinha(Graphics gfx, PointF p1, float x2, float y2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p2 = new PointF(pbMapa.Size.Width * x2 / LARGURA, pbMapa.Size.Height * y2 / ALTURA);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }
*/
