using System;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Interface de uma fila genérica
    interface IQueue<T>
    {
        void Enfileirar(T elemento);
        T Retirar();
        T Inicio();
        T Fim();
        int Tamanho { get; }
        bool EstaVazia();
    }
}