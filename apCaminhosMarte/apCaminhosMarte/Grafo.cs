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
            double raiz = Math.Sqrt(matriz.Length);
            if (raiz - (int)raiz > 0)
                throw new Exception("Matriz inválida");
            quantidade = (int) raiz;

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
            if (origem < 0 || origem >= quantidade)
                throw new Exception("Origem inválida");
            if (destino < 0 || destino >= quantidade)
                throw new Exception("Destino inválido");

            List<Caminho> ret = new List<Caminho>();
            Pilha<int> pilha = new Pilha<int>();
            bool fim = false;
            int cidadeAtual = origem, ind = 0;

            bool[] jaPassou = new bool[quantidade];
            for (int i = 0; i < quantidade; ++i)
                jaPassou[i] = false;

            void Backtracking()
            {
                if (pilha.EstaVazia())
                    fim = true;
                else
                {
                    jaPassou[cidadeAtual] = false;
                    ind = cidadeAtual + 1;
                    cidadeAtual = pilha.Desempilhar();
                }
            }

            while (!fim)
            {
                if (cidadeAtual == destino)
                {
                    //Achou um caminho
                    var inversa = new Pilha<int>();
                    inversa.Empilhar(cidadeAtual);
                    while (!pilha.EstaVazia())
                        inversa.Empilhar(pilha.Desempilhar());
                    int origemDaInversa = inversa.Desempilhar();
                    pilha.Empilhar(origemDaInversa);
                    var caminho = new Caminho(origemDaInversa);
                    while (!inversa.EstaVazia())
                    {
                        int codCidadeSaida = inversa.Desempilhar();
                        caminho.AdicionarARota(codCidadeSaida, matriz[caminho.Rota[caminho.Rota.Count-1] , codCidadeSaida]);
                        if (codCidadeSaida != destino)
                            pilha.Empilhar(codCidadeSaida);
                    }
                    ret.Add(caminho);
                    Backtracking();
                }
                jaPassou[cidadeAtual] = true;
                bool haSaida = false;
                while (!haSaida && ind < quantidade)
                {
                    if (matriz[cidadeAtual, ind] != 0 && !jaPassou[ind])
                    //Achou cidade para sair
                    {
                        if (cidadeAtual == origem)
                        {
                            if (pilha.EstaVazia())
                                pilha.Empilhar(cidadeAtual);
                        }
                        else
                            pilha.Empilhar(cidadeAtual);
                        cidadeAtual = ind;
                        haSaida = true;
                    }
                    ++ind;
                }
                if (!haSaida)
                    //Regressivo
                    Backtracking();
                else
                    ind = 0;
            }
            return ret;
        }
    }
}