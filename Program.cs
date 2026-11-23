using System;
using System.Collections.Generic;
using System.Threading;

namespace EventosYDelegados
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Ejercicio1();
            Ejercicio2();
            Ejercicio3();
            Ejercicio4();
            Ejercicio5();

            Console.ReadKey();
        }

        static void Ejercicio1()
        {
            Console.WriteLine("\n--- Ejercicio 1 ---");
            var registroVentas = new Ventas.RegistroVentas();
            var servicioRegistro = new Ventas.ServicioRegistro();
            var servicioNotificacion = new Ventas.ServicioNotificacion();

            registroVentas.VentaRealizada += servicioRegistro.RegistrarVenta;
            registroVentas.VentaRealizada += servicioNotificacion.EnviarNotificacionVenta;

            registroVentas.ProcesarVenta("Portátil", 899.99);
            registroVentas.ProcesarVenta("Ratón", 24.50);
        }

        static void Ejercicio2()
        {
            Console.WriteLine("\n--- Ejercicio 2 ---");
            var controlTemperatura = new Invernadero.ControlTemperatura(30);
            var servicioAlerta = new Invernadero.ServicioAlerta();
            var servicioRegistroTemperatura = new Invernadero.ServicioRegistroTemperatura();

            controlTemperatura.TemperaturaAlta += servicioAlerta.EnviarAlerta;
            controlTemperatura.TemperaturaAlta += servicioRegistroTemperatura.RegistrarTemperatura;

            controlTemperatura.ComprobarTemperatura(25);
            controlTemperatura.ComprobarTemperatura(34.5);
        }

        static void Ejercicio3()
        {
            Console.WriteLine("\n--- Ejercicio 3 ---");
            var gestorBackups = new Backups.GestorBackups();
            var servicioNotificacion = new Backups.ServicioNotificacion();
            var servicioLog = new Backups.ServicioLog();

            gestorBackups.BackupCompletado += servicioNotificacion.EnviarNotificacionBackup;
            gestorBackups.BackupCompletado += servicioLog.RegistrarBackup;

            gestorBackups.RealizarBackup("informe_ventas.xlsx");
            gestorBackups.RealizarBackup("fotos_vacaciones.zip");
        }

        static void Ejercicio4()
        {
            Console.WriteLine("\n--- Ejercicio 4 ---");
            var sensorMonitoreo = new CasaInteligente.SensorMonitoreo(8, 22);
            var servicioAlarma = new CasaInteligente.ServicioAlarma();
            var servicioRegistroIncidencias = new CasaInteligente.ServicioRegistroIncidencias();

            sensorMonitoreo.AlertaIntruso += servicioAlarma.ActivarAlarma;
            sensorMonitoreo.AlertaIntruso += servicioRegistroIncidencias.GuardarIncidencia;

            sensorMonitoreo.DetectarApertura("Puerta principal", new DateTime(2026, 11, 23, 10, 30, 0));
            sensorMonitoreo.DetectarApertura("Ventana del salón", new DateTime(2026, 11, 23, 3, 15, 0));
            sensorMonitoreo.DetectarApertura("Puerta del garaje", new DateTime(2026, 11, 23, 23, 45, 0));

            servicioRegistroIncidencias.MostrarIncidencias();
        }

        static void Ejercicio5()
        {
            Console.WriteLine("\n--- Ejercicio 5 ---");
            var monitorEnergia = new Energia.MonitorEnergia(3.5);
            var servicioNotificacion = new Energia.ServicioNotificacion();
            var servicioAjusteAutomatizado = new Energia.ServicioAjusteAutomatizado();

            monitorEnergia.ConsumoExcesivoDetectado += servicioNotificacion.EnviarAdvertencia;
            monitorEnergia.ConsumoExcesivoDetectado += servicioAjusteAutomatizado.AjustarDispositivos;

            monitorEnergia.RegistrarConsumo(2.1);
            monitorEnergia.RegistrarConsumo(4.8);
        }
    }
}

namespace EventosYDelegados.Ventas
{
    public class VentaEventArgs : EventArgs
    {
        public string Producto { get; set; }
        public double Precio { get; set; }
    }

    public class RegistroVentas
    {
        public delegate void GestorEventoVentaRealizada(object fuente, VentaEventArgs args);
        public event GestorEventoVentaRealizada VentaRealizada;

        public void ProcesarVenta(string producto, double precio)
        {
            Console.WriteLine($"Procesando la venta de {producto}...");
            Thread.Sleep(1000);
            EnVentaRealizada(producto, precio);
        }

        protected virtual void EnVentaRealizada(string producto, double precio)
        {
            if (VentaRealizada != null)
            {
                VentaRealizada(this, new VentaEventArgs() { Producto = producto, Precio = precio });
            }
        }
    }

    public class ServicioRegistro
    {
        public void RegistrarVenta(object fuente, VentaEventArgs e)
        {
            Console.WriteLine($"ServicioRegistro: venta guardada -> {e.Producto} por {e.Precio:C2}");
        }
    }

    public class ServicioNotificacion
    {
        public void EnviarNotificacionVenta(object fuente, VentaEventArgs e)
        {
            Console.WriteLine($"ServicioNotificacion: gracias por tu compra de {e.Producto}");
        }
    }
}

namespace EventosYDelegados.Invernadero
{
    public class TemperaturaEventArgs : EventArgs
    {
        public double TemperaturaActual { get; set; }
        public double Umbral { get; set; }
    }

    public class ControlTemperatura
    {
        public delegate void GestorEventoTemperaturaAlta(object fuente, TemperaturaEventArgs args);
        public event GestorEventoTemperaturaAlta TemperaturaAlta;

        private double umbral;

        public ControlTemperatura(double umbralMaximo)
        {
            umbral = umbralMaximo;
        }

        public void ComprobarTemperatura(double temperatura)
        {
            Console.WriteLine($"Temperatura medida: {temperatura}°C");
            if (temperatura > umbral)
            {
                EnTemperaturaAlta(temperatura);
            }
        }

        protected virtual void EnTemperaturaAlta(double temperatura)
        {
            if (TemperaturaAlta != null)
            {
                TemperaturaAlta(this, new TemperaturaEventArgs() { TemperaturaActual = temperatura, Umbral = umbral });
            }
        }
    }

    public class ServicioAlerta
    {
        public void EnviarAlerta(object fuente, TemperaturaEventArgs e)
        {
            Console.WriteLine($"ServicioAlerta: ¡ALERTA! La temperatura ha superado el umbral de {e.Umbral}°C");
        }
    }

    public class ServicioRegistroTemperatura
    {
        public void RegistrarTemperatura(object fuente, TemperaturaEventArgs e)
        {
            Console.WriteLine($"ServicioRegistroTemperatura: registrada una temperatura de {e.TemperaturaActual}°C");
        }
    }
}

namespace EventosYDelegados.Backups
{
    public class BackupEventArgs : EventArgs
    {
        public string NombreArchivo { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class GestorBackups
    {
        public delegate void GestorEventoBackupCompletado(object fuente, BackupEventArgs args);
        public event GestorEventoBackupCompletado BackupCompletado;

        public void RealizarBackup(string nombreArchivo)
        {
            Console.WriteLine($"Realizando copia de seguridad de {nombreArchivo}...");
            Thread.Sleep(2000);
            EnBackupCompletado(nombreArchivo);
        }

        protected virtual void EnBackupCompletado(string nombreArchivo)
        {
            if (BackupCompletado != null)
            {
                BackupCompletado(this, new BackupEventArgs() { NombreArchivo = nombreArchivo, Fecha = DateTime.Now });
            }
        }
    }

    public class ServicioNotificacion
    {
        public void EnviarNotificacionBackup(object fuente, BackupEventArgs e)
        {
            Console.WriteLine($"ServicioNotificacion: la copia de {e.NombreArchivo} se ha completado correctamente");
        }
    }

    public class ServicioLog
    {
        public void RegistrarBackup(object fuente, BackupEventArgs e)
        {
            Console.WriteLine($"ServicioLog: [{e.Fecha:dd/MM/yyyy HH:mm:ss}] Backup de {e.NombreArchivo}");
        }
    }
}

namespace EventosYDelegados.CasaInteligente
{
    public class IntrusoEventArgs : EventArgs
    {
        public string NombreSensor { get; set; }
        public DateTime HoraDeteccion { get; set; }
    }

    public class SensorMonitoreo
    {
        public delegate void GestorEventoAlertaIntruso(object fuente, IntrusoEventArgs args);
        public event GestorEventoAlertaIntruso AlertaIntruso;

        private int horaInicio;
        private int horaFin;

        public SensorMonitoreo(int horaInicioPermitida, int horaFinPermitida)
        {
            horaInicio = horaInicioPermitida;
            horaFin = horaFinPermitida;
        }

        public void DetectarApertura(string nombreSensor, DateTime hora)
        {
            Console.WriteLine($"Sensor: {nombreSensor} abierta a las {hora:HH:mm}");
            if (hora.Hour < horaInicio || hora.Hour >= horaFin)
            {
                EnAlertaIntruso(nombreSensor, hora);
            }
        }

        protected virtual void EnAlertaIntruso(string nombreSensor, DateTime hora)
        {
            if (AlertaIntruso != null)
            {
                AlertaIntruso(this, new IntrusoEventArgs() { NombreSensor = nombreSensor, HoraDeteccion = hora });
            }
        }
    }

    public class ServicioAlarma
    {
        public void ActivarAlarma(object fuente, IntrusoEventArgs e)
        {
            Console.WriteLine($"ServicioAlarma: ¡ALARMA ACTIVADA! {e.NombreSensor} abierta fuera del horario permitido");
        }
    }

    public class ServicioRegistroIncidencias
    {
        private List<string> baseDeDatos = new List<string>();

        public void GuardarIncidencia(object fuente, IntrusoEventArgs e)
        {
            string incidencia = $"{e.HoraDeteccion:dd/MM/yyyy HH:mm} - {e.NombreSensor}";
            baseDeDatos.Add(incidencia);
            Console.WriteLine($"ServicioRegistroIncidencias: incidencia guardada en la base de datos ({incidencia})");
        }

        public void MostrarIncidencias()
        {
            Console.WriteLine("Incidencias registradas:");
            foreach (string incidencia in baseDeDatos)
            {
                Console.WriteLine($"- {incidencia}");
            }
        }
    }
}

namespace EventosYDelegados.Energia
{
    public class EnergiaEventArgs : EventArgs
    {
        public double ConsumoActual { get; set; }
        public double Umbral { get; set; }
    }

    public class MonitorEnergia
    {
        public delegate void GestorEventoConsumoExcesivo(object fuente, EnergiaEventArgs args);
        public event GestorEventoConsumoExcesivo ConsumoExcesivoDetectado;

        private double umbral;

        public MonitorEnergia(double umbralMaximo)
        {
            umbral = umbralMaximo;
        }

        public void RegistrarConsumo(double consumo)
        {
            Console.WriteLine($"Consumo registrado: {consumo} kWh");
            if (consumo > umbral)
            {
                EnConsumoExcesivoDetectado(consumo);
            }
        }

        protected virtual void EnConsumoExcesivoDetectado(double consumo)
        {
            if (ConsumoExcesivoDetectado != null)
            {
                ConsumoExcesivoDetectado(this, new EnergiaEventArgs() { ConsumoActual = consumo, Umbral = umbral });
            }
        }
    }

    public class ServicioNotificacion
    {
        public void EnviarAdvertencia(object fuente, EnergiaEventArgs e)
        {
            Console.WriteLine($"ServicioNotificacion: cuidado, estás consumiendo {e.ConsumoActual} kWh y el límite es {e.Umbral} kWh");
        }
    }

    public class ServicioAjusteAutomatizado
    {
        public void AjustarDispositivos(object fuente, EnergiaEventArgs e)
        {
            double exceso = e.ConsumoActual - e.Umbral;
            Console.WriteLine($"ServicioAjusteAutomatizado: bajando la calefacción y apagando dispositivos en espera para reducir {exceso:F2} kWh");
        }
    }
}