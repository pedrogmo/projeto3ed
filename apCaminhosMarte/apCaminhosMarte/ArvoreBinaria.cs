using System;
using System.Collections.Generic;
using System.Text;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe árvore de busca binária para armazenamento de dados
    class ArvoreBinaria<T> where T : IComparable<T>
    {       
        //Nó de raíz da árvore
        protected NoArvore<T> raiz;

        //Quantidade de elementos armazenados
        protected int quantidade;

        //Propriedade getter de nó de raíz
        public NoArvore<T> Raiz { get => raiz; }

        //Propriedade getter de atributo quantidade
        public int Quantidade { get => quantidade; }

        //Construtor padrão da classe, que define a raíz como nula
        public ArvoreBinaria()
        {
            raiz = null;
        }

        //Construtor que inicializa a raíz com valor passado de parâmetro
        public ArvoreBinaria(T info)
        {
            raiz = new NoArvore<T>(info);
        }
     
        //Método que retorna true se a raíz da árvore for nula, false caso contrário
        public bool EstaVazia()
        {
            return raiz == null;
        }

        //Método de inserção de dado
        public void Inserir(T dado)
        {
            //Método interno recutsivo com ponteiros do novo nó e do nó anterior
            void Inserir(NoArvore<T> novo, NoArvore<T> anterior)
            {
                int comparacao = novo.Info.CompareTo(anterior.Info); //Compara-se o nó com o anterior
                if (comparacao < 0) //Se novo for menor
                {
                    if (anterior.Esquerdo == null) //se o Esquerdo do anterior for nulo, passa a ser o novo nó
                        anterior.Esquerdo = novo;
                    else
                        Inserir(novo, anterior.Esquerdo); //Se não, chama a função recursivamente para nó esquerdo de anterior
                }
                else if (comparacao > 0) //Se novo for maior, mesmo procedimento para nó da direita
                {
                    if (anterior.Direito == null)
                        anterior.Direito = novo;
                    else
                        Inserir(novo, anterior.Direito);
                }
            }
            NoArvore<T> novoNo = new NoArvore<T>(dado); //Instanciação de novoNo com dado passado
            if (EstaVazia()) //Se a árvore está vazia, raiz é o novoNo
                raiz = novoNo;
            else //Se não, chama função recursiva a partir da raíz
                Inserir(novoNo, raiz);
            ++quantidade; //Incremento da quantidade
        }
        
        //Função de exclusão de um info passado
        public void Excluir(T info)
        {
            //Método interno que atualiza valor de um nó, que é passado por refereência
            void Excluir(ref NoArvore<T> atual)
            {                
                NoArvore<T> atualAnt;
                if (atual != null)
                { 
                    if (atual.Info.CompareTo(info) > 0)
                    {
                        var e = atual.Esquerdo;
                        Excluir(ref e);
                    }
                    else if (atual.Info.CompareTo(info) < 0)
                    {
                        var d = atual.Direito;
                        Excluir(ref d);
                    }
                    else
                    {
                        atualAnt = atual; // nó a retirar
                        if (atual.Direito == null) //Sem filho direito
                            atual = atual.Esquerdo; //Nó atual passa a ser seu filho esquerdo, seja ele nulo ou não
                        else
                        if (atual.Esquerdo == null) //Sem filho esquerdo
                            atual = atual.Direito;  //Nó atual passa a ser seu filho direito, seja ele nulo ou não
                        else
                        { // pai de 2 filhos
                            var e = atual.Esquerdo;
                            Rearranja(ref e); //Chama função rearranja a partir do esquerdo
                            atualAnt = null; // libera o nó excluído
                        }
                    }
                }

                void Rearranja(ref NoArvore<T> aux)
                    //Procura o maior dos menores nós a partir do nó a ser excluído
                    //Põe o conteúdo do maior no lugar do nó anterior ao da exclusão
                {
                    if (aux.Direito != null)
                    {
                        var d = aux.Direito;
                        Rearranja(ref d); // Procura Maior
                    }
                    else
                    { // Guarda os dados do nó a excluir
                        atualAnt.Info = aux.Info; // troca conteúdo!
                        atualAnt = aux; // funciona com a passagem por referência
                        aux = aux.Esquerdo;
                    }
                }
            }
            Excluir(ref raiz); //Chama método de exclusão a partir ra raíz
            --quantidade; //Decrementa quantidade de elementos
        }        

        //Método que retorna o menor dos elementos
        public T Menor()
        {
            if (EstaVazia())
                return default(T);
            //Função recursiva que procura o último nó à esquerda de todos
            T MenorNo(NoArvore<T> atual)
            {
                if (atual.Esquerdo == null)
                    return atual.Info;
                return MenorNo(atual.Esquerdo);
            }
            return MenorNo(raiz); //Chama função começando pela raíz
        }

        //Método que retorna o maior dos elementos
        public T Maior()
        {
            if (EstaVazia())
                return default(T);
            //Função recursiva que procura último nó à direita de todos
            T MaiorNo(NoArvore<T> atual)
            {
                if (atual.Direito == null)
                    return atual.Info;
                return (MaiorNo(atual.Direito));
            }
            return MaiorNo(raiz); //Chama função interna começando pela raíz
        }

        public void PreOrdem(Action<T> metodo)
        {
            void PreOrdem(NoArvore<T> atual)
            {
                if (atual != null)
                {
                    metodo(atual.Info);
                    PreOrdem(atual.Esquerdo);
                    PreOrdem(atual.Direito);
                }
            }
            PreOrdem(raiz);
        }

        public void InOrdem(Action<T> metodo)
        {
            void InOrdem(NoArvore<T> atual)
            {
                if (atual != null)
                {
                    InOrdem(atual.Esquerdo);
                    metodo(atual.Info);
                    InOrdem(atual.Direito);
                }
            }
            InOrdem(raiz);
        }

        public void PosOrdem(Action<T> metodo)
        {
            void PosOrdem(NoArvore<T> atual)
            {
                if (atual != null)
                {
                    PosOrdem(atual.Esquerdo);
                    PosOrdem(atual.Direito);
                    metodo(atual.Info);
                }
            }
            PosOrdem(raiz);
        }

        public void PorNivel(Action<T> metodo)
        {
            Fila<NoArvore<T>> umaFila = new Fila<NoArvore<T>>();
            var noAtual = raiz;
            while (noAtual != null)
            {
                if (noAtual.Esquerdo != null)
                    umaFila.Enfileirar(noAtual.Esquerdo);
                if (noAtual.Direito != null)
                    umaFila.Enfileirar(noAtual.Direito);
                metodo(noAtual.Info);
                if (umaFila.EstaVazia())
                    noAtual = null;
                else
                    noAtual = umaFila.Retirar();
            }
        }

        public T Buscar(T dado)
        {
            var noArvore = BuscarNo(dado);
            if (noArvore != null)
                return noArvore.Info;
            return default(T);
        }

        protected NoArvore<T> BuscarNo(T dado)
        {
            NoArvore<T> atual = null;
            if (!EstaVazia())
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

        /*public int Tamanho
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
        }*/

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
            T EscreverAntecessor(NoArvore<T> atual, T dado, ref bool achou)
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
            }

            bool achou = false;
            EscreverAntecessor(raiz, dado, ref achou);
            if (!achou)
                Console.WriteLine("Dado não foi achado");
        }*/
    }
}