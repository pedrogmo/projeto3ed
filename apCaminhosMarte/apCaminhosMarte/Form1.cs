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

        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Buscar caminhos entre cidades selecionadas");
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
                matriz[l, c] = int.Parse(linha.Substring(2 * Cidade.TAMANHO_CODIGO, 5).Trim());
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
        }

        private void DesenhaCidade(Cidade c, Graphics gfx)
        {
            int x = pbMapa.Size.Width * c.X / 4096;
            int y = pbMapa.Size.Height * c.Y / 2048;
            SolidBrush corCidade = new SolidBrush(Color.Black);
            gfx.FillEllipse(corCidade, x , y , 10, 10);
            gfx.DrawString(c.Nome, new Font("Century Gothic", 10, FontStyle.Bold), corCidade, new Point(x - 10, y + 10));         
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            int x = (int)tpArvore.Width / 2;
            int y = 0;
            double angulo = Math.PI / 2;
            double incremento = Math.PI / 2.5;
            double comprimento = 450;
            Graphics gfx = e.Graphics;
            arvore.PreOrdem((Cidade c) => {
                DesenhaNo(c, x, y, angulo, incremento, comprimento, gfx);
                angulo += incremento;
                incremento *= 0.60;
                comprimento *= 0.80;
                x = (int)Math.Round(x + Math.Cos(angulo) * comprimento);                
                y = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (c == arvore.Raiz)
                    y = 25;
            });
        }

        private void DesenhaNo(Cidade atual,
                           int x, int y, double angulo, double incremento,
                           double comprimento, Graphics g)
        {
            int xf, yf;
            Pen caneta = new Pen(Color.Red);
            xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
            yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
            if (atual == arvore.Raiz)
                yf = 25;
            g.DrawLine(caneta, x, y, xf, yf);
            SolidBrush preenchimento = new SolidBrush(Color.Blue);
            g.FillEllipse(preenchimento, xf - 15, yf - 15, 30, 30);
            g.DrawString(Convert.ToString(atual.Codigo), new Font("Century Gothic", 12),
                            new SolidBrush(Color.Yellow), xf - 15, yf - 10);
        }
    }
}
