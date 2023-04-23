using System;
using System.Collections.Generic;
using System.Globalization;
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
using Newtonsoft.Json;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace MegaEzhednevnich
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool is_edited_name = false;
        bool is_edited_description = false;
        List<Notes> notes = new List<Notes>();
        List<Notes> today_notes = new List<Notes>();
        public MainWindow()
        {
            try
            {
                DateTime now = DateTime.Today;              
                UpdateList(now);
            }
            catch
            {
                MessageBox.Show("OBKAKISH ALARM!!!");
            }

            InitializeComponent();
        }

        public void UpdateList(DateTime date)
        {    
            try
            {
                today_notes.Clear();
                notes = Notes.Deserialize(notes);
            }
            catch 
            {
                MessageBox.Show(" DESERIALIZATION OBKAKISH ALARM!!!");
            }
            if (NotesList != null)
            {
                NotesList.Items.Clear();
            }
            
            
            foreach (Notes one_note in notes)
            {
                if (one_note.date == date)
                {
                    NotesList.Items.Add(one_note.name);   
                    today_notes.Add(one_note);
                }
            }
            
                                    
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < notes.Count; i++)
            {
                try
                {
                    if (notes[i].name == NotesList.SelectedValue.ToString() &
                    notes[i].date == DateChooser.SelectedDate.Value)
                    // если здесь ошибка, то запускайте без отладки и все будет хорошо 
                    {
                        Notes note = notes[i];
                        note.date = DateChooser.SelectedDate.Value;
                        note.name = NoteName.Text;
                        note.description = NoteDescription.Text;
                        notes[i] = note;
                        Notes.Serialize(notes);
                        UpdateList(DateChooser.SelectedDate.Value);
                        DeleteButtton.IsEnabled = false;
                        SaveButton.IsEnabled = false;
                    }
                }
                catch
                {
                    continue;
                    //Зачем напрягаться если можно не напрягаться
                }
            }
        }

        private void NoteDescription_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!is_edited_description)
            {
                NoteDescription.Clear();
                NoteDescription.Foreground = Brushes.Black;
                NoteDescription.FontStyle = FontStyles.Normal;
                is_edited_description = true;
            }
        }

        private void NoteName_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {          
            if (!is_edited_name)
            {
                NoteName.Clear();
                NoteName.Foreground = Brushes.Black;
                NoteName.FontStyle = FontStyles.Normal;
                is_edited_name = true;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Notes note = new Notes();
            note.date = DateChooser.SelectedDate.Value;
            note.name = NoteName.Text;
            note.description = NoteDescription.Text;
            notes.Add(note);
            Notes.Serialize(notes);            
            UpdateList(DateChooser.SelectedDate.Value);
        }

        private void DateChooser_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            NotesList.Items.Clear();
            UpdateList(DateChooser.SelectedDate.Value);
                   
        }

        private void NotesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Notes choosed_note = new Notes();
            foreach (Notes one_note in today_notes)
            {
                if (one_note.name == NotesList.SelectedValue.ToString())
                {
                    choosed_note = one_note;
                }
            }
            is_edited_description = true;
            is_edited_name = true;
            NoteName.Text = choosed_note.name;
            NoteName.Foreground = Brushes.Black;
            NoteName.FontStyle = FontStyles.Normal;
            NoteDescription.Text = choosed_note.description;
            NoteDescription.Foreground = Brushes.Black;
            NoteDescription.FontStyle = FontStyles.Normal;

            SaveButton.IsEnabled = true;
            DeleteButtton.IsEnabled = true; 
        }

        private void DeleteButtton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i<notes.Count; i++)
            {   
                try
                {
                    if (notes[i].name == NotesList.SelectedValue.ToString() &
                    notes[i].date == DateChooser.SelectedDate.Value)
                    // если здесь ошибка, то запускайте без отладки и все будет хорошо 
                    {
                        notes.Remove(notes[i]);
                        Notes.Serialize(notes);
                        UpdateList(DateChooser.SelectedDate.Value);
                        DeleteButtton.IsEnabled = false;
                        SaveButton.IsEnabled=false;
                    }
                }
                catch 
                {
                    continue;
                    //Зачем напрягаться если можно не напрягаться
                }
                
            }
        }
    }
}
