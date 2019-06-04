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
    static class Desenhador
    {
        #region Constantes
        static Color corNo = Color.Blue;
        static Color corLinhaArvore = Color.Red;
        static Color corLinhaCidade = Color.FromArgb(51, 77, 201);
        static Color corCidade = Color.Black;
        static Color corLinhaCaminho = Color.Red;
        const int DIAMETRO_NO = 30;
        const int DIAMETRO_CIDADE = 10;
        #endregion
        public static void DesenhaLinha(Graphics gfx, Control pb, Cidade um, Cidade dois, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            Point p1 = new Point(pb.Size.Width * um.X / 4096, pb.Size.Height * um.Y / 2048);
            Point p2 = new Point(pb.Size.Width * dois.X / 4096, pb.Size.Height * dois.Y / 2048);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }
        public static void DesenhaLinha(Graphics gfx, Control pb, float x1, float y1, float x2, float y2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pb.Size.Width * x1 / 4096, pb.Size.Height * y1 / 2048);
            PointF p2 = new PointF(pb.Size.Width * x2 / 4096, pb.Size.Height * y2 / 2048);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }
        public static void DesenhaLinha(Graphics gfx, Control pb, Cidade um, float x2, float y2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pb.Size.Width * um.X / 4096, pb.Size.Height * um.Y / 2048);
            PointF p2 = new PointF(pb.Size.Width * x2 / 4096, pb.Size.Height * y2 / 2048);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }
        public static void DesenhaLinha(Graphics gfx, Control pb, float x1, float y1, Cidade dois, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pb.Size.Width * x1 / 4096, pb.Size.Height * y1 / 2048);
            PointF p2 = new PointF(pb.Size.Width * dois.X / 4096, pb.Size.Height * dois.Y / 2048);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }
        public static void DesenhaLinha(Graphics gfx, Control pb, PointF p1, float x2, float y2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p2 = new PointF(pb.Size.Width * x2 / 4096, pb.Size.Height * y2 / 2048);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }
        public static void DesenhaLinha(Graphics gfx, Control pb, float x1, float y1, PointF p2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            PointF p1 = new PointF(pb.Size.Width * x1 / 4096, pb.Size.Height * y1 / 2048);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }
        public static void DesenhaLinha(Graphics gfx, Control pb, PointF p1, PointF p2, bool comSeta)
        {
            var caneta = new Pen(corLinhaCidade, 2.5f);
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
            if (comSeta)
                caneta.CustomEndCap = bigArrow;
            gfx.DrawLine(caneta, p1, p2);
        }
        public static void DesenhaCidade(Graphics gfx, Control pb, Cidade c, bool origem, bool destino)
        {
            int x = pb.Size.Width * c.X / 4096 - DIAMETRO_CIDADE / 2;
            int y = pb.Size.Height * c.Y / 2048 - DIAMETRO_CIDADE / 2;
            gfx.FillEllipse(new SolidBrush(corCidade), x, y, DIAMETRO_CIDADE, DIAMETRO_CIDADE);
            gfx.DrawString(c.Nome, new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(corCidade), new Point(x - 10, y + 10));
            if (origem)
                gfx.DrawImage(Image.FromFile("../../Imagens/cidadeOrigem.png"), x - DIAMETRO_CIDADE * 1.5f, y - DIAMETRO_CIDADE * 3.5f, DIAMETRO_CIDADE * 4, DIAMETRO_CIDADE * 4);
            if (destino)
            {
                Bitmap gif = new Bitmap("../../Imagens/cidadeDestino.gif");
                ImageAnimator.Animate(gif, (object sender, EventArgs e) => { pb.Invalidate(); });
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
        private static void DesenhaLinhaAnimada(Graphics gfx, Pen caneta, Point p1, Point p2)
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
        public static void DesenhaCaminho(Graphics gfx, Control c, List<Cidade> caminhoAtual)
        {
            Cidade anterior = caminhoAtual[0];
            foreach (Cidade atual in caminhoAtual)
            {
                if (!atual.Equals(anterior))
                {
                    Point p1 = new Point(c.Size.Width * anterior.X / 4096, c.Size.Height * anterior.Y / 2048);
                    Point p2 = new Point(c.Size.Width * atual.X / 4096, c.Size.Height * atual.Y / 2048);
                    Pen caneta = new Pen(corLinhaCaminho, 3);
                    caneta.CustomEndCap = new AdjustableArrowCap(DIAMETRO_CIDADE / 2, DIAMETRO_CIDADE / 2);
                    // DesenhaLinhaAnimada(gfx, caneta, p1, p2);
                    DesenhaLinha(gfx, c, anterior, atual, true);
                    anterior = atual;
                }
            }
        }
        public static void DesenharArvore(Graphics gfx, TabPage tpArvore, NoArvore<Cidade> raiz)
        {
            DesenhaArvore(true, raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 2.5, 450, gfx);
        }
        private static void DesenhaArvore(bool primeiraVez, NoArvore<Cidade> raiz,
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
