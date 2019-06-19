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

        //Função que encontra o menor nó a partir de uma determinada posição e o exclui, retornando seu valor
        protected T MaiorExcluir(NoArvore<T> anterior, NoArvore<T> atual)
        {
            if (atual.Direito == null)
            {
                anterior.Direito = atual.Esquerdo;
                return atual.Info;
            }
            return (MaiorExcluir(atual, atual.Direito));
        }

        //Função de exclusão de um info passado
        public void Excluir(T dado)
        {
            void Excluir(NoArvore<T> atual, NoArvore<T> anterior)
            {
                int comparacao = dado.CompareTo(atual.Info); //Comparação entre dado e o ó atual
                int comparacao2 = atual.Info.CompareTo(anterior.Info); //Comparação entre nó atual e seu antecessor
                if (comparacao == 0) //Nó atual deve ser excluído
                {
                    if (atual.Esquerdo != null && atual.Direito != null) //Se o nó a ser excluído tem dois filhos, troca-se o seu valor com o maior valor do nó à esquerda de atual
                        atual.Info = MaiorExcluir(atual, atual.Esquerdo);
                    else if (atual.Esquerdo != null) //Se há filho à esquerda, antecessor apontará para ele
                    {
                        if (comparacao2 > 0)
                            anterior.Direito = atual.Esquerdo;
                        else
                            anterior.Esquerdo = atual.Esquerdo;
                    }
                    else //Se há (ou não) filho à direita, antecessor apontará para ele
                    {
                        if (comparacao2 > 0)
                            anterior.Direito = atual.Direito;
                        else
                            anterior.Esquerdo = atual.Direito;
                    }
                }
                else if (comparacao > 0)
                    Excluir(atual.Direito, atual); //repetir para nó à direita
                else
                    Excluir(atual.Esquerdo, atual); //repetir para nó à esquerda
            }
            Excluir(raiz, raiz); //começa da raíz
            --quantidade;
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

        //Método que executa um método passado por parâmetro em todos os valores da árvore,
        //na ordem Raiz-Esquerdo-Direito
        public void PreOrdem(Action<T> metodo)
        {
            //Método interno recursivo
            void PreOrdem(NoArvore<T> atual)
            {
                if (atual != null)
                {
                    metodo(atual.Info); //Invocação do Action passado como parâmetro
                    PreOrdem(atual.Esquerdo);
                    PreOrdem(atual.Direito);
                }
            }
            PreOrdem(raiz); //Começa pela raíz
        }

        //Método que executa um método passado por parâmetro em todos os valores da árvore,
        //na ordem Esquerdo-Raíz-Direito
        public void InOrdem(Action<T> metodo)
        {
            //Método interno recursivo
            void InOrdem(NoArvore<T> atual)
            {
                if (atual != null)
                {
                    InOrdem(atual.Esquerdo);
                    metodo(atual.Info); //Invocação do Action passado como parâmetro
                    InOrdem(atual.Direito);
                }
            }
            InOrdem(raiz); //Começa pela raíz
        }

        //Método que executa um método passado por parâmetro em todos os valores da árvore,
        //na ordem Esquerdo-Direito-Raíz
        public void PosOrdem(Action<T> metodo)
        {
            //Método interno recursivo
            void PosOrdem(NoArvore<T> atual)
            {
                if (atual != null)
                {
                    PosOrdem(atual.Esquerdo);
                    PosOrdem(atual.Direito);
                    metodo(atual.Info); //Invocação do Action passado como parâmetro
                }
            }
            PosOrdem(raiz); //Começa pela raíz
        }

        //Realiza a busca em largura na árvore executando um método a cada nó
        public void PorNivel(Action<T> metodo)
        {
            Fila<NoArvore<T>> umaFila = new Fila<NoArvore<T>>(); //Fila para armazenar próximos nós
            var noAtual = raiz; //nó para percorrer árvore
            while (noAtual != null)
            {
                //São enfileirados os nós à esquerda e à direita do nó atual se eles não forem nulos
                if (noAtual.Esquerdo != null)
                    umaFila.Enfileirar(noAtual.Esquerdo);
                if (noAtual.Direito != null)
                    umaFila.Enfileirar(noAtual.Direito);
                metodo(noAtual.Info); //Execeução do Action no nó atual
                if (umaFila.EstaVazia())
                    noAtual = null; //Se a fila está vazia, fim da execução
                else
                    noAtual = umaFila.Retirar(); //Se não, nó atual passa a ser o primeiro da fila
            }
        }

        //Busca e retorna um elemento a partir de um modelo passado
        public T Buscar(T dado)
        {
            var noArvore = BuscarNo(dado); //Busca-se o nó correspondente ao valor
            if (noArvore != null) //Se nó não for nulo, retorna-se o seu valor 
                return noArvore.Info;
            return default(T); //Se o nó for nulo, é retornado um valor default da classe
        }

        //Método protegido que retorna nó correspondente de um dado passado
        protected NoArvore<T> BuscarNo(T dado)
        {
            NoArvore<T> atual = null;
            if (!EstaVazia())
            {
                atual = raiz; //Começa pela raíz
                while (atual != null) //Enquanto atual não é nulo, percorre árvore para esquerda ou para a direita dependendo da comparação
                {
                    if (dado.CompareTo(atual.Info) < 0)
                        atual = atual.Esquerdo;
                    else if (dado.CompareTo(atual.Info) > 0)
                        atual = atual.Direito;
                    else
                        return atual; //Se as chaves forem iguais, retorna o nó
                }
            }
            return atual; //Se sair do while, não achou, retornaatual, que é nulo
        }        

        //Retorna quantidade de folhas da árvore
        public int QuantidadeFolhas()
        {
            //Método recursivo de contagem
            int ContarFolhas(NoArvore<T> atual)
            {
                if (atual == null) //Retorna 0 se atual for nulo
                    return 0;
                if (atual.EhFolha()) //Retorna 1 se atual for folha
                    return 1;
                return ContarFolhas(atual.Esquerdo) + ContarFolhas(atual.Direito); //Retorna quantidade de folhas à esquerda + quantidade à direita
            }
            return ContarFolhas(raiz); //Retorna quantidade de folhas a partir da raíz
        }

        //Método que retorna altura da árvore
        public int Altura()
        {
            //Função recursiva
            int ContarAltura(NoArvore<T> atual)
            {
                //a altura de uma árvore é a maior dentre a altura da esquerda e da direita
                int alturaEsquerda, alturaDireita;
                if (atual == null)
                    return 0;
                alturaEsquerda = ContarAltura(atual.Esquerdo); //Calcula altura do nó à esquerda
                alturaDireita = ContarAltura(atual.Direito); //Calcula altura do nó à direita
                if (alturaEsquerda >= alturaDireita) //Retorna a maior das alturas entre esquerda e direita + 1
                    return 1 + alturaEsquerda;
                return 1 + alturaDireita;
            }
            return ContarAltura(raiz); //Altura a partir da raíz
        }

        public override bool Equals(object obj)
        {
            if (this == obj) //Se o ponteiro dos objetos é o mesmo, retorna true
                return true; 
            if (obj == null) //Se obj for nulo, retorna false
                return false;
            ArvoreBinaria<T> arvore = obj as ArvoreBinaria<T>; //Type-cast de obj para arvore
            //Método bool recursivo que compara todos os nós das árvore
            bool ArvoreIgual(NoArvore<T> a, NoArvore<T> b) 
            {
                if (a == null && b == null) //Se ambos forem nulos, retorna true
                    return true;
                if ((a == null) != (b == null)) //Se um deles for nulo, retorna false
                    return false;
                if (a.Info.CompareTo(b.Info) != 0) //Se tiverem chaves iguais, retorna false
                    return false;
                //Se forem iguais, repetir comparação para os filhos
                return ArvoreIgual(a.Esquerdo, b.Esquerdo) && ArvoreIgual(a.Direito, b.Direito);
            }
            return ArvoreIgual(raiz, arvore.raiz); //Comparação das árvores a partir da raíz
        }

        //Método que retorna string compactada com todos os nós
        public override string ToString()
        {
            //Função recursiva que escreve nós na estrutura '(esquerdo)atual(direito)'
            string EscreverNo(NoArvore<T> atual)
            {
                if (atual == null)
                    return "";
                else
                    return $"({EscreverNo(atual.Esquerdo)}){atual.Info.ToString()}({EscreverNo(atual.Direito)})";
            }
            return EscreverNo(raiz); //Escreve nós a partir da raíz
        }
    }
}