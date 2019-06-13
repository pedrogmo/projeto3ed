using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe usada para armazenar matriz de adjacências entre cidades e que realiza a operação de busca
    class Grafo
    {
        //Matriz de inteiros, na qual há valores maiores que 0 se a cidade representada pela linha tem ligação com a cidade da coluna
        protected int[,] matriz;
        //Quantidade de elementos
        protected int quantidade;

        //Construtor da classe com matriz de inteiros como parâmetro
        //Joga exceção de o número de linhas da matriz for diferente do número de colunas
        public Grafo(int[,] m)
        {
            matriz = m;
            double raiz = Math.Sqrt(matriz.Length); //Raíz deve ser inteira para uma matriz com quantidade de linhas igual à quantidade de colunas
            if (raiz - (int)raiz > 0)
                throw new Exception("Matriz inválida");
            quantidade = (int) raiz;
        }

        //Propriedade que retorna a quantidade de vértices do grafo
        public int Quantidade { get => quantidade; }

        //Método que retorna a distância entre duas cidades da matriz
        //Lança exceção se as cidades estiverem fora do intervalo da matriz
        public int DistanciaEntre(int l, int c)
        {
            if (l < 0 || l >= quantidade)
                throw new Exception("Linha inválida");
            if (c < 0 || c >= quantidade)
                throw new Exception("Coluna inválida");
            return matriz[l, c];
        }

        //Método que retorna uma lista de caminhos de uma cidade de origem a uma cidade de destino
        //Lança exceção se as cidades estiverem fora do intervalo da matriz
        public List<Caminho> Caminhos(int origem, int destino)
        {
            if (origem < 0 || origem >= quantidade)
                throw new Exception("Origem inválida");
            if (destino < 0 || destino >= quantidade)
                throw new Exception("Destino inválido");

            List<Caminho> ret = new List<Caminho>(); //Lista que será retornada
            Pilha<int> pilha = new Pilha<int>(); //Pilha que armazenará cada passo dado no mapa
            bool fim = false;
            int cidadeAtual = origem, ind = 0; //cidadeAtual será usada para percorrer as cidades
            //ind será usado para procurar adjacência a partir de cidadeAtual

            bool[] jaPassou = new bool[quantidade]; //Vetor de boolean para impedir que volte para alguma cidade já passou
            for (int i = 0; i < quantidade; ++i)
                jaPassou[i] = false;

            //Método interno que volta para uma cidadeAnterior em busca de alguma possibilidade de sáida
            void Backtracking()
            {
                if (pilha.EstaVazia()) //Se a pilha está vazia, não há para onde ir e, portanto, a busca acabou
                    fim = true;
                else
                {
                    jaPassou[cidadeAtual] = false; //cidadeAtual é removida da rota atual
                    ind = cidadeAtual + 1; //O índice passa a ser o próximo valor de cidadeAtual, de modo que se procure nova possibilidade
                    cidadeAtual = pilha.Desempilhar(); //cidadeAtual passa a ser a anterior
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