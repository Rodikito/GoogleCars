using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCars.Model
{
    public class Carrera
    {
        public int ID { get; set; }
        public bool Libre { get; set; }
        public int Xinicial { get; set; }
        public int Yinicial { get; set; }

        public int Xfinal { get; set; }
        public int Yfinal { get; set; }

        public int Distancia // distancia entre intersección X e Y, es igual a los pasos
        {
            get {
                return Math.Abs((Xinicial - Xfinal) + (Yinicial - Yfinal));
            }
           
        }


        public int InicioMinimo { get; set; } //nº de  paso mínimo  en el que la carrera puede empezar

        public int Final { get; set; } //último paso en el que la carrera debe terminar

        public Carrera(int id, bool libre, int xi, int yi,int xf,int yfinal, int inicio,int final)
        {

            this.ID = id;
            this.Libre = true;
            this.Xinicial = xi;
            this.Xfinal = xf;
            this.Yinicial = yi;
            this.Yfinal = yfinal;
            this.InicioMinimo = inicio;
            this.Final = final;
        }

    }

}

