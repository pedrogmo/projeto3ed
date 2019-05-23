using System;

interface IQueue<Dado>
{
    void Enfileirar(Dado elemento);
    Dado Retirar();
    Dado Inicio { get; }
    Dado Fim { get; }
    int Tamanho { get; }
    bool EstaVazia();
}