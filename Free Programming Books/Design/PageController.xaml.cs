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

namespace Free_Programming_Books.Design
{
    /// <summary>
    /// Interaction logic for PageController.xaml
    /// </summary>
    public partial class PageController : UserControl
    {
        public Int32 totalNumberOfBooks;
        public Int32 currentPage;
        public delegate void ButtonNextClicked();
        public PageController()
        {
            InitializeComponent();
            UpdateResult();
        }

        public void UpdateResult()
        {
            totalBooks.Content = totalNumberOfBooks;
            this.Visibility = (currentPage == 0)? Visibility.Collapsed: Visibility.Visible;
            if (totalNumberOfBooks > currentPage * 10)
            {
                Int32 next = ((totalNumberOfBooks - (currentPage * 10)) > 10) ? 10 : (totalNumberOfBooks - (currentPage * 10));
                buttonNext.Content = "Load next " + next +" Books for your search query";
                bookLoaded.Content = (currentPage * 10);
            }
            else
            {
                bookLoaded.Content = totalNumberOfBooks;
                buttonNext.Content = "It's look like end of search result.";
                buttonNext.IsEnabled = false;
            }
        }

        public void IsNextPageRequestOnGoing(bool request)
        {
            buttonNext.IsEnabled = !request;
        }
    }
}
