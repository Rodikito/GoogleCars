using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCars.Model
{
    public class Vehiculo
    {
        public int ID { get; set; }
        public bool Libre { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int DistanciaRecorrida { get; set; }
        public int DistanciaPorRecorrer { get; set; }
        public Carrera CarreraActual { get; set; }
        public List<Carrera> Carreras{ get; set; }

        public Vehiculo(int i)
        {
            this.ID = i;
            this.Libre = true;
            this.X = 0;
            this.Y = 0;
            this.DistanciaPorRecorrer = 0;
            this.DistanciaRecorrida = 0;
            this.CarreraActual = null;
            Carreras = new List<Carrera>();
        }

    }

}

