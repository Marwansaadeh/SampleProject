using System;
using System.Collections.Generic;
using System.Linq;

namespace BankWebbApp.ViewModels
{
    public class CustomersViewModel
    {
        public string Search { get; set; }
        public List<Items> Items { get; set; }
        public PagingViewModel PagingViewModel { get; set; } = new PagingViewModel();
    }

    public class Items
    {
        public int CustomerId { get; set; }
        public string NationalId { get; set; }

        public string Givenname { get; set; }

        public string Streetaddress { get; set; }

        public string City { get; set; }
    }

    public class PagingViewModel
    {

        public IEnumerable<string> GetPages
        {
            get
            {
                int delta = 2;
                int left = CurrentPage - delta;
                int right = CurrentPage + delta + 1;

                var range = new List<string>();
                for (int i = 1; i <= MaxPages; i++)
                    if (i == 1 || i == MaxPages || (i >= left && i < right))
                        range.Add(i.ToString());

                var rangeIncludingDots = new List<string>();
                int l = 0;
                foreach (var i in range.Select(r => Convert.ToInt32(r)))
                {
                    if (l > 0)
                    {
                        if (i - l == 2)
                            rangeIncludingDots.Add((l + 1).ToString());
                        else if (i - l != 1)
                            rangeIncludingDots.Add("...");
                    }

                    rangeIncludingDots.Add(i.ToString());
                    l = i;
                }

                return rangeIncludingDots;
            }
        }

        public const int PageSize = 50;
        public string ColumnName { get; set; }
        public string Sort { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPages { get; set; }

        public bool ShowPrevButton
        {
            get { return CurrentPage > 1; }
        }


        public bool ShowNextButton
        {
            get { return CurrentPage < MaxPages; }
        }
        //public string SetSort(string sort, string currentcolumn)
        //{
        //     if (currentcolumn == ColumnName)
        //    {
        //        if (Sort == "desc")
        //        {
        //             Sort = "asc";
        //        }
        //        else if (Sort == "asc")
        //        {
        //             Sort = "desc";
        //        }

        //    } 
        //    else if (currentcolumn != ColumnName)
        //        {

        //        Sort="desc";
        //    }
        //    return Sort;
        //}
        public IQueryable<Items> SetSortedItems(IQueryable<Items> items)
        {
            if (string.IsNullOrEmpty(ColumnName))
                ColumnName = "CustomerId";
            if (string.IsNullOrEmpty(Sort))
                Sort = "asc";

            IQueryable<Items> sotreditems;
            switch (ColumnName)
            {
                case "CustomerId":
                    if (Sort == "asc")
                    {
                        sotreditems = items.OrderBy(x => x.CustomerId);
                    }
                    else
                    {
                        sotreditems = items.OrderByDescending(x => x.CustomerId);
                    }
                    break;
                case "GivenName":
                    if (Sort == "asc")
                    {
                        sotreditems = items.OrderBy(x => x.Givenname);
                    }
                    else
                    {
                        sotreditems = items.OrderByDescending(x => x.Givenname);
                    }
                    break;
                case "Address":
                    if (Sort == "asc")
                    {
                        sotreditems = items.OrderBy(x => x.Streetaddress);
                    }
                    else
                    {
                        sotreditems = items.OrderByDescending(x => x.Streetaddress);
                    }
                    break;
                case "City":
                    if (Sort == "asc")
                    {
                        sotreditems = items.OrderBy(x => x.City);
                    }
                    else
                    {
                        sotreditems = items.OrderByDescending(x => x.City);
                    }
                    break;
                case "NationalID":
                    if (Sort == "asc")
                    {
                        sotreditems = items.OrderBy(x => x.NationalId);
                    }
                    else
                    {
                        sotreditems = items.OrderByDescending(x => x.NationalId);
                    }
                    break;
                default:
                    sotreditems = items.OrderBy(x => x.CustomerId);
                    break;
            }
            return sotreditems;
        }

    }

}
