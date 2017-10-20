using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Validaciones;

namespace CalculoModeloCrecimientoGrafica
{
    public partial class Form1 : Form
    {
        //Declaraciones
        double y0, y1, y2;
        int t = 0;
        double constanteE = Math.E;

        double Formula1;
        double Formula2;
        double Formula3;
        double repeticiones = 5;
        double FormulaK;
        string label;
        DataTable dt = new DataTable();
        
        

        public Form1()
        {
            InitializeComponent();

            //Inicia las columnas de la tabla
            dt.Columns.Add("t", typeof(int));
            dt.Columns.Add("NumeroColiforme", typeof(Double));

            //Da formato a la tabla
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 10.125F, GraphicsUnit.Pixel);

            

        }

        //Valida los textBox para que no se pueda ingresar letras, sólo numeros usando la clase validar
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            validar.SoloNumeros(e);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            validar.SoloNumeros(e);
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            validar.SoloNumeros(e);
        }


        //Acciones al dar click en el boton calcular
        private void button1_Click(object sender, EventArgs e)
        {
            //Limpia los datos y la tabla de los datos para la nueva operación
            dt.Rows.Clear();
            dataGridView1.Rows.Clear();

            //Convierte los valores de los textbox a dobles.
            //Valida los datos de entrada
            try
            {
                y0 = Double.Parse(textBox1.Text);
                y1 = Convert.ToDouble(textBox2.Text);
                y2 = Convert.ToDouble(textBox3.Text);
               
            }
            catch (System.FormatException)
            {
                //Se ha producido un error de formato de ingreso de datos
                MessageBox.Show("El formato de dato ingresado no es correcto", "Error al ingresar los datos", MessageBoxButtons.OK);
            }

            //Verifica que los datos cumplan con el modelo

            if (validaModelo(y0, y1, y2, t) == true)
            {
                //Se asignan los valores de los labels con las respuestas de las fórmulas
                label9.Text = calculaB(y0, y1, y2);
                //label11.Text = calcula_y(y0, y1, y2, t);
                label10.Text = calcula_k(y0, y1, y2);

                //Limpia el gráfico antes de graficar
                limpiarGrafico();
                //Establece los parámetros del gráfico, valores mínimos y máximos
                configuraGrafico(y0, y1, y2);
                //Grafica
                grafica(y0, y1, y2, t);

                llenaTabla();

             
            }
            else
            {
                MessageBox.Show("Los datos ingresados no cumplen con el modelo, valor de constante K es negativo", "Datos no corresponden al modelo", MessageBoxButtons.OK);

            }

            
                
        }

        private void llenaTabla()
        {
            

            foreach(DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row[0], row[1]);
            }


        }

        private bool validaModelo(double val0, double val1, double val2, int t)
        {

            Formula1 = (val1 * ((val0 * val1) - (2 * val0 * val2) + (val1 * val2))) / ((val1 * val1) - (val0 * val2));
            Formula2 = (val0 * (val1 - val2)) / (val2 * (val0 - val1));
            Formula3 = (Formula1) / (1 + ((((Formula1 / val0)) - 1) * (Math.Pow(constanteE, (-0.6678 * 0)))));
            FormulaK = (Math.Log(Formula2) / Formula1) * -1;

            if (FormulaK <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }

            throw new NotImplementedException();
        }

        private void limpiarGrafico()
            {
                this.chart1.Series["Datos Experimentales"].Points.Clear();
                this.chart1.Series["datos"].Points.Clear();

            }
        

        private void configuraGrafico(double val0, double val1, double val2)
        {
            Formula1 = (val1 * ((val0 * val1) - (2 * val0 * val2) + (val1 * val2))) / ((val1 * val1) - (val0 * val2));
            Formula2 = (val0 * (val1 - val2)) / (val2 * (val0 - val1));
            Formula3 = (Formula1) / (1 + ((((Formula1 / val0)) - 1) * (Math.Pow(constanteE, (-0.6678 * 0)))));
            FormulaK = (Math.Log(Formula2) / Formula1) * -1;
            repeticiones =  Int16.Parse(textBox5.Text);

            //añade etiquetas al gráfico
            chart1.ChartAreas[0].AxisY.Title = "Número de Coliformes";
            chart1.ChartAreas[0].AxisX.Title = "Tiempo";
            //configura los valores de los ejes minimo y maximo
            this.chart1.ChartAreas[0].AxisY.Minimum = Convert.ToInt16(Formula3);
            this.chart1.ChartAreas[0].AxisY.Maximum = Convert.ToInt16(Formula1);
            this.chart1.ChartAreas[0].AxisX.Minimum = 0;
            this.chart1.ChartAreas[0].AxisX.Maximum = Convert.ToInt16(repeticiones);

        }

        private void grafica(double val0, double val1, double val2, int t)
        {
            
            Formula1 = (val1 * ((val0 * val1) - (2 * val0 * val2) + (val1 * val2))) / ((val1 * val1) - (val0 * val2));
            Formula2 = (val0 * (val1 - val2)) / (val2 * (val0 - val1));
            FormulaK = (Math.Log(Formula2) / Formula1) * -1;
            repeticiones = Int16.Parse(textBox5.Text);
            
            //Grafica la función y añade valores a la lista
            for (int i = 0; i < repeticiones; i++)
            {
                label = "t" + Convert.ToString(i);


                Formula3 = (Formula1) / (1 + ((((Formula1 / val0)) - 1) * (Math.Pow(constanteE, (-0.6678 * i)))));

                this.chart1.Series["datos"].Points.AddXY(i, Formula3);
                
                //Añade cada punto a la lista
                //listBox1.Items.Add( "t "+ i + ":     "+ Formula3.ToString("n2") ); //Lista normal
                dt.Rows.Add( (i) , Formula3.ToString("n0"));
                
            };
            
            //Grafica los puntos
            for (int i = 0; i < 3; i++)
            {
                label = "t" + Convert.ToString(i);
                Formula3 = (Formula1) / (1 + ((((Formula1 / val0)) - 1) * (Math.Pow(constanteE, (-0.6678 * i)))));

                


                this.chart1.Series["Datos Experimentales"].Points.AddXY(i, Formula3);
            };
            Formula3 = (Formula1) / (1 + ((((Formula1 / val0)) - 1) * (Math.Pow(constanteE, (-0.6678 * t)))));

            //Grafica el punto t en color rojo
            //this.chart1.Series["puntos"].Points.AddXY(t, Formula3);
            //this.chart1.Series["puntos"].Points[3].Color = Color.Red;
            this.chart1.Series["Datos Experimentales"].Points[0].Color = Color.Red;
            this.chart1.Series["Datos Experimentales"].Points[1].Color = Color.Red;
            this.chart1.Series["Datos Experimentales"].Points[2].Color = Color.Red;

        }

        private string calculaB(double val0, double val1, double val2)
        {
            //Fórmula 1 de la hoja "B"
            double Formula1 = (val1 * ((val0 * val1) - (2 * val0 * val2) + (val1 * val2))) / ((val1 * val1) - (val0 * val2));
            return (Convert.ToString(Formula1));
            throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Cuando carga el formulario se ejecuta este código

            //Inicia el temporizdor
            timer1.Start();
            //Muestra en los labels la fecha y hora
            lbl_fecha.Text = DateTime.Now.ToLongDateString();
            lbl_hora.Text = DateTime.Now.ToLongTimeString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Se Actualiza la hora con cada click del temporizador
            lbl_hora.Text = DateTime.Now.ToLongTimeString();
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            Pen p = new Pen(Color.Orange, 3);
            gfx.DrawLine(p, 0, 5, 0, e.ClipRectangle.Height - 2);
            gfx.DrawLine(p, 0, 5, 10, 5);
            gfx.DrawLine(p, 62, 5, e.ClipRectangle.Width - 2, 5);
            gfx.DrawLine(p, e.ClipRectangle.Width - 2, 5, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
            gfx.DrawLine(p, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);
        }

        private string calcula_y(double val0, double val1, double val2, int tiempo)
        {
            double Formula1 = (val1 * ((val0 * val1) - (2 * val0 * val2) + (val1 * val2))) / ((val1 * val1) - (val0 * val2));
            double Formula2 = (val0 * (val1 - val2)) / (val2 * (val0 - val1));
            double Formula3 = (Formula1) / (1 + ((((Formula1 / val0)) - 1) * (Math.Pow(constanteE, (-0.6678 * tiempo)))));
            return (Convert.ToString(Formula3));
            throw new NotImplementedException();
        }

        private string calcula_k(double val0, double val1, double val2)
        {
            double Formula1 = (val1 * ((val0 * val1) - (2 * val0 * val2) + (val1 * val2))) / ((val1 * val1) - (val0 * val2));
            double Formula2 = (val0 * (val1 - val2)) / (val2 * (val0 - val1));

            double FormulaK = (Math.Log(Formula2) / Formula1) * -1;
            return (Convert.ToString(FormulaK));
            throw new NotImplementedException();
        }


    }
}
