using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace cehasz_1_1_PROJEKT_KOLKO_KRZYRZYK_NxN_AI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        bool start = true;

        bool TURN = true; //rusza uzytkownik czy komp

        short wynikX = 0;

        short wynikO = 0;

        int NxN;

        int[] controlsum = { 0, 0, 0, 0 };


        List<Button> Btns = new List<Button>();

        private void button1_Click(object senderr, EventArgs ee)
        {


            try
            {
                if (textBox1.Text != "" && start == true && (double.Parse(textBox1.Text.ToString()) >= 3))
                {
                    int licz = 0;
                    NxN = int.Parse(textBox1.Text.ToString());
                    const int Button_WIDTH = 500;
                    const int Button_HEIGHT = 500;


                    for (int row = 0; row < NxN; row++)
                    {

                        for (int col = 0; col < NxN; col++)
                        {

                            Button DynamicButton = new Button();
                            DynamicButton.Width = Button_WIDTH / NxN;
                            DynamicButton.Height = Button_HEIGHT / NxN;
                            DynamicButton.Top = (row * ((Button_HEIGHT / NxN)));
                            DynamicButton.Left = (col * ((Button_WIDTH / NxN)));
                            Btns.Add(DynamicButton);
                            DynamicButton.Name = "0";
                            DynamicButton.Text = "";
                            DynamicButton.Font = new Font("Arial", 31 - NxN, FontStyle.Bold);



                            DynamicButton.Click += new EventHandler((sender, e) => DynamicButton_Click(sender, e));  //event handler z extra argumentami            

                            licz++;
                            DynamicButton.Enabled = true;
                            panel1.Controls.Add(DynamicButton);

                        }
                    }
                    start = false;
                    textBox1.Enabled = false;
                    checkBox1.Enabled = false;
                    trackBar1.Enabled = false;
                    button1.Enabled = false;
                    licz = 1;
                }
                else if (textBox1.Text == "")
                {

                    MessageBox.Show("stary to pustee", "jeest", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {

                    MessageBox.Show("Nie wciskaj tego teraz", "zresetuj lepiej wgl nie wiem", MessageBoxButtons.OK, MessageBoxIcon.Question);

                }
            }
            catch
            {
                MessageBox.Show("błąd", "nieprawidłowy format danych", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Reset();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            panel1.Controls.Clear();
            Btns.Clear();

            foreach (Button a in Btns)
            {
                a.Name = "0";
            }

            start = true;
            trackBar1.Enabled = true;
            textBox1.Enabled = true;
            checkBox1.Enabled = true;
            button1.Enabled = true;


            controlsum[0] = 0;
            controlsum[1] = 0;
            controlsum[2] = 0;
            controlsum[3] = 0;


        }


        private void DynamicButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int idprzycisku_AI;


            if (TURN == true)
            {
                button.Text = "X";
                button.Name = "1";
                label5.Text = "o";

                CHECK_WIN(sender);

                if (start == false)
                {
                    if (checkBox1.Checked == true)
                    {
                        idprzycisku_AI = GENERETE_AI_TURN(sender, e);
                        Btns[idprzycisku_AI].Text = "◯";
                        Btns[idprzycisku_AI].Name = "-1";
                        Btns[idprzycisku_AI].Enabled = false;
                        CHECK_WIN(idprzycisku_AI);      //Jak zamienic to na object??????????????????????????????????????????????????????/ 12.06.2022
                    }
                }
            }
            if (checkBox1.Checked == false)
            {
                if (TURN == false)
                {
                    button.Name = "-1";
                    button.Text = "◯";
                    label5.Text = "x";
                    CHECK_WIN(sender);
                }
            }





            button.Enabled = false;

            if (checkBox1.Checked == false)
                TURN = !TURN;
        }

        private void CHECK_WIN(object sender)
        {
            try
            {
                CHECK_WIN_EAST();
                CHECK_WIN_SOUTH();
                CHECK_WIN_SOUTHEASTn();
                CHECK_WIN_SOUTHWEST();
            }
            catch
            {

            }





            void CHECK_WIN_EAST()  //1 poziomo
            {
                controlsum[0] = 0;


                for (int i = 0; i < (NxN * NxN); i++)
                {



                    if (Btns[i].Name == "-1")
                    {

                        controlsum[0]--;


                    }
                    else if (Btns[i].Name == "1")
                    {

                        controlsum[0]++;

                    }
                    else
                    {
                        controlsum[0] = 0;
                    }

                    if (controlsum[0] == trackBar1.Value || controlsum[0] == -trackBar1.Value)
                    {
                        for (int a = 0; a < trackBar1.Value; a++)
                            Btns[i - a].BackColor = Color.Red;


                        WIN(sender);
                        break;
                    }

                    if (i % NxN == NxN - 1 || Btns[i].Name != Btns[i + 1].Name)      //zeby na krawedziach nie dodawalo kumasz i zeby po zmianie znakow zerowalo
                        controlsum[0] = 0;

                }

                controlsum[0] = 0;
            }

            void CHECK_WIN_SOUTH()  //2 pionowo
            {
                for (int b = 0; b < (NxN); b++)
                {
                    controlsum[1] = 0;

                    for (int i = b; i < (NxN * NxN); i += NxN)
                    {
                        if (Btns[i].Name == "-1")
                        {

                            controlsum[1]--;

                        }
                        else if (Btns[i].Name == "1")
                        {

                            controlsum[1]++;

                        }
                        else
                        {
                            controlsum[1] = 0;
                        }

                        if (controlsum[1] == trackBar1.Value || controlsum[1] == -trackBar1.Value)
                        {
                            for (int a = 0; a < trackBar1.Value; a++)
                                Btns[i - (a * NxN)].BackColor = Color.Red;

                            WIN(sender);
                            break;

                        }

                        for (int chceckbounds = 0; chceckbounds < NxN; chceckbounds++)  //te -2 itd wynikaja z tego z eni ma po co sprawdzac rogow macierzy
                            if (i == (NxN * NxN) - chceckbounds - 1)                        //sprawdzam czy nie wyszla za krawedz albo czy pod soba nie sa takie same
                            {
                                controlsum[1] = 0;
                            }

                        if (i < NxN * NxN - NxN)
                        {
                            if (Btns[i].Name != Btns[i + NxN].Name)
                            {
                                controlsum[1] = 0;
                            }
                        }
                    }

                }


                controlsum[1] = 0;

            }


            void CHECK_WIN_SOUTHEASTn()   //3 ukos w dół dolna polowa
            {
                int counterdown = 0;
                int counterup = 1;

                for (int i = NxN * NxN - NxN - 1; i >= NxN * trackBar1.Value - 1; i -= NxN)
                {
                    liczdol(i, counterup);
                    counterup++;
                }
                for (int i = NxN * NxN - 1; i >= (NxN * NxN) - NxN + trackBar1.Value - 1; i--)
                {
                    liczdol(i, counterdown);
                    counterdown += NxN;
                }

                void liczdol(int i, int counter)
                {
                    for (int a = counter; a <= i; a += NxN + 1)
                    {

                        if (Btns[a].Name == "-1")
                        {

                            controlsum[2]--;

                        }
                        else if (Btns[a].Name == "1")
                        {

                            controlsum[2]++;

                        }
                        else
                        {
                            controlsum[2] = 0;
                        }

                        if (controlsum[2] == trackBar1.Value || controlsum[2] == -trackBar1.Value)
                        {
                            for (int c = 0; c < trackBar1.Value; c++)
                                Btns[a - (c * (NxN + 1))].BackColor = Color.Red; 

                            WIN(sender);
                            break;

                        }

                        for (int chceckbounds = trackBar1.Value - 1; chceckbounds < NxN; chceckbounds++)
                        {
                            if (a == (NxN * NxN) - NxN + chceckbounds)  //przejscie miedzy rzedami na ukos
                            {
                                controlsum[2] = 0;
                            }
                            if (a == (NxN * chceckbounds) - 1)
                            {
                                controlsum[2] = 0;
                            }
                        }

                        if (a < NxN * NxN - NxN - 1)
                        {
                            if (Btns[a].Name != Btns[a + NxN + 1].Name)  //chodzi o to zeby po przejsciu miedzy polami na ukos
                            {
                                controlsum[2] = 0;
                            }

                        }


                    }
                }
            }


            void CHECK_WIN_SOUTHWEST()
            {
                bool WIN_ENDER_OF_FUNTION = false;

                if (WIN_ENDER_OF_FUNTION == false)
                {
                    for (int i = 0; i <= NxN - trackBar1.Value; i++)
                        for (int a = NxN - (NxN - trackBar1.Value) + i - 1; a <= (NxN * trackBar1.Value) - NxN + i * NxN; a += NxN - 1)
                        {

                            COUNT_WIN_POSIBILITIES_UP(a);
                        }

                    for (int i = 0; i < NxN - trackBar1.Value; i++)
                        for (int a = 2 * NxN - 1 + i * NxN; a <= NxN * NxN - NxN + i + 1; a += NxN - 1)
                        {

                            COUNT_WIN_POSIBILITIES_UP(a);
                        }
                }

                void COUNT_WIN_POSIBILITIES_UP(int a)
                {


                    if (Btns[a].Name == "-1")
                    {

                        controlsum[3]--;

                    }
                    else if (Btns[a].Name == "1")
                    {

                        controlsum[3]++;

                    }
                    else
                    {
                        controlsum[3] = 0;
                    }
                    if (controlsum[3] == trackBar1.Value || controlsum[3] == -trackBar1.Value)
                    {
                        for (int c = 0; c < trackBar1.Value; c++)
                            Btns[a - (c * (NxN + -1))].BackColor = Color.Red;

                        WIN(sender);
                        WIN_ENDER_OF_FUNTION = true;
                    }

                    for (int chceckbounds = trackBar1.Value - 1; chceckbounds < NxN; chceckbounds++)
                    {
                        if (a == (NxN * chceckbounds))             //przejscie miedzy rzedami na ukos gora
                        {
                            controlsum[3] = 0;
                        }
                        if (a == (NxN * NxN) - chceckbounds)             //przejscie miedzy rzedami na ukos dol
                        {
                            controlsum[3] = 0;
                        }

                    }

                    if (a <= NxN * NxN - NxN)
                    {
                        if (Btns[a].Name != Btns[a + NxN - 1].Name)  //chodzi o to zeby po przejsciu miedzy polami na ukos
                        {
                            controlsum[3] = 0;
                        }

                    }

                }
            }


        }

        private void WIN(object sender)
        {
            start = true;
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\pshad\source\repos\cehasz 1 1 PROJEKT KOLKO KRZYRZYK NxN AI\resources\nice.wav");
            player.Play();
            MessageBox.Show("Nastąpiło zakreslenie " + trackBar1.Value + " tych samych figur, koniec rundy", "jeest", MessageBoxButtons.OK, MessageBoxIcon.None);
            Button button = sender as Button;
            if (button.Name == "1")
            {
                if (checkBox1.Checked == true)
                {
                    wynikO++;
                    label8.Text = wynikO.ToString();
                }
            }
            if (button.Name == "-1")
            {
                if (checkBox1.Checked == true)
                {
                    wynikX++;
                    label9.Text = wynikX.ToString();
                }
            }

            Reset();

        }

        private int GENERETE_AI_TURN(object sender, EventArgs e)
        {
            Button button = sender as Button;

            int TURNAI = 0;

            if (NxN == 3)
            {
                TURNAI = losowyTURN(sender, e);
            }
            else
            {


                TURNAI = WyliczTURN();
                if (TURNAI == 0)
                {
                    TURNAI = losowyTURN(sender, e);
                }

            }



            return (TURNAI);
        }

        private int losowyTURN(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Button button = sender as Button;

            bool wylosowano = false;
            bool ostatniTURN = false;
            int RANDOMpole = 0;

            for (int i = 0; i < NxN * NxN; i++)
            {
                try
                {
                    if (int.Parse(Btns[i].Name) == 0)
                    {
                        ostatniTURN = true;
                    }
                }
                catch
                {

                }
            }

            while (wylosowano == false && ostatniTURN == true)
            {
                RANDOMpole = rnd.Next(NxN * NxN);
                if (int.Parse(Btns[RANDOMpole].Name) == 0)
                {
                    Btns[RANDOMpole].Text = "◯";
                    Btns[RANDOMpole].Name = "-1";
                    wylosowano = true;
                    Btns[RANDOMpole].Enabled = false;

                }

            }
            return (RANDOMpole);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (trackBar1.Value <= int.Parse(textBox1.Text))
                {


                    if (int.Parse(textBox1.Text) < 30)
                    {


                        if (start == true)
                        {
                            label4.Text = trackBar1.Value.ToString();
                        }
                        else
                        {
                            trackBar1.Enabled = false;
                        }

                    }
                    else
                    {
                        trackBar1.Enabled = false;
                        MessageBox.Show("wez tak mniej niz 30 daj ten rozmiar bo nic nie zobaczysz i tak mordo", "okej", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("poziom trudnosi musi byc <= rozmiar macierzy", "tak jest lepiej zaufaj", MessageBoxButtons.OK);
                }
            }
            catch
            {
                MessageBox.Show("najpierw wpisz cos w rozmiarze macierzy", "dobrze?", MessageBoxButtons.OK);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (trackBar1.Value <= int.Parse(textBox1.Text))
                {
                    trackBar1.Enabled = true;
                }
                else if (textBox1.Text == "")
                {

                }
            }
            catch
            {
                if (textBox1.Text != "")
                    MessageBox.Show("błąd", "nieprawidłowy format danych", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Text = "";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label2.Visible = false;
                label5.Visible = false;
                Reset();

            }
            if (checkBox1.Checked == false)
            {
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label2.Visible = true;
                label5.Visible = true;
                Reset();
            }
        }

        private int WyliczTURN()
        {
            int WyliczonePole = 0;


            for (int CZY_WYGRA = 1; CZY_WYGRA <= trackBar1.Value; CZY_WYGRA++)
            {
                if (WyliczonePole == 0)   //na wypadek gdy jednak w jakiś sposob nie wyliczy zadnego TURNu
                {

                    CHECK_WIN_EAST(CZY_WYGRA);
                    CHECK_WIN_SOUTH(CZY_WYGRA);
                    CHECK_WIN_SOUTHEASTn(CZY_WYGRA);
                    CHECK_WIN_SOUTHWEST(CZY_WYGRA);

                }

            }

            void CHECK_WIN_EAST(int CZY_WYGRA)  //1 poziomo
            {
                controlsum[0] = 0;


                for (int i = 0; i < (NxN * NxN); i++)
                {



                    if (Btns[i].Name == "-1")
                    {

                        controlsum[0]--;


                    }
                    else if (Btns[i].Name == "1")
                    {

                        controlsum[0]++;

                    }
                    else
                    {
                        controlsum[0] = 0;
                    }

                    if (controlsum[0] == (trackBar1.Value - CZY_WYGRA) || controlsum[0] == -(trackBar1.Value - CZY_WYGRA))
                    {

                        try
                        {
                            if (Btns[i + 1].Name == "0")
                            {
                                WyliczonePole = i + 1;
                                //Btns[WyliczonePole].BackColor = Color.Blue;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Btns[i - ((trackBar1.Value - CZY_WYGRA))].Name == "0")
                            {
                                WyliczonePole = i - ((trackBar1.Value - CZY_WYGRA));
                                //Btns[WyliczonePole].BackColor = Color.Blue;
                            }
                        }
                        catch
                        {

                        }





                    }

                    if (i % NxN == NxN - 1 || Btns[i].Name != Btns[i + 1].Name)      //zeby na krawedziach nie dodawalo  i zeby po zmianie znakow zerowalo
                        controlsum[0] = 0;

                }

                controlsum[0] = 0;
            }

            void CHECK_WIN_SOUTH(int CZY_WYGRA)  //2 pionowo
            {
                for (int b = 0; b < (NxN); b++)
                {
                    controlsum[1] = 0;

                    for (int i = b; i < (NxN * NxN); i += NxN)
                    {
                        if (Btns[i].Name == "-1")
                        {

                            controlsum[1]--;

                        }
                        else if (Btns[i].Name == "1")
                        {

                            controlsum[1]++;

                        }
                        else
                        {
                            controlsum[1] = 0;
                        }

                        if (controlsum[1] == (trackBar1.Value - CZY_WYGRA) || controlsum[1] == -(trackBar1.Value - CZY_WYGRA))
                        {

                            try
                            {
                                if (Btns[(i - ((trackBar1.Value - CZY_WYGRA) * NxN))].Name == "0")
                                {
                                    WyliczonePole = (i - ((trackBar1.Value - CZY_WYGRA) * NxN));
                                    // Btns[WyliczonePole].BackColor = Color.Blue;
                                }
                            }
                            catch
                            {

                            }
                            try
                            {
                                if (Btns[i - ((trackBar1.Value - CZY_WYGRA - 1) * NxN) + 2 * NxN].Name == "0")
                                {
                                    WyliczonePole = i - ((trackBar1.Value - CZY_WYGRA - 1) * NxN) + 2 * NxN;
                                    //Btns[WyliczonePole].BackColor = Color.Blue;
                                }

                            }
                            catch
                            {

                            }


                        }

                        for (int chceckbounds = 0; chceckbounds < NxN; chceckbounds++)  //te - itd wynikaja z tego z eni ma po co sprawdzac rogow macierzy
                            if (i == (NxN * NxN) - chceckbounds - 1)                        //sprawdzam czy nie wyszla za krawedz albo czy pod soba nie sa takie same
                            {
                                controlsum[1] = 0;
                            }

                        if (i < NxN * NxN - NxN)
                        {
                            if (Btns[i].Name != Btns[i + NxN].Name)
                            {
                                controlsum[1] = 0;
                            }
                        }
                    }

                }


                controlsum[1] = 0;

            }


            void CHECK_WIN_SOUTHEASTn(int CZY_WYGRA)   //3 ukos w dół dolna polowa
            {
                int counterdown = 0;
                int counterup = 1;

                for (int i = NxN * NxN - NxN - 1; i >= NxN * (trackBar1.Value - CZY_WYGRA) - 1; i -= NxN)
                {
                    liczdol(i, counterup);
                    counterup++;
                }
                for (int i = NxN * NxN - 1; i >= (NxN * NxN) - NxN + (trackBar1.Value - CZY_WYGRA) - 1; i--)
                {
                    liczdol(i, counterdown);
                    counterdown += NxN;
                }

                void liczdol(int i, int counter)
                {
                    for (int a = counter; a <= i; a += NxN + 1)
                    {

                        if (Btns[a].Name == "-1")
                        {

                            controlsum[2]--;

                        }
                        else if (Btns[a].Name == "1")
                        {

                            controlsum[2]++;

                        }
                        else
                        {
                            controlsum[2] = 0;
                        }

                        if (controlsum[2] == (trackBar1.Value - CZY_WYGRA) || controlsum[2] == -(trackBar1.Value - CZY_WYGRA))
                        {

                            try
                            {
                                if (Btns[a + NxN + 1].Name == "0")
                                {
                                    WyliczonePole = a + NxN + +1;
                                    // Btns[WyliczonePole].BackColor = Color.Blue;
                                }
                            }
                            catch
                            {

                            }
                            try
                            {
                                if (Btns[a - ((trackBar1.Value - CZY_WYGRA) * (NxN + 1))].Name == "0")
                                {
                                    WyliczonePole = a - ((trackBar1.Value - CZY_WYGRA) * (NxN + 1));
                                    // Btns[WyliczonePole].BackColor = Color.Blue;
                                }
                            }
                            catch
                            {

                            }




                        }

                        for (int chceckbounds = (trackBar1.Value - CZY_WYGRA) - 1; chceckbounds < NxN; chceckbounds++)
                        {
                            if (a == (NxN * NxN) - NxN + chceckbounds)  //przejscie miedzy rzedami na ukos
                            {
                                controlsum[2] = 0;
                            }
                            if (a == (NxN * chceckbounds) - 1)
                            {
                                controlsum[2] = 0;
                            }
                        }

                        if (a < NxN * NxN - NxN - 1)
                        {
                            if (Btns[a].Name != Btns[a + NxN + 1].Name)  //chodzi o to zeby po przejsciu miedzy polami na ukos
                            {
                                controlsum[2] = 0;
                            }

                        }


                    }
                }
            }


            void CHECK_WIN_SOUTHWEST(int CZY_WYGRA)
            {
                for (int i = 0; i <= NxN - trackBar1.Value; i++)
                    for (int a = NxN - (NxN - trackBar1.Value) + i - 1; a <= (NxN * trackBar1.Value) - NxN + i * NxN; a += NxN - 1)
                    {

                        COUNT_WIN_POSIBILITIES_UP(a);
                    }

                for (int i = 0; i < NxN - trackBar1.Value; i++)
                    for (int a = 2 * NxN - 1 + i * NxN; a <= NxN * NxN - NxN + i + 1; a += NxN - 1)
                    {

                        COUNT_WIN_POSIBILITIES_UP(a);
                    }

                void COUNT_WIN_POSIBILITIES_UP(int a)
                {


                    if (Btns[a].Name == "-1")
                    {

                        controlsum[3]--;

                    }
                    else if (Btns[a].Name == "1")
                    {

                        controlsum[3]++;

                    }
                    else
                    {
                        controlsum[3] = 0;
                    }
                    if (controlsum[3] == (trackBar1.Value - CZY_WYGRA) || controlsum[3] == -(trackBar1.Value - CZY_WYGRA))
                    {


                        try
                        {
                            if (Btns[a + NxN - 1].Name == "0")
                            {
                                WyliczonePole = a + NxN - 1;
                                //  Btns[WyliczonePole].BackColor = Color.Blue;
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (Btns[a - ((trackBar1.Value - CZY_WYGRA) * (NxN - 1))].Name == "0")
                            {
                                WyliczonePole = a - ((trackBar1.Value - CZY_WYGRA) * (NxN - 1));
                                //  Btns[WyliczonePole].BackColor = Color.Blue;
                            }
                        }
                        catch
                        {

                        }



                    }

                    for (int chceckbounds = (trackBar1.Value - CZY_WYGRA) - 1; chceckbounds < NxN; chceckbounds++)
                    {
                        if (a == (NxN * chceckbounds))             //przejscie miedzy rzedami na ukos gora
                        {
                            controlsum[3] = 0;
                        }
                        if (a == (NxN * NxN) - chceckbounds)             //przejscie miedzy rzedami na ukos dol
                        {
                            controlsum[3] = 0;
                        }

                    }

                    if (a <= NxN * NxN - NxN)
                    {
                        if (Btns[a].Name != Btns[a + NxN - 1].Name)  //chodzi o to zeby po przejsciu miedzy polami na ukos
                        {
                            controlsum[3] = 0;
                        }

                    }

                }
            }

            return (WyliczonePole);

        }  //pewnie to try catch nie jest zgodne ze sztuką, ale szczędza jakieś 200 linijek kodu na implementowanie warunków sprawdzania możliwości TURNu.


    }

}



