using System;

namespace apCaminhosMarte
{
    interface IQueue<Dado>
    {
        void Enfileirar(Dado elemento); // inclui objeto “elemento”
        Dado Retirar(); // devolve objeto do início e o
                        // retira da fila
        Dado Inicio(); // devolve objeto do início
                       // sem retirá-lo da fila
        Dado Fim(); // devolve objeto do fim
                    // sem retirá-lo da fila
        int Tamanho { get; } // devolve número de elementos da fila
        bool EstaVazia(); // informa se a fila está vazia ou não
    }
}
