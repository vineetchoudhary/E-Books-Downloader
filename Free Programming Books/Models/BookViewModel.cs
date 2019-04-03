using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;

namespace Free_Programming_Books
{
    public class Books
    {
        public string Error { get; set; }
        public string Time { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Page { get; set; }
        public string Year { get; set; }
        public string Publisher { get; set; }
        public string Image { get; set; }
        public string Download { get; set; }
        public bool IsComplete { get; set; }

        public void LoadAllDetailsForBook(Action<Books> book)
        {
            var client = new RestClient("");
            var request = new RestRequest("/book/{id}");
            request.AddUrlSegment("id", this.ID);
            client.ExecuteAsync<Books>(request, response =>
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    this.Author = response.Data.Author;
                    this.Description = response.Data.Description;
                    this.Download = response.Data.Download;
                    this.Error = response.Data.Error;
                    this.Page = response.Data.Page;
                    this.Publisher = response.Data.Publisher;
                    this.Time = response.Data.Time;
                    this.Year = response.Data.Year;
                    this.IsComplete = true;
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        book(this);
                    }));
                }
                else
                {
                    Debug.Write(response.StatusDescription);
                    MessageBox.Show(this.Error + "\nPlease try again.", "Oops", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    this.IsComplete = false;
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        book(this);
                    }));
                }
            });
        }
    }

    public class Search
    {
        public string Error { get; set; }
        public string Time { get; set; }
        public Int32 Total { get; set; }
        public Int32 Page { get; set; }
        public Int32 NextPage { get; set; }
        public bool isRequestOnGoing { get; set; }
        public List<Books> Books { get; set; }
        public ObservableCollection<Books> BooksItems { get; set; }
        public void SearchRequest(String query, Action<Search> searchResult)
        {
            this.isRequestOnGoing = true;
            if (this.NextPage == 1 || BooksItems == null)
            {
                BooksItems = new ObservableCollection<Books>();
            }
            Books = new List<Books>();
            var client = new RestClient("");
            var request = new RestRequest("search/{searchQuery}/page/{page}", Method.GET);
            request.AddUrlSegment("searchQuery", query);
            request.AddUrlSegment("page", this.NextPage.ToString());
            client.ExecuteAsync<Search>(request, response =>
             {
                 this.isRequestOnGoing = false;
                 if (response.StatusCode == System.Net.HttpStatusCode.OK)
                 {
                     this.Error = response.Data.Error;
                     this.Page = response.Data.Page;
                     this.Time = response.Data.Time;
                     this.Total = response.Data.Total;
                     this.NextPage = this.NextPage + 1;
                     if (response.Data.Books != null)
                     {
                         this.Books.AddRange(response.Data.Books);
                         if (this.Books.Count != 0)
                         {
                             foreach (var item in this.Books)
                             {
                                 Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                                 {
                                     BooksItems.Add(item);
                                 }));
                             }
                         }
                     }
                 }
                 else
                 {
                     Debug.Write(response.StatusDescription);
                     MessageBox.Show("Something goes wrong.Please try again.", "Oops", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                 }
                 Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                 {
                     searchResult(this);
                 }));
             });
        }
    }
}
