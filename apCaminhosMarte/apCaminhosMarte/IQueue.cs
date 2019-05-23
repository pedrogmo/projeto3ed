using System;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    interface IQueue<Dado>
    {
        void Enfileirar(Dado elemento);
        Dado Retirar();
        Dado Inicio { get; }
        Dado Fim { get; }
        int Tamanho { get; }
        bool EstaVazia();
    }
}