using System;
using System.Diagnostics;
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

        public static int RandomNumber(int min, int max)
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
            int num_unsorted = int.Parse(txtTamano.Text);

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
                if(int.Parse(txtTamano.Text) == 0)
                {
                    MessageBox.Show("El tamaño debe ser mayor a 0.");
                }
                else
                    if (Math.Floor(Math.Log10(int.Parse(txtTamano.Text)) + 1) > 6 || Math.Floor(Math.Log10(int.Parse(txtBuscar.Text)) + 1) > 9)
                    {
                        MessageBox.Show("Los valores están muy altos. Disminuya su tamaño.");
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
                        int[] vector2 = vector1;
                        int[] vector3 = vector2;
                        int[] vector4 = vector3;
                        int[] vector5 = vector4;

                        Algoritmos a = new Algoritmos(this);
                        Thread thread1 = new Thread(() => a.BusquedaSecuencial(txtTiempo1, vector1, int.Parse(txtBuscar.Text)));
                        Thread thread2 = new Thread(() => a.BusquedaBinaria(txtTiempo2, vector2, int.Parse(txtBuscar.Text)));
                        Thread thread3 = new Thread(() => a.OrdenamientoDeLaBurbuja(txtTiempo3, vector3));
                        Thread thread4 = new Thread(() => a.QuickSort(txtTiempo4, vector4, 0, vector4.Length - 1));
                        Thread thread5 = new Thread(() => a.OrdenamientoPorInsercion(txtTiempo5, vector5));

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

        public void BusquedaSecuencial(TextBox txt, int[] vector, int n)
        {
            var start = ap_proyecto_final.StartTime();
            Boolean x = false;

            for (int a = 0; a < vector.Length; a++)
            {
                if (vector[a] == n)
                {
                    Console.WriteLine("El número " + n + " se encuentra en la posición " + (a + 1) + ".");
                    x = true;
                    break;
                }
            }

            if (!x)
            {
                Console.WriteLine("El número " + n + " no se encuentra en el arreglo.");
            }

            var end = ap_proyecto_final.EndTime();
            var timeDiff = (end - start);

            ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                       Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                       Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }

        public void BusquedaBinaria(TextBox txt, int[] vector, int n)
        {
            var start = ap_proyecto_final.StartTime();

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

            if (found == false)
            {
                Console.WriteLine("\nEl elemento {0} no está en el arreglo.", n);
            }
            else
            {
                Console.WriteLine("\nEl elemento {0} está en la posición: {1}", n, m + 1);
            }

            var end = ap_proyecto_final.EndTime();
            var timeDiff = (end - start);

            ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                       Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                       Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }

        public void OrdenamientoDeLaBurbuja(TextBox txt, int[] vector)
        {
            var start = ap_proyecto_final.StartTime();

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

            for (int f = 0; f < vector.Length; f++)
            {
                Console.WriteLine(vector[f] + "\n");
            }

            var end = ap_proyecto_final.EndTime();
            var timeDiff = (end - start);

            ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                       Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                       Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }

        public void QuickSort(TextBox txt, int[] vector, int principio, int final)
        {
            var start = ap_proyecto_final.StartTime();
            int i, j, k = vector.Length, centro; double pivote;
            centro = (principio + final) / 2; pivote = vector[centro]; i = principio; j = final;

            do
            {
                while (vector[i] < pivote)
                    i++;

                while (vector[j] > pivote)
                    j--;

                if (i <= j)
                {
                    int temp;
                    temp = vector[i];
                    vector[i] = vector[j];
                    vector[j] = temp;
                    i++;
                    j--;
                }

                if (j < 0)
                {
                    for (int l = 0; l < k; l++)
                    {
                        Console.WriteLine(vector[l] + "\n");
                    }
                }
            } while (i <= j);

            if (principio < j)
                QuickSort(txt, vector, principio, j);
            if (i < final)
                QuickSort(txt, vector, i, final);

            var end = ap_proyecto_final.EndTime();
            var timeDiff = (end - start);

            ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                       Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                       Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }

        public void OrdenamientoPorInsercion(TextBox txt, int[] vector)
        {
            var start = ap_proyecto_final.StartTime();

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

            for (int i = 0; i < vector.Length; i++)
            {
                Console.WriteLine(vector[i] + "\n");
            }

            var end = ap_proyecto_final.EndTime();
            var timeDiff = (end - start);

            ap_proyecto_final_form.Invoke(new Action(() => txt.Text = (Convert.ToDecimal(timeDiff.TotalMilliseconds) >= 1000 ?
                                                                       Convert.ToDecimal(timeDiff.TotalSeconds).ToString() + " segundos" :
                                                                       Convert.ToDecimal(timeDiff.TotalMilliseconds).ToString() + " milisegundos").ToString()));
        }
    }
}