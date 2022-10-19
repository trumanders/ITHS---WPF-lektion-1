using System;
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
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace ITHS___WPF_lektion_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Person> allPersons = new List<Person>();
        bool showingFileContent = false;
        const string SHOW_SAVED = "Show saved";
        const string HIDE_SAVED = "Hide saved";
        const string FILENAME = "savedEntries.txt";

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(FILENAME))
                File.AppendAllText(FILENAME, "");

            // Show entries count
            UpdateSavedUnsavedText();
            UpdateSaveAndShowButtons();
        }    


        // Add entered info to List
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAndAddInfoToList())
            {
                ShowListContent();
                if (showingFileContent)
                    ShowFileContent();
                txt_lastName.Clear();
                txt_firstName.Clear();
                txt_email.Clear();
            }
            UpdateSaveButton();
        }


        // Show or hide file content
        private void btn_toggleFileContent_Click(object sender, RoutedEventArgs e)
        {
            ShowListContent();
            if (showingFileContent)
            {
                btn_toggleFileContent.Content = SHOW_SAVED;
                showingFileContent = false;
            }
            
            else
            {
                ShowFileContent();
                btn_toggleFileContent.Content = HIDE_SAVED;
                showingFileContent = true;
            }            
        }


        // Show file content
        private void ShowFileContent()
        {
            // Show file content
            foreach (var line in File.ReadAllLines(FILENAME))
            {
                lbl_info.Content += line + "\n";
            }
        }


        // Save List to file. If entry already exists, skip
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            int numOfLinesAdded = 0;
            foreach (Person pers in allPersons)
            {                
                bool toSave = true;
                foreach (var line in File.ReadAllLines(FILENAME))
                {
                    if (line.Contains(pers.Email))
                        toSave = false;
                }
                if (toSave)
                {
                    File.AppendAllText(FILENAME, $"{pers.FirstName} {pers.LastName}, {pers.Email}\n");
                    numOfLinesAdded++;
                }
            }

            // Clear the list, update text and buttons
            allPersons.Clear();
            ShowListContent();
            UpdateSavedUnsavedText();
            UpdateSaveAndShowButtons();
            MessageBox.Show($"{numOfLinesAdded} entries saved.");           
        }


        private void UpdateSavedUnsavedText()
        {
            lbl_saved.Content = "Saved entries: " + CountNumberOfFileEntries();
            lbl_unsaved.Content = "Unsaved entries: " + CountNumberOfListEntries();
        }


        // Count number of entries in List (unsaved)
        private int CountNumberOfListEntries()
        {
            int listCount = 0;
            foreach (var item in allPersons)
                listCount++;
            return listCount;
        }


        // Count number of entries in file (saved)
        private int CountNumberOfFileEntries()
        {
            int fileCount = 0;
            foreach (var item in File.ReadAllLines(FILENAME))
                fileCount++;
            return fileCount;
        }


        // Output the content of the List to the screen
        private void ShowListContent()
        {
            lbl_info.Content = "";
            foreach (Person pers in allPersons)              
                lbl_info.Content += $"{pers.FirstName} {pers.LastName}, {pers.Email}\n";
            UpdateSavedUnsavedText();
        }

      
        // Check for valid input, and 
        private bool ValidateAndAddInfoToList()
        {
            Regex emailRegEx = new Regex(@"^[a-zA-Z]\w*@\w+\.[a-zA-Z]{2,}$");
            if (txt_firstName.Text.Length < 2 || !Char.IsUpper(txt_firstName.Text[0]))
            {
                MessageBox.Show("Please enter a valid first name");
                return false;
            }
            else if (txt_lastName.Text.Length < 2 || !Char.IsUpper(txt_lastName.Text[0]))
            {
                MessageBox.Show("Please enter a valid last name");
                return false;
            }
            else if (!emailRegEx.IsMatch(txt_email.Text))
            {
                MessageBox.Show("Please enter a valid email adress");
                return false;
            }
            allPersons.Add(new Person(txt_firstName.Text, txt_lastName.Text, txt_email.Text));
            return true;
        }


        // Enable / disable buttons
        private void UpdateSaveButton()
        {
            if (CountNumberOfListEntries() == 0)           
                btn_save.IsEnabled = false;
            else btn_save.IsEnabled = true;
        }

        private void UpdateShowButton()
        {
            if (CountNumberOfFileEntries() == 0)
                btn_toggleFileContent.IsEnabled = false;
            else btn_toggleFileContent.IsEnabled = true;
        }

        private void UpdateSaveAndShowButtons()
        {
            UpdateSaveButton();
            UpdateShowButton();
        }
    }
}
