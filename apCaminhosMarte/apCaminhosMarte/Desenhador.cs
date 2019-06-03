using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    class Desenhador
    {
        #region Atributos e Constantes
        PictureBox pb;
        Graphics gfx;
        ArvoreBinaria<Cidade> arvore;
        Bitmap gif;
        Color corNo = Color.Blue;
        Color corLinhaArvore = Color.Red;
        Color corLinhaCidade = Color.FromArgb(51, 77, 201);
        Color corCidade = Color.Black;
        Color corLinhaCaminho = Color.Red;
        const int DIAMETRO_NO = 30;
        const int DIAMETRO_CIDADE = 10;
        int[] caminhoAtual;
        #endregion

        public Graphics Gfx { set => gfx = value; }
        public int[] CaminhoAtual { set => caminhoAtual = value; }
        public Desenhador(ref PictureBox p, ArvoreBinaria<Cidade> a)
        {
            gif = new Bitmap("../../Imagens/cidadeDestino.gif");
            ImageAnimator.Animate(gif, DrawFrame);
            pb = p;
            arvore = a;
        }
        public void DesenhaLinha(int cod1, int cod2)
        {
            Cidade um = arvore.Buscar(new Cidade(cod1));
            Cidade dois = arvore.Buscar(new Cidade(cod2));
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            Point p1 = new Point(pb.Size.Width * um.X / 4096, pb.Size.Height * um.Y / 2048);
            Point p2 = new Point(pb.Size.Width * dois.X / 4096, pb.Size.Height * dois.Y / 2048);
            if (um.Nome == "Gondor" && (dois.Nome == "Arrakeen" || dois.Nome == "Senzeni Na"))
            {
                gfx.DrawLine(caneta, p1.X, p1.Y, pb.Size.Width - 1, p2.Y);
                caneta.CustomEndCap = bigArrow;
                gfx.DrawLine(caneta, 0, p2.Y, p2.X, p2.Y);
            }
            else if (dois.Nome == "Gondor" && (um.Nome == "Arrakeen" || um.Nome == "Senzeni Na"))
            {
                gfx.DrawLine(caneta, 0, p1.Y, p1.X, p1.Y);
                caneta.CustomEndCap = bigArrow;
                gfx.DrawLine(caneta, pb.Size.Width - 1, p1.Y, p2.X, p2.Y);
            }
            else
            {
                caneta.CustomEndCap = bigArrow;
                gfx.DrawLine(caneta, p1, p2);
            }
        }
        public void DesenhaCidade(Cidade c, bool origem, bool destino)
        {
            int x = pb.Size.Width * c.X / 4096 - DIAMETRO_CIDADE / 2;
            int y = pb.Size.Height * c.Y / 2048 - DIAMETRO_CIDADE / 2;
            gfx.FillEllipse(new SolidBrush(corCidade), x, y, DIAMETRO_CIDADE, DIAMETRO_CIDADE);
            gfx.DrawString(c.Nome, new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(corCidade), new Point(x - 10, y + 10));
            if (origem)
                gfx.DrawImage(Image.FromFile("../../Imagens/cidadeOrigem.png"), x - DIAMETRO_CIDADE * 1.5f, y - DIAMETRO_CIDADE * 3.5f, DIAMETRO_CIDADE * 4, DIAMETRO_CIDADE * 4);
            if (destino)
            {
                ImageAnimator.UpdateFrames(gif);
                using (Bitmap bmp = new Bitmap(gif.Width, gif.Height))
                {
                    using (Graphics gr = Graphics.FromImage(bmp))
                    {
                        gr.DrawImage(gif, 0, 0);
                    }
                    gfx.DrawImage(bmp, x - DIAMETRO_CIDADE * 2.5f, y - DIAMETRO_CIDADE * 2.5f, DIAMETRO_CIDADE * 6, DIAMETRO_CIDADE * 6);
                }
            }
        }
        private void DesenhaLinhaAnimada(Pen caneta, Point p1, Point p2)
        {
            int qtdPassos = 0;
            int duracao = 500;
            int intervalo = 100;
            double c = p1.Y - p2.Y;
            double b = p2.X - p1.X;
            double a = Math.Round(Math.Sqrt(Convert.ToDouble(Math.Pow(b, 2) + Math.Pow(c, 2))));
            double tamanhoPasso = a/duracao;
            double angulo = 0;
            if (b > 0)
                angulo = Math.Atan2(c, b);
            else
                angulo = Math.Acos(b / a) + Math.PI;
            bool acabou = false;
            void Tick()
            {
                while (!acabou)
                {
                    qtdPassos++;
                    float x = Convert.ToSingle(qtdPassos * tamanhoPasso);
                    float y = Convert.ToSingle(angulo * x);
                    x = b < 0 ? x * -1 : x;
                    gfx.DrawLine(new Pen(corLinhaCaminho, 3), p1, new PointF(x + p1.X, p1.Y - y));
                    if (qtdPassos * tamanhoPasso > a)
                        acabou = true;
                }
            }
            Tick();
        }
        private void DrawFrame(object sender, EventArgs e)
        {
            pb.Invalidate();
        }
        public void DesenhaCaminho(Caminho caminhoAtual)
        {
            Cidade c1;
            Cidade c2;
            var rota = caminhoAtual.Rota.ToArray();
            for (int i = 1; i < rota.Length; i++)
            {
                c1 = arvore.Buscar(new Cidade(rota[i - 1]));
                c2 = arvore.Buscar(new Cidade(rota[i]));
                Point p1 = new Point(pb.Size.Width * c1.X / 4096, pb.Size.Height * c1.Y / 2048);
                Point p2 = new Point(pb.Size.Width * c2.X / 4096, pb.Size.Height * c2.Y / 2048);
                Pen caneta = new Pen(corLinhaCaminho, 3);
                caneta.CustomEndCap = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
                DesenhaLinhaAnimada(caneta, p1, p2);
            }
        }
        public void DesenharArvore(TabPage tpArvore)
        {
            DesenhaArvore(true, arvore.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 2.5, 450, gfx);
        }
        private void DesenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,
                   int x, int y, double angulo, double incremento,
                   double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);
                var esq = raiz.Esquerdo;
                DesenhaArvore(false, esq, xf, yf, Math.PI / 2 + incremento,
                                                 incremento * 0.60, comprimento * 0.8, g);
                var dir = raiz.Direito;
                DesenhaArvore(false, dir, xf, yf, Math.PI / 2 - incremento,
                                                  incremento * 0.60, comprimento * 0.8, g);
                // sleep(100);
                SolidBrush preenchimento = new SolidBrush(Color.Blue);
                g.FillEllipse(preenchimento, xf - 15, yf - 15, 30, 30);
                g.DrawString(Convert.ToString(raiz.Info.Codigo), new Font("Comic Sans", 12),
                              new SolidBrush(Color.Yellow), xf - 15, yf - 10);
            }
        }
    }
}
