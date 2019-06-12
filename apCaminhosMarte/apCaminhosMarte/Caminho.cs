using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apCaminhosMarte;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe que armazena um caminho entre duas cidades
    class Caminho
    {
        //Lista de int para cada cidade percorrida, da origem ao destino
        protected List<int> rota;
        //Distância total percorrida
        protected int distanciaTotal;

        //Propriedade getter para rota
        public List<int> Rota { get => rota; }

        //Propriedade getter para distância total
        public int DistanciaTotal { get => distanciaTotal; }

        //Construtor com origem, inicializa distanciaTotal para 0 e armazena origem na rota
        public Caminho(int origem)
        {
            distanciaTotal = 0;            
            rota = new List<int>();
            rota.Add(origem);
        }

        //Construtor com lista de inteiros rota e inteiro distancia total
        public Caminho(List<int> rota, int distanciaTotal)
        {
            this.rota = rota;
            this.distanciaTotal = distanciaTotal;
        }

        //Método que adiciona uma nova cidade à rota, aumentando a distanciaTotal em dist
        //Joga exceção para cidade e dist inválidas
        public void AdicionarARota(int cidadeNova, int dist)
        {
            if (cidadeNova < 0)
                throw new Exception("Cidade inválida");
            if (dist < 0)
                throw new Exception("Distância inválida");
            distanciaTotal += dist;
            rota.Add(cidadeNova);
        }
    }
}
