using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Free_Programming_Books.Design
{
    /// <summary>
    /// Interaction logic for BookDetailedView.xaml
    /// </summary>
    public partial class BookDetailedView : UserControl
    {
        public Books book { get; set; }
        public BookDetailedView()
        {
            InitializeComponent();
        }
        
        public void setupInitialBookDetails()
        {
            this.Visibility = Visibility.Visible;
            contentGrid.Visibility = Visibility.Hidden;
            progressBar.Visibility = Visibility.Visible;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(book.Image);
            image.EndInit();
            bookTitle.Text = book.Title;
            bookImage.Source = image;
            bookISBN.Content = book.ISBN;
            buttonBookDownload.IsEnabled = false;
            book.LoadAllDetailsForBook(bookComplete =>
            {
                book = bookComplete;
                setupFullBookDetails();
            });
        }

        public void setupFullBookDetails()
        {
            if (book.IsComplete)
            {
                contentGrid.Visibility = Visibility.Visible;
                progressBar.Visibility = Visibility.Hidden;
                bookDesc.Text = book.Description;
                bookAuthor.Content = book.Author;
                bookPages.Content = book.Page;
                bookPublisher.Content = book.Publisher;
                bookYear.Content = book.Year;
                buttonBookDownload.Content = "  Download " + book.Title + "  ";
                buttonBookDownload.IsEnabled = true;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void buttonBookDownload_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(book.Download);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
