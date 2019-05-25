using System;
using System.Collections.Generic;
using System.Text;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class ArvoreBinaria<T> where T : IComparable<T>
    {       
        protected NoArvore<T> raiz;
        public T Raiz { get => raiz.Info; }

        public ArvoreBinaria()
        {
            raiz = null;
        }
        public ArvoreBinaria(T info)
        {
            raiz = new NoArvore<T>(info);
        }
        public ArvoreBinaria(ArvoreBinaria<T> outra)
        {
            raiz = outra.raiz;
        }
        public ArvoreBinaria(T[] v)
        {
            foreach (T t in v)
                Inserir(t);
        }
        public ArvoreBinaria(IEnumerable<T> l)
        {
            foreach (T t in l)
                Inserir(t);
        }

        public bool EstaVazia
        {
            get => raiz == null;
        }

        public void Inserir(T dado)
        {
            void Inserir(NoArvore<T> novo, NoArvore<T> anterior)
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
            NoArvore<T> novoNo = new NoArvore<T>(dado);
            if (EstaVazia)
                raiz = novoNo;
            else
                Inserir(novoNo, raiz);
        }
        /*public void Excluir(T dado)
        {
            if (EstaVazia)
                throw new Exception("Árvore vazia");
            NoArvore<T> BuscarNoAntecessor(T dadoAtual)
            {
                NoArvore<T> noAtual = null, anterior = null;
                if (!EstaVazia)
                {
                    noAtual = raiz;
                    while (noAtual != null && noAtual.Info.CompareTo(dado) != 0)
                    {
                        if (dado.CompareTo(noAtual.Info) < 0)
                        {
                            anterior = noAtual;
                            noAtual = noAtual.Esquerdo;
                        }
                        else if (dado.CompareTo(noAtual.Info) > 0)
                        {
                            anterior = noAtual;
                            noAtual = noAtual.Direito;
                        }
                        else
                            return anterior;
                    }
                }
                return anterior;
            }
            NoArvore<T> antecessor = BuscarNoAntecessor(dado);
            if (antecessor != null)
            {
                NoArvore<T> atual = null;
                if (antecessor.Esquerdo.Info.CompareTo(dado) == 0)
                    atual = antecessor.Esquerdo;
                else
                    atual = antecessor.Direito;
                if (atual.Esquerdo == null || atual.Direito == null)
                {
                    if (atual.Info.CompareTo(antecessor.Info) > 0)
                    {
                        if (atual.Esquerdo != null)
                            antecessor.Direito = atual.Esquerdo;
                        else if (atual.Direito != null)
                            antecessor.Direito = atual.Direito;
                        else
                            antecessor.Direito = null;
                    }
                    else
                    {
                        if (atual.Esquerdo != null)
                            antecessor.Esquerdo = atual.Esquerdo;
                        else if (atual.Direito != null)
                            antecessor.Esquerdo = atual.Direito;
                        else
                            antecessor.Esquerdo = null;
                    }
                }
                else
                {
                    NoArvore<T> aux = antecessor.Esquerdo;
                    while (aux != null && aux.Direito.Direito != null) aux = aux.Direito;
                    T info = aux.Direito.Info;
                    aux.Direito = null;
                    antecessor.Info = info;
                }
            }
        }*/

        public void Excluir(T info)
        {           
            void Excluir(NoArvore<T> atual, NoArvore<T> anterior)
            {
                T MaiorExcluir(NoArvore<T> ant, NoArvore<T> atu)
                {
                    if (atu.Direito == null)
                    {
                        ant.Direito = atu.Esquerdo;
                        return atu.Info;
                    }
                    return (MaiorExcluir(atu, atu.Direito));
                }

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
                    Excluir(atual.Direito, atual);
                else
                    Excluir(atual.Esquerdo, atual);
            }
            Excluir(raiz, raiz);
        }        

        public T Menor()
        {
            if (EstaVazia)
                throw new Exception("Árvore vazia");
            T MenorNo(NoArvore<T> atual)
            {
                if (atual.Esquerdo == null)
                    return atual.Info;
                return MenorNo(atual.Esquerdo);
            }
            return MenorNo(raiz);
        }
        public T Maior()
        {
            if (EstaVazia)
                throw new Exception("Árvore vazia");
            T MaiorNo(NoArvore<T> atual)
            {
                if (atual.Direito == null)
                    return atual.Info;
                return (MaiorNo(atual.Direito));
            }
            return MaiorNo(raiz);
        }

        public void PreOrdem(Action<T> callback)
        {
            void PreOrdem(NoArvore<T> atual)
            {
                if (atual != null)
                {
                    callback(atual.Info);
                    PreOrdem(atual.Esquerdo);
                    PreOrdem(atual.Direito);
                }
            }
            PreOrdem(raiz);
        }
        public void InOrdem(Action<T> callback)
        {
            void InOrdem(NoArvore<T> atual)
            {
                if (atual != null)
                {
                    InOrdem(atual.Esquerdo);
                    callback(atual.Info);
                    InOrdem(atual.Direito);
                }
            }
            InOrdem(raiz);
        }
        public void PosOrdem(Action<T> callback)
        {
            void PosOrdem(NoArvore<T> atual)
            {
                if (atual != null)
                {
                    PosOrdem(atual.Esquerdo);
                    PosOrdem(atual.Direito);
                    callback(atual.Info);
                }
            }
            PosOrdem(raiz);
        }
        public void PorNivel(Action<T> callback)
        {
            Fila<NoArvore<T>> umaFila = new Fila<NoArvore<T>>();
            var noAtual = raiz;
            while (noAtual != null)
            {
                if (noAtual.Esquerdo != null)
                    umaFila.Enfileirar(noAtual.Esquerdo);
                if (noAtual.Direito != null)
                    umaFila.Enfileirar(noAtual.Direito);
                callback(noAtual.Info);
                if (umaFila.EstaVazia())
                    noAtual = null;
                else
                    noAtual = umaFila.Retirar();
            }
        }

        public T Buscar(T dado)
        {
            var NoArvore = BuscarNo(dado);
            if (NoArvore != null)
                return NoArvore.Info;
            return default(T);
        }
        protected NoArvore<T> BuscarNo(T dado)
        {
            NoArvore<T> atual = null;
            if (!EstaVazia)
            {
                atual = raiz;
                while (atual != null)
                {
                    if (dado.CompareTo(atual.Info) < 0)
                        atual = atual.Esquerdo;
                    else if (dado.CompareTo(atual.Info) > 0)
                        atual = atual.Direito;
                    else
                        return atual;
                }
            }
            return atual;
        }
        public int Tamanho
        {
            get
            {
                int Contar(NoArvore<T> atual)
                {
                    if (atual == null)
                        return 0;
                    //nó raíz é tratado previamente -> préordem
                    return 1 + Contar(atual.Esquerdo) + Contar(atual.Direito);
                }
                return Contar(raiz);
            }
        }
        public int QuantidadeFolhas
        {
            get
            {
                int ContarFolhas(NoArvore<T> atual)
                {
                    if (atual == null)
                        return 0;
                    if (atual.EhFolha())
                        return 1;
                    return ContarFolhas(atual.Esquerdo) + ContarFolhas(atual.Direito);
                }
                return ContarFolhas(raiz);
            }
        }
        public int Altura
        {
            get
            {
                int ContarAltura(NoArvore<T> atual)
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
                return ContarAltura(raiz);
            }
        }
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            ArvoreBinaria<T> arvore = obj as ArvoreBinaria<T>;
            bool ArvoreIgual(NoArvore<T> a, NoArvore<T> b)
            {
                if (a == null && b == null)
                    return true;
                if (a != b)
                    return false;
                return ArvoreIgual(a.Esquerdo, b.Esquerdo) && ArvoreIgual(a.Direito, b.Direito);
            }
            return ArvoreIgual(raiz, arvore.raiz);
        }
        public override string ToString()
        {
            string EscreverNo(NoArvore<T> atual)
            {
                if (atual == null)
                    return "";
                else
                    return $"({EscreverNo(atual.Esquerdo)}){atual.Info.ToString()}({EscreverNo(atual.Direito)})";
            }
            return EscreverNo(raiz);
        }
        /*protected bool DoisFilhos(NoArvore<T> atual)
        {
           if (atual.Esquerdo != null && atual.Direito != null)
               return DoisFilhos(atual.Esquerdo) && DoisFilhos(atual.Direito);
           if (atual.EhFolha())
               return true;
           return false;
        }
        public void EscreverAntecessores(T dado)
        {
           bool achou = false;
           EscreverAntecessor(raiz, dado, ref achou);
           if (!achou)
               Console.WriteLine("Dado não foi achado");
        }
        protected T EscreverAntecessor(NoArvore<T> atual, T dado, ref bool achou)
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
                   return atual.Info;
           }
           return default(T);
        }*/
    }
}