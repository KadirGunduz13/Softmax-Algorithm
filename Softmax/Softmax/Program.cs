using System;
using System.Collections.Generic;
using System.Linq;

namespace Softmax
{
    // Mahalle sınıfında mahalle adı ve her bir kriter için puanlama değerleri tutuyoruz.
    class Mahalle
    {
        public string Ad { get; set; }
        public double NufusYogunlugu { get; set; }
        public double UlasimAltyapisi { get; set; }
        public double Maliyet { get; set; }
        public double CevreselEtki { get; set; }
        public double SosyalFayda { get; set; }

        public double Skor { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Mahallelerin adını ve her bir kriter için puanlama değerlerini belirledim.
            var mahalleler = new List<Mahalle>
            {
                new Mahalle { Ad = "Karahıdır Mahallesi", NufusYogunlugu = 8.0, UlasimAltyapisi = 5.0, Maliyet = 2.0, CevreselEtki = 4.0, SosyalFayda = 7.0 },
                new Mahalle { Ad = "İstasyon Mahallesi", NufusYogunlugu = 6.0, UlasimAltyapisi = 4.5, Maliyet = 3.0, CevreselEtki = 3.5, SosyalFayda = 6.5 },
                new Mahalle { Ad = "Pınar Mahallesi", NufusYogunlugu = 7.5, UlasimAltyapisi = 6.0, Maliyet = 2.5, CevreselEtki = 3.0, SosyalFayda = 7.5 }
            };

            // Her bir kriter için softmax uygulayarak normalize ettik.
            mahalleler = SoftmaxNormalize(mahalleler);

            // Her bir mahalle için toplam skor hesapla ve ekrana yazdırdık.
            foreach (var mahalle in mahalleler)
            {
                mahalle.Skor = mahalle.NufusYogunlugu + mahalle.UlasimAltyapisi + 
                               mahalle.Maliyet + mahalle.CevreselEtki + mahalle.SosyalFayda;
                Console.WriteLine($"{mahalle.Ad} - Toplam Skor: {mahalle.Skor:F3}");
            }

            Console.WriteLine();

            // En yüksek skora sahip mahalleyi bulup ekrana yazdırdık.
            var enUygunMahalle = mahalleler.OrderByDescending(m => m.Skor).First();
            Console.WriteLine($"En uygun güzergah: {enUygunMahalle.Ad}");

            Console.Read();
        }

        // Her bir kriter için softmax uygulayarak normalize eden fonksiyonu yazdık.
        static List<Mahalle> SoftmaxNormalize(List<Mahalle> mahalleler)
        {
            // Her bir kriter için puanlama değerlerini diziye çevirdik.
            double[] nufusScores = mahalleler.Select(m => m.NufusYogunlugu).ToArray();
            double[] ulasimScores = mahalleler.Select(m => m.UlasimAltyapisi).ToArray();
            double[] maliyetScores = mahalleler.Select(m => m.Maliyet).ToArray();
            double[] cevreselScores = mahalleler.Select(m => m.CevreselEtki).ToArray();
            double[] sosyalScores = mahalleler.Select(m => m.SosyalFayda).ToArray();

            // Burada softmax fonksiyonunu çağırarak her bir kriter için normalize edilmiş değerleri aldık.
            double[] softNufus = Softmax(nufusScores);
            double[] softUlasim = Softmax(ulasimScores);
            double[] softMaliyet = Softmax(maliyetScores);
            double[] softCevresel = Softmax(cevreselScores);
            double[] softSosyal = Softmax(sosyalScores);

            // Normalize edilmiş değerleri tekrar mahalle nesnelerine atadık.
            for (int i = 0; i < mahalleler.Count; i++)
            {
                mahalleler[i].NufusYogunlugu = softNufus[i];
                mahalleler[i].UlasimAltyapisi = softUlasim[i];
                mahalleler[i].Maliyet = softMaliyet[i];
                mahalleler[i].CevreselEtki = softCevresel[i];
                mahalleler[i].SosyalFayda = softSosyal[i];
            }

            // Normalize edilmiş değerleri içeren mahalle listesini geri döndürdük.
            return mahalleler;
        }

        // Softmax fonksiyonunu yazdık.
        static double[] Softmax(double[] scores)
        {
            // Burada softmax fonksiyonu uygulandı ve normalize edilmiş değerler dizi olarak geri döndürüldü.
            double sumExp = scores.Sum(s => Math.Exp(s));
            return scores.Select(s => Math.Exp(s) / sumExp).ToArray();
        }
    }
}
