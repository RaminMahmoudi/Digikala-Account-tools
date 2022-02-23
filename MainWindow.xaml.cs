using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using LiveCharts.Helpers;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;

namespace Digikala_Account_tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }


        class items
                {
            public string Order_id { get; set; }
            public string Created_at { get; set; }
            public double Payable_price { get; set; }
            public string Cart_id { get; set; }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<items> it = new List<items>();
                List<items> it2 = new List<items>();
                List<items> it3 = new List<items>();
                List<items> it4 = new List<items>();
                string login = txt_User.Text;
                string pass = txt_Pass.Password;
                string formParams = "{\"username\":\"" + login + "\",\"password\":\"" + pass + "\"}";
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create("https://sirius.digikala.com/v1/user/login/");
                request1.Method = "POST";
                string postData = "{\"username\":\"" + login + "\",\"password\":\"" + pass + "\"}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request1.ContentType = "application/json";
                Stream dataStream = request1.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                HttpWebResponse response = (HttpWebResponse)request1.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string str1 = reader.ReadToEnd();
                if (str1.Contains("200"))
                {
                    HandyControl.Controls.Growl.Success("Login Successfully!");
                    Match token = Regex.Match(str1, "\"token\":\"(.*?)\"");
                    HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create("https://sirius.digikala.com/v1/profile/");
                    request2.Method = "GET";
                    request2.Headers.Add("authorization", token.Groups[1].Value);
                    HttpWebResponse Response2 = (HttpWebResponse)request2.GetResponse();
                    string str2 = new StreamReader(Response2.GetResponseStream()).ReadToEnd();

                    Match idnumber = Regex.Match(str2, "\"id\":(.*?),\"");
                    string id = Regex.Unescape(idnumber.Groups[1].Value);
                    control_AccInfo.txtbox_ID.Text = id;

                    Match firstname = Regex.Match(str2, "\"first_name\":\"(.*?)\"");
                    string fname = Regex.Unescape(firstname.Groups[1].Value);
                    control_AccInfo.txtbox_fname.Text = fname;

                    Match lastname = Regex.Match(str2, "\"last_name\":\"(.*?)\"");
                    string lname = Regex.Unescape(lastname.Groups[1].Value);
                    control_AccInfo.txtbox_lname.Text = lname;

                    Match phonenumber = Regex.Match(str2, "\"phone\":\"(.*?)\"");
                    string phone = Regex.Unescape(phonenumber.Groups[1].Value);
                    control_AccInfo.txtbox_phone.Text = phone;

                    Match cityid = Regex.Match(str2, "\"city_id\":(.*?),\"");
                    string cid = Regex.Unescape(cityid.Groups[1].Value);
                    control_AccInfo.txtbox_cityid.Text = cid;

                    Match mobilenumber = Regex.Match(str2, "\"mobile\":\"(.*?)\"");
                    string mobile = Regex.Unescape(mobilenumber.Groups[1].Value);
                    control_AccInfo.txtbox_mobile.Text = mobile;

                    Match emailaddress = Regex.Match(str2, "\"email\":\"(.*?)\"");
                    string email = Regex.Unescape(emailaddress.Groups[1].Value);
                    control_AccInfo.txtbox_email.Text = email;

                    Match national_identity_number = Regex.Match(str2, "\"national_identity_number\":\"(.*?)\"");
                    string identitynumber = Regex.Unescape(national_identity_number.Groups[1].Value);
                    control_AccInfo.txtbox_identity_number.Text = identitynumber;

                    Match cart_number = Regex.Match(str2, "\"cart_number\":\"(.*?)\"");
                    string cardnumber = Regex.Unescape(cart_number.Groups[1].Value);
                    control_AccInfo.txtbox_card_number.Text = cardnumber;

                    Match ibannumber = Regex.Match(str2, "\"iban\":\"(.*?)\"");
                    string iban = Regex.Unescape(ibannumber.Groups[1].Value);
                    control_AccInfo.txtbox_iban.Text = iban;

                    Match digiclubactive = Regex.Match(str2, "digiclub\":{\"is_activated\":(.*?),\"");
                    string digiclub = Regex.Unescape(digiclubactive.Groups[1].Value);
                    if (digiclub == "true")
                    {
                        control_AccInfo.toggle_DigiClub.IsChecked = true;
                    }

                    Match digiclubpoints = Regex.Match(str2, "\"points\":(.*?)},");
                    string points = Regex.Unescape(digiclubpoints.Groups[1].Value);
                    control_AccInfo.txtbox_club.Text = points;

                    Match digiplusactive = Regex.Match(str2, "digiplus\":{\"is_activated\":(.*?),\"");
                    string digiplus = Regex.Unescape(digiplusactive.Groups[1].Value);
                    if (digiplus == "true")
                    {
                        control_AccInfo.toggle_DigiPlus.IsChecked = true;
                    }

                    HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create("https://sirius.digikala.com/v1/wallet/cash-in/");
                    request3.Method = "GET";
                    request3.Headers.Add("authorization", token.Groups[1].Value);
                    HttpWebResponse Response3 = (HttpWebResponse)request3.GetResponse();
                    string str3 = new StreamReader(Response3.GetResponseStream()).ReadToEnd();

                    Match walletcredit = Regex.Match(str3, "\"credit\":(.*?),\"");
                    string wallet = Regex.Unescape(walletcredit.Groups[1].Value);
                    control_AccInfo.txtbox_wallet.Text = wallet;

                    HttpWebRequest request4 = (HttpWebRequest)WebRequest.Create("https://sirius.digikala.com/v1/init/");
                    request4.Method = "GET";
                    request4.Headers.Add("authorization", token.Groups[1].Value);
                    HttpWebResponse Response4 = (HttpWebResponse)request4.GetResponse();
                    string str4 = new StreamReader(Response4.GetResponseStream()).ReadToEnd();

                    Match fulladdress = Regex.Match(str4, "\"address\":\"(.*?)\"");
                    string address = Regex.Unescape(fulladdress.Groups[1].Value);
                    control_AccInfo.txtbox_adress.Text = address;

                    Match postal_code = Regex.Match(str4, "\"postal_code\":\"(.*?)\"");
                    string pcode = Regex.Unescape(postal_code.Groups[1].Value);
                    control_AccInfo.txtbox_postalcode.Text = pcode;

                    HttpWebRequest request5 = (HttpWebRequest)WebRequest.Create("https://sirius.digikala.com/v2/profile/order-tab/");
                    request5.Method = "GET";
                    request5.Headers.Add("authorization", token.Groups[1].Value);
                    HttpWebResponse Response5 = (HttpWebResponse)request5.GetResponse();
                    string str5 = new StreamReader(Response5.GetResponseStream()).ReadToEnd();

                    Match pendingcount = Regex.Match(str5, "\"pending\",\"count\":(.*?),\"");
                    string pending = Regex.Unescape(pendingcount.Groups[1].Value);
                    txt_pending.Text = "Pending: " + pending;

                    Match inprogresscount = Regex.Match(str5, "\"in_progress\",\"count\":(.*?),\"");
                    string inprogress = Regex.Unescape(inprogresscount.Groups[1].Value);
                    txt_inprogress.Text = "In Progress: " + inprogress;

                    Match sentcount = Regex.Match(str5, "\"sent\",\"count\":(.*?),\"");
                    string sent = Regex.Unescape(sentcount.Groups[1].Value);
                    txt_sent.Text = "Sent: " + sent;

                    Match cancelledcount = Regex.Match(str5, "\"cancelled\",\"count\":(.*?),\"");
                    string cancelled = Regex.Unescape(cancelledcount.Groups[1].Value);
                    txt_cancel.Text = "Cancelled: " + cancelled;

                    int page;
                    int EndPage;
                    string str6 = "";
                    EndPage = Convert.ToInt32(sent) / 20 + 1;
                    for (page = 1; page <= EndPage; page++)
                    {
                        HttpWebRequest request6 = (HttpWebRequest)WebRequest.Create("https://sirius.digikala.com/v2/profile/order/?page=" + page.ToString() + "&status=sent");
                        request6.Method = "GET";
                        request6.Headers.Add("authorization", token.Groups[1].Value);
                        HttpWebResponse Response6 = (HttpWebResponse)request6.GetResponse();
                        string str = new StreamReader(Response6.GetResponseStream()).ReadToEnd();
                        str6 += str;
                    }

                    Regex expression = new Regex("\"order_id\":(?<Identifier1>.*?),\"");
                    var results = expression.Matches(str6);
                    foreach (Match match in results)
                    {
                        it.Add(new items { Order_id = match.Groups["Identifier1"].Value.ToString() });

                    }

                    Regex expression2 = new Regex("\"cart_id\":(?<Identifier2>.*?),\"");
                    var results2 = expression2.Matches(str6);

                    foreach (Match match in results2)
                    {
                        it2.Add(new items { Cart_id = match.Groups["Identifier2"].Value.ToString() });
                    }

                    Regex expression3 = new Regex("\"created_at\":(?<Identifier3>.*?),\"");
                    var results3 = expression3.Matches(str6);
                    foreach (Match match in results3)
                    {
                        it3.Add(new items { Created_at = (match.Groups["Identifier3"].Value.ToString()).Replace("\"", null).Replace("\\", null) });
                    }

                    Regex expression4 = new Regex("\"payable_price\":(?<Identifier4>.*?),\"");
                    var results4 = expression4.Matches(str6);
                    foreach (Match match in results4)
                    {
                        it4.Add(new items { Payable_price = Convert.ToDouble(match.Groups["Identifier4"].Value) });
                    }
                    int i = 0;
                    foreach (var x in it2)
                    {
                        it[i].Cart_id = x.Cart_id;
                        i++;
                    }
                    i = 0;
                    List<string> list = new List<string>();
                    foreach (var x in it3)
                    {
                        it[i].Created_at = x.Created_at;
                        list.Add(x.Created_at);
                        i++;
                    }
                    i = 0;
                    var YChart = new ChartValues<double>();
                    foreach (var x in it4)
                    {
                        it[i].Payable_price = x.Payable_price;
                        YChart.Add(x.Payable_price);
                        i++;
                    }
                    DG.ItemsSource = it;

                    SeriesCollection = new SeriesCollection
                    {

                        new LineSeries
                        {
                            Values = YChart,
                            
                            Stroke = (Brush)Application.Current.Resources["PrimaryColor"],
                            Fill = (Brush)Application.Current.Resources["LightColor"],
                    Title ="Purchase Chart"
                        }
                    };
                    XFormatter = list.ToArray();
                    YFormatter = Values => Values.ToString();

                    DataContext = this;


                }
                else if (str1.Contains("300"))
                {
                    HandyControl.Controls.Growl.Error("Login Failed!");
                }
                else if (str1.Contains("500"))
                {
                    HandyControl.Controls.Growl.Warning("Please Enter Your Credentials!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HandyControl.Controls.Growl.Fatal("Check Your Connections!");
            }
        }

        private void btn_savecsv_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            saveFileDialog.FileName = "Order History";
            if (saveFileDialog.ShowDialog() == true)
            {
            DG.SelectAllCells();
            DG.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, DG);
            DG.UnselectAllCells();
            String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            File.AppendAllText(saveFileDialog.FileName, result, UnicodeEncoding.UTF8);
            }
        }

        private void btn_savetxt_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            saveFileDialog.FileName = "Order History";
            if (saveFileDialog.ShowDialog() == true)
            {
                DG.SelectAllCells();
                DG.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, DG);
                DG.UnselectAllCells();
                String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                File.AppendAllText(saveFileDialog.FileName, result, UnicodeEncoding.UTF8);
            }
        }
        public static BitmapSource RenderChartAsImage(FrameworkElement chart)
        {
            RenderTargetBitmap image = new RenderTargetBitmap((int)chart.ActualWidth, (int)chart.ActualHeight,92, 92, PixelFormats.Default);
            image.Render(chart);
            return image;
        }

        public static void SaveChartImage(FrameworkElement chart)
        {
            BitmapSource bitmap = RenderChartAsImage(chart);

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName += "mychart";
            saveDialog.DefaultExt = ".png";
            saveDialog.Filter = "Image (*.png)|*.png";

            if (saveDialog.ShowDialog() == true)
            {
                using (FileStream stream = File.Create(saveDialog.FileName))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(stream);
                }
            }
        }
        private void btn_savepng_Click(object sender, RoutedEventArgs e)
        {
            SaveChartImage(Order_Chart);
        }
    }
}