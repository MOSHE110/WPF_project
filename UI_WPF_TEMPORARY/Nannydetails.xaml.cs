﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using BE;
using BL;
using RoyT.TimePicker;
namespace UI_WPF_TEMPORARY
{
    /// <summary>
    /// Interaction logic for Nannydetails.xaml
    /// </summary>
    public partial class Nannydetails : UserControl
    {
        public Window fr;
        //public static BL_imp bl = new BL_imp();
        public IBL bl;
        public DigitalTime MinTime { get { return new DigitalTime(00, 00); } set { } }
        public DigitalTime MaxTime { get { return new DigitalTime(22, 00); } set { } }
        public DigitalTime Time { get { return new DigitalTime(8, 00); } set { } }
        public bool isUpdate = false;
        Nanny nanny;
        NannyT details;
        System.Threading.Thread t = null;
        public Nannydetails(Window f,Nanny nanny=null,bool IsSaveable=true)
        {
            InitializeComponent();
            bl = FactoryBL.IBLInstance;
            if (IsSaveable == false)
                NannySavebutton.Visibility = Visibility.Collapsed;
            this.nanny = nanny;
            new_Birthdate.DisplayDateEnd = DateTime.Today;
            if (nanny == null)
                details = new NannyT(new Nanny(), this);
            else
                details = new NannyT(nanny, this);
            var values = from Enum e in Enum.GetValues(typeof(MyEnum.Vacation))
                         select new { ID = e, Name = e.ToString() };
            foreach (var value in values)
            {
                vacationmethod.Items.Add(value.Name);
            }
            this.DataContext = details;
            fr = f;
            isUpdate = false;
            if (nanny != null)
            {
                isUpdate = true;

                vacationmethod.Text = nanny.Vacation_days.ToString();

            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9]+");
            e.Handled = (regex.IsMatch(e.Text) ||
                        (new_Phonenumber.Text.ToString().Count() > 9) || 
                        (new_Phonenumber.Text.ToString().Count()==0  && e.Text[0] != '0'));
        }
    private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /* = new Nanny(new_ID.Text, new_LastName.Text, new_Firstname.Text
                                        , new_Birthdate.Text, new_Phonenumber.Text, new_Address.Text,
                                        new_elevator_exists.IsChecked == true,
                                        new_Floor.Text, new_experience.Text, new_Max_Num_Kids.Text,
                                        new_min_age.Text, new_max_age.Text, possible_hourly_rate.IsChecked == true,
                                        new_hourrate.Text, new_monthlyrate.Text,
                                        new bool[] {new_Sunday.IsChecked==true, new_Monday.IsChecked == true ,
                                    new_Tuesday.IsChecked==true,new_Wednesday.IsChecked==true,new_Thursday.IsChecked==true,
                                    new_Friday.IsChecked==true,new_Saturday.IsChecked==true},
                                        new TimeSpan[,] { {new_Sunday_start.Time.ToTimeSpan(),new_Sunday_end.Time.ToTimeSpan() },
                                    {new_Monday_start.Time.ToTimeSpan(),new_Monday_end.Time.ToTimeSpan() },
                                    {new_Tuesday_start.Time.ToTimeSpan(),new_Tuesday_end.Time.ToTimeSpan() },
                                    {new_Wednesday_start.Time.ToTimeSpan(),new_Wednesday_end.Time.ToTimeSpan() },
                                    {new_Thursday_start.Time.ToTimeSpan(),new_Thursday_end.Time.ToTimeSpan() },
                                    {new_Friday_start.Time.ToTimeSpan(),new_Friday_end.Time.ToTimeSpan() },
                                    {new_Saturday_start.Time.ToTimeSpan(),new_Saturday_end.Time.ToTimeSpan() }},
                                        vacationmethod.Text, new_recommendations.Text, new_additional_info.Text);*/
                if (isUpdate)
                    bl.UpdateNanny(details.Info);
                else
                    bl.AddNanny(details.Info);
                fr.Close();
            }
            catch (Exception w)
            {
                MessageBox.Show(w.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            TimePickerSlider slider;
            if (((CheckBox)(sender)).IsChecked == true)
            {
                slider = (TimePickerSlider)FindName("new_" + ((CheckBox)(sender)).Content + "_start");
                slider.Visibility = Visibility.Visible;
                slider = (TimePickerSlider)FindName("new_" + ((CheckBox)(sender)).Content + "_end");
                slider.Visibility = Visibility.Visible;
            }
            else
            {
                slider = (TimePickerSlider)FindName("new_" + ((CheckBox)(sender)).Content + "_start");
                slider.Visibility = Visibility.Collapsed;
                slider = (TimePickerSlider)FindName("new_" + ((CheckBox)(sender)).Content + "_end");
                slider.Visibility = Visibility.Collapsed;
            }

        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            NannySavebutton.Height += 5;
            NannySavebutton.Width += 5;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            NannySavebutton.Height -= 5;
            NannySavebutton.Width -= 5;
        }
        private void new_ID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = (regex.IsMatch(e.Text) ||
                        (new_ID.Text.ToString().Count() > 8)
                        /* ||(new_ID.Text.ToString().Count() == 0 && e.Text[0] != '0')*/);
        }
        private void new_Address_TextChanged(object sender, TextChangedEventArgs e)
        {
            new_Address_TextChangedHelp(Address_suggestion, new_Address);
        }
        private void new_Address_TextChangedHelp(ListBox Address_suggestion, TextBox new_Address)
        {
            List<String> s = new List<String>();
            string str = new_Address.Text;

            if (t != null && t.IsAlive)
                t.Abort();
            t = new System.Threading.Thread(() =>
            {
                try
                {
                    s = BL.BL_imp.GetPlaceAutoComplete(str);
                }
                catch (Exception f)
                {
                    // fix Nanny?
                    int a = 5;
                }
            });
            t.Start();
            t.Join();
            if (s.Contains(str))
            {
                new_Address.Text = str;
                Address_suggestion.Visibility = Visibility.Collapsed;
                Address_suggestion.ItemsSource = null;
                return;
            }
            if (s.Count() > 0)
            {
                Address_suggestion.ItemsSource = s;
                Address_suggestion.Visibility = Visibility.Visible;
            }
            else
            {
                Address_suggestion.Visibility = Visibility.Collapsed;
                Address_suggestion.ItemsSource = null;
            }

        }
        private void surround_Address_suggestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Address_suggestion_SelectionChangedhelp(Address_suggestion, new_Address, new TextChangedEventHandler(new_Address_TextChanged));
        }
        private void Address_suggestion_SelectionChangedhelp(ListBox suggestion, TextBox Address, TextChangedEventHandler a)
        {
            if (suggestion.ItemsSource != null)
            {
                suggestion.Visibility = Visibility.Collapsed;
                Address.TextChanged -= a;
                if (suggestion.SelectedIndex != -1)
                {
                    Address.Text = suggestion.SelectedItem.ToString();
                }
                Address.TextChanged += a;
                Address.Focus();

            }
        }

        private void new_Birthdate_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
    public class NannyT
    {
        public Nanny Info { get; set; }
        public Nannydetails Daily_Working_hours { get; set; }
        public NannyT(Nanny d, Nannydetails a) { Info = d; Daily_Working_hours = a; }
    }
}
