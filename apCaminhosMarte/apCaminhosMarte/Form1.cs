﻿using System;
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

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    public partial class Form1 : Form
    {
        ArvoreBinaria<Cidade> arvore;
        Grafo grafo;
        Color corNo = Color.Blue;
        Color corLinhaArvore = Color.Red;
        Color corLinhaCidade = Color.DarkBlue;
        Color corCidade = Color.Black;
        const int TAMANHO_NO = 30;
        const int TAMANHO_CIDADE = 10;

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
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arvore = new ArvoreBinaria<Cidade>();
            var leitorCidades = new StreamReader("cidades.txt", Encoding.UTF7);
            int qtdCidades = 0;
            while (!leitorCidades.EndOfStream)
            {
                arvore.Inserir(new Cidade(leitorCidades.ReadLine()));
                ++qtdCidades;
            }
            leitorCidades.Close();
            int[,] matriz = new int[qtdCidades, qtdCidades];
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
            arvore.InOrdem((Cidade c) => {
                DesenhaCidade(c, gfx);
            });
            for (int l = 0; l < arvore.Tamanho; ++l)
                for (int c = 0; c < arvore.Tamanho; ++c)
                {
                    int dist = grafo.DistanciaEntre(l, c);
                    if (dist != 0)
                        DesenhaLinha(l,c,dist,gfx);
                }
        }

        private void DesenhaLinha(int cod1, int cod2, int dist, Graphics gfx)
        {
            Cidade um = arvore.Buscar(new Cidade(cod1));
            Cidade dois = arvore.Buscar(new Cidade(cod2));
            int x1 = pbMapa.Size.Width * um.X / 4096;
            int y1 = pbMapa.Size.Height * um.Y / 2048;
            int x2 = pbMapa.Size.Width * dois.X / 4096;
            int y2 = pbMapa.Size.Height * dois.Y / 2048;
            gfx.DrawLine(new Pen(corLinhaCidade), x1, y1, x2, y2);
        }

        private void DesenhaCidade(Cidade c, Graphics gfx)
        {
            int x = pbMapa.Size.Width * c.X / 4096 - TAMANHO_CIDADE / 2;
            int y = pbMapa.Size.Height * c.Y / 2048 - TAMANHO_CIDADE / 2;
            gfx.FillEllipse(new SolidBrush(corCidade), x , y , TAMANHO_CIDADE, TAMANHO_CIDADE);
            gfx.DrawString(c.Nome, new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(corCidade), new Point(x - 10, y + 10));         
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            desenhaArvore(true, arvore.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 2.5, 450, gfx);
        }

        private void desenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,
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
                desenhaArvore(false, esq, xf, yf, Math.PI / 2 + incremento,
                                                 incremento * 0.60, comprimento * 0.8, g);
                var dir = raiz.Direito;
                desenhaArvore(false, dir, xf, yf, Math.PI / 2 - incremento,
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
