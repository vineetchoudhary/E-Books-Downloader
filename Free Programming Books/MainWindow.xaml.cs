using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using RestSharp;

namespace Free_Programming_Books
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Search searchObject;
        public MainWindow()
        {
            InitializeComponent();
            //setup search object
            searchObject = new Search();
            pageController.buttonNext.Click += ButtonNext_Click;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            booksForSearchQuery();
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            String searchQuery = textBoxSearch.Text.Trim();
            if (searchQuery.Length > 1)
            {
                searchObject.NextPage = 1;
                booksForSearchQuery();
            }
            else
            {

            }
        }

        public void booksForSearchQuery()
        {
            String searchQuery = textBoxSearch.Text.Trim();
            progressBar.Visibility = Visibility.Visible;
            pageController.IsNextPageRequestOnGoing(true);
            searchObject.SearchRequest(searchQuery, searchResult => {
                pageController.IsNextPageRequestOnGoing(false);
                bookListView.ItemsSource = searchResult.BooksItems;
                bookListView.Items.Refresh();
                progressBar.Visibility = System.Windows.Visibility.Hidden;
                pageController.totalNumberOfBooks = searchResult.Total;
                pageController.currentPage = searchResult.NextPage - 1;
                pageController.UpdateResult();
            });
        }
        
        private void bookListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(((ListView)sender).SelectedIndex == -1))
            {
                bookDetailedView.book = ((Books)((ListView)sender).SelectedItem);
                bookDetailedView.setupInitialBookDetails();
                bookListView.UnselectAll();
            }
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxSearch.Text.Length > 49)
            {
                textBoxSearch.Text = textBoxSearch.Text.Substring(0, 50);
                MessageBox.Show("Search query limit 50 characters maximum.", "Oops", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
    }
}
