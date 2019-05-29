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
            var leitorCidades = new StreamReader("cidades.txt");
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

        }
    }
}
