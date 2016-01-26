using EmitMapper;
using EmitMapper.MappingConfiguration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitMapperConApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Sourse src = new Sourse
            {
                A = 1,
                B = 10M,
                C = DateTime.Parse("2011/9/21 0:00:00"),
                //D = new Inner
                //{
                //    D2 = Guid.NewGuid()
                //},
                E = "test"
            };
            Dest dst = null;
            //dst = mapper.Map(src);

            //Console.WriteLine(dst.A);
            //Console.WriteLine(dst.B);
            //Console.WriteLine(dst.C);
            ////Console.WriteLine(dst.D.D1);
            ////Console.WriteLine(dst.D.D2);
            //Console.WriteLine(dst.F);

            Stopwatch sw = new Stopwatch();
            int seed = 500000;

            ObjectsMapper<Sourse, Dest> mapper = ObjectMapperManager.DefaultInstance.GetMapper<Sourse, Dest>();
            var mapperImpl = mapper.MapperImpl;
            sw.Restart();
            for (var i = 0; i < seed; i++)
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<Sourse, Dest>();
                //mapper = new ObjectsMapper<Sourse, Dest>(mapperImpl);
                dst = mapper.Map(src);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            dst = ModelConverter.ConvertObject<Sourse, Dest>(src);

            sw.Restart();
            for (var i = 0; i < seed; i++)
            {
                dst = ModelConverter.ConvertObject<Sourse, Dest>(src);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            for (var i = 0; i < seed; i++)
            {
                dst = ModelConverter.ConvertModel<Sourse, Dest>(src);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        
    }

    public class Sourse
    {
        public int A { get; set; }
        public decimal? B { get; set; }
        public DateTime C { get; set; }
        //public Inner D;
        public string E { get; set; }
    }

    public class Dest
    {
        public int? A { get; set; }
        public decimal B { get; set; }
        public DateTime C { get; set; }
        //public Inner D;
        public string F { get; set; }
    }

    public class Inner
    {
        public long D1;
        public Guid D2;
    }

}
