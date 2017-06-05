using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WhatsDown.Models
{
    public class SelectUsersModel
    {
        public SelectUsersModel()
        {
            Title = "";
            SelectedIds = new List<string>();
            Users = new List<SelectListItem>();
        }

        public string Title { get; set; }

        public IEnumerable<string> SelectedIds
        {
            get; set;
        }

        public IEnumerable<SelectListItem> Users
        {
            get; set;
        }
    }
    public class Automobile
    {
        [Display(Name = "Makes")]
        public string Makes
        {
            get; set;
        }
    }

    public class MyViewModel
    {
        public int Id;
        public string Name;

        public IEnumerable<string> SelectedIds
        {
            get; set;
        }

        public IEnumerable<SelectListItem> Items
        {
            get; set;
        }
    }
}