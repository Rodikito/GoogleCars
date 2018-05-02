using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleCars.Model;

namespace GoogleCars
{
    class Program
    {

        static List<Carrera> misCarreras = new List<Carrera>();
        static List<Carrera> carrerasLibres = new List<Carrera>();
        static List<Vehiculo> misVehiculos = new List<Vehiculo>();
        static void Main(string[] args)
        {

            StreamReader r = new StreamReader(@"C:\Users\jrodes\Downloads\e_high_bonus.in");
            string cabeceras = r.ReadLine();
            string[] entrada = cabeceras.Split(' '); 
            int Filas = int.Parse(entrada[0]);
            int Columnas = int.Parse(entrada[1]);
            int Vehiculos = int.Parse(entrada[2]);
            int Carreras = int.Parse(entrada[3]);
            int Bonus = int.Parse(entrada[4]);
            int Pasos = int.Parse(entrada[5]);

             /*Console.WriteLine("Filas="+Filas);
             Console.WriteLine("Columnas=" + Columnas);
             Console.WriteLine("Vehiculos=" + Vehiculos);
             Console.WriteLine("Carreras=" + Carreras);
             Console.WriteLine("Pasos=" + Pasos);*/


            //CREO VEHICULOS
            

            for (int i = 1; i <= Vehiculos; i++)
            {
                misVehiculos.Add(new Vehiculo(i));
            }

            //CREO CARRERAS
            
            for (int i = 1; i <= Carreras; i++){
                string datos = r.ReadLine();
                string[] datosCarrera = datos.Split(' ');

               misCarreras.Add(new Carrera(i,
                                        true,
                                        int.Parse(datosCarrera[0]), 
                                        int.Parse(datosCarrera[1]), 
                                        int.Parse(datosCarrera[2]), 
                                        int.Parse(datosCarrera[3]), 
                                        int.Parse(datosCarrera[4]), 
                                        int.Parse(datosCarrera[5])));


            }
            StreamWriter wr = new StreamWriter(@"C:\python\res\e_high_bonus_jorge.out");
           // Carrera carr;

            Vehiculo VehiculoMasCercanoOcupado;
            Vehiculo VehiculoMasCercanoLibre;
            Vehiculo VehiculoMasCercano;

            int DistanciaVLibre;
            int DistanciaVOcupado;
            int DistanciaV;

            int posx = 0 ;
            int posy = 0;

           
            for (int p = 0; p <= Pasos-1; p++)
            {
            


                carrerasLibres = misCarreras.Where(c => c.Libre == true)                    
                                       .Where(c=>c.InicioMinimo==p) //inicio mínimo  igual que p para que puntúe y además tenga bonus
                                        
                                          .OrderBy(c=>c.Distancia)                           //orden  por número de pasos de la la carrera
                                           .ToList();

                for (int c = 0; c <= carrerasLibres.Count -1; c++)
                {


                    //busco el vehículo más cercano,esté libre o no (puede que algún vehículo esté ocupado pero le dé tiempo a llegar antes)

                    //LIBRE -> distancia desde la posición actual del vehículo hasta el punto de inicio de la carrera
                   
                    VehiculoMasCercanoLibre = misVehiculos
                                    .Where(v => (v.Libre == true)
                                        && (p + Distancia(v.X, carrerasLibres[c].Xinicial, v.Y, carrerasLibres[c].Yinicial) + carrerasLibres[c].Distancia < carrerasLibres[c].Final)//paso actual + distancia + distancia de carrera debe ser menor que latest finish
                                     //   && (p +Distancia(v.X, carrerasLibres[c].Xinicial, v.Y, carrerasLibres[c].Yinicial) + carrerasLibres[c].Distancia >= carrerasLibres[c].InicioMinimo)// y mayor o igual que earliest start
                                      )
                                    .OrderBy(v => Distancia(v.X, carrerasLibres[c].Xinicial, v.Y, carrerasLibres[c].Yinicial))
                                   
                                   .FirstOrDefault();

                    if(VehiculoMasCercanoLibre != null)
                    {
                        DistanciaVLibre = Distancia(VehiculoMasCercanoLibre.X, carrerasLibres[c].Xinicial, VehiculoMasCercanoLibre.Y, carrerasLibres[c].Yinicial);
                    }
                    else
                    {
                        DistanciaVLibre = 99999;
                    }



                    //OCUPADO -> distancia entre el punto final de la carrera que el vehículo hace actualmente , y el punto de inicio de la carrera , sumándole la distancia pendiente por recorrer
                    if (misVehiculos.Where(v => (v.Libre == false)
                        && (p + v.DistanciaPorRecorrer + Distancia(v.X, carrerasLibres[c].Xinicial, v.Y, carrerasLibres[c].Yinicial) < carrerasLibres[c].Final))
                        .Any())

                    {
                        VehiculoMasCercanoOcupado = misVehiculos
                                        .Where(v => (v.Libre == false)
                                            && (p + v.DistanciaPorRecorrer + Distancia(v.X, carrerasLibres[c].Xinicial, v.Y, carrerasLibres[c].Yinicial) + carrerasLibres[c].Distancia < carrerasLibres[c].Final) //paso actual + distancia debe ser menor que latest finish
                                          //  && (Distancia(v.X,v.Y,carrerasLibres[c].Xinicial,carrerasLibres[c].Yinicial) >= carrerasLibres[c].InicioMinimo)// y mayor o igual que earliest start
                                            )
                                        

                                        .OrderBy(v => v.DistanciaPorRecorrer + Distancia(v.CarreraActual.Xinicial, carrerasLibres[c].Xinicial, v.CarreraActual.Yinicial, carrerasLibres[c].Yinicial))
                                        .FirstOrDefault();

                        if (VehiculoMasCercanoOcupado != null)
                        {
                            DistanciaVOcupado = VehiculoMasCercanoOcupado.DistanciaPorRecorrer + Distancia(VehiculoMasCercanoOcupado.CarreraActual.Xfinal,carrerasLibres[c].Xinicial,VehiculoMasCercanoOcupado.CarreraActual.Yfinal,carrerasLibres[c].Yinicial);
                        }
                        else
                        {
                            DistanciaVOcupado = 99999;
                        }
                    }
                    else
                    {
                        DistanciaVOcupado = 99999;
                        VehiculoMasCercanoOcupado = null;
                    }

                    //el vehículo más cercano será qué más cerca esté de los libres y los ocupados anteriormente obtenidos
                    
                    if(DistanciaVLibre < DistanciaVOcupado)
                    {
                        VehiculoMasCercano = VehiculoMasCercanoLibre;
                        DistanciaV = DistanciaVLibre;
                    }
                    else
                    {
                        VehiculoMasCercano = VehiculoMasCercanoOcupado;
                        DistanciaV = DistanciaVOcupado;
                    }
                    


                    //asignar la carrera a ese vehículo
                    if (VehiculoMasCercano != null)
                    {
                      /* if (DistanciaV + p< carrerasLibres[c].InicioMinimo)
                        {
                            //debería esperar a iniciar la carrera, así que vamos a la siguiente
                            continue;
                        }*/

                        VehiculoMasCercano.Libre = false;
   

                        VehiculoMasCercano.Carreras.Add(carrerasLibres[c]);
                        posx++;
                        posy++;
                        VehiculoMasCercano.CarreraActual = carrerasLibres[c];
                        VehiculoMasCercano.DistanciaPorRecorrer = carrerasLibres[c].Distancia + Distancia(VehiculoMasCercano.X, VehiculoMasCercano.Y, carrerasLibres[c].Xinicial, carrerasLibres[c].Yinicial);
                        carrerasLibres[c].Libre = false;
                    }
                }

                for (int i = 1; i <= misVehiculos.Count; i++)
                {
                    if (misVehiculos[i - 1].Libre == false)
                    {
                        misVehiculos[i - 1].DistanciaRecorrida++; // si están en plena carrera, les aumento 1 a la distancia recorrida
                        misVehiculos[i - 1].DistanciaPorRecorrer--; //y les resto 1 a la distancia por recorrer
                        if (misVehiculos[i - 1].DistanciaRecorrida == misVehiculos[i - 1].DistanciaPorRecorrer)
                        {
                            misVehiculos[i - 1].X = misVehiculos[i - 1].CarreraActual.Xfinal;
                            misVehiculos[i - 1].Y = misVehiculos[i - 1].CarreraActual.Yfinal;
                            misVehiculos[i - 1].Libre = true;
                            misVehiculos[i - 1].CarreraActual = null;
                            misVehiculos[i - 1].DistanciaRecorrida = 0;
                            misVehiculos[i - 1].DistanciaPorRecorrer = 0;
                        }
                    }

                }
            }


            for (int i = 1; i <= misVehiculos.Count; i++)
            {
                wr.Write(misVehiculos[i-1].ID);
                
                foreach (var c in misVehiculos[i - 1].Carreras)
                {
                    wr.Write(" ");
                    wr.Write(c.ID-1);
                    
                }

                wr.WriteLine();
            }

                wr.Close();

            Environment.Exit(0);
   
        }

        public static int Distancia(int x, int y, int a, int b)
        {
            return Math.Abs(x - a) + Math.Abs(y - b);
        }


    }
}
