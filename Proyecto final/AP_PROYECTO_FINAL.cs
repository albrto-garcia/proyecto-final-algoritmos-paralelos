using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AP
{
    public partial class AP_PROYECTO_FINAL : Form
    {
        private static DateTime start_time = new DateTime(), end_time = new DateTime();
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public AP_PROYECTO_FINAL()
        {
            InitializeComponent();
        }

        public int RandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                return random.Next(min, max);
            }
        }

        public DateTime StartTime()
        {
            lock (syncLock)
            {
                start_time = DateTime.UtcNow;
                return start_time;
            }
        }

        public DateTime EndTime()
        {
            lock (syncLock)
            {
                end_time = DateTime.UtcNow;
                return end_time;
            }
        }

        private int[] Desordenar(int[] vector)
        {
            int num_unsorted = int.Parse(txtTamano.Text) / 2;

            for (int i = 0; i < num_unsorted; i++)
            {
                int index1 = RandomNumber(0, int.Parse(txtTamano.Text) - 1);
                int index2 = RandomNumber(0, int.Parse(txtTamano.Text) - 1);

                int temp = vector[index1];
                vector[index1] = vector[index2];
                vector[index2] = temp;
            }

            return vector;
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtTamano.Text) && !String.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                if (int.Parse(txtTamano.Text) <= 1)
                {
                    MessageBox.Show("El tamaño debe ser mayor a 1.");
                }
                else
                if (int.Parse(txtIteraciones.Text) == 0)
                {
                    MessageBox.Show("Las iteraciones deben ser mayor a 0.");
                }
                else
                {
                    txtTiempo1.Text = txtTiempo2.Text = txtTiempo3.Text = txtTiempo4.Text = txtTiempo5.Text = "";

                    int[] vector1 = new int[int.Parse(txtTamano.Text)];

                    for (int c = 0; c < vector1.Length; c++)
                    {
                        vector1[c] = c;
                    }

                    vector1 = Desordenar(vector1);
                    int[] vector2 = (int[])vector1.Clone(); 
                    int[] vector3 = (int[])vector1.Clone();
                    int[] vector4 = (int[])vector1.Clone();
                    int[] vector5 = (int[])vector1.Clone();

                    Algoritmos a = new Algoritmos(this);
                    Thread thread1 = new Thread(() => a.BusquedaSecuencial(txtTiempo1, vector1, int.Parse(txtBuscar.Text), int.Parse(txtIteraciones.Text)));
                    Thread thread2 = new Thread(() => a.BusquedaBinaria(txtTiempo2, vector2, int.Parse(txtBuscar.Text), int.Parse(txtIteraciones.Text)));
                    Thread thread3 = new Thread(() => a.OrdenamientoDeLaBurbuja(txtTiempo3, vector3, int.Parse(txtIteraciones.Text)));
                    Thread thread4 = new Thread(() => a.QuickSort(txtTiempo4, vector4, int.Parse(txtIteraciones.Text)));
                    Thread thread5 = new Thread(() => a.OrdenamientoPorInsercion(txtTiempo5, vector5, int.Parse(txtIteraciones.Text)));

                    thread1.Start();
                    thread2.Start();
                    thread3.Start();
                    thread4.Start();
                    thread5.Start();
                }
            }
            else
            {
                MessageBox.Show("El tamaño del vector y el valor a buscar deben estar llenos para poder ejecutar los algoritmos.");
            }
        }

        private void txtTamano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void AP_PROYECTO_FINAL_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }

        private void txtIteraciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void linkRepositorio_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/albrto-garcia/proyecto-final-algoritmos-paralelos");
            Process.Start(sInfo);
        }
    }

    public class Algoritmos
    {
        AP_PROYECTO_FINAL ap_proyecto_final = new AP_PROYECTO_FINAL();
        Form ap_proyecto_final_form;

        public Algoritmos(Form ap_proyecto_final_form)
        {
            this.ap_proyecto_final_form = ap_proyecto_final_form;
        }

        public void BusquedaSecuencial(TextBox txt, int[] vector, int n, int iteraciones)
        {
            var start = ap_proyecto_final.StartTime();

            if (vector.Length == 1)
                return;

            var vector_copy = (int[])vector.Clone();

            for (int c = 0; c < iteraciones; c++)
            {
                Boolean x = false;

                for (int a = 0; a < vector.Length; a++)
                {
                    if (vector[a] == n)
                    {
                        x = true;
                        break;
                    }
                }

                vector = (int[])vector_copy.Clone();
            }

            var end = ap_proyecto_final.EndTime();
            var timeDiff = (end - start);

            ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                       Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                       Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }

        public void BusquedaBinaria(TextBox txt, int[] vector, int n, int iteraciones)
        {
            var start = ap_proyecto_final.StartTime();

            if (vector.Length == 1)
                return;

            var vector_copy = (int[])vector.Clone();

            for (int a = 0; a < iteraciones; a++)
            {
                Array.Sort(vector);

                int l = 0, h = vector.Length - 1;
                int m = 0;
                bool found = false;

                while (l <= h && found == false)
                {
                    m = (l + h) / 2;

                    if (vector[m] == n)
                    {
                        found = true;
                        break;
                    }

                    if (vector[m] > n)
                        h = m - 1;
                    else
                        l = m + 1;
                }

                vector = (int[])vector_copy.Clone();
            }

            var end = ap_proyecto_final.EndTime();
            var timeDiff = (end - start);

            ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                       Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                       Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }

        public void OrdenamientoDeLaBurbuja(TextBox txt, int[] vector, int iteraciones)
        {
            var start = ap_proyecto_final.StartTime();

            if (vector.Length == 1)
                return;

            var vector_copy = (int[])vector.Clone();

            for (int c = 0; c < iteraciones; c++)
            {
                int t;

                for (int a = 1; a < vector.Length; a++)
                {
                    for (int b = vector.Length - 1; b >= a; b--)
                    {
                        if (vector[b - 1] > vector[b])
                        {
                            t = vector[b - 1];
                            vector[b - 1] = vector[b];
                            vector[b] = t;
                        }
                    }
                }

                vector = (int[])vector_copy.Clone();
            }

            var end = ap_proyecto_final.EndTime();
            var timeDiff = (end - start);

            ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                       Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                       Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }

        public void QuickSort(TextBox txt, int[] vector, int iteraciones)
        {
            var start = ap_proyecto_final.StartTime();

            if (vector.Length == 1)
                return;

            var vector_copy = (int[])vector.Clone();

            for (int c = 0; c < iteraciones; c++)
            {
                int startIndex = 0;
                int endIndex = vector.Length - 1;
                int top = -1;
                int[] stack = new int[vector.Length];

                stack[++top] = startIndex;
                stack[++top] = endIndex;

                while (top >= 0)
                {
                    endIndex = stack[top--];
                    startIndex = stack[top--];

                    int p = Particion(ref vector, startIndex, endIndex);

                    if (p - 1 > startIndex)
                    {
                        stack[++top] = startIndex;
                        stack[++top] = p - 1;
                    }

                    if (p + 1 < endIndex)
                    {
                        stack[++top] = p + 1;
                        stack[++top] = endIndex;
                    }
                }

                vector = (int[])vector_copy.Clone();
            }
           
                var end = ap_proyecto_final.EndTime();
                var timeDiff = (end - start);

                ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                           Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                           Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }

        private static int Particion(ref int[] vector, int principio, int final)
        {
            int x = vector[final];
            int i = (principio - 1);

            for (int j = principio; j <= final - 1; ++j)
            {
                if (vector[j] <= x)
                {
                    ++i;
                    Intercambio(ref vector[i], ref vector[j]);
                }
            }

            Intercambio(ref vector[i + 1], ref vector[final]);

            return (i + 1);
        }

        private static void Intercambio(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public void OrdenamientoPorInsercion(TextBox txt, int[] vector, int iteraciones)
        {
            var start = ap_proyecto_final.StartTime();

            if (vector.Length == 1)
                return;

            var vector_copy = (int[])vector.Clone();

            for (int c = 0; c < iteraciones; c++)
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    int pos = i;
                    int aux = vector[i];

                    while (pos > 0 && vector[pos - 1] > aux)
                    {
                        vector[pos] = vector[pos - 1];
                        pos--;
                    }

                    vector[pos] = aux;
                }

                vector = (int[])vector_copy.Clone();
            }

            var end = ap_proyecto_final.EndTime();
            var timeDiff = (end - start);

            ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                       Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                       Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }
    }
}