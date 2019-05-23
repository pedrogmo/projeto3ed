using System;
using System.Collections.Generic;
using System.Text;

namespace apArvore
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class ArvoreBinaria<X> where X : IComparable<X>
    {
        protected internal class No<X> where X : IComparable<X>
        {
            protected X info;
            protected No<X> esquerdo, direito;

            public No(X i, No<X> e, No<X> d)
            {
                Info = i;
                Esquerdo = e;
                Direito = d;
            }

            public X Info
            {
                get => info;
                set
                {
                    if (value == null)
                        throw new Exception("Informação inválida");
                    info = value;
                }
            }

            public No<X> Esquerdo
            {
                get => esquerdo;
                set => esquerdo = value;
            }

            public No<X> Direito
            {
                get => direito;
                set => direito = value;
            }

            public bool EhFolha()
            {
                return esquerdo == null && direito == null;
            }

            public static bool operator !=(No<X> um, No<X> outro)
            {
                return !(um == outro);
            }

            public static bool operator ==(No<X> um, No<X> outro)
            {
                if (um is null && outro is null)
                    return true;
                if (um is null || outro is null)
                    return false;
                return um.info.CompareTo(outro.info) == 0;
            }
        }

        protected No<X> raiz;

        public ArvoreBinaria()
        {
            raiz = null;
        }

        public bool EstaVazia
        {
            get => raiz == null;
        }

        public void Inserir(X dado)
        {
            No<X> novoNo = new No<X>(dado, null, null);
            if (EstaVazia)
                raiz = novoNo;
            else
                Inserir(novoNo, raiz);
        }

        protected void Inserir(No<X> novo, No<X> anterior)
        {
            int comparacao = novo.Info.CompareTo(anterior.Info);
            if (comparacao < 0)
            {
                if (anterior.Esquerdo == null)
                    anterior.Esquerdo = novo;
                else
                    Inserir(novo, anterior.Esquerdo);
            }
            else if (comparacao > 0)
            {
                if (anterior.Direito == null)
                    anterior.Direito = novo;
                else
                    Inserir(novo, anterior.Direito);
            }
        }

        public void Excluir(X dado)
        {
            if (EstaVazia)
                throw new Exception("Árvore vazia");
            Excluir(dado, raiz, raiz);
        }

        protected void Excluir(X info, No<X> atual, No<X> anterior)
        {
            int comparacao = info.CompareTo(atual.Info);
            int comparacao2 = atual.Info.CompareTo(anterior.Info);
            if (comparacao == 0) //achou nó
            {
                if (atual.EhFolha())
                {
                    if (comparacao2 > 0)
                        anterior.Direito = null;
                    else
                        anterior.Esquerdo = null;
                }
                else
                {
                    if (atual.Esquerdo != null && atual.Direito != null)
                        atual.Info = MaiorExcluir(atual, atual.Esquerdo);
                    else if (atual.Esquerdo != null)
                    {
                        if (comparacao2 > 0)
                            anterior.Direito = atual.Esquerdo;
                        else
                            anterior.Esquerdo = atual.Esquerdo;
                    }
                    else
                    {
                        if (comparacao2 > 0)
                            anterior.Direito = atual.Direito;
                        else
                            anterior.Esquerdo = atual.Direito;
                    }
                }
            }
            else if (comparacao > 0)
                Excluir(info, atual.Direito, atual);
            else
                Excluir(info, atual.Esquerdo, atual);
        }

        public X Menor()
        {
            if (EstaVazia)
                throw new Exception("Árvore vazia");
            return Menor(raiz);
        }
        public X Maior()
        {
            if (EstaVazia)
                throw new Exception("Árvore vazia");
            return Maior(raiz);
        }

        protected X Menor(No<X> atual)
        {
            if (atual.Esquerdo == null)
                return atual.Info;
            return Menor(atual.Esquerdo);
        }

        protected X Maior(No<X> atual)
        {
            if (atual.Direito == null)
                return atual.Info;
            return (Maior(atual.Direito));
        }

        protected X MaiorExcluir(No<X> anterior, No<X> atual)
        {
            if (atual.Direito == null)
            {
                anterior.Direito = atual.Esquerdo;
                return atual.Info;
            }
            return (MaiorExcluir(atual, atual.Direito));
        }


        public void PreOrdem(Action<X> callback)
        {
            if (EstaVazia)
                Console.WriteLine("Árvore vazia");
            else
                PreOrdem(raiz, callback);
        }

        protected void PreOrdem(No<X> atual, Action<X> callback)
        {
            if (atual != null)
            {
                callback(atual.Info);
                PreOrdem(atual.Esquerdo, callback);
                PreOrdem(atual.Direito, callback);
            }
        }

        public void InOrdem(Action<X> callback)
        {
            if (EstaVazia)
                Console.WriteLine("Árvore vazia");
            else
                InOrdem(raiz, callback);
        }

        protected void InOrdem(No<X> atual, Action<X> callback)
        {
            if (atual != null)
            {                
                InOrdem(atual.Esquerdo, callback);
                callback(atual.Info);
                InOrdem(atual.Direito, callback);
            }
        }

        public void PosOrdem(Action<X> callback)
        {
            if (EstaVazia)
                Console.WriteLine("Árvore vazia");
            else
                PosOrdem(raiz, callback);
        }

        protected void PosOrdem(No<X> atual, Action<X> callback)
        {
            if (atual != null)
            {
                PosOrdem(atual.Esquerdo, callback);
                PosOrdem(atual.Direito, callback);
                callback(atual.Info);
            }
        }

        public X Buscar(X dado)
        {
            return Buscar(dado, raiz);
        }

        protected X Buscar(X dado, No<X> atual)
        {
            if (atual == null)
                return default(X);
            if (dado.CompareTo(atual.Info) < 0)
                return Buscar(dado, atual.Esquerdo);
            else if (dado.CompareTo(atual.Info) > 0)
                return Buscar(dado, atual.Direito);
            return atual.Info; //aqui é igual
        }

        //exercício 1
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            ArvoreBinaria<X> a = (ArvoreBinaria<X>)obj;
            return ArvoreIgual(raiz, a.raiz);
        }

        protected bool ArvoreIgual(No<X> a, No<X> b)
        {
            if (a == null && b == null)
                return true;
            if (a != b)
                return false;
            return ArvoreIgual(a.Esquerdo, b.Esquerdo) && ArvoreIgual(a.Direito, b.Direito);
        }

        //exercício 2
        public int Quantidade()
        {
            return Contar(raiz);
        }

        protected int Contar(No<X> atual)
        {
            if (atual == null)
                return 0;
            //nó raíz é tratado previamente -> préordem
            return 1 + Contar(atual.Esquerdo) + Contar(atual.Direito);
        }

        //exercício 3
        public int QuantidadeFolhas()
        {
            return ContarFolhas(raiz);         
        }

        protected int ContarFolhas(No<X> atual)
        {
            if (atual == null)
                return 0;
            if (atual.EhFolha())
                return 1;
            return ContarFolhas(atual.Esquerdo) + ContarFolhas(atual.Direito);
        }

        //exercício 4
        public bool EstritamenteBinaria()
            //não tem nós unários
            //todo nó que não é folha tem dois filhos
        {
            if (EstaVazia)
                return false;
            return DoisFilhos(raiz);
        }

        protected bool DoisFilhos(No<X> atual)
        {
            if (atual.Esquerdo != null && atual.Direito != null)
                return DoisFilhos(atual.Esquerdo) && DoisFilhos(atual.Direito);
            if (atual.EhFolha())
                return true;
            return false;
        }

        //exercício 5
        public int Altura()
        {
            return ContarAltura(raiz);
        }

        protected int ContarAltura(No<X> atual)
        {
            //a altura de uma árvore é a maior dentre a altura da esquerda e da direita
            int alturaEsquerda, alturaDireita;
            if (atual == null)
                return 0;
            alturaEsquerda = ContarAltura(atual.Esquerdo);
            alturaDireita = ContarAltura(atual.Direito);
            if (alturaEsquerda >= alturaDireita)
                return 1 + alturaEsquerda;
            return 1 + alturaDireita;
        }

        //exercício 6
        public void EscreverEstrutura()
        {
            EscreverNo(raiz);
        }

        protected void EscreverNo(No<X> atual)
        {
            if (atual == null)
                Console.Write("()");
            else
            {
                Console.Write("(" + atual.Info + ":");
                EscreverNo(atual.Esquerdo);
                Console.Write(",");
                EscreverNo(atual.Direito);
                Console.Write(")");
            }
        }

        //exercício 7
        public void Espelhar() 
            //retira ordenação da árvore
        {
            Trocar(raiz);
        }

        protected void Trocar(No<X> noAtual)
        {
            No<X> auxiliar;
            if (noAtual != null)
            {
                //troca esquerdo com direito:
                auxiliar = noAtual.Esquerdo;
                noAtual.Esquerdo = noAtual.Direito;
                noAtual.Direito = auxiliar;

                //chama função para esquerdo e direito
                Trocar(noAtual.Esquerdo);
                Trocar(noAtual.Direito);
            }
        }

        //exercício 8
        public void EscrevePorNiveis()
        {
            FilaLista<No<X>> umaFila = new FilaLista<No<X>>();
            var noAtual = raiz;
            while (noAtual != null)
            {
                if (noAtual.Esquerdo != null)
                    umaFila.Enfileirar(noAtual.Esquerdo);
                if (noAtual.Direito != null)
                    umaFila.Enfileirar(noAtual.Direito);
                Console.WriteLine(noAtual.Info);
                if (umaFila.EstaVazia())
                    noAtual = null;
                else
                    noAtual = umaFila.Retirar();
            }
        }

        //exercício 9
        public int Largura()
        {
            int[] nosPorNivel = new int[1000];
            for (int i = 0; i < 1000; i++)
                nosPorNivel[i] = 0;
            ContarLargura(raiz, ref nosPorNivel, 0);
            return Metodos.Maior(nosPorNivel);
        }

        protected void ContarLargura(No<X> atual, ref int[] v, int h)
        {
            if (atual != null)
            {
                ++v[h];
                ContarLargura(atual.Esquerdo, ref v, h + 1);
                ContarLargura(atual.Direito, ref v, h + 1);
            }
        }

        //exercício 10
        public void EscreverAntecessores(X dado)
        {
            bool achou = false;
            EscreverAntecessor(raiz, dado, ref achou);
            if (!achou)
                Console.WriteLine("Dado não foi achado");
        }

        protected void EscreverAntecessor(No<X> atual, X dado, ref bool achou)
        {
            if (atual != null)
            {
                if (!achou)
                    EscreverAntecessor(atual.Esquerdo, dado, ref achou);
                if (!achou)
                    EscreverAntecessor(atual.Direito, dado, ref achou);
                if (atual.Info.CompareTo(dado) == 0)
                    achou = true;
                if (achou)
                    Console.WriteLine(atual.Info);
            }
        }
    }
}
