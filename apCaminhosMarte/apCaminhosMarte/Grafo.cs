using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class Grafo
    {
        protected int[,] matriz;
        protected int quantidade;

        public Grafo(int[,] m)
        {
            matriz = m;
            quantidade = (int) Math.Sqrt(matriz.Length);
        }

        public int Quantidade { get => quantidade; }

        public int DistanciaEntre(int l, int c)
        {
            if (l < 0 || l >= quantidade)
                throw new Exception("Linha inválida");
            if (c < 0 || c >= quantidade)
                throw new Exception("Coluna inválida");
            return matriz[l, c];
        }

        public List<Caminho> Caminhos(int origem, int destino)
        {
            List<Caminho> ret = new List<Caminho>();
            Pilha<int> pilha = new Pilha<int>();
            int cidadeAtual = origem;
            bool[] jaPassou = new bool[quantidade];
            for (int i = 0; i < quantidade; ++i)
                jaPassou[i] = false;
            bool fim = false;            
            while (!fim)
            {
                if (matriz[cidadeAtual, destino] != 0)
                {
                    pilha.Empilhar(cidadeAtual);
                    pilha.Empilhar(destino);
                    /*
                     * Achou um caminho, fazer algo
                    */
                    var inversa = new Pilha<int>();
                    var caminho = new Caminho(origem, destino);
                    while (!pilha.EstaVazia())
                        inversa.Empilhar(pilha.Desempilhar());
                    int cidadeAnterior = -1;
                    while (!inversa.EstaVazia())
                    {
                        int atual = inversa.Desempilhar();
                        pilha.Empilhar(atual);
                        if (cidadeAnterior != -1)
                            caminho.AdicionarARota(cidadeAtual, matriz[atual, cidadeAnterior]);
                        cidadeAnterior = atual;
                    }
                    ret.Add(caminho);
                }

                else
                {
                    jaPassou[cidadeAtual] = true;
                    bool haParaOndeIr = false;
                    int saida = 0;
                    while (saida < quantidade && !haParaOndeIr)
                        if (matriz[cidadeAtual, saida] != 0 && !jaPassou[saida])
                            cidadeAtual = saida;
                    if (!haParaOndeIr)
                    {
                        if (pilha.EstaVazia())
                            fim = true;
                        else
                            cidadeAtual = pilha.Desempilhar();
                    }
                }
            }
            return ret;
        }
    }
}
