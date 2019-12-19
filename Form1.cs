using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transitions;

namespace SigloXXITotem
{
    public partial class Form1 : Form
    {
        #region ArrarstarForm

        //METODO PARA ARRASTRAR EL FORMULARIO---------------------------------------------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        #endregion
        


        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbfont, uint cbfont, IntPtr pdv, [In] ref uint pcFonts);

        FontFamily ff;
        Font font;

        private void loadFont()
        {
            byte[] fontArray = SigloXXITotem.Properties.Resources.Pacifico;
            int dataLength = SigloXXITotem.Properties.Resources.Pacifico.Length;

            IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);

            Marshal.Copy(fontArray, 0, ptrData, dataLength);

            uint cFonts = 0;

            AddFontMemResourceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref cFonts);

            pfc.AddMemoryFont(ptrData, dataLength);

            Marshal.FreeCoTaskMem(ptrData);

            ff = pfc.Families[0];
            font = new Font(ff, 15f, FontStyle.Regular);
        }

        private void AllocFont(Font f, Control c, float size)
        {
            FontStyle fontStyle = FontStyle.Regular;

            c.Font = new Font(ff, size, fontStyle);
        }



        //El id de cada una de las mesas en la base de datos ordenadas según su numero real EJ: la mesa[0] (real mesa 1) tiene el id 321, la mesa[1] = 341, etc.. (ubiese sido mejor que la mesa 1 tuviese id 1 y así)
        private Image boton_verde;
        private Image boton_rojo;
        private Image usuario;
        PrivateFontCollection pfc = new PrivateFontCollection();        
        List<PictureBox> listabotones = new List<PictureBox>();
        List<string> words;
        string[] URLS = System.IO.File.ReadAllLines(@"Config/URL.txt");





        public Form1()
        {

            InitializeComponent();
            loadFont();
            AllocFont(font, lblBoton, 55);
            AllocFont(font, lblDisponible, 18);
            AllocFont(font, lblOcupada, 18);
            AllocFont(font, lblCap, 18);
            AllocFont(font, lblCarga, 50);
            AllocFont(font, lblSalir, 18);

            
            

            boton_verde = SigloXXITotem.Properties.Resources.Boton_Verde;
            boton_rojo = SigloXXITotem.Properties.Resources.Boton_Rojo;
            usuario = SigloXXITotem.Properties.Resources.persona;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            iconoPersona.BackColor = Color.Transparent;           
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer,
            true);
            
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, panelContenedor, new object[] { true });
            
            
            flechaIzq.Visible = false;            
            
            panelPrueba.Width = panelPrueba.Width * 2;
        }

        
        private void LimpiarBotones()
        {
            panelPrueba.Controls.Clear();
        }
        
        

        private async Task NuevoListar()
        {
            
            List<Mesa> model = null;
            var client = new HttpClient();
            
            var task = await client.GetAsync(URLS[0]);
            var jsonString = await task.Content.ReadAsStringAsync();
            try
            {
                model = JsonConvert.DeserializeObject<List<Mesa>>(jsonString);


                int cantMesas = model.Count;

                int x2 = 110;
                int x4 = 110;
                int x6 = 110;
                int y2 = 20;
                int y4 = 176;
                int y6 = 332;                
                int contMesas2 = 0;
                int contMesas4 = 0;
                int contMesas6 = 0;

                for (int i = 0; i < model.Count; i++)
                {
                    if (model[i].capacidad == 2)
                    {

                        if (contMesas2 == 0)
                        {
                            CrearBotonDinamico(model[i].nombre, x2, y2, model[i].id, model[i].capacidad, model[i].estado);

                            contMesas2 += 1;
                        }
                        else if (contMesas2 < 5)
                        {
                            x2 += 196;
                            CrearBotonDinamico(model[i].nombre, x2, y2, model[i].id, model[i].capacidad, model[i].estado);
                            contMesas2 += 1;
                        }
                        else
                        {
                            
                            if (contMesas2 == 5)
                            {
                                x2 += 353;
                                CrearBotonDinamico(model[i].nombre, x2, y2, model[i].id, model[i].capacidad, model[i].estado);
                                contMesas2 += 1;
                            }
                            else
                            {
                                x2 += 196;
                                CrearBotonDinamico(model[i].nombre, x2, y2, model[i].id, model[i].capacidad, model[i].estado);
                                contMesas2 += 1;
                            }

                        }

                    }
                    else if (model[i].capacidad == 4)
                    {
                        if (contMesas4 == 0)
                        {
                            CrearBotonDinamico(model[i].nombre, x4, y4, model[i].id, model[i].capacidad, model[i].estado);

                            contMesas4 += 1;
                        }
                        else if (contMesas4 < 5)
                        {
                            x4 += 196;
                            CrearBotonDinamico(model[i].nombre, x4, y4, model[i].id, model[i].capacidad, model[i].estado);
                            contMesas4 += 1;
                        }
                        else
                        {

                            
                            if (contMesas4 == 5)
                            {
                                x4 += 353;
                                CrearBotonDinamico(model[i].nombre, x4, y4, model[i].id, model[i].capacidad, model[i].estado);
                                contMesas4 += 1;
                            }
                            else
                            {
                                x4 += 196;
                                CrearBotonDinamico(model[i].nombre, x4, y4, model[i].id, model[i].capacidad, model[i].estado);
                                contMesas4 += 1;
                            }



                        }
                    }
                    else
                    {
                        if (contMesas6 == 0)
                        {
                            CrearBotonDinamico(model[i].nombre, x6, y6, model[i].id, model[i].capacidad, model[i].estado);

                            contMesas6 += 1;
                        }
                        else if(contMesas6 < 5)
                        {
                            x6 += 196;
                            CrearBotonDinamico(model[i].nombre, x6, y6, model[i].id, model[i].capacidad, model[i].estado);
                            contMesas4 += 1;

                        }
                        else
                        {
                            if (contMesas6 == 5)
                            {
                                x6 += 353;
                                CrearBotonDinamico(model[i].nombre, x6, y6, model[i].id, model[i].capacidad, model[i].estado);
                                contMesas6 += 1;
                            }
                            else
                            {
                                x6 += 196;
                                CrearBotonDinamico(model[i].nombre, x6, y6, model[i].id, model[i].capacidad, model[i].estado);
                                contMesas6 += 1;
                            }
                        }
                    }
                    
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("ERROR: NO SE PUEDE CREAR UNA LISTA TIPO MESA CON EL JSON " + jsonString);
                MessageBox.Show("ERROR DE LECTURA DEL JSON");
            }
            

        }
        
        private bool PedirConfirmacion(string nombre)
        {
            Label mensaje = new Label();
            Confirmacion confirmacion = new Confirmacion();

            confirmacion.lblMensaje.Font = new System.Drawing.Font("Tw Cen MT Condensed Extra Bold", 32, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            confirmacion.lblMensaje.Text = "¿Esta seguro que desea seleccionar" + " la " + nombre + "?";
            confirmacion.lblMensaje.Location = new Point(12, 41);
            TiempoEspera.Stop();
            var resultado = confirmacion.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                confirmacion.Dispose();
                return true;
                
            }
            else
            {
                confirmacion.Dispose();
                TiempoEspera.Start();
                return false;

            }
            

        }
        private void CrearBotonDinamico(string nombre, int x, int y, int idMesa, int capacidad, int estado)
        {
            

            PictureBox user = new PictureBox();
            PictureBox boton = new PictureBox();
            Label texto = new Label();
            Label texto2 = new Label();            
            //Propiedades del boton (imagen)
            boton.Height = 133;
            boton.Width = 133;
            if (estado == 0)
            {
                boton.Image = boton_verde; 
            }
            else
            {        
                boton.Image = boton_rojo;
            }                 

            boton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            boton.Cursor = System.Windows.Forms.Cursors.Hand;
            boton.ForeColor = Color.White;
            boton.Location = new Point(x, y);
            boton.Name = nombre;
            boton.TabIndex = 2;
            //Propiedades del texto              
            texto.Font = new System.Drawing.Font("Tw Cen MT Condensed Extra Bold", 22, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            texto2.Font = new System.Drawing.Font("Tw Cen MT Condensed Extra Bold", 22, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            boton.Click += async (sender, EventArgs) => { DynamicButton_Click(sender, EventArgs, nombre, idMesa, capacidad); };
            //propiedades del icono usuario
            user.Image = usuario;
            user.Height = 35;
            user.Width = 35;
            user.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;


            panelPrueba.Controls.Add(boton);
            
            boton.SendToBack();
            user.BackColor = Color.Transparent;
            user.Parent = boton;
            user.Location = new Point((boton.Width / 2) -30 , 74);

            boton.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                
                SizeF textSize = e.Graphics.MeasureString(nombre, Font);
                PointF locationToDraw = new PointF();
                locationToDraw.X = (boton.Width / 2) - (textSize.Width / 2 + 26);
                locationToDraw.Y = (boton.Height / 2) - (textSize.Height / 2) - 22;

                e.Graphics.DrawString(nombre, texto.Font, Brushes.White, locationToDraw);
            });
            boton.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                SizeF textSize = e.Graphics.MeasureString(nombre, Font);
                PointF locationToDraw = new PointF();
                locationToDraw.X = (boton.Width / 2) - (textSize.Width / 2) + 25;
                locationToDraw.Y = (boton.Height / 2) - (textSize.Height / 2) + 18;

                e.Graphics.DrawString(capacidad.ToString(), texto2.Font, Brushes.White, locationToDraw);
            });            
            
        }

        //PROGRAMACION ASINCRONA 
        //EJECUTAR CODIGO CON DEMASIADA CARGA EN UN NUCLEO PARALELO DEL PROCESADOR PARA NO CONGELAR LA APLICACION DURANTE SU EJECUCIÓN
        private async void DynamicButton_Click(object sender, EventArgs e, string nombre, int idMesa, int cant)
        {
            PictureBox btn = sender as PictureBox;

            if (btn.Image == boton_verde)
            {
                if (PedirConfirmacion(nombre))
                {

                    lblResultado.Text = ("Ha seleccionado la " + nombre + Environment.NewLine + " puede dirigirse a su mesa");                                     

                    panel1.Visible = true;
                    await Task.Run(() => CambiarEstado(idMesa)); //TAREA ASINCRONA (ENVÍA UN DATO AL SERVIDOR Y ESPERA LA RESPUESTA) 
                    await Task.Delay(3500);
                    CambiarColorBoton(btn);
                           
                    panel1.Visible = false;
                    panelInicio.Visible = true;
                    LimpiarBotones();
            }
           }            
        }

        

        #region Funciones JSON

        private void CambiarEstado(int idMesa)
        {
            ClienteREST clienteR = new ClienteREST();
            clienteR.uri = URLS[1] + idMesa;
            string strRespuesta;
            clienteR.httpMethod = httpVerb.PUT;
            strRespuesta = clienteR.MakeRequest();
            Debug.WriteLine(strRespuesta);
        }




        #endregion 

        





        //METODO PARA CAMBIAR DE COLOR EL BOTON EN EVENTOS 
        private void CambiarColorBoton(PictureBox img)
        {
            if (img.Image == boton_verde)
            {
                img.Image = boton_rojo;       
            }
            else
            {
                img.Image = boton_verde;
                
            }
        }

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnRestaurar.Visible = true;
            btnMaximizar.Visible = false;
        }                

        private void button2_Click(object sender, EventArgs e)
        {
            //panelInicio.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer,
            true);
            
        }

        private void flechaIzq_Click(object sender, EventArgs e)
        {
            Transition t = new Transition(new TransitionType_EaseInEaseOut(500));
            t.add(panelPrueba, "Left", 111);            
            t.run();
            flechaDer.Visible = true;
            flechaIzq.Visible = false;
        }


        
        private void flechaDer_Click(object sender, EventArgs e)
        {
            Transition t = new Transition(new TransitionType_EaseInEaseOut(500));
            t.add(panelPrueba, "Left", -1027);
            t.run();
            flechaIzq.Visible = true;
            flechaDer.Visible = false;
            
        }
              

        private async void botonSeleccionar_Click(object sender, EventArgs e)
        {
            btnBlanco.Visible = false;
            lblBoton.Visible = false;            
            carga.Visible = true;
            lblCarga.BringToFront();
            flechaDer.Visible = true;
            flechaIzq.Visible = false;
            lblCarga.Visible = true;
            Transition t = new Transition(new TransitionType_EaseInEaseOut(500));
            t.add(panelPrueba, "Left", 111);
            t.run();
            await NuevoListar();


            
            panelInicio.Visible = false;
            btnBlanco.Visible = true;
            lblBoton.Visible = true;
            carga.Visible = false;
            lblCarga.Visible = false;
            



            TiempoEspera.Start();
        }

        private void TiempoEspera_Tick(object sender, EventArgs e)
        {
            TiempoEspera.Stop();
            panelInicio.Visible = true;
            LimpiarBotones();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            LimpiarBotones();
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            await NuevoListar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            TiempoEspera.Stop();
            panelInicio.Visible = true;
            LimpiarBotones();
        }

    }
}
